using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using AssetLoader;

namespace DarkHavoc.Engine.Effects
{
	/// <summary>
	/// A Simple Snowfall effect.
	/// </summary>
	/// <remarks>
	/// Based on BlakPilar's code from http://www.omnimaga.org/index.php?topic=10341.0
	/// Thanks!! :)
	/// </remarks>
	public class SimpleSnowfall
	{
		private int w, h;

		private ScreenManager screenManagerInstance;
		private Point[] snow = new Point[511];
		private bool isSnowing = false;
		private Texture2D snowTexture;
		private Point quitPoint; // The point at which we need to recycle the snow particle.
		private Point startPoint; // The point where the snow particle gets recycled to.
		private JoshoRandom random;

		public int Width
		{
			get { return this.w; }
		}

		public int Height
		{
			get { return this.h; }
		}

		public SimpleSnowfall(ScreenManager scrmgr)
		{
			this.screenManagerInstance = scrmgr;
		}

		public void Initialize()
		{
			random = new JoshoRandom();

			this.snowTexture = Assets.snowflakeTexture;

			this.w = screenManagerInstance.Game.Window.ClientBounds.Width;
			this.h = screenManagerInstance.Game.Window.ClientBounds.Height;

			// Set the snow's quit point. It's the bottom of the screen plus the texture's height so it looks like the snow goes completely off screen.
			this.quitPoint = new Point(0, this.Height + this.snowTexture.Height);

			// Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing
			this.startPoint = new Point(0, 0 - this.snowTexture.Height);

			isSnowing = true;
		}

		/// <summary>
		/// Draw the falling snow.
		/// </summary>
		/// <param name="spriteBatch">SpriteBatch that has been initialized and began.</param>
		public void Draw(SpriteBatch spriteBatch)
		{
			// If it's not supposed to be snowing, then exit.
			if (!isSnowing)
				return;

			// This will be used as the index within our snow array.
			int i;

			// NOTE: The following conditional is not exactly the best "initializer."
			// If snow has not been initialized.
			if (this.snow[0] == new Point(0, 0))
			{
				// Make the random a new random
				this.random = new JoshoRandom();

				// For every snow particle within our snow array,
				for (i = 0; i < this.snow.Length; i++)
				{
					// Give it a new, random x and y. This will give the illusion that it was already snowing and won't cluster the particles
					this.snow[i] = new Point(random.NextInt(0, (this.Width - this.snowTexture.Width)), (random.NextInt(0, this.Height)));
				}
			}

			// Reinitialize the random number generator.
			this.random = new JoshoRandom();

			// Go back to the start.
			i = 0;

			// Begin displaying the snow
			foreach (Point snowPnt in this.snow)
			{
				// Get the exact rectangle for the snow particle
				Rectangle snowParticle = new Rectangle(snowPnt.X, snowPnt.Y, this.snowTexture.Width, this.snowTexture.Height);

				// Draw the snow particle (change white if you want any kind of tinting)
				spriteBatch.Draw(this.snowTexture, snowParticle, Color.White);

				//Make the current particle go down, but randomize it for a staggering snow
				this.snow[i].Y += random.NextInt(0, 5);

				// Make sure the point's location is not below the quit point's.
				if (this.snow[i].Y >= this.quitPoint.Y)
				{
					// If it is, give it a random X value, and the starting point variable's Y value.
					this.snow[i] = new Point((random.NextInt(0, (this.Width - this.snowTexture.Width))), this.startPoint.Y);
				}

				// Go to the next snowflake (or snow particle).
				i++;
			}
		}
	}

	/// <summary>
	/// Snowflake.
	/// </summary>
	internal class Snowflake
	{
		public Point Position;
		public float Rotation;
		public byte Direction;
		public float RotationSpeed;
		public float Scale;
		public byte Speed;
		public Vector2 Origin;
		private JoshoRandom random;

		public Snowflake(Point startingPosition)
		{
			this.Position = startingPosition;
			this.random = new JoshoRandom();

			this.Initialize();
		}

		public Snowflake(Point startingPosition, JoshoRandom rand)
		{
			this.Position = startingPosition;
			this.random = rand;

			this.Initialize();
		}

