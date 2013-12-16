using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using AssetLoader;

namespace DarkHavoc
{
    /// <summary>
    /// The main menu screen that the player can start access options and start the game (and quit. :P)
    /// </summary>
    internal class MainMenuScreen : GameScreen
    {
        // Our buttons.
        private MenuButton playGameButton;
        private MenuButton optionsHelpButton;
#if WINDOWS
        private MenuButton exitButton;
#endif

        // Center of screen
        private Vector2 centerOfScreen;

        public override void LoadContent()
        {
            // Set the mouse to be visible so the user can see the mouse to use the UI.
            ScreenManager.Game.IsMouseVisible = true;

            // Get the center of the screen (right under the Dark Havoc logo).
            centerOfScreen = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y + (Assets.darkHavocLogo.Height / 2) + 30);

            // Play Button initialization
            playGameButton = new MenuButton(centerOfScreen, "Play!");
            playGameButton.Selected += new EventHandler<EventArgs>(playGameButton_Selected);
            playGameButton.Position = new Vector2(centerOfScreen.X - (MenuButton.ButtonWidth * 2) - 25, centerOfScreen.Y);
            playGameButton.Initialize();

            // Options/Help button initialization
            optionsHelpButton = new MenuButton(centerOfScreen, "Options");
            optionsHelpButton.Selected += new EventHandler<EventArgs>(optionsHelpButton_Selected);
            optionsHelpButton.Position = new Vector2(centerOfScreen.X - (MenuButton.ButtonWidth / 2), centerOfScreen.Y);
            optionsHelpButton.Initialize();

#if WINDOWS
            // Exit button initialization
            exitButton = new MenuButton(centerOfScreen, "Exit Game");
            exitButton.Selected += new EventHandler<EventArgs>(exitButton_Selected);
            exitButton.Position = new Vector2(centerOfScreen.X + MenuButton.ButtonWidth + 25, centerOfScreen.Y);
            exitButton.Initialize();
#endif
        }

        void exitButton_Selected(object sender, EventArgs e)
        {
            // Exit game.
            ScreenManager.Game.Exit();
        }

        void optionsHelpButton_Selected(object sender, EventArgs e)
        {
            // Go to the options menu.
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }

        void playGameButton_Selected(object sender, EventArgs e)
        {
            // Go to the play game menu.
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new PlayGameMenuScreen());
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Update the buttons.
            playGameButton.Update(gameTime);
            optionsHelpButton.Update(gameTime);
            exitButton.Update(gameTime);
        }

        private void CheckButtonClick(InputState input, MenuButton button)
        {
            // If we clicked the mouse
            if (input.IsNewLeftMouseClick())
            {
                // We check the mouse to see if we're in the confines of the button and the button is enabled and we can use the mouse.
                if (input.LastMouseState.X > button.RectangleCollider.Left &&
                    input.LastMouseState.X < button.RectangleCollider.Right &&
                    input.LastMouseState.Y < button.RectangleCollider.Bottom &&
                    input.LastMouseState.Y > button.RectangleCollider.Top &&
                    button.IsEnabled)
                {
                    // Fire the button's selected event.
                    button.OnSelectEntry(PlayerIndex.One);
                }
            }
        }

        private void CheckButtonHover(InputState input, MenuButton button)
        {
            // We check the mouse to see if we're in the confines of the button and the button is enabled and we can use the mouse.
            if (input.LastMouseState.X > button.RectangleCollider.Left &&
                input.LastMouseState.X < button.RectangleCollider.Right &&
                input.LastMouseState.Y < button.RectangleCollider.Bottom &&
                input.LastMouseState.Y > button.RectangleCollider.Top &&
                button.IsEnabled)
            {
                // Set the hovering to true.
                button.IsHovering = true;
            }
            else
            {
                // Set the hovering to false.
                button.IsHovering = false;
            }
        }

        public override void HandleInput(InputState input)
        {
            // Left mouse click
            
            // Play Game Button
            CheckButtonClick(input, playGameButton);

            // Options/Help button
            CheckButtonClick(input, optionsHelpButton);

            // Exit button
            CheckButtonClick(input, exitButton);

            // Hovering

            // Play Game Button
            CheckButtonHover(input, playGameButton);
            
            // Options/Help button
            CheckButtonHover(input, optionsHelpButton);

            // Exit button.
            CheckButtonHover(input, exitButton);
        }

        public override void Draw(GameTime gameTime)
        {
            // Local spritebatch from our screenmanager.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // Begin.
            spriteBatch.Begin();

            // Draw the play button.
            playGameButton.Draw(spriteBatch);

            // Draw the options/help button
            optionsHelpButton.Draw(spriteBatch);

            // Draw the exit button
            exitButton.Draw(spriteBatch);

            // End.
            spriteBatch.End();
        }
    }
}
