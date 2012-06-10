using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;

namespace Radgie.Input
{
    /// <summary>
    /// Interfaz de un dispositivo de entrada que se conecta a Radgie.
    /// </summary>
    public interface IDevice: Radgie.Core.IUpdateable
    {
        #region Properties
        
        /// <summary>
        /// Jugador al que esta asociado el dispositivo.
        /// </summary>
        PlayerIndex Index { get; }

        /// <summary>
        /// Tiempo transcurrido entre las dos ultimas actualizaciones del dispositivo.
        /// </summary>
        TimeSpan TimeElapsed { get; }
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Indica si cambio el estado del dispositivo desde la ultima vez que se actualizo.
        /// </summary>
        bool HasChanged();
        
        #endregion
    }
}
