using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkHavoc.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetLoader;
using DarkHavoc.Engine.API;

namespace DarkHavoc.Engine.Menu
{
    public class MenuEntry
    {
        private string text; // The item text

        private Vector2 position; // The 2d position of the item

        private float selectionFade; // When the item is deselected, it fades out of focus

        private bool enabled;

        /// <summary>
        /// Gets or sets the text of this menu entry.
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Gets or sets if the MenuEntry is enabled.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null && enabled)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }

        public MenuEntry(string text)
        {
            this.text = text;
            enabled = true;
        }

        public MenuEntry(string text, bool enabled)
        {
            this.text = text;
            this.enabled = enabled;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 2;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }

        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
#if WINDOWS_PHONE || PS_VITA
            isSelected = false;
#endif
            Color color = enabled ? isSelected ? Color.OrangeRed : Color.WhiteSmoke : Color.SlateGray;

            color *= screen.TransitionAlpha;

            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = Assets.menuFont;

            Vector2 origin = new Vector2(0, font.LineSpacing / 2);

            ShadowedString.DrawShadowedString(font, text, position, color, 0, origin, 1.0f, spriteBatch);
        }

        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(MenuScreen screen)
        {
            return Assets.menuFont.LineSpacing;
        }

        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)Assets.menuFont.MeasureString(text).X;
        }
    }
}
