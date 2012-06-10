using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Control que representa un control de direccion del Gamepad.
    /// </summary>
    public class GamepadDPad: ADeviceControl<IGamepad>
    {
        #region Properties
        #region Buttons

        public IDigitalControl Left
        {
            get
            {
                return mLeft;
            }
        }
        protected GamepadButton mLeft;

        public IDigitalControl Right
        {
            get
            {
                return mRight;
            }
        }
        protected GamepadButton mRight;

        public IDigitalControl Up
        {
            get
            {
                return mUp;
            }
        }
        protected GamepadButton mUp;

        public IDigitalControl Down
        {
            get
            {
                return mDown;
            }
        }
        protected GamepadButton mDown;

        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo conrol de direccion.
        /// </summary>
        /// <param name="parent">Dispositivo que contiene al control.</param>
        public GamepadDPad(IGamepad parent): base(parent)
        {
            mLeft = new GamepadButton(parent, delegate(GamePadState state) { return state.DPad.Left; });
            mRight = new GamepadButton(parent, delegate(GamePadState state) { return state.DPad.Right; });
            mUp = new GamepadButton(parent, delegate(GamePadState state) { return state.DPad.Up; });
            mDown = new GamepadButton(parent, delegate(GamePadState state) { return state.DPad.Down; });
        }
        #endregion
    }
}
