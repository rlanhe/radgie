using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroidsStorm.GameComponents.Spaceship;
using Microsoft.Xna.Framework;
using Radgie.Input.Action;
using Radgie.Input.Adapters;
using Radgie.Input.Device.Keyboard;
using AsteroidsStorm.GameComponents.FreeCamera;
using Radgie.Input.Device.Mouse;
using AsteroidsStorm.States.Menu;
using Radgie.Input.Device.Gamepad;

namespace AsteroidsStorm.States.Game
{
    /// <summary>
    /// Controlador del juego.
    /// </summary>
    class GameController: ISpaceshipController, IFreeCameraInputController
    {
        private PlayerIndex mPIndex;

        /// <summary>
        /// Accion para mover a izquierda y derecha.
        /// </summary>
        public AnalogicalAction LeftRight
        {
            get
            {
                return mSteer;
            }
        }
        private AnalogicalAction mSteer;

        /// <summary>
        /// Accion para mover arriba y abajo.
        /// </summary>
        public AnalogicalAction UpDown
        {
            get
            {
                if (InvertAxes)
                {
                    return mFrontRearInvert;
                }
                else
                {
                    return mFrontRear;
                }
            }
        }
        private AnalogicalAction mFrontRear;
        private AnalogicalAction mFrontRearInvert;

        /// <summary>
        /// Accion para usar un bonus.
        /// </summary>
        public DigitalAction UseBonus
        {
            get
            {
                return mUseBonus;
            }
        }
        private DigitalAction mUseBonus;
        
        /// <summary>
        /// Accion para mover a la izquierda la camara.
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
        /// Accion para mover a la derecha la camara.
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
        /// Accion para mover hacia delante la camara.
        /// </summary>
        public DigitalAction Forward
        {
            get
            {
                return mForward;
            }
        }
        private DigitalAction mForward;

        /// <summary>
        /// Accion para mover hacia atras la camara.
        /// </summary>
        public DigitalAction Backward
        {
            get
            {
                return mBackward;
            }
        }
        private DigitalAction mBackward;

        /// <summary>
        /// Accion para activar/desactivar la rotacion.
        /// </summary>
        public DigitalAction RotationActivated
        {
            get
            {
                return mRotationActivated;
            }
        }
        private DigitalAction mRotationActivated;

        /// <summary>
        /// Anula las rotaciones d ela camara.
        /// </summary>
        public DigitalAction RotationReset
        {
            get
            {
                return mRotationReset;
            }
        }
        private DigitalAction mRotationReset;

        /// <summary>
        /// Rotacion de la camara.
        /// </summary>
        public PositionAction Rotation
        {
            get
            {
                return mRotation;
            }
        }
        private PositionAction mRotation;

        /// <summary>
        /// Activa/desactiva la informacion de debug.
        /// </summary>
        public DigitalAction Debug
        {
            get
            {
                return mDebug;
            }
        }
        private DigitalAction mDebug;

        /// <summary>
        /// Se mueve hacia arriba en las opciones de menu.
        /// </summary>
        public DigitalAction Up
        {
            get
            {
                return mForward;
            }
        }

        /// <summary>
        /// Se mueve hacia abajo en las opciones de menu.
        /// </summary>
        public DigitalAction Down
        {
            get
            {
                return mBackward;
            }
        }

        /// <summary>
        /// Acepta la opcion de menu.
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
        /// Cancela la opcion de menu.
        /// </summary>
        public DigitalAction Cancel
        {
            get
            {
                return mCancel;
            }
        }
        private DigitalAction mCancel;

        public bool InvertAxes
        {
            get
            {
                return mInvertAxes;
            }
            set
            {
                mInvertAxes = value;
            }
        }
        private bool mInvertAxes;

