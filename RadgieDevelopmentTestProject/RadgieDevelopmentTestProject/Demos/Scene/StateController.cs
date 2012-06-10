using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Input.Action;
using Radgie.Input.Device.Mouse;

namespace RadgieDevelopmentTestProject.Demos.Scene
{
    public class StateController
    {
        private PlayerIndex mPIndex;

        public DigitalAction Action;

        public StateController(PlayerIndex pIndex)
        {
            mPIndex = pIndex;

            Action = new DigitalAction();
            Action.AddBinding(Mouse.Get(mPIndex).LeftButton);
        }
    }
}
