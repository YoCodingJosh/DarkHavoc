#region File Description
//-----------------------------------------------------------------------------
// ScreenManager.cs
//
// Dark Havoc
// Copyright (C) 2011 Joshua Kennedy.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using AssetLoader;
#endregion

namespace DarkHavoc.Engine
{
    /// <summary>
    /// The screen manager is a component which manages one or more GameScreen
    /// instances. It maintains a stack of screens, calls their Update and Draw
    /// methods at the appropriate times, and automatically routes input to the
    /// topmost active screen.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        InputState input = new InputState();

        SpriteBatch spriteBatch;

        bool isInitialized;

        bool traceEnabled;

        const float timeToShowVersionInfo = 2.75f;
        float versionInfoContainer;
        private bool shownAlready;

        #endregion

        #region Properties

        /// <summary>
        /// A default SpriteBatch shared by all the screens. This saves
        /// each screen having to bother creating their own local instance.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// If true, the manager prints out a list of all the screens
        /// each time it is updated. This can be useful for making sure
        /// everything is being added and removed at the right times.
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructs a new screen manager component.
        /// </summary>
        public ScreenManager(Game game)
            : base(game)
        {
            shownAlready = false;
            versionInfoContainer = 0.0f;
        }

        /// <summary>
        /// Initializes the screen manager component.
        /// </summary>
        public override void Initialize()
        {
			isInitialized = true;

            base.Initialize();
        }

        /// <summary>
        /// Load your graphics content.
        /// </summary>
        protected override void LoadContent()
        {
            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            //if (!this.GraphicsDevice.Adapter.IsProfileSupported(GraphicsProfile.HiDef))
            //{
            //    System.Windows.Forms.MessageBox.Show("Unfortunately, Dark Havoc doesn't support your computer's hardware configuration.");
            //    Environment.Exit(-1);
            //}

            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Tell each of the screens to load their content.
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unload your graphics content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Tell each of the screens to unload their content.
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows each screen to run logic.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (screensToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            if (!shownAlready)
            {
                versionInfoContainer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (versionInfoContainer > timeToShowVersionInfo)
                    shownAlready = true;
            }

            // Print debug trace?
            if (traceEnabled)
                TraceScreens();

            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        /// <summary>
        /// Prints a list of all the screens, for debugging.
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

            Debug.WriteLine(string.Join(", ", screenNames.ToArray()));
        }

        /// <summary>
        /// Tells each screen to draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }

            if (!shownAlready)
            {
                //SpriteBatch spriteBatch = this.SpriteBatch;

                //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap /* Must be set to Wrap */, null, null);
                //spriteBatch.DrawString(Assets.dialogFont, "Build Date: " + ScreenManager.RetrieveLinkerTimestamp().ToString(), new Vector2(GraphicsDevice.Viewport.TitleSafeArea.Left + 10, GraphicsDevice.Viewport.TitleSafeArea.Height - 45), Color.Gold);
                //spriteBatch.End();
            }
        }

        #endregion

        #region Public Methods

        public bool screenLoaded;

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screenLoaded = false;

            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);

            screenLoaded = true;
        }

        /// <summary>
        /// Adds a new screen to the screen manager.
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            screenLoaded = false;

            screen.ControllingPlayer = null;
            screen.ScreenManager = this;
            screen.IsExiting = false;

            // If we have a graphics device, tell the screen to load content.
            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);

            screenLoaded = true;
        }

        /// <summary>
        /// Removes a screen from the screen manager. You should normally
        /// use GameScreen.ExitScreen instead of calling this directly, so
        /// the screen can gradually transition off rather than just being
        /// instantly removed.
        /// </summary>
        public void RemoveScreen(GameScreen screen)
        {
            // If we have a graphics device, tell the screen to unload content.
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);

            // if there is a screen still in the manager, update TouchPanel
            // to respond to gestures that screen is interested in.
            if (screens.Count > 0)
            {
            }
        }

        /// <summary>
        /// Expose an array holding all the screens. We return a copy rather
        /// than the real master list, because screens should only ever be added
        /// or removed using the AddScreen and RemoveScreen methods.
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        /// <summary>
        /// Helper draws a translucent black fullscreen sprite, used for fading
        /// screens in and out, and for darkening the background behind popups.
        /// </summary>
        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin();

            spriteBatch.Draw(Assets.blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            spriteBatch.End();
        }

        //#if WINDOWS && NOSTEAM || WINDOWS && DEBUG
        //        public void TakeScreenshot(string prefix)
        //        {
        //            int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
        //            int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

        //            // Force a frame to be drawn (otherwise back buffer is empty) 
        //            Draw(new GameTime());

        //            // Pull the picture from the buffer 
        //            int[] backBuffer = new int[w * h];
        //            GraphicsDevice.GetBackBufferData(backBuffer);

        //            // Copy into a texture 
        //            Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat);
        //            texture.SetData(backBuffer);

        //            // Save to disk
        //            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Joshua Kennedy Games\\Dark Havoc\\Screenshots\\"))
        //            {
        //                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Joshua Kennedy Games\\Dark Havoc\\Screenshots\\");
        //            }

        //            Stream stream = File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Joshua Kennedy Games\\Dark Havoc\\Screenshots\\" + prefix + "_" + DateTime.Now.ToFileTime() + ".png");
        //            texture.SaveAsPng(stream, w, h);
        //            stream.Close();
        //        }
        //#endif

        #endregion
    }
}
