using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Objeto que se puede identificar por su nombre.
    /// </summary>
    public interface IIdentifiable
    {
        #region Properties
        /// <summary>
        /// Identificador del objeto
        /// </summary>
        string Id { get; }
        #endregion
    }
}
