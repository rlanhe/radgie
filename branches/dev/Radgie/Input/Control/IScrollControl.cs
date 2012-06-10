using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Input.Control
{
    /// <summary>
    /// Interfaz para una rueda de desplazamiento.
    /// </summary>
    public interface IScrollControl : IControl
    {
        #region Properties
        /// <summary>
        /// Valor del controlador.
        /// </summary>
        int Value { get; }
        /// <summary>
        /// Valor del controlador en la actualizacion anterior.
        /// </summary>
        int PreviousValue { get; }
        #endregion
    }
}
