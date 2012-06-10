using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Util;

namespace Tutorial
{
    public class IntroState: AState
    {
        public static Event EXIT_INTRO = new Event("Exit_Intro");

        private BackCounter mNextStateCounter;

        public IntroState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            mNextStateCounter = new BackCounter(TimeSpan.FromSeconds(1.0d));
        }

        public override void OnEntry()
        {
            base.OnEntry();

            mNextStateCounter.Start();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (mNextStateCounter.Finished())
            {
                SendEvent(EXIT_INTRO);
            }
        }
    }
}
