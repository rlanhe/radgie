using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Keyboard
{
    /// <summary>
    /// Control que representa una tecla del teclado
    /// </summary>
    public class KeyboardKey : ADeviceControl<IKeyboard>, IDigitalControl
    {
        #region Delegates and events
        /// <summary>
        /// Definicion del delegado para la actualizacion del estado de la tecla.
        /// </summary>
        /// <param name="state">Estado del teclado.</param>
        /// <returns>True si esta pulsado, False en caso contrario</returns>
        public delegate bool KeyboardButtonDelegate(KeyboardState state);
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
                return mGetter(mParent.State);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.PreviousValue"/>
        /// </summary>
        public bool PreviousValue
        {
            get
            {
                return mGetter(mParent.PreviousState);
            }
        }
        #endregion

        /// <summary>
        /// Delegado usado para actualizar el estado del control.
        /// </summary>
        protected KeyboardButtonDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa un dispositivo de tipo Keyboard.
        /// </summary>
        /// <param name="parent">Dispositivo padre del control.</param>
        /// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
        public KeyboardKey(IKeyboard parent, KeyboardButtonDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion
    }
}
