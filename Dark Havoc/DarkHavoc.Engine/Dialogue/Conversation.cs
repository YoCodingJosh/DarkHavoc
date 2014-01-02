using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace DarkHavoc.Engine.Dialogue
{
    public static class Conversation
    {
        public static List<Speaker> ConversationSpeakers = new List<Speaker>();
        private static int currentConversationSpeakerIndex = 0;
        public static string ConversationFileLocation;
    }
}
