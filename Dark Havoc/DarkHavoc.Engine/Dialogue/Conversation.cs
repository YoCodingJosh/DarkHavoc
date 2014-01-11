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
    /// Really Kickass Dialogue Engine!
    /// </summary>
    /// <remarks>
    /// Based on Ian Mitchell's code from:
    /// http://blog.centasm.net/2011/04/xna-journal-rpg-style-dialogue/
    /// http://blog.centasm.net/2011/04/xna-journal-dialogue-version-2/
    /// </remarks>
    public class Conversation
    {
        public static Vector2 DefaultPosition = new Vector2(100, 200);

        private ScreenManager screenManager;

        public List<Speaker> ConversationSpeakers;
        public int CurrentSpeakerIndex = 0;
        public List<Texture2D> SpeakerAvatars;
        public Rectangle textRectangle;

        private string revealedMessage;

        /// <summary>
        /// Returns if the current speaker in the middle of speaking (e.g. text being typed out on screen)
        /// </summary>
        public bool IsSpeaking;

        /// <summary>
        /// Create a new instance of the Dialogue Engine.
        /// </summary>
        /// <param name="scrMan">Initialized ScreenManager instance.</param>
        public Conversation(ScreenManager scrMan)
        {
            this.screenManager = scrMan;
            this.ConversationSpeakers = new List<Speaker>();
            this.SpeakerAvatars = new List<Texture2D>();
        }

        /// <summary>
        /// Initializes the Dialogue Engine.
        /// </summary>
        /// <param name="speakers">The loaded conversation.</param>
        /// <param name="avatars">List of avatars that are going to partake in the conversation.</param>
        public void Initialize(List<Speaker> speakers, List<Texture2D> avatars)
        {
            this.ConversationSpeakers = speakers;
            this.SpeakerAvatars = avatars;

            this.CurrentSpeakerIndex = 0;
            this.IsSpeaking = false;

            this.revealedMessage = "";
        }

        public void StartConversation()
        {

        }

        private void createBox()
        {
        }

        /// <summary>
        /// Constrains a string within the confines of the dialogue box.
        /// </summary>
        /// <param name="message">The dialogue message.</param>
        /// <returns></returns>
        private string constrainString(string message)
        {
            bool filled = false;
            string line = "";
            string resultString = "";
            string[] wordsArray = message.Split(' ');

            // We iterate through the space separated word array.
            foreach (string word in wordsArray)
            {
                // If the message is longer than the width of the box...
                if (Assets.conversationDialogueFont.MeasureString(line + word).X > textRectangle.Width)
                {
                    // then place it on a new line (if space permits).
                    if (Assets.conversationDialogueFont.MeasureString(resultString + line + '\n').Y > textRectangle.Height)
                    {
                        resultString += line + '\n';
                        line = "";
                    }
                    else if (!filled)
                    {
                        filled = true;
                        resultString += line;
                        line = "";
                    }
                }

                line += word + " ";
            }

            if (filled)
            {
                // If vertical space doesn't permit new lines, then we need to create a new Speaker object.
                Speaker currentSpeaker = ConversationSpeakers[CurrentSpeakerIndex];

                ConversationSpeakers.Insert(CurrentSpeakerIndex + 1,
                    new Speaker(currentSpeaker.AvatarName, currentSpeaker.Message, currentSpeaker.Choices, currentSpeaker.ChoicesPrompt, currentSpeaker.RevealStyle));
            }
            else
                return resultString + line;

            return resultString;
        }

        public void HandleInput(InputState input, PlayerIndex? controllingPlayer)
        {
            int playerIndex = (int)controllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}
