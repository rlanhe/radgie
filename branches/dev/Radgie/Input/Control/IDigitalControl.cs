using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Input.Control
{
    /// <summary>
    /// Interfaz de un controlador digital.
    /// </summary>
    public interface IDigitalControl: IControl
    {
        #region Properties
        /// <summary>
        /// Valor del control.
        /// True o false.
        /// </summary>
        bool Pressed { get; }
        /// <summary>
        /// Valor anterior del control
        /// </summary>
        bool PreviousValue { get; }
        #endregion
    }
}
