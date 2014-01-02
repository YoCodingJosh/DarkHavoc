using System;

namespace DarkHavoc.Engine.Dialogue
{
    [Serializable]
    public class Speaker
    {
        public int AvatarIndex;
        public string Message;

        public Speaker()
        {
            this.AvatarIndex = 0;
            this.Message = "";
        }

        public Speaker(int avatar, string msg)
        {
            this.AvatarIndex = avatar;
            this.Message = msg;
        }
    }
}
