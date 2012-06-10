using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz de los objetos que se pueden dibujar por el renderer.
    /// </summary>
    public interface IDrawable: IDraw, IComparable<IDrawable>
    {
        #region Properties
        /// <summary>
        /// Valor para ordenar los objetos a la hora de dibujarlos.
        /// Valores mas bajos de esta propiedad hacen que se dibujen primero. En el caso de dibujar objetos con transparencias, estos deben dibujarse en ultimo lugar
        /// para evitar problemas en el resultado final.
        /// </summary>
        int DrawOrder { get; }
        #endregion
    }
}
