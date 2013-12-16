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
    /// This is the game screen that advertises "Joshua Kennedy" at the beginning of the game.
    /// </summary>
    class SplashScreen : GameScreen
    {
        // Our keyboard state
        KeyboardState currentKeyboardState;

        // Our controller state
        GamePadState currentGamePadState;

        // How long should the screen stay fully visible
        const float timeToStayOnScreen = 1.0f;

        // Keep track of how much time has passed
        float timer = 0f;

        // Our current fade value.
        byte fade;

        // How fast the fade animation goes.
        float fadeSpeed;

        // The color of our fading, it's white but with the alpha value being our fade byte.
        Color fadeColor;

        // What's our FadeState? We check this to determine the position of our animation.
        FadeState myFadeState;

        // This signals the end of our animation.
        bool fadeOut;

        public SplashScreen()
        {
            // How long to fade in
            TransitionOnTime = TimeSpan.FromSeconds(1.0);

            // How long to fade out
            TransitionOffTime = TimeSpan.FromSeconds(0.48);
        }

        public override void LoadContent()
        {
            // Set our initial values.
            fade = 1;
            fadeSpeed = 1.996f;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Don't forget that a byte is between 0 and 255!

            // If we're transitioning...
            if (myFadeState == FadeState.Transitioning)
            {
                // If this is the end of our animation...
                if (fadeOut)
                {
                    // This checks if we're not zero (not showing) or we're equal to 255 (fully visible)
                    if (fade != 0 || fade == 255)
                    {
                        // We decrement our fade variable.
                        fade -= 5;
                    }
                }
                else
                {
                    // We're transitioning to fully visible
                    if (fade != 255)
                    {
                        // so we increment our fade byte.
                        fade++;
                    }
                }
            }

            // We set our fade color to white, with the alpha value being our fade byte.
            fadeColor = Color.White;
            fadeColor.A = fade;

            // We assign our fade state.

            // If our fade is 0 (invisible), we set our fadeState to invisible.
            if (fade == 0)
            {
                myFadeState = FadeState.Invisible;
            }
            else if (fade > 0 && fade < 255) // If we're greater than 0 and less than 255, set our fadeState to transitioning.
            {
                myFadeState = FadeState.Transitioning;
            }
            else if (fade == 255) // We're at 255, then we're fully visible and we set our fadeState to visible.
            {
                myFadeState = FadeState.Visible;
            }

            if (myFadeState == FadeState.Invisible)
                ExitScreen();

            // We check the transition of our GameScreen to see if we're active (not transitioning).
            if (ScreenState == ScreenState.Active)
            {
                // When this screen is fully active, we want to begin our timer so we know when to fade out.
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timer += elapsed;

                // If our fade state is visible...
                if (myFadeState == FadeState.Visible)
                {
                    // Let's go the opposite direction.
                    fadeOut = true;
                    myFadeState = FadeState.Transitioning;
                }
            }
            else if (ScreenState == ScreenState.TransitionOff) // if not, then is our GameScreen transitioning off?
            {
                // If our TransitionPosition is 1 (fully transitioned off)...
                if (TransitionPosition == 1)
                {
                    // We exit our screen.
                    ScreenManager.RemoveScreen(this);

                    // and we load our next screens.
                    ScreenManager.AddScreen(new MenuBackgroundScreen());
                    ScreenManager.AddScreen(new TitleScreen());
                }
            }
        }

        public override void HandleInput(InputState input)
        {
            // We get our keyboard and gamepad states.
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // We check to see if the keyboard's space, enter, or escape keys, or the gamepad's A, B, or Start buttons are being pressed.
            if (currentKeyboardState.IsKeyDown(Keys.Space) || currentKeyboardState.IsKeyDown(Keys.Enter) || currentKeyboardState.IsKeyDown(Keys.Escape) ||
                currentGamePadState.IsButtonDown(Buttons.A) || currentGamePadState.IsButtonDown(Buttons.B) || currentGamePadState.IsButtonDown(Buttons.Start))
            {
                // If so, we tell the ScreenManager to get rid of this screen (nicely).
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // assign our spritebatch globally in this method.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Set the second texture in our textures collection to the trippy waterfall texture.
            ScreenManager.GraphicsDevice.Textures[1] = Assets.waterfallTexture;

            // Set the OverlayScroll paramter to our circle value.
            Assets.disappearEffect.Parameters["OverlayScroll"].SetValue(Helper.MoveInCircle(gameTime, fadeSpeed) * 0.75f);

            // Let's begin with our effect.
            spriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, Assets.disappearEffect);

            // Draw the logo, centered, and with our special fadeColor.
            spriteBatch.Draw(Assets.joshuaKennedyLogoTexture, Helper.CenterOnScreen(ScreenManager, Assets.joshuaKennedyLogoTexture), fadeColor);

            // Conclude our drawing.
            spriteBatch.End();
        }
    }
}
