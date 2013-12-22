using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace DarkHavoc.Engine
{
    /// <summary>
    /// Handles the JoshoEngine as a whole.
    /// </summary>
    public static class JoshoEngine
    {
        private static ScreenManager myScreenManager;

		private static bool engineRunning = false;

        public static bool IsEngineRunning
        {
            get { return engineRunning; }
        }

        /// <summary>
        /// Creates a new instance of JoshoEngine.
        /// </summary>
        /// <param name="game">Game class to inject the JoshoEngine into.</param>
        /// <param name="initialScreen">The initial GameScreen to add.</param>
        public static void CreateEngine(Game game, GameScreen initialScreen)
        {
            if (!engineRunning)
            {
                Debug.WriteLine("[JoshoEngine] Injecting engine...");

                // Create the screen manager component.
                myScreenManager = new ScreenManager(game);

                // Add the screen manager to the component stack.
                game.Components.Add(myScreenManager);

                // We've started the engine!
                engineRunning = true;

                Debug.WriteLine("[JoshoEngine] Injected new engine instance.");

                // Add the initial screen to the ScreenManager
                myScreenManager.AddScreen(initialScreen);
            }
            else
                Debug.WriteLine("[JoshoEngine] Engine instance already created!");
        }

        /// <summary>
        /// Destroys the instance of JoshoEngine in the specified Game class.
        /// </summary>
        /// <param name="game">The game class to remove JoshoEngine from.</param>
        public static void DestroyEngine(Game game)
        {
            // Remove ScreenManager component from our game.
            if (game.Components.Remove(myScreenManager))
                Debug.WriteLine("[JoshoEngine] Destroyed engine instance.");
            else
                Debug.WriteLine("[JoshoEngine] Couldn't find engine instance to destroy!");

            // Stop other threads.
            engineRunning = false;
        }
    }
}
