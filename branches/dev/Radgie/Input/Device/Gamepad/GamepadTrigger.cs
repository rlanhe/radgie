using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Dispositivo Trigger de un Gamepad.
    /// </summary>
    public class GamepadTrigger : ADeviceControl<IGamepad>, IAnalogicalControl
    {
        #region Delegates and Events.
        public delegate float GamepadTriggerDelegate(GamePadState state);
        #endregion

        #region Properties
        #region IAnalogicalControl members
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.Value"/>
        /// </summary>
        public float Value
        {
            get
            {
                return mGetter(mParent.State);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.PreviousValue"/>
        /// </summary>
        public float PreviousValue
        {
            get
            {
                return mGetter(mParent.PreviousState);
            }
        }
        #endregion

        protected GamepadTriggerDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el control.
        /// </summary>
        /// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
        public GamepadTrigger(IGamepad parent, GamepadTriggerDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion
    }
}
