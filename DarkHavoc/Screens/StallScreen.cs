using System;
using System.ComponentModel;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Animation;
using DarkHavoc.Engine.API;

namespace DarkHavoc
{
    /// <summary>
    /// This is used to stall, and/or make the game wait for something on a separate thread without blocking the main thread.
    /// </summary>
    internal class StallScreen : GameScreen
    {
        private ulong howLongToWaitInMilliseconds;
        private BackgroundWorker waitingBackgroundWorker;

        public StallScreen(ulong milliseconds)
        {
            howLongToWaitInMilliseconds = milliseconds;
            waitingBackgroundWorker = new BackgroundWorker();
        }
    }
}
