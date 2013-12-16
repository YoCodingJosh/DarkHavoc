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

namespace DarkHavoc.Engine
{
    /// <summary>
    /// Base class for all on screen entities.
    /// </summary>
    public abstract class JoshoEntity
    {
        // Geometric variables.
        public int Width;
        public int Height;
        public float Rotation;
        public float Scale;
        public Vector2 Position;
        public Vector2 Origin;
        public Vector2 TextureCenter;

        // Used for collision.
        public Rectangle RectangleCollider;

        // The texture of the entity.
        public Texture2D MyTexture;

        // 
        public bool Active;

        /// <summary>
        /// Returns a array of pixels containing the pixel data of the texture.
        /// </summary>
        public Color[] ColorData;

        public JoshoEntity(Texture2D texture, float xPos, float yPos)
        {
            MyTexture = texture;
            Position = new Vector2(xPos, yPos);

            Initialize();
        }

        public JoshoEntity(Texture2D texture, Vector2 position)
        {
            MyTexture = texture;
            Position = position;

            Initialize();
        }

        private void Initialize()
        {
            Scale = 1.0f;
            Rotation = 0.0f;
            Origin = Vector2.Zero;

            Width = MyTexture.Width;
            Height = MyTexture.Height;

            GetColorData(out ColorData, MyTexture);

            TextureCenter = new Vector2(Width / 2, Height / 2);

            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Active)
            {
                RectangleCollider = new Rectangle((int)Origin.X, (int)Origin.Y, Width, Height);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                spriteBatch.Draw(MyTexture, Position, null, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 0f);
        }

        public static void GetColorData(out Color[] colorData, Texture2D texture)
        {
            colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
        }

        public bool HitTest(JoshoEntity otherEntity)
        {
            return HitTest(this, otherEntity);
        }

        public bool HitTestPrecise(JoshoEntity otherEntity)
        {
            return HitTestPrecise(this, otherEntity);
        }

        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                           Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        public static bool HitTest(JoshoEntity entity1, JoshoEntity entity2)
        {
            return entity1.RectangleCollider.Intersects(entity2.RectangleCollider);
        }

        public static bool HitTestPrecise(JoshoEntity entity1, JoshoEntity entity2)
        {
            return IntersectPixels(entity1.RectangleCollider, entity1.ColorData, entity2.RectangleCollider, entity2.ColorData);
        }
    }
}
