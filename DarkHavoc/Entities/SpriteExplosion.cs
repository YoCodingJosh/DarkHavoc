using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Animation;
using DarkHavoc.Engine.DataReaders;
using AssetLoader;

namespace DarkHavoc
{
    internal class SpriteExplosion : JoshoEntity
    {
        SpriteStrip animation;

        public SpriteExplosion(Texture2D explosion, Vector2 position, SpriteStripMetadata metadata)
            : base(explosion, position)
        {
            animation = new SpriteStrip();

            animation.Initialize(explosion, position, metadata);
        }

        public SpriteExplosion(Texture2D explosion, Vector2 position, string metadataFile)
            : base(explosion, position)
        {
            animation = new SpriteStrip();

            animation.Initialize(explosion, position, new SpriteStripMetadata(metadataFile));
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Active)
            {
                animation.Update(gameTime);

                if (!animation.Active)
                {
                    this.Active = false;
                }

                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                animation.Draw(spriteBatch, this.TextureCenter);
        }
    }
}
