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

namespace DarkHavoc
{
    internal class OptionsMenuScreen : MenuScreen
    {
        private Options options;
        MenuEntry fullscreen;
        
        MenuEntry music;
        MenuEntry sound;
        MenuEntry save;
        MenuEntry back;
 
        public OptionsMenuScreen()
            : base("Game Options")
        {
            options = new Options();

#if PC
            options.ChangeSetting(Options.Setting.Fullscreen, DarkHavocGame.GameOptions.IsFullscreen);
#endif
            options.ChangeSetting(Options.Setting.Music, DarkHavocGame.GameOptions.IsMusic);
            options.ChangeSetting(Options.Setting.Sound, DarkHavocGame.GameOptions.IsSound);

#if PC
            fullscreen = new MenuEntry(string.Empty);
#endif
            music = new MenuEntry(string.Empty);
            sound = new MenuEntry(string.Empty);
            save = new MenuEntry("Save");
            back = new MenuEntry("Back");

#if PC
            fullscreen.Selected += new EventHandler<PlayerIndexEventArgs>(fullscreen_Selected);
#endif
            music.Selected += new EventHandler<PlayerIndexEventArgs>(music_Selected);
            sound.Selected += new EventHandler<PlayerIndexEventArgs>(sound_Selected);
            save.Selected += new EventHandler<PlayerIndexEventArgs>(save_Selected);
            back.Selected += OnCancel;

            SetMenuText();

#if PC
            MenuEntries.Add(fullscreen);
#endif
            MenuEntries.Add(music);
            MenuEntries.Add(sound);
            MenuEntries.Add(save);
            MenuEntries.Add(back);
        }

        private void SetMenuText()
        {
#if PC
            fullscreen.Text = (options.IsFullscreen) ? "Full Screen" : "Windowed";
#endif
            music.Text = "Music: " + ((options.IsMusic) ? "On" : "Off");
            sound.Text = "Sound: " + ((options.IsSound) ? "On" : "Off");
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (!Options.CheckDifferences(options, DarkHavocGame.GameOptions))
            {
                // prompt to ask if they want to save
                Debug.WriteLine("Options changed!");
            }
            else
                base.OnCancel(playerIndex);
        }

        void save_Selected(object sender, PlayerIndexEventArgs e)
        {
#if PC
            if (options.IsFullscreen != DarkHavocGame.GameOptions.IsFullscreen)
				DarkHavocGame.ToggleFullScreen(options.IsFullscreen);
#endif

            DarkHavocGame.GameOptions = options;

			Options.SerializeToFile(DarkHavocGame.AppDataLocation, DarkHavocGame.GameOptions);
        }

        void sound_Selected(object sender, PlayerIndexEventArgs e)
        {
            options.ChangeSetting(Options.Setting.Sound, !options.IsSound);
            SetMenuText();
        }

        void music_Selected(object sender, PlayerIndexEventArgs e)
        {
            options.ChangeSetting(Options.Setting.Music, !options.IsMusic);
            SetMenuText();
        }

#if PC
        void fullscreen_Selected(object sender, PlayerIndexEventArgs e)
        {
            options.ChangeSetting(Options.Setting.Fullscreen, !options.IsFullscreen);
            SetMenuText();
        }
#endif
    }
}
