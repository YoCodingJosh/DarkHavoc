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

        bool isSpecialThanks;

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

            isSpecialThanks = false;

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
                    CreditsMajorTask majorTask = null;

                    if (!isSpecialThanks)
                        majorTask = credits.MajorTasks[currentCredits];

                    float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    timerOn += elapsed;

                    if (timerOn >= timeToStayOnScreen)
                    {
                        timerOn = 0.0f;
                        isOff = true;

                        currentTask++;

                        if (!isSpecialThanks && currentTask >= majorTask.Tasks.Count)
                        {
                            currentTask = 0;
                            currentCredits++;

                            if (currentCredits >= credits.MajorTasks.Count && !isSpecialThanks)
                            {
                                currentCredits = 0;
                                isSpecialThanks = true;
                            }
                        }

                        if (isSpecialThanks && currentTask >= credits.SpecialThanks.Thanks.Count)
                        {
                            creditsFinished = true;
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
                    string specialThank = null;
                    if (!isSpecialThanks)
                    {
                        CreditsMajorTask majorTask = credits.MajorTasks[currentCredits];
                        currentMajorTask = majorTask.Name;
                        currentTaskTitle = majorTask.Tasks[currentTask].Title;
                        currentTaskPerson = majorTask.Tasks[currentTask].Name;
                    }
                    else
                    {
                        specialThank = credits.SpecialThanks.Thanks[currentTask];
                    }

                    Vector2 majorTaskCenter = Vector2.Zero;
                    Vector2 taskTitleCenter = Vector2.Zero;
                    Vector2 taskPersonCenter = Vector2.Zero;

                    if (!isSpecialThanks)
                    {
                        majorTaskCenter = new Vector2(centerOfScreen.X - (Assets.creditsSubtitleFont.MeasureString(currentMajorTask).X / 2), centerOfScreen.Y - Assets.creditsSubtitleFont.Spacing - 100);
                        taskTitleCenter = new Vector2(centerOfScreen.X - (Assets.creditsMajorTaskFont.MeasureString(currentTaskTitle).X / 2), centerOfScreen.Y - Assets.creditsMajorTaskFont.Spacing - Assets.creditsSubtitleFont.Spacing - 33);
                        taskPersonCenter = new Vector2(centerOfScreen.X - (Assets.creditsTaskAuthorFont.MeasureString(currentTaskPerson).X / 2), centerOfScreen.Y - Assets.creditsTaskAuthorFont.Spacing + Assets.creditsMajorTaskFont.Spacing);
                    }
                    else
                    {
                        majorTaskCenter = new Vector2(centerOfScreen.X - (Assets.creditsSubtitleFont.MeasureString("Special Thanks").X / 2), centerOfScreen.Y - Assets.creditsSubtitleFont.Spacing - 100);
                        taskTitleCenter = new Vector2(centerOfScreen.X - (Assets.creditsMajorTaskFont.MeasureString(specialThank).X / 2), centerOfScreen.Y - Assets.creditsMajorTaskFont.Spacing - 33);
                    }

                    if (!isSpecialThanks)
                        spriteBatch.DrawString(Assets.creditsSubtitleFont, currentMajorTask, majorTaskCenter, Color.White);
                    else
                        spriteBatch.DrawString(Assets.creditsSubtitleFont, "Special Thanks", majorTaskCenter, Color.White);

                    if (!isOff)
                    {
                        if (!isSpecialThanks)
                        {
                            spriteBatch.DrawString(Assets.creditsMajorTaskFont, currentTaskTitle, taskTitleCenter, Color.White);
                            spriteBatch.DrawString(Assets.creditsTaskAuthorFont, currentTaskPerson, taskPersonCenter, Color.White);
                        }
                        else
                        {
                            spriteBatch.DrawString(Assets.creditsMajorTaskFont, specialThank, taskTitleCenter, Color.White);
                        }
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
