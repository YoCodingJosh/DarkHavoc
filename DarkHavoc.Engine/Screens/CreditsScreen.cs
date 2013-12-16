using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DarkHavoc.Engine;
using DarkHavoc.Engine.API;
using DarkHavoc.Engine.DataReaders;
using AssetLoader;

namespace DarkHavoc.Engine.Screens
{
    public class CreditsScreen : GameScreen
    {
        Credits credits;

        //bool isMusic;

        bool creditsFinished;
        int currentCredits;
        int currentTask;
        bool showLogo;

        const float timeToStayOffScreen = 0.66f;
        const float timeToStayOnScreen = 1.55f;

        float timerOff;
        float timerOn;

        bool isOff;

        string currentMajorTask;
        string currentTaskTitle;
        string currentTaskPerson;

        Vector2 centerOfScreen;

        /// <summary>
        /// Event raised when the credits have ended, or the user has ended it.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> CreditsEnd;

        private void OnCreditsEnd(PlayerIndex playerIndex)
        {
            if (CreditsEnd != null && creditsFinished)
            {
                CreditsEnd(this, new PlayerIndexEventArgs(playerIndex));
            }
        }

        public CreditsScreen(string creditsFile, string musicFile)
        {
            credits = new Credits(creditsFile);

            //isMusic = true;

            Initialize();
        }

        public CreditsScreen(string creditsFile)
        {
            credits = new Credits(creditsFile);

            //isMusic = false;

            Initialize();
        }

        public CreditsScreen(Credits creditsFile, string musicFile)
        {
            credits = creditsFile;

            //isMusic = true;

            Initialize();
        }

        public CreditsScreen(Credits creditsFile)
        {
            credits = creditsFile;
        }

        private void Initialize()
        {
            creditsFinished = false;
            currentCredits = 0;
            currentTask = 0;

            showLogo = credits.ShowLogo;

            timerOff = 0.0f;
            timerOn = 0.0f;

            isOff = false;

            Debug.WriteLine("[JoshoEngine] Credits loaded.");
        }

        public override void LoadContent()
        {
            centerOfScreen = new Vector2(ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.X, ScreenManager.GraphicsDevice.Viewport.TitleSafeArea.Center.Y);
        }

        public override void UnloadContent()
        {

        }

        private void UpdateCredits(GameTime gameTime)
        {
            if (showLogo)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timerOn += elapsed;

                if (timerOn >= timeToStayOnScreen)
                {
                    showLogo = false;
                    isOff = true;

                    timerOn = 0.0f;
                }
            }
            else
            {
                if (isOff)
                {
                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    timerOff += elapsed;

                    if (timerOff >= timeToStayOffScreen)
                    {
                        isOff = false;

                        timerOff = 0.0f;
                    }
                }
                else
                {
                    CreditsMajorTask majorTask = credits.MajorTasks[currentCredits];

                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    timerOn += elapsed;

                    if (timerOn >= timeToStayOnScreen)
                    {
                        timerOn = 0.0f;
                        isOff = true;

                        currentTask++;

                        if (currentTask >= majorTask.Tasks.Count)
                        {
                            currentTask = 0;
                            currentCredits++;

                            if (currentCredits >= credits.MajorTasks.Count)
                            {
                                creditsFinished = true;
                            }
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!creditsFinished)
            {
                UpdateCredits(gameTime);
            }
            else
            {
                // Fire exit event handler to pass back to previous screen.
                OnCreditsEnd(PlayerIndex.One);
            }
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;

            if (input.IsMenuCancel(null, out playerIndex))
            {
                creditsFinished = true;
                OnCreditsEnd(playerIndex);
            }
        }
        
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            if (!creditsFinished)
            {
                if (showLogo)
                {
                    spriteBatch.Draw(Assets.darkHavocLogo, Helper.CenterOnScreen(ScreenManager, Assets.darkHavocLogo), Color.White);
                }
                else
                {
                    CreditsMajorTask majorTask = credits.MajorTasks[currentCredits];
                    currentMajorTask = majorTask.Name;
                    currentTaskTitle = majorTask.Tasks[currentTask].Title;
                    currentTaskPerson = majorTask.Tasks[currentTask].Name;

                    Vector2 majorTaskCenter = new Vector2(centerOfScreen.X - (Assets.creditsSubtitleFont.MeasureString(currentMajorTask).X / 2), centerOfScreen.Y - Assets.creditsSubtitleFont.Spacing - 100);
                    Vector2 taskTitleCenter = new Vector2(centerOfScreen.X - (Assets.creditsMajorTaskFont.MeasureString(currentTaskTitle).X / 2), centerOfScreen.Y - Assets.creditsMajorTaskFont.Spacing - Assets.creditsSubtitleFont.Spacing - 33);
                    Vector2 taskPersonCenter = new Vector2(centerOfScreen.X - (Assets.creditsTaskAuthorFont.MeasureString(currentTaskPerson).X / 2), centerOfScreen.Y - Assets.creditsTaskAuthorFont.Spacing + Assets.creditsMajorTaskFont.Spacing);

                    spriteBatch.DrawString(Assets.creditsSubtitleFont, currentMajorTask, majorTaskCenter, Color.White);

                    if (!isOff)
                    {
                        spriteBatch.DrawString(Assets.creditsMajorTaskFont, currentTaskTitle, taskTitleCenter, Color.White);
                        spriteBatch.DrawString(Assets.creditsTaskAuthorFont, currentTaskPerson, taskPersonCenter, Color.White);
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
