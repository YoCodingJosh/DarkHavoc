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
    abstract internal class ButtonMenuScreen : GameScreen
    {
        // Contains all of the buttons.
        List<MenuButton> menuEntries;

        // The selected button entry.
        int selectedEntry = 0;

        // Used to see if the mouse is being used.
        bool useMouse;

        // The mouse's current position.
        Vector2 mousePosition;

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuButton> MenuEntries
        {
            get { return menuEntries; }
        }

        /// <summary>
        /// Gets or sets the currently selected menu entry.
        /// </summary>
        public int SelectedEntry
        {
            get { return selectedEntry; }
            set { selectedEntry = value; }
        }

        public ButtonMenuScreen()
        {
            // Instantiate new list that contains the buttons.
            menuEntries = new List<MenuButton>();

            // Initialize the vector that contains the mouse's position.
            mousePosition = new Vector2();

            // We're going to initially set this to true.
            useMouse = true;
        }

        public override void LoadContent()
        {
            // Goes through each button and initializes it.
            foreach (MenuButton button in menuEntries)
            {
                button.Initialize();
            }
        }
    }

    abstract internal class ButtonMenuScreenAlpha : GameScreen
    {
        List<MenuButton> menuEntries;
        int selectedEntry = 0;
        MouseState mouseState;
        Vector2 mousePosition;
        bool useMouse;
        string menuTitle;

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        protected IList<MenuButton> MenuEntries
        {
            get { return menuEntries; }
        }

        public int SelectedEntry
        {
            get { return selectedEntry; }
            set { selectedEntry = value; }
        }

        public ButtonMenuScreenAlpha()
        {
            Initialize();
        }

        public ButtonMenuScreenAlpha(string title)
        {
            menuTitle = title;

            Initialize();
        }

        private void Initialize()
        {
            menuEntries = new List<MenuButton>();

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            mouseState = new MouseState();
            mousePosition = new Vector2();

            useMouse = false;
        }

        public override void LoadContent()
        {
            ScreenManager.Game.IsMouseVisible = true;

            //UpdateEntryLocations();
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            mouseState = Mouse.GetState();

            mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (input.IsNewKeyPress(Keys.Left, ControllingPlayer, out playerIndex) && !useMouse)
            {
                MoveMenuLeft();
                useMouse = false;
            }
            else if (input.IsNewKeyPress(Keys.Right, ControllingPlayer, out playerIndex) && !useMouse)
            {
                MoveMenuRight();
                useMouse = false;
            }
            else if (input.IsMenuUp(ControllingPlayer) && !useMouse)
            {
                MoveMenuLeft();
            }
            else if (input.IsMenuDown(ControllingPlayer) && !useMouse)
            {
                MoveMenuRight();
            }
            else if (input.IsNewLeftMouseClick())
            {
                foreach (MenuButton button in MenuEntries)
                {
                    CheckButtonClick(input, button);
                }
            }
            else if (input.IsMenuSelect(ControllingPlayer, out playerIndex) && !useMouse)
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex) && !useMouse)
            {
                OnCancel(playerIndex);
            }

            for (int b = 0; b < menuEntries.Count; b++)
            {
                MenuButton button = menuEntries[b];

                button.IsHovering = false;

                CheckButtonHover(input, button);

                if (button.IsHovering)
                {
                    selectedEntry = b;
                }
            }

            // If the current mouse position doesn't equal the last, then our mouse is active!
            if (input.LastMouseState.X != mousePosition.X || input.LastMouseState.Y != mousePosition.Y)
            {
                useMouse = true;
            }

            if (!useMouse)
            {

            }
        }

        private void MoveMenuLeft()
        {
            // Play Sound
            //TODO

            // Decrement our selected entry.
            selectedEntry--;

            // If we're out of bounds, then move to the last entry.
            if (selectedEntry < 0)
                selectedEntry = menuEntries.Count - 1;

            menuEntries[selectedEntry].IsHovering = true;

            if (!menuEntries[selectedEntry].IsEnabled && selectedEntry >= 0)
            {
                for (int i = selectedEntry; i >= 0; i--)
                {
                    if (menuEntries[i].IsEnabled)
                    {
                        selectedEntry = i;
                        menuEntries[i].IsHovering = true;
                        break;
                    }
                }

                return;
            }
        }

        private void MoveMenuRight()
        {
            // Play Sound
            //TODO

            // Increment our selected entry.
            selectedEntry++;

            // If we're out of bounds, then move to the first entry.
            if (selectedEntry >= menuEntries.Count)
                selectedEntry = 0;

            menuEntries[selectedEntry].IsHovering = true;

            if (!menuEntries[selectedEntry].IsEnabled && selectedEntry < menuEntries.Count)
            {
                for (int i = selectedEntry; i < menuEntries.Count - 1; i++)
                {
                    if (menuEntries[i].IsEnabled)
                    {
                        selectedEntry = i;
                        menuEntries[i].IsHovering = true;
                        break;
                    }
                }

                return;
            }
        }

        private void CheckButtonClick(InputState input, MenuButton button)
        {
            // We check the mouse to see if we're in the confines of the button and the button is enabled and we can use the mouse.
            if (input.LastMouseState.X > button.RectangleCollider.Left &&
                input.LastMouseState.X < button.RectangleCollider.Right &&
                input.LastMouseState.Y < button.RectangleCollider.Bottom &&
                input.LastMouseState.Y > button.RectangleCollider.Top &&
                button.IsEnabled && useMouse)
            {
                // Fire the button's selected event.
                button.OnSelectEntry(PlayerIndex.One);
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
                useMouse = true;
            }
            else
            {
                // Set the hovering to false.
                button.IsHovering = false;
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }

        protected virtual void UpdateEntryLocations()
        {
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuButton entry = menuEntries[i];

                entry.Position.X = (ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X - (Assets.darkHavocLogo.Width / 2)) + (i * ((MenuButton.ButtonWidth * 1.5f) - 25));
                entry.Position.Y = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y + (Assets.darkHavocLogo.Height / 2) + 30;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            //UpdateEntryLocations();

            foreach (MenuButton entry in menuEntries)
            {
                entry.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            foreach (MenuButton entry in menuEntries)
            {
                entry.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
