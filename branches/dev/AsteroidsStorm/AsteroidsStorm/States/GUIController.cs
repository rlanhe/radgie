using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Device.Keyboard;
using Radgie.Input.Action;
using Radgie.Input.Adapters;
using Radgie.Input.Device.Gamepad;

namespace AsteroidsStorm.States
{
    /// <summary>
    /// Controlador para manejar el menu de la aplicacion.
    /// </summary>
    class GUIController: IGUIController
    {
        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Up"/>
        /// </summary>
        public DigitalAction Up
        {
            get
            {
                return mUp;
            }
        }
        private DigitalAction mUp;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Down"/>
        /// </summary>
        public DigitalAction Down
        {
            get
            {
                return mDown;
            }
        }
        private DigitalAction mDown;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Left"/>
        /// </summary>
        public DigitalAction Left
        {
            get
            {
                return mLeft;
            }
        }
        private DigitalAction mLeft;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Right"/>
        /// </summary>
        public DigitalAction Right
        {
            get
            {
                return mRight;
            }
        }
        private DigitalAction mRight;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Ok"/>
        /// </summary>
        public DigitalAction Ok
        {
            get
            {
                return mOk;
            }
        }
        private DigitalAction mOk;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Cancel"/>
        /// </summary>
        public DigitalAction Cancel
        {
            get
            {
                return mCancel;
            }
        }
        private DigitalAction mCancel;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.States.IGUIController.Pause"/>
        /// </summary>
        public DigitalAction Pause
        {
            get
            {
                return mPause;
            }
        }
        private DigitalAction mPause;

        /// <summary>
        /// Crea e inicializa el controlador de menu.
        /// </summary>
        public GUIController()
        {
            IKeyboard keyboard = Keyboard.Get(Microsoft.Xna.Framework.PlayerIndex.One);
            IGamepad gamepad = Gamepad.Get(Microsoft.Xna.Framework.PlayerIndex.One);

            mUp = new DigitalAction();
            mUp.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(keyboard.UpKey));
            mUp.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(new Analogical2DigitalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.Y), 0.5f)));
            mUp.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(gamepad.DPad.Up));

            mDown = new DigitalAction();
            mDown.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(keyboard.DownKey));
            mDown.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(new Analogical2DigitalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.Y), -0.5f)));
            mDown.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(gamepad.DPad.Down));

            mRight = new DigitalAction();
            mRight.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(keyboard.RightKey));
            mRight.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(new Analogical2DigitalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.X), 0.5f)));
            mRight.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(gamepad.DPad.Right));

            mLeft = new DigitalAction();
            mLeft.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(keyboard.LeftKey));
            mLeft.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(new Analogical2DigitalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.X), -0.5f)));
            mLeft.AddBinding(new DigitalOneShotWithDelayAndRepetitionAdapter(gamepad.DPad.Left));

            mPause = new DigitalAction();
            mPause.AddBinding(new DigitalOneShotAdapter(keyboard.EscapeKey));
            mPause.AddBinding(new DigitalOneShotAdapter(gamepad.StartButton));

            mOk = new DigitalAction();
            mOk.AddBinding(new DigitalOneShotAdapter(keyboard.EnterKey));
            mOk.AddBinding(new DigitalOneShotAdapter(gamepad.AButton));

            mCancel = new DigitalAction();
            mCancel.AddBinding(new DigitalOneShotAdapter(keyboard.DeleteKey));
            mCancel.AddBinding(new DigitalOneShotAdapter(gamepad.BButton));
        }
    }
}
