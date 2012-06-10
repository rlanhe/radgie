using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de los objetos que definen un sistema.
    /// Un sistema es el encargado de gestionar y mantener un conjunto de objetos relacionados por su funcionalidad. Dicho de otra manera, existira un sistema de grafico,
    /// otro de sonido, otro de fisica, etc.
    /// </summary>
    public interface ISystem: IUpdateable
    {
        #region Properties
        /// <summary>
        /// Indica la ultima vez que fue actualizado el sistema.
        /// </summary>
        GameTime LastTimeUpdated { get; }
        #endregion
    }
}
