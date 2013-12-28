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
using DarkHavoc.Engine.Effects;
using AssetLoader;

namespace DarkHavoc
{
    /// <summary>
    /// This is the screen that will be the underlay of the menu screen.
    /// </summary>
    class MenuBackgroundScreen : GameScreen
    {
		// For managing our Christmas theme.
		DateTime currentDate;
		public static bool IsChristmas;

        // Our starfield.
        Starfield starfield;

		// Our falling snow. (For Christmas Time!)
		Snowfall snowfall;

        public MenuBackgroundScreen()
        {
            // If we just started the game...
            if (!TitleScreen.passThrough)
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
            // Create new instance of Starfield with a handle to our ScreenManager.
            starfield = new Starfield(ScreenManager);

            // Initialize the starfield.
            starfield.Initialize();

			// Create the snowfall with ScreenManager handle being passed to it.
			snowfall = new Snowfall(ScreenManager);

			// Initialize it.
			snowfall.Initialize();

			currentDate = DateTime.Now;

			if (currentDate.Month == 12 && (currentDate.Day == 24 || currentDate.Day == 25))
			{
				IsChristmas = true;
			}
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
			if (IsChristmas)
			{
				// Update snowfall.
				snowfall.Update(gameTime);
			}
			else
			{
				// Update starfield.
				starfield.Update(gameTime);
			}
        }

        public override void Draw(GameTime gameTime)
        {
            // Store our SpriteBatch in a local variable.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Begin SpriteBatch.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            // Draw Starfield.
			if (!IsChristmas)
				starfield.Draw(spriteBatch, gameTime);
			else
				snowfall.Draw(spriteBatch);

            // End SpriteBatch.
            spriteBatch.End();
        }
    }
}
