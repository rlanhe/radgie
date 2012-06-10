using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.State
{
    /// <summary>
    /// Estadisticas del sistema de estados.
    /// </summary>
    public class StateSystemStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Numero de objetos en el pool de objetos a actualizar por el sistema de estados.
        /// </summary>
        public long NumberOfStateObjectsInPool { get; set; }
        /// <summary>
        /// Numero de objetos que acutaliza el sistema de estados.
        /// </summary>
        public long NumberOfStateObjectsToUpdateInPool { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Resetea los contadores de las estadisticas del sistema de estados.
        /// </summary>
        public void Reset()
        {
            NumberOfStateObjectsInPool = 0;
            NumberOfStateObjectsToUpdateInPool = 0;
        }
        #endregion
    }
}
