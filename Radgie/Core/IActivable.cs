using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de los objetos que son activables/desactivables.
    /// </summary>
    public interface IActivable
    {
        #region Properties
        /// <summary>
        /// Activa/Desactiva el objeto.
        /// </summary>
        bool Active { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Comprueba si el objeto esta activo.
        /// Aunque el objeto se haya marcado como activo, puede ser que no lo este (p.e. su padre esta desactivado)
        /// </summary>
        /// <returns>True si esta activo, False en caso contrario.</returns>
        bool IsActive();
        #endregion
    }
}
