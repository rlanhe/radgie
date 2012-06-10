using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de un renderizador de instancias.
    /// </summary>
    public interface IEntityInstancesRenderer : IDrawable
    {
        #region Properties
        /// <summary>
        /// Entidad de la que se dibujan distancias.
        /// </summary>
        IGraphicEntity Entity { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Annade una instancia a la lista de instancias a dibujar.
        /// </summary>
        /// <param name="instance">Nueva instancia.</param>
        void AddInstance(IGraphicInstance instance);
        #endregion
    }
}
