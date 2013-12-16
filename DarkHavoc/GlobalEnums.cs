using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DarkHavoc
{
    /// <summary>
    /// The fade state for Stealth Titan and other related enemies.
    /// </summary>
    public enum FadeState : ushort
    {
        Invisible = 0,
        Transitioning = 1,
        Visible = 255,
    };

    /// <summary>
    /// The current game type, used in pause menu.
    /// </summary>
    public enum GameType : uint
    {
        Training = 0,
        Arcade = 1,
        Campaign = 2,
    };
}
