using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Sound
{
    /// <summary>
    /// Estadisticas del sistema de sonido.
    /// </summary>
    public class SoundSystemStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Numero de items en el pool de sonidos a actualizar por el sisteam.
        /// </summary>
        public long NumberOfItemsInPool { get; set; }
        /// <summary>
        /// Numero de items en el pool de sonidos que realmente se actualizan por el sistema.
        /// </summary>
        public long NumberOfItemsInPoolToUpdate { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Resetea los contadores de las estadisticas del sistema.
        /// </summary>
        public void Reset()
        {
            NumberOfItemsInPool = 0;
            NumberOfItemsInPoolToUpdate = 0;
        }
        #endregion
    }
}
