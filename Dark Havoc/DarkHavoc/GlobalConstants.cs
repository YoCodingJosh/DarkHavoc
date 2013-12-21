using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;

namespace DarkHavoc
{
    internal static class GlobalConstants
    {
        public static Rectangle titleSafeViewport;

        public static void Initialize(ScreenManager screenManager)
        {
            titleSafeViewport = screenManager.GraphicsDevice.Viewport.TitleSafeArea;
        }
    }
}
