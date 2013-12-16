using System;
using System.Collections.Generic;
using System.Diagnostics;
using DarkHavoc.Engine;

namespace DarkHavoc
{
    static class Program
    {
        // Our game instance.
        static DarkHavocGame gameInstance;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Debug.WriteLine("[Dark Havoc] Starting up...");

            // Add application exit event hook.
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            // Create game instance.
            gameInstance = new DarkHavocGame();

            // Run game.
            gameInstance.Run();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Debug.WriteLine("[Dark Havoc] Exiting game...");
            JoshoEngine.DestroyEngine(gameInstance);
            Debug.WriteLine("[Dark Havoc] Bye!");
        }
    }
}

