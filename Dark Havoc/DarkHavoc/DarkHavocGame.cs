using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
#if WINDOWS
using System.ComponentModel;
#elif MONOMAC
using MonoMac;
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
#endif
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.DataReaders;

namespace DarkHavoc
{
    /// <summary>
    /// The amazingly awesome Dark Havoc! :3
    /// </summary>
    class DarkHavocGame : Game
    {
        // A public static handle to our game's graphics manager.
        public static GraphicsDeviceManager graphics;

        // Public static game options, so we can access it outside of the game object.
        public static Options GameOptions;

        public static Credits creditsFile;

		/// <summary>
		/// The location to store application data like settings, game saves, etc.
		/// </summary>
		/// <remarks>
		/// On Windows and Linux, this will default to the location local to the application.
		/// On Mac OS X, this will default to "~/Library/Application Support/Dark Havoc/"
		/// </remarks>
		public static string AppDataLocation;

#if WINDOWS
        public static Point GetCenterOfScreen(GraphicsDeviceManager gdm)
        {
            int boundsWidth = Screen.PrimaryScreen.Bounds.Width;
            int boundsHeight = Screen.PrimaryScreen.Bounds.Height;

            int x = boundsWidth - gdm.PreferredBackBufferWidth;
            int y = boundsHeight - gdm.PreferredBackBufferHeight;

            return new Point(x / 2, y / 2);
        }

        private static Form GetForm(IntPtr handle)
        {
            return ((handle == IntPtr.Zero) ? null : Control.FromHandle(handle) as Form);
        }

        // Hopefully, we can remove this when they fix the slow startup times. :)

        private BackgroundWorker windowCreationBackgroundWorker;

        private void windowCreationBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (GameOptions.IsFullscreen)
                Thread.Sleep(750);
            else
                Thread.Sleep(500);
        }

        private void windowCreationBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Debug.WriteLine("[Dark Havoc] I'm done waiting for MonoGame... I'm starting Dark Havoc now!");
            JoshoEngine.CreateEngine(this, new RatingDisplayScreen());
        }
#elif MONOMAC
		private void CenterWindow()
		{
			int index;
			int upperBound;
			float fScreenWidth, fScreenHeight, fNewX, fNewY, fWindowWidth, fWindowHeight, fTitleBarHeight;
			Screen[] screens = Screen.AllScreens;

			fScreenWidth = fScreenHeight = 0;

			upperBound = screens.GetUpperBound(0);
			for (index = 0; index <= upperBound; index++)
			{
				if (screens[index].Primary)
				{
					fScreenWidth = (float)screens[index].Bounds.Width;
					fScreenHeight = (float)screens[index].Bounds.Height;
					index = upperBound;
				}
			}

			fWindowWidth = graphics.PreferredBackBufferWidth;
			fWindowHeight = graphics.PreferredBackBufferHeight;

			fNewX = (fScreenWidth - fWindowWidth) / 2;
			fNewY = (fScreenHeight - fWindowHeight) / 2;

			fTitleBarHeight = this.Window.Window.Frame.Height - fWindowHeight;

			System.Drawing.PointF pfLocation = new System.Drawing.PointF(fNewX,fNewY);
			System.Drawing.PointF pfSize = new System.Drawing.PointF(fWindowWidth, fWindowHeight + fTitleBarHeight);
			System.Drawing.SizeF sfSize = new System.Drawing.SizeF(pfSize);
			System.Drawing.RectangleF rectTemp = new System.Drawing.RectangleF(pfLocation, sfSize);
			this.Window.Window.SetFrame(rectTemp, true);
		}
#endif

#if PC
		/*
		public static void ToggleFullScreen()
        {
            // 720p is best suited for us.
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            graphics.ToggleFullScreen();

            graphics.ApplyChanges();
        }
        */

		public static void ToggleFullScreen(bool toggle)
		{
			// 720p is best suited for us.
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;

			graphics.IsFullScreen = toggle;

#if MONOMAC
			Program.gameInstance.CenterWindow();
#elif WINDOWS
			this.Window.SetPosition(GetCenterOfScreen(graphics));
#endif

			graphics.ApplyChanges();
		}
