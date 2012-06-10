using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de las instancias de entidades.
    /// Una instancia es una referencia a una entidad que puede ser reusada desde otro GameComponent distinto al que esta asociada la entidad.
    /// </summary>
    public interface IInstance: IGameObject, IDisposable
    {
        #region Properties
        /// <summary>
        /// Entidad a la que hace referencia.
        /// </summary>
        IEntity Entity { get; }
        #endregion
    }
}
