using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.Menu;

namespace DarkHavoc
{
    internal class PauseMenuScreen : MenuScreen
    {
        MenuEntry resume;
        MenuEntry loadGame;
        MenuEntry saveGame;
        MenuEntry options;
        MenuEntry quitToMenu;
        MenuEntry quitToOS;

        GameType myGameType;

        public PauseMenuScreen(GameType gameType)
            : base ("Paused")
        {
            myGameType = gameType;

            // Instantiate menu entries.
            resume = new MenuEntry("Resume Game");

            if (gameType == GameType.Campaign)
            {
                loadGame = new MenuEntry("Load Game");
                saveGame = new MenuEntry("Save Game");
            }

            options = new MenuEntry("Options");
            quitToMenu = new MenuEntry("Quit to Main Menu");
#if PC
			quitToOS = new MenuEntry("Exit Game");
#endif

            // Attach event hooks to entry selected handler.
            resume.Selected += OnCancel;

			if (myGameType == GameType.Campaign)
            {
				// implement load and save game menu entries. :)
            }

            options.Selected += new EventHandler<PlayerIndexEventArgs>(options_Selected);
            quitToMenu.Selected += new EventHandler<PlayerIndexEventArgs>(quitToMenu_Selected);

            // Add entries to stack.
            MenuEntries.Add(resume);

            if (gameType == GameType.Campaign)
            {
                MenuEntries.Add(loadGame);
                MenuEntries.Add(saveGame);
            }

            MenuEntries.Add(options);
            MenuEntries.Add(quitToMenu);

#if PC
			quitToOS.Selected += new EventHandler<PlayerIndexEventArgs>(quitToOS_Selected);
			MenuEntries.Add(quitToOS);
#endif
        }

        public override void UnloadContent()
        {
            ScreenManager.Game.IsMouseVisible = false;
            base.UnloadContent();
        }

		void quitToOS_Selected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.Game.Exit();
		}

        void quitToMenu_Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new MenuBackgroundScreen(), new TitleScreen(), new MainMenuScreen());
        }

        void options_Selected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen());
        }
    }
}
