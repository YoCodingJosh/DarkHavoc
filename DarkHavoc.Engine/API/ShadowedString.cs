using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkHavoc.Engine.API
{
    public static class ShadowedString
    {
        public static void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(2.0f, 2.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        public static void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(2.0f, 2.0f), Color.Black, rotation, origin, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, value, position, color, rotation, origin, scale, SpriteEffects.None, 0);
        }

        public static void DrawShadowedString(SpriteFont font, string value, Rectangle bounds, Helper.TextAlignment alignment, Color color, SpriteBatch spriteBatch)
        {
            Helper.DrawAlignedString(spriteBatch, font, value, new Rectangle(bounds.X + 2, bounds.Y + 2, bounds.Width - 2, bounds.Height - 2), alignment, Color.Black);
            Helper.DrawAlignedString(spriteBatch, font, value, bounds, alignment, color);
        }
    }
}
