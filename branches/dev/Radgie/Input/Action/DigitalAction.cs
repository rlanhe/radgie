using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;

namespace Radgie.Input.Action
{
    /// <summary>
    /// Accion digital a la que asociar acciones.
    /// </summary>
    public class DigitalAction: AAction<IDigitalControl>, IDigitalControl
    {
        #region Properties

        #region IDigitalControl Properties
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.Pressed"/>
        /// </summary>
        public bool Pressed
        {
            get
            {
                foreach (IDigitalControl control in mBindings)
                {
                    if (control.Pressed)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.PreviousValue"/>
        /// </summary>
        public bool PreviousValue
        {
            get
            {
                foreach (IDigitalControl control in mBindings)
                {
                    if (control.PreviousValue)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        #endregion
        #endregion
    }
}
