using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DarkHavoc.Engine;
using AssetLoader;

namespace DarkHavoc.Engine.Effects
{
    /// <summary>
    /// A really cool starfield effect.
    /// </summary>
    public class Starfield
    {
        // Array of stars, so we don't have to keep dynamically allocating memory.
        Star[] stars = new Star[128];
        
        // Our random number generator. :)
        JoshoRandom rand = new JoshoRandom();

        // The star texture.
        Texture2D starTexture;

        // Reference to the ScreenManager.
        ScreenManager screenManager;

        /// <summary>
        /// Constructs a new starfield.
        /// </summary>
        /// <param name="texture">The star texture to use.</param>
        public Starfield(ScreenManager screenManager, Texture2D texture)
        {
            // Since we're not a GameScreen, we need to have the user give us our ScreenManager.
            this.screenManager = screenManager;

            // Set the texture to a user defined one.
            starTexture = texture;
        }

        /// <summary>
        /// Constructs a new starfield, but uses the built in star texture.
        /// </summary>
        public Starfield(ScreenManager screenManager)
        {
            // Since we're not a GameScreen, we need to have the user give us our ScreenManager.
            this.screenManager = screenManager;

            // We use the default star texture.
            starTexture = Assets.starTexture;
        }

        /// <summary>
        /// Initializes data.
        /// </summary>
        public void Initialize()
        {
            // Iterate through all the stars in the array.
            for (int i = 0; i < stars.Length; i++)
            {
                // Create a new star object.
                Star star = new Star();
                
                // Generate a random star color.
                star.Color = GenerateStarColor();

                // Random Position
                star.Position = new Vector2(rand.NextInt(screenManager.Game.Window.ClientBounds.Width), rand.NextInt(screenManager.Game.Window.ClientBounds.Height));
                star.Speed = (float)rand.NextDouble() * 5 + 2;
                stars[i] = star;
            }
        }

        /// <summary>
        /// Updates the starfield.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //int height = screenManager.Game.Window.ClientBounds.Height;
            //int left = screenManager.Game.Window.ClientBounds.Left;
            int left = 0;

            for (int i = 0; i < stars.Length; i++)
            {
                Star star = stars[i];
                if ((star.Position.X -= star.Speed) < left)
                {
                    // "generate" a new star
                    star.Position = new Vector2(screenManager.Game.Window.ClientBounds.Width, rand.NextInt(screenManager.Game.Window.ClientBounds.Height));
                    star.Speed = (float)rand.NextDouble() * 5 + 2;
                    star.Color = GenerateStarColor();
                }
            }
        }

        private Color GenerateStarColor()
        {
            return new Color(rand.NextInt(256), rand.NextInt(256), rand.NextInt(256), 128);
        }

        /// <summary>
        /// Draws the starfield.
        /// </summary>
        /// <remarks>
        /// Make sure you call SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
        /// </remarks>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Star star in stars)
            {
                spriteBatch.Draw(starTexture, star.Position, null, star.Color, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            }
        }
    }
}
