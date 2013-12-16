using System;
using Microsoft.Xna.Framework;

namespace DarkHavoc.Engine.API
{
    public static class Vector2DExtensions
    {
        public static double Magnitude(this Vector2 vector)
        {
            return (Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y)));
        }

        public static void Rotate(this Vector2 vector, double angle)
        {
            double s = Math.Sin(angle);
            double c = Math.Cos(angle);

            double nx = (c * vector.X) - (s * vector.Y);
            double ny = (s * vector.X) + (c * vector.Y);

            vector.X = (float)nx;
            vector.Y = (float)ny;
        }
    }
}
