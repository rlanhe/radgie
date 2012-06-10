
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Dispositivo Gamepad.
    /// </summary>
    public class Gamepad : ADevice<IGamepad>, IGamepad
    {
        #region Properties
        #region IGamepad members
        #region Buttons

        public IDigitalControl AButton
        {
            get
            {
                return mAButton;
            }
        }
        protected GamepadButton mAButton;

        public IDigitalControl BButton
        {
            get
            {
                return mBButton;
            }
        }
        protected GamepadButton mBButton;

        public IDigitalControl XButton
        {
            get
            {
                return mXButton;
            }
        }
        protected GamepadButton mXButton;

        public IDigitalControl YButton
        {
            get
            {
                return mYButton;
            }
        }
        protected GamepadButton mYButton;

        public IDigitalControl BackButton
        {
            get
            {
                return mBackButton;
            }
        }
        protected GamepadButton mBackButton;

        public IDigitalControl StartButton
        {
            get
            {
                return mStartButton;
            }
        }
        protected GamepadButton mStartButton;

        public IDigitalControl LeftShoulderButton
        {
            get
            {
                return mLeftShoulderButton;
            }
        }
        protected GamepadButton mLeftShoulderButton;

        public IDigitalControl RightShoulderButton
        {
            get
            {
                return mRightShoulderButton;
            }
        }
        protected GamepadButton mRightShoulderButton;

        public IDigitalControl LeftStickButton
        {
            get
            {
                return mLeftStickButton;
            }
        }
        protected GamepadButton mLeftStickButton;

        public IDigitalControl RightStickButton
        {
            get
            {
                return mRightStickButton;
            }
        }
        protected GamepadButton mRightStickButton;

        public IDigitalControl BigButton
        {
            get
            {
                return mBigButton;
            }
        }
        protected GamepadButton mBigButton;

        #endregion

        #region Triggers

        public IAnalogicalControl LeftTrigger
        {
            get
            {
                return mLeftTrigger;
            }
        }
        protected GamepadTrigger mLeftTrigger;

        public IAnalogicalControl RightTrigger
        {
            get
            {
                return mRightTrigger;
            }
        }
        protected GamepadTrigger mRightTrigger;

        #endregion

        #region Directional Pad

        public GamepadDPad DPad
        {
            get
            {
                return mDPad;
            }
        }
        protected GamepadDPad mDPad; 

        #endregion

        #region Thumbs

        public IAnalogicalDirectionControl LeftThumb
        {
            get
            {
                return mLeftThumb;
            }
        }
        protected GamepadThumb mLeftThumb;

        public IAnalogicalDirectionControl RightThumb
        {
            get
            {
                return mRightThumb;
            }
        }
        protected GamepadThumb mRightThumb;

        #endregion
        #endregion

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Gamepad.IGamepad.State"/>
        /// </summary>
        public GamePadState State
        {
            get
            {
                return mState;
            }
        }
        private GamePadState mState;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Gamepad.IGamepad.PreviousState"/>
        /// </summary>
        public GamePadState PreviousState
        {
            get
            {
                return mPreviousState;
            }
        }
        private GamePadState mPreviousState;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa un objeto Gamepad.
        /// </summary>
        /// <param name="index">Id del jugador con el que esta asociado.</param>
        public Gamepad(PlayerIndex index): base(index)
        {
            AddDevice(index, this);
            mControls.Add(mAButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.A; }));
            mControls.Add(mBButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.B; }));
            mControls.Add(mXButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.X; }));
            mControls.Add(mYButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.Y; }));
            mControls.Add(mBackButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.Back; }));
            mControls.Add(mStartButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.Start; }));
            mControls.Add(mLeftShoulderButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.LeftShoulder; }));
            mControls.Add(mRightShoulderButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.RightShoulder; }));
            mControls.Add(mLeftStickButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.LeftStick; }));
            mControls.Add(mRightStickButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.RightStick; }));
            mControls.Add(mBigButton = new GamepadButton(this, delegate(GamePadState state) { return state.Buttons.BigButton; }));

            mControls.Add(mLeftTrigger = new GamepadTrigger(this, delegate(GamePadState state) { return state.Triggers.Left; }));
            mControls.Add(mRightTrigger = new GamepadTrigger(this, delegate(GamePadState state) { return state.Triggers.Right; }));

            mControls.Add(mDPad = new GamepadDPad(this));

            mControls.Add(mLeftThumb = new GamepadThumb(this, delegate(GamePadState state) { return state.ThumbSticks.Left; }));
            mControls.Add(mRightThumb = new GamepadThumb(this, delegate(GamePadState state) { return state.ThumbSticks.Right; }));

            mState = mPreviousState = Microsoft.Xna.Framework.Input.GamePad.GetState(mIndex);
        }
        #endregion

        #region Methods
        #region ADevice members
        /// <summary>
        /// Ver <see cref="Radgie.Input.ADevice.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            mPreviousState = mState;
            mState = Microsoft.Xna.Framework.Input.GamePad.GetState(mIndex);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.ADevice.HasChanged"/>
        /// </summary>
        public override bool HasChanged()
        {
            return mState != mPreviousState;
        }
        #endregion

        #region IGamepad members
        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Gamepad.IGamepad.SetVibration"/>
        /// </summary>
        public bool SetVibration(float leftMotor, float rightMotor)
        {
            return Microsoft.Xna.Framework.Input.GamePad.SetVibration(mIndex, leftMotor, rightMotor);
        }
        #endregion
        #endregion
    }
}
