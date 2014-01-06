using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace DarkHavoc.Engine.Pipeline
{
    public class Speaker
    {
        public string Message;
        public string AvatarName;
        public Dictionary<string, int> Choices;
        public string ChoicesPrompt;

        public bool IsChoices
        {
            get { return Choices != null; }
        }

        public Speaker()
        {
            this.Message = "";
            this.Choices = null;
            this.AvatarName = "";
            this.ChoicesPrompt = "";
        }

        public Speaker(string avatarName, string msg, Dictionary<string, int> choices)
        {
            this.AvatarName = avatarName;
            this.Message = msg;
            this.Choices = choices;
        }
    }
}