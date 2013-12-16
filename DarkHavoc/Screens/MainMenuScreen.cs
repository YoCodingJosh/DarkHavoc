using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using DarkHavoc.Engine;
using DarkHavoc.Engine.Menu;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.Screens;
using Microsoft.Xna.Framework;

namespace DarkHavoc
{
    internal class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base(string.Empty)
        {
            MenuEntry play = new MenuEntry("Play!");
            MenuEntry options = new MenuEntry("Options");
            MenuEntry help = new MenuEntry("Help");
            MenuEntry credits = new MenuEntry("Credits");
#if PC
            MenuEntry exit = new MenuEntry("Exit");
#endif

            play.Selected += new EventHandler<PlayerIndexEventArgs>(play_Selected);
            options.Selected += new EventHandler<PlayerIndexEventArgs>(options_Selected);
            help.Selected += new EventHandler<PlayerIndexEventArgs>(help_Selected);
            credits.Selected += new EventHandler<PlayerIndexEventArgs>(credits_Selected);
#if PC
            exit.Selected += new EventHandler<PlayerIndexEventArgs>(exit_Selected);
#endif

            MenuEntries.Add(play);
            MenuEntries.Add(options);
            MenuEntries.Add(help);
            MenuEntries.Add(credits);
#if PC
            MenuEntries.Add(exit);
#endif
        }

#if PC
        void exit_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
#endif

        void credits_Selected(object sender, PlayerIndexEventArgs e)
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

            // Stop Menu Music
            //JoshoEngine.MusicPlayer.Stop();

            // Add new instance of the CreditsScreen
            CreditsScreen credits = new CreditsScreen(DarkHavocGame.creditsFile, "./Resources/Music/betrayer_-_one.xm");
            credits.CreditsEnd += new EventHandler<PlayerIndexEventArgs>(credits_CreditsEnd);
            LoadingScreen.Load(ScreenManager, false, null, credits);
        }

        void credits_CreditsEnd(object sender, PlayerIndexEventArgs e)
        {
            Debug.WriteLine("[Dark Havoc] Credits finished.");

            foreach (GameScreen screen in ScreenManager.GetScreens())
            {
                ScreenManager.RemoveScreen(screen);
            }

            ScreenManager.Game.IsMouseVisible = true;

            LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new MenuBackgroundScreen(), new TitleScreen(), new MainMenuScreen());

            //JoshoEngine.MusicPlayer.Play("./Resources/Music/kool-gro.xm");
        }

        void help_Selected(object sender, PlayerIndexEventArgs e)
        {
            throw new NotImplementedException();
        }

        void options_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void play_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new PlayGameMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
#if PC
            ScreenManager.Game.Exit();
#endif
        }

    }
}
