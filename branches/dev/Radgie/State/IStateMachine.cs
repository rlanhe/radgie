using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Util.Collection.Context;

namespace Radgie.State
{
	/// <summary>
	/// Interfaz de una maquina de estados.
	/// </summary>
    public interface IStateMachine: IState, IHasContext
    {
        #region Properties
        /// <summary>
        /// ultimo evento que se produjo durante la ejecucion de la maquina de estados.
        /// </summary>
        Event LastEvent { get; }
        #endregion

        #region Methods
        /// <summary>
		/// Metodo para la comunicacion de los estados hijos con la maquina de estados.
		/// </summary>
		/// <param name="ev">Evento producido dentro del estado al que debe responder la propia maquina.</param>
		void CastEvent(Event ev);
		#endregion
	}
}
