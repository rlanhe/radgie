using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de una luz.
    /// </summary>
    public interface ILight
    {
        #region Methods
        /// <summary>
        /// Posicion de la luz.
        /// </summary>
        Vector3 Position { get; }
        /// <summary>
        /// Radio de accion de la luz.
        /// </summary>
        float Radius { get; set; }
        /// <summary>
        /// Factor de atenuacion de la luz.
        /// </summary>
        float FallOff { get; set; }
        /// <summary>
        /// Color de la luz.
        /// </summary>
        Color Color { get; set; }
        #endregion
    }
}
