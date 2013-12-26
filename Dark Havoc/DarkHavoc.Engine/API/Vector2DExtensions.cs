using System;
using Microsoft.Xna.Framework;

namespace DarkHavoc.Engine.API
{
	/// <summary>
	/// Very useful extensions to the 2D Vector Library.
	/// </summary>
    public static class Vector2DExtensions
    {
		/// <summary>
		/// Returns the Magnitude of this vector.
		/// </summary>
		/// <remarks>
		/// The result will need to be cast to a float in order to be usable.
		/// </remarks>
        public static double Magnitude(this Vector2 vector)
        {
            return (Math.Sqrt((vector.X * vector.X) + (vector.Y * vector.Y)));
        }

		/// <summary>
		/// Rotate this vector to the specified angle.
		/// </summary>
		/// <param name="angle">The angle to rotate to.</param>
		/// <remarks>
		/// There will be some loss of precision because internally there is casting from double to float.
		/// </remarks>
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
