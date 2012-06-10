using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz vacio para la seleccion rapdia de objetos de debug.
    /// Los objetos de debug son aquellos utlizados para mostrar elementos no visibles durante el juego pero afectan a su comportamiento, como por ejemplo los volumenes de colision.
    /// </summary>
    public interface IDebugObject: IGameObject
    {
    }
}
