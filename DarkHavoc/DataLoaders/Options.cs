using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DarkHavoc
{
    [Serializable]
    internal class Options
    {
        public enum Setting : uint
        {
#if PC
            Fullscreen = 0,
#endif
            Music = 1,
            Sound = 2,
        };

#if PC
        private bool fullscreen;
#endif
        private bool music;
        private bool sound;

#if PC
        public bool IsFullscreen
        {
            get { return fullscreen; }
        }
#endif

        public bool IsMusic
        {
            get { return music; }
        }

        public bool IsSound
        {
            get { return sound; }
        }

        private string UserOS;

        public Options()
        {
#if PC
            fullscreen = false;
#endif
            music = true;
            sound = true;

            UserOS = ((Environment.Is64BitOperatingSystem) ? "64-bit" : "32-bit") + " " + Environment.OSVersion.ToString();

            if (!File.Exists("./Settings.josho"))
                ResetOptions();
        }

        public void ResetOptions()
        {
#if PC
            fullscreen = true;
#endif
            music = true;
            sound = true;
        }

        public void ChangeSetting(Setting setting, bool value)
        {
            switch(setting)
            {
#if PC
                case Setting.Fullscreen:
                    fullscreen = value;
                    break;
#endif
                case Setting.Music:
                    music = value;
                    break;
                case Setting.Sound:
                    sound = value;
                    break;
            }
        }

        public static void SerializeToFile(Options options)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("./Settings.josho", FileMode.Create);

            bf.Serialize(fs, options);

            fs.Close();
            fs.Dispose();
        }

        public static void DeserializeToObject(out Options options)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("./Settings.josho", FileMode.Open);

            options = (Options)bf.Deserialize(fs);

            fs.Close();
            fs.Dispose();
        }

        public static bool CheckDifferences(Options options1, Options options2)
        {
#if PC
            if (options1.fullscreen == options2.fullscreen && options1.music == options2.music && options1.sound == options2.sound)
                return true;
            else
                return false;
#else
            if (options1.music == options2.music && options1.sound == options2.sound)
                return true;
            else
                return false;
#endif
        }
    }
}
