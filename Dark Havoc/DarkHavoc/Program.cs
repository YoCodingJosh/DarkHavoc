using System;
using System.Collections.Generic;
using System.Diagnostics;
using DarkHavoc.Engine;
#if MONOMAC
using MonoMac;
using MonoMac.AppKit;
using MonoMac.Foundation;
#endif

namespace DarkHavoc
{
    static class Program
    {
        // Our game instance.
		public static DarkHavocGame gameInstance;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Debug.WriteLine("[Dark Havoc] Starting up...");

#if !MONOMAC
            // Add application exit event hook.
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            // Create game instance.
            gameInstance = new DarkHavocGame();

            // Run game.
            gameInstance.Run();

#else
			Debug.WriteLine("[Dark Havoc] Initializing Mac OS X Subsystem...");

			NSApplication.Init();

			using (var p = new NSAutoreleasePool()) 
			{
				NSApplication.SharedApplication.Delegate = new AppDelegate();
				NSApplication.Main(args);
			}
#endif
        }

#if !MONOMAC
		public static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Debug.WriteLine("[Dark Havoc] Exiting game...");
            JoshoEngine.DestroyEngine(gameInstance);
            Debug.WriteLine("[Dark Havoc] Bye!");
        }
#endif
    }

#if MONOMAC
	internal class AppDelegate : NSApplicationDelegate
	{
		public override void FinishedLaunching(NSObject notification)
		{
			Program.gameInstance = new DarkHavocGame();
			Program.gameInstance.Run();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
		{
			return true;
		}

		public override NSApplicationTerminateReply ApplicationShouldTerminate (NSApplication sender)
		{
			Debug.WriteLine("[Dark Havoc] Exiting game...");
			JoshoEngine.DestroyEngine(Program.gameInstance);
			Debug.WriteLine("[Dark Havoc] Passing NSApplicationTerminateReply.Now to the OS. Good bye, I'll miss you!");

			return NSApplicationTerminateReply.Now;
		}
	}
#endif
}

