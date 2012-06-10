using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;
using Radgie.Input.Device.Keyboard;
using Microsoft.Xna.Framework;
using Radgie.Input.Device.Gamepad;

namespace RadgieDevelopmentTestProject.Demos
{
    public class DemoController: IDemoController
    {
        public DigitalAction NextDemo
        {
            get
            {
                return mNextDemo;
            }
        }
        private DigitalAction mNextDemo;

        public DigitalAction PreviousDemo
        {
            get
            {
                return mPreviousDemo;
            }
        }
        private DigitalAction mPreviousDemo;

        public DigitalAction ExitDemo
        {
            get
            {
                return mExitDemo;
            }
        }
        private DigitalAction mExitDemo;

        public DemoController()
        {
            mNextDemo = new DigitalAction();
            mNextDemo.AddBinding(Keyboard.Get(PlayerIndex.One).PageDownKey);
            mNextDemo.AddBinding(Gamepad.Get(PlayerIndex.One).RightShoulderButton);

            mPreviousDemo = new DigitalAction();
            mPreviousDemo.AddBinding(Keyboard.Get(PlayerIndex.One).PageUpKey);
            mPreviousDemo.AddBinding(Gamepad.Get(PlayerIndex.One).LeftShoulderButton);

            mExitDemo = new DigitalAction();
            mExitDemo.AddBinding(Keyboard.Get(PlayerIndex.One).EscapeKey);
            mExitDemo.AddBinding(Gamepad.Get(PlayerIndex.One).BackButton);
        }
    }
}
