using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DarkHavoc.Engine.API;

namespace DarkHavoc.Engine.DataReaders
{
    public class SpriteStripMetadata
    {
        // Private member attributes
        private IniFile ini;

        private Color myColor;
        private int myFrameSpeed;
        private int myFrameWidth;
        private int myFrameHeight;
        private int myFrameCount;
        private bool myLoop;
        private float myScale;
        private int myStartingFrame;

        // Accessors
        public Color TintColor
        {
            get { return myColor; }
        }

        public int FrameSpeed
        {
            get { return myFrameSpeed; }
        }

        public int FrameWidth
        {
            get { return myFrameWidth; }
        }

        public int FrameHeight
        {
            get { return myFrameHeight; }
        }

        public int FrameCount
        {
            get { return myFrameCount; }
        }

        public bool Loop
        {
            get { return myLoop; }
        }

        public float Scale
        {
            get { return myScale; }
        }

        public int StartingFrame
        {
            get { return myStartingFrame; }
        }

        public SpriteStripMetadata(string configurationFile)
        {
            ini = new IniFile(configurationFile, IniFile.FileMode.Read);

            myColor = ColorUtil.FromString(ini.GetValueFromKey("Color"));
            myFrameSpeed = Convert.ToInt32(ini.GetValueFromKey("FrameSpeed"));
            myFrameWidth = Convert.ToInt32(ini.GetValueFromKey("FrameWidth"));
            myFrameHeight = Convert.ToInt32(ini.GetValueFromKey("FrameHeight"));
            myFrameCount = Convert.ToInt32(ini.GetValueFromKey("FrameCount"));
            myLoop = Convert.ToBoolean(ini.GetValueFromKey("Loop"));
            myScale = Convert.ToSingle(ini.GetValueFromKey("Scale"));
            myStartingFrame = Convert.ToInt32(ini.GetValueFromKey("StartingFrame"));
        }
    }
}
