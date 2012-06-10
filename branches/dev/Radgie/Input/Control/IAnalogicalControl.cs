using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Input.Control
{
    /// <summary>
    /// Interfaz para un controlador analogico.
    /// </summary>
    public interface IAnalogicalControl: IControl
    {
        #region Properties
        /// <summary>
        /// Valor del controlador.
        /// El valor de este tipo de controladores esta comprendido entre 0 y 1.
        /// </summary>
        float Value { get; }
        /// <summary>
        /// Valor del controlador en la actualizacion anterior.
        /// El valor de este tipo de controladores esta comprendido entre 0 y 1.
        /// </summary>
        float PreviousValue { get; }
        #endregion
    }
}
