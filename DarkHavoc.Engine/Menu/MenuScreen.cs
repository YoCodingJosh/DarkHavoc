using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AssetLoader;
using DarkHavoc.Engine.API;

namespace DarkHavoc.Engine.Menu
{
    public abstract class MenuScreen : GameScreen
    {
        private List<MenuEntry> menuEntries = new List<MenuEntry>();
        private string menuTitle;
        private int selectedEntry;
        private const int spaceBetweenEntries = 5;
        private MouseState mouseState;
        private Vector2 mousePos;
        private bool useMouse;

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        public int SelectedEntry
        {
            get { return selectedEntry; }
            set { selectedEntry = value; }
        }

        public MenuScreen(string title)
        {
            menuTitle = title;

            selectedEntry = 0;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            mouseState = new MouseState();
            mousePos = new Vector2();
        }

        public override void LoadContent()
        {
            ScreenManager.Game.IsMouseVisible = true;

            useMouse = true;
        }

        public override void HandleInput(InputState input)
        {
            // previous entry
            if (input.IsMenuUp(ControllingPlayer))
            {
                useMouse = false;

                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;

                if (!menuEntries[selectedEntry].Enabled && selectedEntry >= 0)
                {
                    for (int i = selectedEntry; i >= 0; i--)
                    {
                        if (menuEntries[i].Enabled)
                        {
                            selectedEntry = i;
                            break;
                        }
                    }

                    return;
                }
            }

            // next entry
            if (input.IsMenuDown(ControllingPlayer))
            {
                useMouse = false;

                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;

                if (!menuEntries[selectedEntry].Enabled && selectedEntry < menuEntries.Count)
                {
                    for (int i = selectedEntry; i < menuEntries.Count - 1; i++)
                    {
                        if (menuEntries[i].Enabled)
                        {
                            selectedEntry = i;
                            break;
                        }
                    }
                    return;
                }
            }

            if (input.LastMouseState.X != mousePos.X || input.LastMouseState.Y != mousePos.Y)
            {
                useMouse = true;
            }

            for (int i = 0; i < menuEntries.Count; i++)
            {
                if (input.LastMouseState.Y > menuEntries[i].Position.Y - Assets.menuFont.LineSpacing / 2 &&
                    input.LastMouseState.Y < menuEntries[i].Position.Y + Assets.menuFont.LineSpacing / 2 &&
                    input.LastMouseState.X > menuEntries[i].Position.X &&
                    input.LastMouseState.X < menuEntries[i].Position.X + Assets.menuFont.MeasureString(menuEntries[i].Text).X && useMouse)
                {
                    selectedEntry = i;
                }
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                //if (OptionsFile.IsSound())
                //    Assets.menuSelectSound.Play();
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                //if (OptionsFile.IsSound())
                //    Assets.menuSelectSound.Play();
                OnCancel(playerIndex);
            }
            else if (input.IsNewLeftMouseClick())
            {
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    if (input.LastMouseState.Y > menuEntries[i].Position.Y &&
                        input.LastMouseState.Y < menuEntries[i].Position.Y + Assets.menuFont.LineSpacing)
                    {
                        //if (OptionsFile.IsSound())
                        //    Assets.menuSelectSound.Play();

                        OnSelectEntry(i, playerIndex);
                    }
                }
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

        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y + 32);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - menuEntry.GetWidth(this) / 2;

                // each entry is to be right justified
                //position.X = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Width - 50 - menuEntry.GetWidth(this);

                if (ScreenState == ScreenState.TransitionOn)
                    position.Y += transitionOffset * 128;
                else
                    position.Y -= transitionOffset * 256;
                    
                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this) + (spaceBetweenEntries * 2);
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            mouseState = Mouse.GetState();
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }

            //if (!useMouse)
            //    ScreenManager.Game.IsMouseVisible = false;
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = Assets.menuFont;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.TitleSafeArea.Width - 50 - Assets.menuFont.MeasureString(menuTitle).X, 250);
            //new Vector2(graphics.Viewport.Width / 2, 80); ^^
            Vector2 titleOrigin = Vector2.Zero;//font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(199, 21, 133) * TransitionAlpha; // 238, 230, 133
            float titleScale = 1.0f;

            if (ScreenState == ScreenState.TransitionOn)
                titlePosition.Y -= transitionOffset * 100;
            else
                titlePosition.Y += transitionOffset * 128;

            ShadowedString.DrawShadowedString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, spriteBatch);

            spriteBatch.End();
        }
    }
}
