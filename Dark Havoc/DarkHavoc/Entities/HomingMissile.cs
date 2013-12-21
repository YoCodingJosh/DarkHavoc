using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;

namespace DarkHavoc
{
    internal class HomingMissile : JoshoEntity
    {
        public enum State : ushort
        {
            Sleeping = 0,
            Seeking = 1,
            Exploding = 2,
            Dead = 3,
        };

        protected State MyState;

        public Vector2 Destination;
        private Vector2 currentVelocity;
        private Vector2 currentDirection;
        private Vector2 currentAcceleration;

        float maxSpeed;
        float maxAcceleration;

        public HomingMissile(Texture2D texture, Vector2 startingPosition)
            : base(texture, startingPosition)
        {
            //this.Origin.X = (startingPosition.X / 2) - (this.MyTexture.Width / 2);
            //this.Origin.Y = (startingPosition.Y / 2) - (this.MyTexture.Height / 2);

            maxSpeed = 4.20f;
            maxAcceleration = 1.0f;

            currentVelocity = Vector2.Zero;

            this.Active = true;
            MyState = State.Seeking;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Active && MyState == State.Seeking)
            {
                currentDirection = Destination - this.Position;

                Vector2 directionNormalized = currentDirection;
                directionNormalized.Normalize();

                currentAcceleration = directionNormalized * maxAcceleration;

                currentVelocity += currentAcceleration;

                if (currentVelocity.Magnitude() > maxSpeed)
                {
                    Vector2 velocityNormalized = currentVelocity;
                    velocityNormalized.Normalize();

                    currentVelocity = velocityNormalized * maxSpeed;
                }

                this.Position += currentVelocity;

                this.Rotation = (float)Math.Atan2(currentVelocity.Y, currentVelocity.X);

                if (Vector2.Distance(this.Position, Destination) <= (this.Width / 2) - 5)
                {
                    MyState = State.Exploding;
                }
            }
            else if (MyState == State.Exploding)
            {
                // something something update explosion.
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Active && MyState == State.Seeking)
            {
                SpriteEffects effect;
                effect = SpriteEffects.None;

                spriteBatch.Draw(this.MyTexture, this.Position, null, Color.White, this.Rotation, this.TextureCenter, 1.0f, effect, 1.0f);
            }
        }

        public void TurnTowards(Vector2 target, float turnSpeed)
        {
            Vector2 difference = target - this.Position;
            float objectRotation = (float)Math.Atan2(difference.Y, difference.X);
            float deltaRotation = MathHelper.WrapAngle(objectRotation - this.Rotation);
            this.Rotation += MathHelper.Clamp(deltaRotation, -turnSpeed, turnSpeed);
        }

        public void MoveTorward(float speed)
        {
            this.Position.X += (float)Math.Cos(this.Rotation) * speed;
            this.Position.Y += (float)Math.Sin(this.Rotation) * speed;
        }
    }
}
