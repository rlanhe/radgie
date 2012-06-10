using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Util.Collection.Context;

namespace Radgie.State
{
	/// <summary>
	/// Interfaz de los estados.
	/// </summary>
    public interface IState : IUpdateable
	{
        #region Properties
        /// <summary>
        /// Owner del estado.
        /// Entidad que contiene al estado. Puede que no lo contenga directamente, si no que contenga una maquina de estados entre los que esta este.
        /// </summary>
        IStateEntity Owner { get; set; }
        #endregion

		#region Methods
		/// <summary>
		/// Metodo a ejecutar cuando se entra en el estado.
		/// </summary>
        void OnEntry();
		/// <summary>
		/// Metodo a ejecutar cuando se sale del estado.
		/// </summary>
        void OnExit();
		#endregion
	}
}
