using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Radgie.Util;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Adapters
{
    public class DigitalOneShotWithDelayAndRepetitionAdapter: DigitalOneShotAdapter
    {
        public override bool Pressed
        {
            get
            {
                Update();
                return mPressed;
            }
        }
        private bool mPressed;

        public TimeSpan FirstDelay
        {
            get
            {
                return mFirstDelay;
            }
            set
            {
                mFirstDelay = value;
            }
        }
        private TimeSpan mFirstDelay;

        public TimeSpan RepetitionDelay
        {
            get
            {
                return mRepetitionDelay;
            }
            set
            {
                mRepetitionDelay = value;
            }
        }
        private TimeSpan mRepetitionDelay;

        protected Timer mLastPressedTimer = Timer.StartNew();
        protected bool mRepetitionPhase = false;

        public DigitalOneShotWithDelayAndRepetitionAdapter(IDigitalControl dControl)
            : this(dControl, TimeSpan.FromSeconds(1.0d), TimeSpan.FromMilliseconds(100.0d))
        {
        }

        public DigitalOneShotWithDelayAndRepetitionAdapter(IDigitalControl dControl, TimeSpan firstDelay, TimeSpan repetitionDelay)
            : base(dControl)
        {
            FirstDelay = firstDelay;
            RepetitionDelay = repetitionDelay;
            mPressed = false;
            mLastTimeUpdated = mInputSystem.LastTimeUpdated.TotalGameTime;
            mLastPressedTimer.Stop();
        }

        private TimeSpan mLastTimeUpdated;
        private static IInputSystem mInputSystem = (IInputSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IInputSystem));

        private void Update()
        {
            GameTime systemLastTimeUpdated = mInputSystem.LastTimeUpdated;

            // Se volvio a actualizar el dispositivo
            if (systemLastTimeUpdated.TotalGameTime > mLastTimeUpdated)
            {
                if (mDControl.Pressed)
                {
                    bool wasPressed = mDControl.PreviousValue;

                    if (!wasPressed)
                    {
                        mLastPressedTimer.Start();
                        mPressed = true;
                    }
                    else
                    {
                        if (!mRepetitionPhase)
                        {
                            if (mLastPressedTimer.GetTotalTime() < FirstDelay)
                            {
                                mPressed = false;
                            }
                            else
                            {
                                mRepetitionPhase = true;
                                mLastPressedTimer.Start();
                                mPressed = true;
                            }
                        }
                        else
                        {
                            if (mLastPressedTimer.GetTotalTime() < RepetitionDelay)
                            {
                                mPressed = false;
                            }
                            else
                            {
                                mLastPressedTimer.Start();
                                mPressed = true;
                            }
                        }
                    }
                }
                else
                {
                    mLastPressedTimer.Stop();
                    mRepetitionPhase = false;
                    mPressed = false;
                }

                mLastTimeUpdated = systemLastTimeUpdated.TotalGameTime;
            }
        }
    }
}
