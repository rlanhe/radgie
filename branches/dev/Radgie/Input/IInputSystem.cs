using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Input
{
    /// <summary>
    /// Interfaz del sistema de dispositivos de entrada.
    /// </summary>
    public interface IInputSystem: Core.ISystem
    {
        #region Properties
        /// <summary>
        /// Estadisticas del sistema de input.
        /// </summary>
        InputSystemStatistics Statistics { get; }
        #endregion
    }
}
