using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Animation;
using DarkHavoc.Engine.API;
using AssetLoader;

namespace DarkHavoc.Engine.Menu
{
    /// <summary>
    /// The buttons used for the main menu and options.
    /// </summary>
    public class MenuButton : JoshoEntity
    {
        // The label for our button.
        private string text;

        // If the mouse is hovering over this button.
        public bool IsHovering;

        // Is the button selectable?
        public bool IsEnabled;

        // Event handler for when the button is selected.
        public event EventHandler<EventArgs> Selected;

        // Button's Color.
        private Color buttonColor;

        // Label Color.
        private Color textColor;

        // This is used for lerping the highlight color and the idle color.
        private Color nextHoverColor;

        // The amount to lerp the two colors by.
        private float lerpAmount;

        public const int ButtonWidth = 175;
        public const int ButtonHeight = 50;

        /// <summary>
        /// Constructs new MenuButton object.
        /// </summary>
        /// <param name="screenManager">The current ScreenManager.</param>
        /// <param name="position">The position of the button.</param>
        /// <param name="label">The button's label.</param>
        public MenuButton(Vector2 position, string label)
            : base(Assets.blankTexture, position)
        {
            // Self initialization.
            MyTexture = Assets.blankTexture;
            Position = position;
            text = label;
            IsEnabled = true;
            IsHovering = false;
            textColor = Color.WhiteSmoke;

            Initialize();
        }

        /// <summary>
        /// Constructs new MenuButton object.
        /// </summary>
        /// <param name="screenManager">The current ScreenManager.</param>
        /// <param name="position">The position of the button.</param>
        /// <param name="label">The button's label.</param>
        /// <param name="enabled">The enabled status of the button.</param>
        public MenuButton(Vector2 position, string label, bool enabled)
            : base(Assets.blankTexture, position)
        {
            // Self initialization.
            MyTexture = Assets.blankTexture;
            Position = position;
            text = label;
            IsEnabled = enabled;
            IsHovering = false;
            textColor = Color.WhiteSmoke;

            Initialize();
        }

        // Save space and redundancy in our constructors.
        public void Initialize()
        {
            // Post JoshoEntity initialization.
            this.Width = ButtonWidth;
            this.Height = ButtonHeight;
            this.Active = true;
            this.RectangleCollider = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            nextHoverColor = Color.Orange;
        }

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        public virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null && IsEnabled)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public override void Update(GameTime gameTime)
        {
            // Update collision rectangle with the new size and position.
            this.RectangleCollider = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);

            // Ternary operation to determine which color to use.
            // We assign buttonColor to itself if the button is hovering, because we are assigning buttonColor its value down below.
            buttonColor = IsEnabled ? IsHovering ? buttonColor : Color.Orange : Color.DarkGray;

            // If the user has this button highlighted, then pulsate the button background.
            if (IsHovering && IsEnabled)
            {
                // Increment lerpAmount by 10%
                lerpAmount += 0.25f;

                // Linearlly interpolate the colors.
                buttonColor = Color.Lerp(buttonColor, nextHoverColor, lerpAmount);

                // If we converted to the other color.
                if (lerpAmount >= 1.0f)
                {
                    // Reset lerpAmount.
                    lerpAmount = 0.00f;

                    // And swap the colors.
                    if (nextHoverColor == Color.OrangeRed)
                    {
                        nextHoverColor = Color.Orange;
                    }
                    else
                    {
                        nextHoverColor = Color.OrangeRed;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the button background.
            spriteBatch.Draw(this.MyTexture, this.RectangleCollider, buttonColor);

            // Draw the text.
            ShadowedString.DrawShadowedString(Assets.menuFont, text, this.RectangleCollider, Helper.TextAlignment.Center, textColor, spriteBatch);
        }
    }
}
