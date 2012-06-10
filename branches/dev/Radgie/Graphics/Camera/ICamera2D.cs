using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Camera
{
    /// <summary>
    /// Interfaz de una camara 2D.
    /// </summary>
    public interface ICamera2D: ICamera
    {
        #region Properties
        /// <summary>
        /// Vector Up de la camara.
        /// </summary>
        Vector2 Up { get; set; }
        #endregion
    }
}
