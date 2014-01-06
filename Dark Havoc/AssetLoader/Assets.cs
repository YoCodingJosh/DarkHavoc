using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DarkHavoc.Engine.Pipeline;

namespace AssetLoader
{
    /// <summary>
    /// Container that holds all of the assets and contains a method that will load the assets.
    /// </summary>
    public static class Assets
    {
        // Private Attributes
        private static bool isLoaded;

        // Images
        public static Texture2D blankTexture;
        public static Texture2D joshuaKennedyLogoTexture;
        public static Texture2D waterfallTexture;
        public static Texture2D darkHavocLogo;

        // Fonts
        public static SpriteFont dialogBoxFont;
        public static SpriteFont consoleFont;
        public static SpriteFont menuFont;
        public static SpriteFont creditsSubtitleFont;
        public static SpriteFont creditsMajorTaskFont;
        public static SpriteFont creditsTaskAuthorFont;
        public static SpriteFont conversationDialogueFont;

        // Effects
        public static Effect disappearEffect;
        public static Effect desaturateEffect;

        // Sprites
        public static Texture2D halfCircleTexture;
        public static Texture2D lightningSegmentTexture;
        public static Texture2D starTexture;
        public static Texture2D playerShipTexture;
        public static Texture2D enemyMissileTexture;
		public static Texture2D snowflakeTexture;
        public static Texture2D continueConversationTexture;
        public static Texture2D conversationBoxBorderTexture;

        // Animations
        public static Texture2D fileAccessAnimationTexture;
        public static Texture2D missileExplosionAnimationTexture; // Actually is spritesheet

        // Dialogues
        public static List<Speaker> trainingDialogue;

        /// <summary>
        /// Loads up all of the assets synchrously.
        /// </summary>
        /// <param name="content">Initialized ContentManager object.</param>
        public static void StartCache(ContentManager content)
        {
            Debug.WriteLine("[Asset Server] Loading Assets...");
            
            // Let's check to see if we loaded already.
            if (!isLoaded)
            {
                // Images
                blankTexture = content.Load<Texture2D>("./Images/blank");
                joshuaKennedyLogoTexture = content.Load<Texture2D>("./Images/JoshuaKennedyLogo");
                waterfallTexture = content.Load<Texture2D>("./Images/waterfall");
                darkHavocLogo = content.Load<Texture2D>("./Images/DarkHavocLogo");

                // Fonts
                dialogBoxFont = content.Load<SpriteFont>("./Fonts/DialogFont");
                consoleFont = content.Load<SpriteFont>("./Fonts/ConsoleFont");
                menuFont = content.Load<SpriteFont>("./Fonts/MenuFont");
                creditsSubtitleFont = content.Load<SpriteFont>("./Fonts/CreditsSubtitleFont");
                creditsMajorTaskFont = content.Load<SpriteFont>("./Fonts/CreditsMajorTaskFont");
                creditsTaskAuthorFont = content.Load<SpriteFont>("./Fonts/CreditsTaskAuthorFont");
                conversationDialogueFont = content.Load<SpriteFont>("./Fonts/ConversationDialogueFont");

                // Effects
                disappearEffect = content.Load<Effect>("./Effects/disappear.mgfx");
                desaturateEffect = content.Load<Effect>("./Effects/desaturate.mgfx");

                // Sprites
                halfCircleTexture = content.Load<Texture2D>("./Sprites/HalfCircle");
                lightningSegmentTexture = content.Load<Texture2D>("./Sprites/LightningSegment");
                starTexture = content.Load<Texture2D>("./Sprites/star");
                playerShipTexture = content.Load<Texture2D>("./Sprites/PlayerShip");
                enemyMissileTexture = content.Load<Texture2D>("./Sprites/HomingMissile");
				snowflakeTexture = content.Load<Texture2D>("./Sprites/Snowflake");
                continueConversationTexture = content.Load<Texture2D>("./Sprites/ConversationContinue");
                conversationBoxBorderTexture = content.Load<Texture2D>("./Sprites/DialogueBorder");

                // Animations
                fileAccessAnimationTexture = content.Load<Texture2D>("./Animations/HDDACCESSINDICATOR");
                missileExplosionAnimationTexture = content.Load<Texture2D>("./Animations/MissileExplosion");

                // Dialogues
                trainingDialogue = content.Load<List<Speaker>>("./Dialogue/Training");

                // Finalization
                isLoaded = true;
                Debug.WriteLine("[Asset Server] Assets loaded!");
            }
            else
            {
                Debug.WriteLine("[Asset Server] Assets already loaded!");
            }
        }
    }
}
