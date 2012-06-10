using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Input
{
    /// <summary>
    /// Estadisticas del sistema de input.
    /// </summary>
    public class InputSystemStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Numero de dispositivos de entrada.
        /// </summary>
        public long NumberOfDevices { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Resetea los contadores de las estadisticas del sistema.
        /// </summary>
        public void Reset()
        {
            NumberOfDevices = 0;
        }
        #endregion
    }
}