		public void Initialize()
		{
			this.Direction = (byte)(random.NextInt(-1, 1) * -1);
			this.Rotation = (float)random.NextDouble();
			this.RotationSpeed = (float)random.NextDouble();
			this.Origin = new Vector2(random.NextInt(0, 5), random.NextInt(0, 5));
			this.Scale = (float)(random.NextInt(0, 2) == random.NextInt(0, 2) ? random.NextDouble() : (random.NextInt(0, 2) == random.NextInt(0, 2) ? random.NextDouble() + 1 : random.NextDouble() + 3));

			if (this.Scale > 1.0f)
				this.Speed = 3;
			else if (this.Scale > 1.75f)
				this.Speed = 4;
			else if (this.Scale < 1.0f)
				this.Speed = 1;
			else
				this.Speed = 2;
		}
	}

	/// <summary>
	/// A little bit more complex Snowfall effect.
	/// </summary>
	/// <remarks>
	/// Based on BlakPilar's code from http://www.omnimaga.org/index.php?topic=10341.0
	/// Muchas gracias! :D
	/// </remarks>
	public class Snowfall
	{
		private int w, h;

		private ScreenManager screenManagerInstance;
		private Snowflake[] snow = new Snowflake[511];
		private bool isSnowing = false;
		private Texture2D snowTexture;
		private Point quitPoint; // The point at which we need to recycle the snow particle.
		private Point startPoint; // The point where the snow particle gets recycled to.
		private JoshoRandom random;

		public int Width
		{
			get { return this.w; }
		}

		public int Height
		{
			get { return this.h; }
		}

		public Snowfall(ScreenManager screenManager)
		{
			this.screenManagerInstance = screenManager;
		}

		public void Initialize()
		{
			random = new JoshoRandom();

			this.snowTexture = Assets.snowflakeTexture;

			//this.w = screenManagerInstance.Game.Window.ClientBounds.Width;
			//this.h = screenManagerInstance.Game.Window.ClientBounds.Height;
			this.w = screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Width;
			this.h = screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Height;

			// Set the snow's quit point. It's the bottom of the screen plus the texture's height so it looks like the snow goes completely off screen.
			this.quitPoint = new Point(0, this.Height + this.snowTexture.Height);

			// Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing.
			this.startPoint = new Point(0, 0 - this.snowTexture.Height);

			isSnowing = true;

			for (int i = 0; i < this.snow.Length; i++)
			{
				this.snow[i] = new Snowflake(this.startPoint);
				this.snow[i].Initialize();
			}
		}

		public void Update(GameTime gameTime)
		{
			if (!isSnowing)
				return;

			if (this.Width != screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Width || this.Height != screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Height)
			{
				this.w = screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Width;
				this.h = screenManagerInstance.GraphicsDevice.Viewport.TitleSafeArea.Height;

				// Set the snow's quit point. It's the bottom of the screen plus the texture's height so it looks like the snow goes completely off screen.
				this.quitPoint = new Point(0, this.Height + this.snowTexture.Height);

				// Set the snow's start point. It's the top of the screen minus the texture's height so it looks like it comes from somewhere, rather than appearing.
				this.startPoint = new Point(0, 0 - this.snowTexture.Height);

				this.Initialize();
			}

			for (int i = 0; i < this.snow.Length; i++)
			{
				Snowflake particle = this.snow[i];

				if (particle.Position == startPoint)
				{
					particle.Initialize(); // Reinitialize it so it won't be the same snowflake.
					particle.Position = new Point(random.NextInt(0, (this.Width - this.snowTexture.Width)), (random.NextInt(0, this.Height)));
				}

				float circle = MathHelper.Pi * 2; // To define an upper bound of 2*pi radians.

				float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

				elapsed *= particle.RotationSpeed;

				particle.Rotation += elapsed * circle;

				particle.Position.Y += particle.Speed;

				if (particle.Position.Y >= this.quitPoint.Y)
				{
					particle.Position = new Point(random.NextInt(0, this.Width), this.startPoint.Y);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!isSnowing)
				return;

			for (int i = 0; i < this.snow.Length; i++)
			{
				Snowflake particle = this.snow[i];

				Rectangle particleRect = new Rectangle(particle.Position.X, particle.Position.Y, this.snowTexture.Width, this.snowTexture.Height);

				spriteBatch.Draw(this.snowTexture, new Vector2(particle.Position.X, particle.Position.Y), null, Color.White, particle.Rotation, particle.Origin, particle.Scale, SpriteEffects.None, 0f);
			}
		}
	}
}
