using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Mouse
{
    /// <summary>
    /// Control para obtener el valor de la rueda de scroll del Mouse
    /// </summary>
    public class MouseWheel: ADeviceControl<IMouse>, IScrollControl
    {
        #region Delegates and Events
        /// <summary>
        /// Definicion del delegado que actualiza el estado del control.
        /// </summary>
        /// <param name="state">Estado del dispositivo.</param>
        /// <returns>Desplazamiento acumulado desde el comienzo de la ejecucion de la aplicacion.</returns>
        public delegate int MouseWheelDelegate(MouseState state);
        #endregion

        #region Properties
        #region IScrollControl
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IScrollControl.Value"/>
        /// </summary>
        public int Value
        {
            get
            {
                return mGetter(mParent.State);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IScrollControl.PreviousValue"/>
        /// </summary>
        public int PreviousValue
        {
            get
            {
                return mGetter(mParent.PreviousState);
            }
        }
        #endregion

        /// <summary>
        /// Delegado para actualizar el valor del control.
        /// </summary>
        protected MouseWheelDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el control.
        /// </summary>
        /// <param name="getter">Delegado para obtener el estado del control a partir del estado del dispositivo</param>
        public MouseWheel(IMouse parent, MouseWheelDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion
    }
}
