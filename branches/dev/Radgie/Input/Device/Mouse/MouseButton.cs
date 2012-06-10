using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Mouse
{
    /// <summary>
    /// Control que representa un boton.
    /// </summary>
    public class MouseButton : ADeviceControl<IMouse>, IDigitalControl
    {
        #region Delegates and Events
        /// <summary>
        /// Definicion del delegado para la actualizacion de controles de tipo MouseButton.
        /// </summary>
        /// <param name="state">Estado del dispositivo.</param>
        /// <returns>Estado del boton.</returns>
        public delegate ButtonState MouseButtonDelegate(MouseState state);
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
        protected MouseButtonDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el control.
        /// </summary>
        /// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
        public MouseButton(IMouse parent, MouseButtonDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Devuelvel el estado del boton en formato boolean.
        /// </summary>
        /// <param name="state">Estado del boton.</param>
        /// <returns>True si esta presionado, False en caso contrario.</returns>
        private bool GetValue(ButtonState state)
        {
            return (state == ButtonState.Pressed);
        }
        #endregion
    }
}
