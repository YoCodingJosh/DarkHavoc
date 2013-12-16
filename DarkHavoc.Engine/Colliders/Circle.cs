using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DarkHavoc.Engine.Colliders
{
    /// <summary>
    /// Represents a 2D collision circle.
    /// </summary>
    public struct Circle
    {
        /// <summary>
        /// Center position of the circle.
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// Radius of the circle.
        /// </summary>
        public float Radius;

        /// <summary>
        /// Constructs a new circle.
        /// </summary>
        public Circle(Vector2 position, float radius)
        {
            Center = position;
            Radius = radius;
        }

        /// <summary>
        /// Determines if a circle intersects a rectangle.
        /// </summary>
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns>
        public bool Intersects(Rectangle rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = Center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }
    }
}
