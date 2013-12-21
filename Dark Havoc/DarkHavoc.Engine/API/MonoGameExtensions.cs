using System;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace DarkHavoc.Engine.API
{
    public static class MonoGameExtensions
    {
#if PC
        public static void SetPosition(this GameWindow window, Point position)
        {
            OpenTK.GameWindow OTKWindow = GetForm(window);
            if (OTKWindow != null)
            {
                OTKWindow.X = position.X;
                OTKWindow.Y = position.Y;
            }
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            Type type = typeof(OpenTKGameWindow);
            FieldInfo field = type.GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
                return field.GetValue(gameWindow) as OpenTK.GameWindow;
            return null;
        }

        public static void CenterWindow(this GameWindow window)
        {
            Point center = new Point();

            center.X = window.ClientBounds.Width / 2;
            center.Y = window.ClientBounds.Height / 2;

            SetPosition(window, center);
        }
#endif
    }
}
