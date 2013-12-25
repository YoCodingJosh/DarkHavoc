using System;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DarkHavoc
{
    internal class RatingDisplayScreen : GameScreen
    {
        private Texture2D ratingLogoTexture;
        //private SpriteFont headingFont;
        //private string headingText;
        //private Vector2 ratingPosition;
		//private Vector2 ratingCenterPosition;
        //private Vector2 headingPosition;
        private float currentTime;
        private float timeToStayOnScreen;

        public RatingDisplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.75);
            TransitionOffTime = TimeSpan.FromSeconds(0.75);

            timeToStayOnScreen = 1.75f;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

			ratingLogoTexture = content.Load<Texture2D>("./Images/esrb_rp");

            //headingFont = content.Load<SpriteFont>("./Fonts/ConsoleFont");

            //headingText = "THIS GAME" + Environment.NewLine + "HAS NOT YET BEEN RATED!";
            
			//ratingCenterPosition = Helper.CenterOnScreen(ScreenManager, ratingLogoTexture);
			//ratingCenterPosition = centerOfScreen;

            //ratingPosition = new Vector2(ratingCenterPosition.X - (ratingLogoTexture.Width / 2) - 30, ratingCenterPosition.Y);

            //headingPosition = new Vector2(ratingCenterPosition.X + 150, ratingCenterPosition.Y + (ratingLogoTexture.Height / 4));

			//base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (ScreenState == Engine.ScreenState.Active)
            {
                // Get the total elapsed time in seconds.
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                
                // increment the current time.
                currentTime += elapsed;

                // If we're stayed on the screen long enough.
                if (currentTime >= timeToStayOnScreen)
                {
                    // Quit this screen.
                    ExitScreen();
                }
            }
            else if (ScreenState == Engine.ScreenState.TransitionOff)
            {
                if (TransitionPosition == 1)
                {
                    ScreenManager.AddScreen(new FileAccessNotificationScreen());
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Color myColor = new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha);
			Vector2 centerOfScreen = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

			spriteBatch.Draw(ratingLogoTexture, centerOfScreen, null, myColor, 0f, new Vector2(ratingLogoTexture.Width / 2, ratingLogoTexture.Height / 2), 1.0f, SpriteEffects.None, 0f); // 8

            //spriteBatch.DrawString(headingFont, headingText, headingPosition, myColor);

            spriteBatch.End();
        }
    }
}
