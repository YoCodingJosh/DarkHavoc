using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;

namespace DarkHavoc.Engine.API
{
    public delegate void TimerEnd(object sender, EventArgs e);

    public sealed class CountdownTimer
    {
        private Timer myTimer;
        public int EndTimer;
        public bool isRunning { get; private set; }
        public bool isFinished { get; private set; }

        public event TimerEnd TimerEndEvent;

        public CountdownTimer()
        {
            EndTimer = 0;
            isRunning = false;
            isFinished = false;
            myTimer = new Timer(1000.000000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
        }

        void myTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isFinished)
            {
                if (isRunning)
                {
                    EndTimer--;

                    if (EndTimer <= 0)
                    {
                        EndTimer = 0;
                        isFinished = true;

                        if (TimerEndEvent != null)
                            TimerEndEvent(this, null);
                    }
                }
            }
        }

        public void Start(int seconds)
        {
            EndTimer = seconds;
            isRunning = true;
            myTimer.Start();
        }

        public void Reset()
        {
            isRunning = false;
            isFinished = false;
            EndTimer = 0;
        }
    }
}
