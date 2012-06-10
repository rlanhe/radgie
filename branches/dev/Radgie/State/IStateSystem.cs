using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util.Collection.ReferencePool;
using Radgie.Util.Collection.Context;
using Radgie.Core;

namespace Radgie.State
{
    /// <summary>
    /// Sistema de estados.
    /// Controla la logica del juego. Permite al usuario personalizar el juego mediante la programacion de estados y las transicciones entre estos.
    /// </summary>
    public interface IStateSystem: Core.ISystem, IHasContext
    {
        #region Properties
        /// <summary>
        /// Estadisticas del sistema de estados.
        /// </summary>
        StateSystemStatistics Statistics { get; }
        /// <summary>
        /// Pool con las entidades del sistema de estados que deben ser actualizadas periodicamente.
        /// </summary>
        IReferencePool<IStateEntity> StateEntityReferences { get; }
        /// <summary>
        /// Contexto principal del sistema de estados.
        /// </summary>
        new IContext Context { get; }
        #endregion
    }
}
