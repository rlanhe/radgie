using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Control
{
    /// <summary>
    /// Interfaz de un control de posicion.
    /// Determina las coordenadas del cursor. La esquina superior izquierda de la pantalla es el (0,0).
    /// </summary>
    public interface IPositionControl: IControl
    {
        #region Properties
        /// <summary>
        /// Coordenada X.
        /// Valor >= 0.
        /// </summary>
        int X { get; }
        /// <summary>
        /// Coordenada Y.
        /// Valor >= 0.
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Coordenada anterior X.
        /// Valor >= 0.
        /// </summary>
        int PreviousX { get; }
        /// <summary>
        /// Coordenada anterior Y.
        /// Valor >= 0.
        /// </summary>
        int PreviousY { get; }
        #endregion
    }
}
