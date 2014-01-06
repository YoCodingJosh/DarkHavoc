using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.Pipeline;
using AssetLoader;

namespace DarkHavoc.Engine.Dialogue
{
    /// <summary>
    /// Dialogue Engine.
    /// </summary>
    /// <remarks>
    /// Based on Ian Mitchell's code from:
    /// http://blog.centasm.net/2011/04/xna-journal-rpg-style-dialogue/
    /// http://blog.centasm.net/2011/04/xna-journal-dialogue-version-2/
    /// </remarks>
    public class Conversation
    {
        private ScreenManager screenManager;
        public List<Speaker> ConversationSpeakers;
        private int currentSpeakerIndex = 0;
        public List<Texture2D> SpeakerAvatars;


        public Conversation(ScreenManager scrMan)
        {
            this.screenManager = scrMan;
            this.ConversationSpeakers = new List<Speaker>();
            this.SpeakerAvatars = new List<Texture2D>();
        }

        /// <summary>
        /// Initializes the Dialogue Engine.
        /// </summary>
        /// <param name="speakers">The loaded conversation</param>
        /// <param name="avatars"></param>
        public void Initialize(List<Speaker> speakers, List<Texture2D> avatars)
        {
            this.ConversationSpeakers = speakers;
            this.SpeakerAvatars = avatars;
        }


        public static void StartConversation()
        {
        }
    }
}
