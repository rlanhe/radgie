using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input
{
    /// <summary>
    /// Clase base de un control de un dispositivo.
    /// </summary>
    public abstract class ADeviceControl<T> where T: IDevice
    {
        #region Properties

        /// <summary>
        /// Dispositivo que contiene a el control.
        /// </summary>
        public T Parent
        {
            get
            {
                return mParent;
            }
        }
        protected T mParent;

        #endregion

        #region Constructors

        /// <summary>
        /// Crea un control de un dispositivo.
        /// </summary>
        /// <param name="parent">Dispositivo que lo contiene.</param>
        public ADeviceControl(T parent)
        {
            mParent = parent;
        }

        #endregion
    }
}
