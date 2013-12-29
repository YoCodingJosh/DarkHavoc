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
using DarkHavoc.Engine.API;
using AssetLoader;

namespace DarkHavoc
{
    /// <summary>
    /// Have the player press any key to continue. Also used as another background to the menu.
    /// </summary>
    class TitleScreen : GameScreen
    {
        // When the player returns to the main menu from playing, we don't want them to have to go through the title screen.
        public static bool passThrough;

        // The scale of our text, calculated from the sine of the current elapsed time in seconds times 5 and other stuff.
        private float scale;

        private float pauseAlpha;

        private Vector2 logoPosition;
        Vector2 endPos;

        /// <summary>
        /// Construct new title screen object.
        /// </summary>
        public TitleScreen()
        {
            // If we just started the game...
            if (!passThrough)
            {
                // Then make the transition to the title screen slower.
                TransitionOnTime = TimeSpan.FromSeconds(1.25);
            }
            else
            {
                // otherwise make it go a bit faster.
                TransitionOnTime = TimeSpan.FromSeconds(0.75);
            }

            // Make the transition from the title screen go sort of fast.
            TransitionOffTime = TimeSpan.FromSeconds(0.75);
        }

        public override void LoadContent()
        {
			logoPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
			endPos = new Vector2(logoPosition.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Top + 232);

            //mainMenuMusic = JoshoEngine.MusicPlayer.LoadModule("./Resources/Music/kool-gro.xm");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

			logoPosition.X = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X;

            // If we're at the press any key screen then update the pulsating press any key text.
            if (!passThrough)
            {
                // Keeps track of the elapsed time (used for calculating the scale)
                double time = gameTime.TotalGameTime.TotalSeconds;

                // Calculate our pulsation
                float pulsate = (float)Math.Sin(time * 5) + 1;

                // Make sure the scale looks good.
                scale = 1 + pulsate * 0.05f * 3.0f;
            }

            // We transition from being covered by other screens.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            if (passThrough)
            {
				//float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

                if (logoPosition.Y >= endPos.Y)
                {
                    logoPosition.Y -= 5;
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            if (!passThrough)
            {
                // We need input to not be null.
                if (input == null)
                    throw new ArgumentNullException("input");

                // Get the keyboard and gamepad states.
                KeyboardState keyboardState = Keyboard.GetState();
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

				// If the number keys on the keyboard is greater than 0 (in case they're pressing 2 keys at once)...
                // or the mouse is clicked
                if (keyboardState.GetPressedKeys().Length > 0 || input.IsNewLeftMouseClick())
                {
                    // Proceed to the main menu.
                    GotoMainMenu();
                }

                // If the start, a, b, x, or y buttons are being pressed...
                if (gamePadState.IsButtonDown(Buttons.Start) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.B) || gamePadState.IsButtonDown(Buttons.X) || gamePadState.IsButtonDown(Buttons.Y))
                {
                    // Proceed to the main menu.
                    GotoMainMenu();
                }
            }
        }

        private void GotoMainMenu()
        {
            // Set the static bool passThrough to true.
            passThrough = true;

            // If the sound is enabled, then play the menu select sound.
            //if (OptionsFile.IsSound())
            //    Assets.menuSelectSound.Play();

            // Add the main menu screen to the ScreenManager stack.
            ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);
        }

        private void DrawLogo(SpriteBatch spriteBatch)
        {
            // If we're at the gate, then draw the logo normally.
            if (!passThrough)
            {
                spriteBatch.Begin();

				spriteBatch.Draw(Assets.darkHavocLogo, logoPosition, null, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha), 0f,
					new Vector2(Assets.darkHavocLogo.Width / 2, Assets.darkHavocLogo.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                spriteBatch.End();
            }
            else
            {
                // otherwise desaturate it.
                spriteBatch.Begin(0, null, null, null, null, Assets.desaturateEffect);

				spriteBatch.Draw(Assets.darkHavocLogo, logoPosition, null, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha, 0), 0f,
					new Vector2(Assets.darkHavocLogo.Width / 2, Assets.darkHavocLogo.Height / 2), 1.0f, SpriteEffects.None, 1.0f);

                spriteBatch.End();
            }
        }

        private void DrawText(SpriteBatch spriteBatch)
        {
            if (!passThrough)
            {
                // Our text that we're going to draw.
                string startText;
#if PC
                startText = "Press Any Key!";
#else
                startText = "Press Start!";
#endif
                // The vectors that will position the text right underneath the logo.
                Vector2 textSize = Assets.menuFont.MeasureString(startText);
                Vector2 textOrigin = new Vector2(textSize.X / 2, textSize.Y / 2);
                Vector2 fontPos = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2);
                Vector2 textPosition = new Vector2(fontPos.X, fontPos.Y + (Assets.darkHavocLogo.Height / 2) + 42);

                // Let's draw shall we?
                spriteBatch.Begin();

                ShadowedString.DrawShadowedString(Assets.menuFont, startText, textPosition, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha), 0.0f, textOrigin, scale, spriteBatch);

                // End drawing.
                spriteBatch.End();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Locally declare our spritebatch.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Draw the logo.
            DrawLogo(spriteBatch);

            // Draw the "press any key" text
            DrawText(spriteBatch);

            // Manage the transition alpha.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