        /// <summary>
        /// Indica si se realizo alguna accion del controlador.
        /// </summary>
        /// <returns>True si se realizo alguna opcion, False en caso contrario.</returns>
        public bool ActionFired()
        {
            if((this.Backward.Pressed) || (this.Cancel.Pressed) || (this.Down.Pressed) || (this.Forward.Pressed) || (this.Left.Pressed) || (this.LeftRight.Value != 0.0f) || (this.Ok.Pressed) || (this.Right.Pressed) || (this.Up.Pressed) || (this.UpDown.Value != 0.0f) || (this.UseBonus.Pressed))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Crea e inicializa el controlador.
        /// </summary>
        /// <param name="pIndex">indice del jugador.</param>
        public GameController(PlayerIndex pIndex)
        {
            mPIndex = pIndex;
            IKeyboard keyboard = Keyboard.Get(mPIndex);
            IGamepad gamepad = Gamepad.Get(mPIndex);

            mSteer = new AnalogicalAction(0.0f, 0.0f);
            mSteer.AddBinding(new Digitals2AnalogicalAdapter(keyboard.LeftKey, keyboard.RightKey, -1.0f, 1.0f, 2.0f));
            mSteer.AddBinding(new Digitals2AnalogicalAdapter(gamepad.DPad.Left, gamepad.DPad.Right, -1.0f, 1.0f, 2.0f));
            mSteer.AddBinding(new Analogical2AnalogicalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.X),-1.0f,1.0f,0.05f));

            mFrontRear = new AnalogicalAction(0.0f, 0.0f);
            mFrontRear.AddBinding(new Digitals2AnalogicalAdapter(keyboard.UpKey, keyboard.DownKey, -1.0f, 1.0f, 2.0f));
            mFrontRear.AddBinding(new Digitals2AnalogicalAdapter(gamepad.DPad.Up, gamepad.DPad.Down, -1.0f, 1.0f, 2.0f));
            mFrontRear.AddBinding(new Analogical2AnalogicalInverterAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.Y), -1.0f, 1.0f, 0.05f));

            mFrontRearInvert = new AnalogicalAction(0.0f, 0.0f);
            mFrontRearInvert.AddBinding(new Digitals2AnalogicalAdapter(keyboard.DownKey, keyboard.UpKey, -1.0f, 1.0f, 2.0f));
            mFrontRearInvert.AddBinding(new Digitals2AnalogicalAdapter(gamepad.DPad.Down, gamepad.DPad.Up, -1.0f, 1.0f, 2.0f));
            mFrontRearInvert.AddBinding(new Analogical2AnalogicalAdapter(new AnalogicalDirectionControl2AnalogicalAdapter(gamepad.LeftThumb, AnalogicalDirectionControl2AnalogicalAdapter.Component.Y), -1.0f, 1.0f, 0.05f));

            mUseBonus = new DigitalAction();
            mUseBonus.AddBinding(new DigitalOneShotAdapter(keyboard.SpaceKey));
            mUseBonus.AddBinding(new DigitalOneShotAdapter(gamepad.AButton));

            mLeft = new DigitalAction();
            //mLeft.AddBinding(keyboard.AKey);

            mRight = new DigitalAction();
            //mRight.AddBinding(keyboard.DKey);

            mForward = new DigitalAction();
            //mForward.AddBinding(keyboard.WKey);

            mBackward = new DigitalAction();
            mBackward.AddBinding(new DigitalOneShotAdapter(keyboard.EscapeKey));
            mBackward.AddBinding(new DigitalOneShotAdapter(gamepad.BackButton));

            mRotationActivated = new DigitalAction();
            //mRotationActivated.AddBinding(Mouse.Get(mPIndex).LeftButton);

            mRotationReset = new DigitalAction();
            //mRotationReset.AddBinding(Mouse.Get(mPIndex).RightButton);

            mRotation = new PositionAction();
            //mRotation.AddBinding(Mouse.Get(mPIndex).Position);

            mDebug = new DigitalAction();
            //mDebug.AddBinding(new DigitalOneShotAdapter(keyboard.F1Key));

            mOk = new DigitalAction();
            //mOk.AddBinding(new DigitalOneShotAdapter(keyboard.EnterKey));

            mCancel = new DigitalAction();
            //mCancel.AddBinding(new DigitalOneShotAdapter(Keyboard.Get(mPIndex).EscapeKey));
        }
    }
}
