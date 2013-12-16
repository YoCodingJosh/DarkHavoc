using System;
using System.Security.Cryptography;

namespace DarkHavoc.Engine
{
    /// <summary>
    /// A better random number generator.
    /// </summary>
    /// <remarks>
    /// This class can not be inherited.
    /// </remarks>
    public sealed class JoshoRandom : RandomNumberGenerator
    {
        private static RandomNumberGenerator random;

        /// <summary>
        /// Creates a new instance of JoshoRandom.
        /// </summary>
        public JoshoRandom()
        {
            random = RandomNumberGenerator.Create();
        }

        /// <summary>
        /// Fills a byte array with random numbers.
        /// </summary>
        /// <param name="data">The byte array to fill with random numbers.</param>
        public override void GetBytes(byte[] data)
        {
            random.GetBytes(data);
        }

        /// <summary>
        /// Fills a byte array with random numbers that aren't zero.
        /// </summary>
        /// <param name="data">The byte array to fill with random numbers.</param>
        public override void GetNonZeroBytes(byte[] data)
        {
            random.GetNonZeroBytes(data);
        }

        /// <summary>
        /// Generates a random double precision floating point number.
        /// </summary>
        /// <returns>A value between 0 and 1.</returns>
        public double NextDouble()
        {
            byte[] myBytes = new byte[4];

            random.GetBytes(myBytes);

            return (double)BitConverter.ToUInt32(myBytes, 0) / UInt32.MaxValue;
        }

        /// <summary>
        /// Generates a random integer between 2 values.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>A value between min and max.</returns>
        public int NextInt(int min, int max)
        {
            return (int)(NextDouble() * (max - min - 1) + min);
        }

        /// <summary>
        /// Generates a positve random integer between 0 and max - 1.
        /// </summary>
        /// <param name="max">The max value.</param>
        /// <returns></returns>
        public int NextInt(int max)
        {
            return (int)(NextDouble() * (max - 1));
        }

        /// <summary>
        /// Generates a random integer.
        /// </summary>
        /// <returns>A value between 0 and the maximum 32-bit integer value.</returns>
        public int NextInt()
        {
            return NextInt(0, Int32.MaxValue);
        }
    }
}