#endif

        /// <summary>
        /// Instantiates new object of DarkHavocGame.
        /// </summary>
        public DarkHavocGame()
        {
#if MONOMAC
			// Set the Window Background Color to Black, since it defaults to gray.
			Window.Window.BackgroundColor = NSColor.Black;
#endif
            // Create new Options in case it has been deleted or we're starting for the first time.
            GameOptions = new Options();

			Debug.WriteLine("[Dark Havoc] Running on " + GameOptions.UserOS);
#if MONOMAC
			NSDictionary systemVersionDictionary = new NSDictionary(@"/System/Library/CoreServices/SystemVersion.plist");
			NSString versionString = (NSString)systemVersionDictionary.ObjectForKey((NSString)@"ProductVersion");

			Debug.WriteLine("[Dark Havoc] Running on Mac OS X Version: " + versionString.ToString());
#endif
			string location = "";

#if !MONOMAC
			location = "./";
#else
			location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Application Support/Dark Havoc/");
			if (!Directory.Exists(location))
				Directory.CreateDirectory(location);
#endif

			AppDataLocation = location;

            // Does the options file exist?
			if (File.Exists(location + "Settings.josho"))
				Options.DeserializeToObject(location, out GameOptions); // If so then deserialize it!
            else
            {
#if WINDOWS
				MessageBox.Show("Thanks for playing Dark Havoc!\n\nIf you run into any issues, please don't hesitate to contact me!", "Dark Havoc - First Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
#elif MONOMAC
				NSAlert dialogue = new NSAlert();
				dialogue.InformativeText = "Thanks for playing Dark Havoc!\n\nIf you run into any issues, please don't hesitate to contact me!";
				dialogue.MessageText = "First Run";
				dialogue.RunModal();
#endif
				Options.SerializeToFile(location, GameOptions); // Since we instantiated a default Options, just recreate it.
            }

            // Set the window title to Dark Havoc! :)
			this.Window.Title = "Dark Havoc";

            // Set the content root directory to the Resources folder.
#if WINDOWS
            Content.RootDirectory = "Resources";
#elif MONOMAC
			Content.RootDirectory = "Content";
#endif

            // Create new instance of GraphicsDeviceManager with the pointer to this game class.
            graphics = new GraphicsDeviceManager(this);

            // Hide the mouse until we need it.
            this.IsMouseVisible = false;

#if WINDOWS
            // This is where we handle MonoGame WinGL being slow at creating the window.
            windowCreationBackgroundWorker = new BackgroundWorker();
            windowCreationBackgroundWorker.DoWork += new DoWorkEventHandler(windowCreationBackgroundWorker_DoWork);
            windowCreationBackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(windowCreationBackgroundWorker_RunWorkerCompleted);
#endif
        }

#if !WINDOWS
		protected override void Initialize ()
		{
			base.Initialize();
		}
#endif

        protected override void LoadContent()
        {
            // Make the MediaPlayer repeat itself.
            MediaPlayer.IsRepeating = true;

            // 720p is best suited for us.
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            // Sync with the vertical trace.
            graphics.SynchronizeWithVerticalRetrace = true;

            // Enable multi-sampled back buffer.
            graphics.PreferMultiSampling = true;

			//graphics.ApplyChanges();

#if PC
            Console.WriteLine("Fullscreen is " + GameOptions.IsFullscreen);
			ToggleFullScreen(GameOptions.IsFullscreen);
#endif

#if WINDOWS
			// On Linux, it might get automatically centered.
            // On PS4, it'll just be full screen. (duh)
            this.Window.SetPosition(GetCenterOfScreen(graphics));
#elif MONOMAC
			// On Mac, it doesn't automatically center.
			CenterWindow();
#endif

            // Create new engine instance of the JoshoEngine and load up the first screen.
#if WINDOWS
            // MonoGame Win32 GL creates the window slowly, but still updates, so we'll handle it here...
            Debug.WriteLine("[Dark Havoc] MonoGame WinGL is slow starting up... :(");
            windowCreationBackgroundWorker.RunWorkerAsync();
#else
			JoshoEngine.CreateEngine(this, new RatingDisplayScreen());
#endif
        }

        protected override void UnloadContent()
        {
            // code here will only get called when the user exits the game using the in-game exit option.
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen to black.
            graphics.GraphicsDevice.Clear(Color.Black);

            // Draw the entire stack of components (our game!)
            base.Draw(gameTime);
        }
    }
}
