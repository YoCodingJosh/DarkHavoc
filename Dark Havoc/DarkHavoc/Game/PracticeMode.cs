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
    internal class PracticeMode : GameScreen
    {
        // Declarations
        float pauseAlpha;
        Player player;
        HomingMissile missile;

        public PracticeMode()
        {
            Debug.WriteLine("[Dark Havoc] Starting Practice Mode...");

            // How long to fade in
            TransitionOnTime = TimeSpan.FromSeconds(0.75);

            // How long to fade out
            TransitionOffTime = TimeSpan.FromSeconds(0.75);
        }

        public override void LoadContent()
        {
            player = new Player(Assets.playerShipTexture, new Vector2(100, 100));
            missile = new HomingMissile(Assets.enemyMissileTexture, new Vector2(666, 250));
            missile.Destination = player.Origin;
            Debug.WriteLine("[Dark Havoc] Practice Mode has started!");
        }

        public override void UnloadContent()
        {
            Debug.WriteLine("[Dark Havoc] Quiting Practice Mode...");
            base.UnloadContent();
        }

        public override void HandleInput(InputState input)
        {
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(GameType.Training));
            }
            else
            {
                player.UpdateInput(input, PlayerIndex.One);
            }

            player.UpdateClamp(GlobalConstants.titleSafeViewport.Top, GlobalConstants.titleSafeViewport.Bottom, GlobalConstants.titleSafeViewport.Left, GlobalConstants.titleSafeViewport.Right);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                player.Update(gameTime);
                missile.Destination = player.Position;
                missile.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            spriteBatch.DrawString(Assets.consoleFont, "This is PRACTICE MODE!!!! RAWR. :3", new Vector2(100, 100), Color.LimeGreen);

            player.Draw(spriteBatch, gameTime);
            missile.Draw(spriteBatch);
            //spriteBatch.Draw(Assets.blankTexture, missile.Position, missile.RectangleCollider, new Color(255, 0, 0, 10), 0f, missile.Origin, Vector2.One, SpriteEffects.None, 1.0f);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
