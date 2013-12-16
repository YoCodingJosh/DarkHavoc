using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DarkHavoc.Engine.DataReaders;

namespace DarkHavoc.Engine.Animation
{
    public class SpriteStrip
    {
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;

        // The scale used to display the sprite strip
        float scale;

        // The time since we last updated the frame
        int elapsedTime;

        // The time we display a frame until the next one
        int frameTime;

        // The number of frames that the animation contains
        int frameCount;

        // The index of the current frame we are displaying
        int currentFrame;

        // The color of the frame we will be displaying
        Color color;

        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();

        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();

        // Width of a given frame
        public int FrameWidth;

        // Height of a given frame
        public int FrameHeight;

        // The state of the Animation
        public bool Active;

        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;

        // Width of a given frame
        public Vector2 Position;

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping)
        {
            // Keep a local copy of the values passed in
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = 0;

            // Set the Animation to active by default
            Active = true;
        }

        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping, int startingFrame)
        {
            // Keep a local copy of the values passed in
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            // Set the time to zero
            elapsedTime = 0;
            currentFrame = startingFrame;

            // Set the Animation to active by default
            Active = true;
        }

        public void Initialize(Texture2D texture, Vector2 position, SpriteStripMetadata metadata)
        {
            this.spriteStrip = texture;
            this.scale = metadata.Scale;
            this.frameCount = metadata.FrameCount;
            this.frameTime = metadata.FrameSpeed;
            this.FrameWidth = metadata.FrameWidth;
            this.FrameHeight = metadata.FrameHeight;
            this.Looping = metadata.Loop;
            this.color = metadata.TintColor;

            this.Position = position;

            this.elapsedTime = 0;
            this.currentFrame = metadata.StartingFrame;

            this.Active = true;
        }

        public void Update(GameTime gameTime)
        {
            // Do not update the game if we are not active
            if (Active == false)
                return;

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;

                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }

                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
            (int)Position.Y - (int)(FrameHeight * scale) / 2,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));
        }

        // Draw the Animation Strip
        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
            }
        }

        // Draw the Animation Strip with a defined alpha
        public void Draw(SpriteBatch spriteBatch, byte alpha)
        {
            // Only draw the animation when we are active
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, new Color(color.R, color.G, color.B, alpha));
            }
        }

        // Draw the animation strip with a custom color
        public void Draw(SpriteBatch spriteBatch, Color myColor)
        {
            // Only draw when we're active.
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, myColor);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 origin)
        {
            if (Active)
            {
                spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color, 0f, origin, SpriteEffects.None, 0f);
            }
        }
    }
}
