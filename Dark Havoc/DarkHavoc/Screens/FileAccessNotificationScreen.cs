using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Animation;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.DataReaders;

namespace DarkHavoc
{
    internal class FileAccessNotificationScreen : GameScreen
    {
        private Texture2D notificationAnimationTexture;
        private SpriteStrip notificationAnimation;
        private SpriteFont notificationFont;
        private string notificationString;
        private float currentTime;
        private float timeToStayOnScreen;
        private SpriteStripMetadata notificationAnimationMetadata;

        public FileAccessNotificationScreen()
        {
            string separator = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            notificationString = "Hey, when you see this symbol:" + separator + "do not power off the system!!!";

            TransitionOnTime = TimeSpan.FromSeconds(0.75);
            TransitionOffTime = TimeSpan.FromSeconds(0.75);

            timeToStayOnScreen = 2.50f;
        }

        public override void LoadContent()
        {
#if !MONOMAC
            notificationAnimationMetadata = new SpriteStripMetadata("./Resources/Animations/HDDACCESSINDICATOR.ini");
#else
			notificationAnimationMetadata = new SpriteStripMetadata("./Content/Animations/HDDACCESSINDICATOR.ini");
#endif
            Vector2 centerOfScreen = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);

            ContentManager content = ScreenManager.Game.Content;

            notificationAnimationTexture = content.Load<Texture2D>("./Animations/HDDACCESSINDICATOR");

            notificationAnimation = new SpriteStrip();

            notificationAnimation.Initialize(notificationAnimationTexture, centerOfScreen, notificationAnimationMetadata);
            //notificationAnimation.Initialize(notificationAnimationTexture, centerOfScreen, 128, 128, 30, 30, Color.White, 1.0f, true);

            notificationFont = content.Load<SpriteFont>("./Fonts/ConsoleFont");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update the animation
            notificationAnimation.Update(gameTime);

            // Our screen timer.
            if (ScreenState == Engine.ScreenState.Active)
            {
                // Get the total elapsed time in seconds.
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // increment the current time.
                currentTime += elapsed;

                // If we've stayed on the screen long enough.
                if (currentTime >= timeToStayOnScreen)
                {
                    // Quit this screen.
                    ExitScreen();
                }
            }
            else if (ScreenState == Engine.ScreenState.TransitionOff)
            {
                // When we're fully transitioned...
                if (TransitionPosition == 1)
                {
                    // ... add the next screen.
                    ScreenManager.AddScreen(new AssetLoadScreen());
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Color myColor = new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha);

            Vector2 centerOfScreen = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
            Vector2 sizeofString = notificationFont.MeasureString(notificationString);
            Vector2 stringCenter = new Vector2(sizeofString.X / 2, (sizeofString.Y / 2));

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            notificationAnimation.Draw(spriteBatch, myColor);

			spriteBatch.DrawString(notificationFont, notificationString, centerOfScreen, myColor, 0f, stringCenter, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
