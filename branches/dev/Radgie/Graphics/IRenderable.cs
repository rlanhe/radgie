using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics.Camera;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de los objetos implicados en el renderizado grafico.
    /// </summary>
    public interface IRenderable
    {
        #region Properties
        /// <summary>
        /// Camera a usar en el renderizado.
        /// </summary>
        ICamera Camera { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Metodo de renderizado.
        /// </summary>
        void Render(IRenderer renderer);
        #endregion
    }
}
