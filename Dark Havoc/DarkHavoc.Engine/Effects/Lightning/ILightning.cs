using System;
using Microsoft.Xna.Framework.Graphics;

namespace DarkHavoc.Engine.Effects.Lightning
{
    // A common interface for LightningBolt and BranchLightning
    public interface ILightning
    {
        bool IsComplete { get; }

        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
