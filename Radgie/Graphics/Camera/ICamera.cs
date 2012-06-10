using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Graphics.Camera
{
    /// <summary>
    /// Interfaz generico para una camara.
    /// </summary>
    public interface ICamera : Radgie.Core.IGameComponent
    {
        #region Properties
        /// <summary>
        /// Viewport de la camara.
        /// </summary>
        Viewport Viewport { get; set; }
        /// <summary>
        /// Distancia al plano near de la camara.
        /// </summary>
        float NearPlaneDistance { get; set; }
        /// <summary>
        /// Distancia al plano Far de la camara.
        /// </summary>
        float FarPlaneDistance { get; set; }
        /// <summary>
        /// Campo de vision de la camara.
        /// En radianes.
        /// </summary>
        float FieldOfView { get; set; }
        /// <summary>
        /// Color de fondo de la camara.
        /// </summary>
        Color BackgroundColor { get; set; }
        /// <summary>
        /// Matriz de vista de la camara.
        /// </summary>
        Matrix View { get; }
        /// <summary>
        /// Matriz de proyeccion de la camara.
        /// </summary>
        Matrix Projection { get; }
        #endregion
    }
}
