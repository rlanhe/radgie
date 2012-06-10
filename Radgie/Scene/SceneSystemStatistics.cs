using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Scene
{
    /// <summary>
    /// Estadisticas del sistema de escenas.
    /// </summary>
    public class SceneSystemStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Numero de escenas.
        /// </summary>
        public long NumberOfScenes { get; set; }
        /// <summary>
        /// Numero de escenas a actualizar.
        /// </summary>
        public long NumberOfScenesToUpdate { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Resetea los contadores de la estadisticas del sistema de escenas.
        /// </summary>
        public void Reset()
        {
            NumberOfScenes = 0;
            NumberOfScenesToUpdate = 0;
        }
        #endregion
    }
}
