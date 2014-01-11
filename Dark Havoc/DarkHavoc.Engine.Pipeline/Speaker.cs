using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if WINDOWS
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
#endif

namespace DarkHavoc.Engine.Pipeline
{
    /// <summary>
    /// A Single Speaker in a Dialogue.
    /// </summary>
    /// <remarks>
    /// Based on Ian Mitchell's code from:
    /// http://blog.centasm.net/2011/04/xna-journal-rpg-style-dialogue/
    /// http://blog.centasm.net/2011/04/xna-journal-dialogue-version-2/
    /// </remarks>
    public class Speaker
    {
        /// <summary>
        /// Determines how fast the text will reveal itself.
        /// </summary>
        /// <remarks>
        /// If this is for a choices speaker, the value passed doesn't matter.
        /// </remarks>
        public enum TextRevealStyle : uint
        {
            Normal = 0,
            Random = 1,
            Slow = 2,
            Fast = 3,
        };

        public string Message;
        public string AvatarName;
        public string ChoicesPrompt;
        public Dictionary<string, int> Choices;
        public TextRevealStyle RevealStyle;

        public bool IsChoices
        {
            get { return Choices != null; }
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        /// <remarks>
        /// DON'T USE THIS CONSTRUCTOR!!
        /// This is needed for the XNB Asset Compiler. -_-'
        /// </remarks>
        public Speaker()
        {
            this.Message = "";
            this.Choices = null;
            this.AvatarName = "";
            this.ChoicesPrompt = "";
            this.RevealStyle = TextRevealStyle.Normal;
        }

        /// <summary>
        /// Creates a new Speaker object.
        /// </summary>
        /// <remarks>
        /// This is the good constructor!! :)
        /// </remarks>
        public Speaker(string avatarName, string msg, Dictionary<string, int> choices, string prompt, TextRevealStyle style)
        {
            this.AvatarName = avatarName;
            this.Message = msg;
            this.Choices = choices;
            this.RevealStyle = style;
            this.ChoicesPrompt = prompt;
        }
    }
}
