using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Util.Collection.Context
{
    /// <summary>
    /// Contenedor generico de objetos al que asociar un nombre.
    /// </summary>
    public class Variable: IIdentifiable
    {
        #region Properties
        /// <summary>
        /// Ver <see cref="Radgie.Core.IIdentifiable.Id"/>
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
        }
        private string mId;

        /// <summary>
        /// Valor de la variable.
        /// </summary>
        private object mValue;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva variable.
        /// </summary>
        /// <param name="id">Identificador de la variable.</param>
        public Variable(string id)
        {
            mId = id;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el objeto que encapsula la variable.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto buscado.</typeparam>
        /// <returns>Objeto buscado.</returns>
        public T Get<T>()
        {
            return (T)mValue;
        }

        /// <summary>
        /// Establece el objeto de una variable.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto.</typeparam>
        /// <param name="value">Objeto.</param>
        public void Set<T>(T value)
        {
            mValue = value;
        }
        #endregion
    }
}
