using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de los entidades graficas.
    /// </summary>
    public interface IGraphicEntity: IEntity, IDrawable
    {
        #region Properties
        /// <summary>
        /// Objeto encargado de dibujar las instancias de la entidad.
        /// </summary>
        IEntityInstancesRenderer InstancesRenderer { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Dibuja una instancia de la entidad.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        void DrawInstance(IRenderer renderer);
        /// <summary>
        /// Calcula el boundingVolume del objeto.
        /// </summary>
        /// <returns>BoundingVolume</returns>
        IBoundingVolume CalculateBoundingVolume();
        #endregion
    }
}
