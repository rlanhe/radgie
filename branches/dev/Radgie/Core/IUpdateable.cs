using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de objetos que necesitan ser actualizados de manera automatica por el motor cada cierto tiempo.
    /// </summary>
    public interface IUpdateable
    {
        #region Methods
        /// <summary>
        /// Accion que se ejecuta cada vez que se actualiza el objeto.
        /// </summary>
        /// <param name="time">Tiempo desde la ultima actualizacion.</param>
        void Update(GameTime time);
        #endregion
    }
}
