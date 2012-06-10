using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;
using System.Xml;

namespace Radgie.Input.Device.Mouse
{
    /// <summary>
    /// Dispositivo Mouse.
    /// Solo existe un dispositivo Mouse en el sistema.
    /// </summary>
    public class Mouse: ADevice<IMouse>, IMouse
    {
        #region Properties
        #region IMouse members

        #region Buttons

        public IDigitalControl LeftButton
        {
            get
            {
                return mLeftButton;
            }
        }
        protected MouseButton mLeftButton;

        public IDigitalControl MiddleButton
        {
            get
            {
                return mMiddleButton;
            }
        }
        protected MouseButton mMiddleButton;

        public IDigitalControl RightButton
        {
            get
            {
                return mRightButton;
            }
        }
        protected MouseButton mRightButton;

        public IDigitalControl X1Button
        {
            get
            {
                return mX1Button;
            }
        }
        protected MouseButton mX1Button;

        public IDigitalControl X2Button
        {
            get
            {
                return mX2Button;
            }
        }
        protected MouseButton mX2Button;

        #endregion

        #region Position

        public IPositionControl Position
        {
            get
            {
                return mPosition;
            }
        }
        protected MousePosition mPosition;

        #endregion

        #region Scroll

        public IScrollControl Wheel
        {
            get
            {
                return mWheel;
            }
        }
        protected MouseWheel mWheel;

        #endregion

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Mouse.IMouse.State"/>
        /// </summary>
        public MouseState State
        {
            get
            {
                return mState;
            }
        }
        private MouseState mState;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Mouse.IMouse.PreviousState"/>
        /// </summary>
        public MouseState PreviousState
        {
            get
            {
                return mPreviousState;
            }
        }
        private MouseState mPreviousState;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un dispositivo de tipo Mouse.
        /// </summary>
        /// <param name="pIndex">Jugador con el que esta relacionado.</param>
        public Mouse(PlayerIndex pIndex): base(pIndex)
        {
            AddDevice(pIndex, this);
            mControls.Add(mLeftButton = new MouseButton(this, delegate(MouseState state) { return state.LeftButton; }));
            mControls.Add(mRightButton = new MouseButton(this, delegate(MouseState state) { return state.RightButton; }));
            mControls.Add(mMiddleButton = new MouseButton(this, delegate(MouseState state) { return state.MiddleButton; }));
            mControls.Add(mX1Button = new MouseButton(this, delegate(MouseState state) { return state.XButton1; }));
            mControls.Add(mX2Button = new MouseButton(this, delegate(MouseState state) { return state.XButton2; }));
            mControls.Add(mPosition = new MousePosition(this, delegate(MouseState state) { return new Vector2(state.X, state.Y); }));
            mControls.Add(mWheel = new MouseWheel(this, delegate(MouseState state) { return state.ScrollWheelValue; }));
            mState = mPreviousState = Microsoft.Xna.Framework.Input.Mouse.GetState();
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
            mState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.ADevice.HasChanged"/>
        /// </summary>
        public override bool HasChanged()
        {
            return mState != mPreviousState;
        }
        #endregion

        #region IMouse members

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Mouse.IMouse.SetPosition"/>
        /// </summary>
        public void SetPosition(int x, int y)
        {
            Microsoft.Xna.Framework.Input.Mouse.SetPosition(x, y);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Device.Mouse.IMouse.SetPosition"/>
        /// </summary>
        public void SetPosition(Vector2 position)
        {
            SetPosition((int)position.X, (int)position.Y);
        }
        #endregion

        /// <summary>
        /// Devuelve una instancia del Mouse.
        /// </summary>
        /// <returns>Instancia del Mouse</returns>
        public static IMouse Get()
        {
            return Get(PlayerIndex.One);
        }

        #endregion
    }
}
