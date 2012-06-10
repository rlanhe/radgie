using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.State
{
    /// <summary>
    /// Entidad del sistema de estados.
    /// </summary>
    public abstract class AStateEntity: AEntity, IStateEntity
    {
        #region Constructors
        /// <summary>
        /// Crea y annade la entidad a la lista de entidades a actualizar por el sistema de estados.
        /// </summary>
        public AStateEntity()
        {
            // TODO: la maquina de estados principal tambien se esta annadiendo a esta lista. Se esta actualizando dos veces por frame??
            ((IStateSystem)RadgieGame.Instance.GetSystem(typeof(IStateSystem))).StateEntityReferences.Add(this);
        }
        #endregion
    }
}
