using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Control que representa un boton del Gamepad
    /// </summary>
    public class GamepadButton : ADeviceControl<IGamepad>, IDigitalControl
    {
        #region Delegates and events
        /// <summary>
        /// Definicion del delegado usado para actualizar el estado del contro.
        /// </summary>
        /// <param name="state">Estado del dispositivo.</param>
        /// <returns>Estado del boton.</returns>
        public delegate ButtonState GamepadButtonDelegate(GamePadState state);
        #endregion

        #region Properties
        #region IDigitalControl members
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.Pressed"/>
        /// </summary>
        public bool Pressed
        {
            get
            {
                return GetValue(mGetter(mParent.State));
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.PreviousValue"/>
        /// </summary>
        public bool PreviousValue
        {
            get
            {
                return GetValue(mGetter(mParent.PreviousState));
            }
        }
        #endregion

        /// <summary>
        /// Delegado usado para actualizar el estado del control.
        /// </summary>
        protected GamepadButtonDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa un dispositivo de tipo Gamepad.
        /// </summary>
        /// <param name="parent">Dispositivo que contiene al control.</param>
        /// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
        public GamepadButton(IGamepad parent, GamepadButtonDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el valor del estado del control.
        /// </summary>
        /// <param name="state">Estado del boton.</param>
        /// <returns>True si esta pulsado, False en caso contrario.</returns>
        private bool GetValue(ButtonState state)
        {
            return state == ButtonState.Pressed;
        }
        #endregion
    }
}
