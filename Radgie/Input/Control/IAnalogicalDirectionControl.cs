using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Control
{
    /// <summary>
    /// Interfaz de un control de direccion analogico.
    /// </summary>
    public interface IAnalogicalDirectionControl: IControl
    {
        #region Properties
        /// <summary>
        /// Coordenada X.
        /// </summary>
        float X { get; }
        /// <summary>
        /// Coordenada Y.
        /// </summary>
        float Y { get; }
        /// <summary>
        /// Coordenada anterior X.
        /// </summary>
        float PreviousX { get; }
        /// <summary>
        /// Coordenada anterior Y.
        /// </summary>
        float PreviousY { get; }
        #endregion
    }
}
