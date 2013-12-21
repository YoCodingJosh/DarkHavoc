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
using DarkHavoc.Engine.Menu;
using AssetLoader;

namespace DarkHavoc
{
    /// <summary>
    /// The menu where the player decides what gameplay mode to play.
    /// </summary>
    internal class PlayGameMenuScreen : MenuScreen
    {
        /// <summary>
        /// Creates new PlayGameMenuScreen
        /// </summary>
        public PlayGameMenuScreen()
            : base("Play Game")
        {
            MenuEntry campaign = new MenuEntry("Campaign");
            MenuEntry arcade = new MenuEntry("Arcade Mode");
            MenuEntry practice = new MenuEntry("Training");
            MenuEntry back = new MenuEntry("Back");

            campaign.Selected += new EventHandler<PlayerIndexEventArgs>(campaign_Selected);
            arcade.Selected += new EventHandler<PlayerIndexEventArgs>(arcade_Selected);
            practice.Selected += new EventHandler<PlayerIndexEventArgs>(practice_Selected);
            back.Selected += OnCancel;

            MenuEntries.Add(campaign);
            MenuEntries.Add(arcade);
            MenuEntries.Add(practice);
            MenuEntries.Add(back);
        }

        void practice_Selected(object sender, PlayerIndexEventArgs e)
        {
            // Remove the previous screens.
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                // We want to fade out of the other screens, but instantly remove the background screen.
                if (screen is MenuBackgroundScreen)
                {
                    ScreenManager.RemoveScreen(screen);
                    break;
                }
            }

            // Make the mouse invisible.
            ScreenManager.Game.IsMouseVisible = false;

            // Add new instance of the ArcadeMode.
            LoadingScreen.Load(ScreenManager, false, null, new PracticeMode());
        }

        void arcade_Selected(object sender, PlayerIndexEventArgs e)
        {
            // Remove the previous screens.
            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                // We want to fade out of the other screens, but instantly remove the background screen.
                if (screen is MenuBackgroundScreen)
                {
                    ScreenManager.RemoveScreen(screen);
                    break;
                }
            }

            // Make the mouse invisible.
            ScreenManager.Game.IsMouseVisible = false;

            // Add new instance of the ArcadeMode.
            LoadingScreen.Load(ScreenManager, false, null, new ArcadeMode());
        }

        void campaign_Selected(object sender, PlayerIndexEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
