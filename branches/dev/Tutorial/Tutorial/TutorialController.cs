using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;
using Radgie.Core;
using Radgie.Input.Device.Keyboard;
using Radgie.Input;

namespace Tutorial
{
    public class TutorialController
    {
        public DigitalAction OKAction
        {
            get
            {
                return mOKAction;
            }
        }
        private DigitalAction mOKAction;

        public TutorialController()
        {
            IKeyboard keyboard = Keyboard.Get(Microsoft.Xna.Framework.PlayerIndex.One);
            mOKAction = new DigitalAction();
            mOKAction.AddBinding(keyboard.SpaceKey);
            mOKAction.AddBinding(keyboard.EnterKey);
        }
    }
}
