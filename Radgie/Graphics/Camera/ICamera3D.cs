using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Camera
{
    /// <summary>
    /// Interfaz de la camara 3D.
    /// </summary>
    public interface ICamera3D: ICamera
    {
        #region Properties
        /// <summary>
        /// Target de la camara.
        /// Posicion a la que mira.
        /// </summary>
        Vector3 Target {get; set;}
        /// <summary>
        /// Vector Up de la camara.
        /// </summary>
        Vector3 Up { get; set; }
        #endregion
    }
}
