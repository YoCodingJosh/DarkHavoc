using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Animation;
using DarkHavoc.Engine.DataReaders;
using AssetLoader;

namespace DarkHavoc
{
    /// <summary>
    /// The GameScreen that will launch the AssetLoader's method that will load the assets.
    /// </summary>
    internal class AssetLoadScreen : GameScreen
    {
        // A separate thread that will call the AssetLoader::Assets::StartCache() method asynchrously.
        private BackgroundWorker bgWorker;

        // Used to see if we're done loading.
        private bool loadingComplete;

        // Used on the main thread to see if the separate thread is still running.
        private bool isLoading;

        // The font used to display the loading text.
        SpriteFont loadingFont;

        // The first color we're going to change to.
        Color color1;

        // The second color we're going to change to.
        Color color2;

        // Our current color.
        Color myColor;

        // Our fade value to lerp.
        float fadeValue = 0.0f;

        // Our awesome random number generator (CSPRNG).
        JoshoRandom random;

        // Our loading text.
        string loadingText;

        // Our variables that keep track of the time, so we know when to show the blinking underscore.
        private TimeSpan blinkTime;
        private TimeSpan previousBlinkTime;
        private bool isBlink;

        public AssetLoadScreen()
        {
            // Initialize random number generator.
            random = new JoshoRandom();

            // Set the blink time so we know when to blink.
            blinkTime = TimeSpan.FromSeconds(0.1);

            // Let's load.
            loadingComplete = false;
            isLoading = false;

            // Set the first color to white.
            color1 = Color.White;
            myColor = Color.White;

            // Set the second color to a random color.
            color2 = GenerateRandomColor();

            // This BackgroundWorker will be responsible for loading the assets and doing other initialization steps on a separate thread.
            bgWorker = new BackgroundWorker();

            // Add event handler to the method that's going to do work!
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);

            // Add event handler to when the thread is done working.
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
        }

        // This is the stuff that gets executed on a separate thread.
        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // In case the user has a fast computer, the asset load screen won't just be a flicker.
            Thread.Sleep(1000);

			// Our magic method. This loads the assets into memsory.
            Assets.StartCache(ScreenManager.Game.Content);

            // Load and Parse our credits file.
#if !MONOMAC
            DarkHavocGame.creditsFile = new Credits("./Resources/Data/CREDITS.JXD");
#else
			DarkHavocGame.creditsFile = new Credits("./Content/Data/CREDITS.JXD");
#endif

            // Initialize our constants.
            GlobalConstants.Initialize(this.ScreenManager);

            // For equality on both sides of the initialization.
            Thread.Sleep(1000);
        }

        // This is the stuff that gets executed when the thread is done.
        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Set these values so we don't have thread errors.
            loadingComplete = true;
            isLoading = false;

            // Remove me from the ScreenManager stack.
            ScreenManager.RemoveScreen(this);

            // And display the SplashScreen.
            ScreenManager.AddScreen(new SplashScreen(), this.ControllingPlayer);
        }

        public override void LoadContent()
        {
            // Locally declare our contentmanager with the value of our game's contentmanager.
            ContentManager content = ScreenManager.Game.Content;

            // Loading font.
            loadingFont = content.Load<SpriteFont>("./Fonts/ConsoleFont");

            // Start with it blinking or not.
            switch (random.NextInt(0, 1))
            {
                case 0:
                    isBlink = true;
                    break;
                case 1:
                    isBlink = false;
                    break;
            }

            // Random loading text. Let's make it interesting!
            switch (random.NextInt(0, 15))
            {
                case 0:
                    loadingText = "Loading... ";
                    break;
                case 1:
                    loadingText = "Initializing... ";
                    break;
                case 2:
                    loadingText = "Starting up... ";
                    break;
                case 3:
                    loadingText = "Doing something... ";
                    break;
                case 4:
                    loadingText = "Please wait... ";
                    break;
                case 5:
                    loadingText = "Preparing to lunch... ";
                    break;
                case 6:
                    loadingText = "Hacking into secret things... ";
                    break;
                case 7:
                    loadingText = "Getting a papercut... ";
                    break;
                case 8:
                    loadingText = "Eating some files... ";
                    break;
                case 9:
                    loadingText = "Performing magic... ";
                    break;
                case 10:
                    loadingText = "Browsing reddit... ";
                    break;
                case 11:
                    loadingText = "Eating some flies... ";
                    break;
                case 12:
                    loadingText = "Generating compiler errors... ";
                    break;
                case 13:
                    loadingText = "Taking a break... ";
                    break;
                case 14:
                    loadingText = "Going bowling with Roman... ";
                    break;
            }
        }

        public override void UnloadContent()
        {
            GC.Collect();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // If we haven't loaded the game and we're not currently loading the game...
            if (!loadingComplete && !isLoading)
            {
                // ...then load the game!!
                bgWorker.RunWorkerAsync();

                // Let's not have multiple BackgroundWorkers going. :)
                isLoading = true;
            }

            // If we're loading and not done loading.
            if (isLoading && !loadingComplete)
            {
                // Increase our fadeValue.
                fadeValue += 0.15f;

                // Gradually change the color from 1 to 2 by fadeValue percent each frame.
                myColor = Color.Lerp(color1, color2, fadeValue);

                // If the current color is equal to the next color (we transitioned)
                if (fadeValue >= 1.0f)
                {
                    // Swap the colors.
                    color1 = color2;

                    // and generate a new color.
                    color2 = GenerateRandomColorWithoutAlpha();

                    // and reset.
                    fadeValue = 0.0f;
                }

                // If our total game time minus the last time we blinked is greater than the designated blinkTime, then blink!
                if (gameTime.TotalGameTime - previousBlinkTime > blinkTime)
                {
                    // Set the previous game time to the current gametime. This essentially resets the timer.
                    previousBlinkTime = gameTime.TotalGameTime;

                    // If we're blinking, then set the text to no underscore, and set the isblink variable to false.
                    if (isBlink)
                    {
                        loadingText = loadingText.Remove(loadingText.Length - 1, 1) + " ";
                        isBlink = false;
                    }
                    else
                    {
                        // Let's add an underscore, and set the isBlink variable to true.
                        loadingText = loadingText.Remove(loadingText.Length - 1, 1) + "_";
                        isBlink = true;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // Clear to black.
            ScreenManager.GraphicsDevice.Clear(Color.Black);

            // Only draw the loading stuff when it's actually loading!
            if (isLoading && !loadingComplete)
            {
                // Assign spritebatch to our screenmanager's spritebatch.
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

                // Let's begin, shall we?
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                // Draw our loading text with a random color.
                spriteBatch.DrawString(loadingFont, loadingText, new Vector2(10, (ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Bottom - loadingFont.LineSpacing) - 10), myColor);

                // Conclude our drawing.
                spriteBatch.End();
            }
        }

        // Generate a color with random R, G, B, and A values.
        private Color GenerateRandomColor()
        {
            return new Color(random.NextInt(0, 256), random.NextInt(0, 256), random.NextInt(0, 256), random.NextInt(0, 256));
        }

        // Generate a color with random R, G, B values.
        private Color GenerateRandomColorWithoutAlpha()
        {
            return new Color(random.NextInt(0, 256), random.NextInt(0, 256), random.NextInt(0, 256));
        }
    }
}
