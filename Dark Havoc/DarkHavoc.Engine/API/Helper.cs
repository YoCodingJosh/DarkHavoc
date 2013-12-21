using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkHavoc.Engine.API
{
    public static class Helper
    {
        /// <summary>
        /// Helper calculates the destination rectangle needed
        /// to draw a sprite to one quarter of the screen.
        /// </summary>
        public static Rectangle QuarterOfScreen(ScreenManager screenManager, int x, int y)
        {
            Viewport viewport = screenManager.GraphicsDevice.Viewport;

            int w = viewport.Width / 2;
            int h = viewport.Height / 2;

            return new Rectangle(w * x, h * y, w, h);
        }


        /// <summary>
        /// Helper calculates the destination position needed
        /// to center a sprite in the middle of the screen.
        /// </summary>
        public static Vector2 CenterOnScreen(ScreenManager screenManager, Texture2D texture)
        {
            Viewport viewport = screenManager.GraphicsDevice.Viewport;

            int x = (viewport.TitleSafeArea.Width - texture.Width) / 2;
            int y = (viewport.TitleSafeArea.Height - texture.Height) / 2;

            return new Vector2(x, y);
        }


        /// <summary>
        /// Helper computes a value that oscillates over time.
        /// </summary>
        public static float Pulsate(GameTime gameTime, float speed, float min, float max)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            return min + ((float)Math.Sin(time) + 1) / 2 * (max - min);
        }


        /// <summary>
        /// Helper for moving a value around in a circle.
        /// </summary>
        public static Vector2 MoveInCircle(GameTime gameTime, float speed)
        {
            double time = gameTime.TotalGameTime.TotalSeconds * speed;

            float x = (float)Math.Cos(time);
            float y = (float)Math.Sin(time);

            return new Vector2(x, y);
        }


        /// <summary>
        /// Helper for moving a sprite around in a circle.
        /// </summary>
        public static Vector2 MoveInCircle(ScreenManager screenManager, GameTime gameTime, Texture2D texture, float speed)
        {
            Viewport viewport = screenManager.GraphicsDevice.Viewport;

            float x = (viewport.Width - texture.Width) / 2;
            float y = (viewport.Height - texture.Height) / 2;

            return MoveInCircle(gameTime, speed) * 128 + new Vector2(x, y);
        }

        /// <summary>
        /// Will draw a border (hollow rectangle) of the given 'thicknessOfBorder' (in pixels)
        /// of the specified color.
        ///
        /// By Sean Colombo, from http://bluelinegamestudios.com/blog
        /// </summary>
        /// <remarks>
        /// HOLY FUCKING SHIT! DO NOT USE THIS! IT CREATES A MEMORY LEAK THAT TRIPLES THE CLIENT APPLICATION MEMORY USAGE!
        /// </remarks>
        public static void DrawBorder(ScreenManager screenManager, SpriteBatch spriteBatch, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // At the top of your class:
            Texture2D pixel;

            // Somewhere in your LoadContent() method:
            pixel = new Texture2D(screenManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White }); // so that we can draw whatever color we want on top of it

            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        [Flags]
        public enum TextAlignment { Center = 0, Left = 1, Right = 2, Top = 4, Bottom = 8 };

        public static void DrawAlignedString(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle bounds, TextAlignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 pos = new Vector2(bounds.Center.X, bounds.Center.Y);
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(TextAlignment.Left))
                origin.X += bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(TextAlignment.Right))
                origin.X -= bounds.Width / 2 - size.X / 2;

            if (align.HasFlag(TextAlignment.Top))
                origin.Y += bounds.Height / 2 - size.Y / 2;

            if (align.HasFlag(TextAlignment.Bottom))
                origin.Y -= bounds.Height / 2 - size.Y / 2;

            spriteBatch.DrawString(font, text, pos, color, 0, origin, 1, SpriteEffects.None, 0);
        }

        /// <summary>Converts an <seealso cref="object"/>to a type<typeparamref name="T"/>.</summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>The given data converted to a type<seealso cref="T"/>.</returns>
        public static T ConvertToTypeT<T>(object obj)
            where T : IConvertible
        {
            var t = (T)Convert.ChangeType(obj, typeof(T));

            if (t != null)
            {
                return t;
            }

            return default(T);
        }

        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the angle that an object should face, given its position, its
        /// target's position, its current angle, and its maximum turning speed.
        /// </summary>
        public static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
        {
            // consider this diagram:
            //         C 
            //        /|
            //      /  |
            //    /    | y
            //  / o    |
            // S--------
            //     x
            // 
            // where S is the position of the spot light, C is the position of the cat,
            // and "o" is the angle that the spot light should be facing in order to 
            // point at the cat. we need to know what o is. using trig, we know that
            //      tan(theta)       = opposite / adjacent
            //      tan(o)           = y / x
            // if we take the arctan of both sides of this equation...
            //      arctan( tan(o) ) = arctan( y / x )
            //      o                = arctan( y / x )
            // so, we can use x and y to find o, our "desiredAngle."
            // x and y are just the differences in position between the two objects.
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;

            // we'll use the Atan2 function. Atan will calculates the arc tangent of 
            // y / x for us, and has the added benefit that it will use the signs of x
            // and y to determine what cartesian quadrant to put the result in.
            // http://msdn2.microsoft.com/en-us/library/system.math.atan2.aspx
            float desiredAngle = (float)Math.Atan2(y, x);

            // so now we know where we WANT to be facing, and where we ARE facing...
            // if we weren't constrained by turnSpeed, this would be easy: we'd just 
            // return desiredAngle.
            // instead, we have to calculate how much we WANT to turn, and then make
            // sure that's not more than turnSpeed.

            // first, figure out how much we want to turn, using WrapAngle to get our
            // result from -Pi to Pi ( -180 degrees to 180 degrees )
            float difference = WrapAngle(desiredAngle - currentAngle);

            // clamp that between -turnSpeed and turnSpeed.
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);

            // so, the closest we can get to our target is currentAngle + difference.
            // return that, using WrapAngle again.
            return WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Returns the angle expressed in radians between -Pi and Pi.
        /// </summary>
        public static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }
    }
}
