using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de los objetos que especifican como deben ser dibujados.
    /// </summary>
    public interface IDraw
    {
        #region Methods
        /// <summary>
        /// Dibuja el objeto.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        void Draw(IRenderer renderer);
        #endregion
    }
}
