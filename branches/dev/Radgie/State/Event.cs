using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.State
{
	/// <summary>
	/// Evento que se produce dentro de la ejecucion de un estado.
	/// </summary>
    public class Event: IIdentifiable
	{
		#region Properties
		#region IIdentifiable Members
		/// <summary>
		/// Identificador del evento.
		/// </summary>
		public string Id
        {
            get
            {
                return mId;
            }
        }
        private readonly string mId = null;
		#endregion
		#endregion

		#region Constructors
		/// <summary>
		/// Crea un nuevo evento.
		/// </summary>
		/// <param name="id">Id del evento</param>
		public Event(string id)
        {
            if (id == null)
            {
                throw new NullReferenceException("id is null");
            }
            mId = id;
		}
		#endregion
	}
}
