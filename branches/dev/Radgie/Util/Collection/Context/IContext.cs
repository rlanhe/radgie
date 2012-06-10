using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Util.Collection.Context
{
    /// <summary>
    /// Interfaz de un contexto.
    /// Un contexto es un contenedor de propiedades dinamicas para un objeto.
    /// </summary>
    public interface IContext
    {
        #region Properties
        /// <summary>
        /// Objeto propietario del contexto.
        /// </summary>
        IHasContext Owner { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene una variable a partir del nombre.
        /// </summary>
        /// <param name="id">Id de la variable.</param>
        /// <returns>Variable buscada.</returns>
        Variable Get(string id);
        /// <summary>
        /// Obtiene un objeto a partir de su nombre.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto.</typeparam>
        /// <param name="id">Id del objeto.</param>
        /// <returns>Objeto buscado.</returns>
        T Get<T>(string id);
        /// <summary>
        /// Establece una variable dentro del contexto.
        /// </summary>
        /// <param name="id">Id de la variable.</param>
        /// <param name="value">Valor de la variable.</param>
        void Set(string id, Variable value);
        /// <summary>
        /// Establece un objeto dentro del contexto.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto.</typeparam>
        /// <param name="id">Id del objeto dentro del contexto.</param>
        /// <param name="value">Objeto</param>
        void Set<T>(string id, T value);
        /// <summary>
        /// Elimina una entrada del contexto.
        /// </summary>
        /// <param name="id">Nombre de la entrada del contexto.</param>
        /// <returns>True si la elimina, False en caso contrario.</returns>
        bool Remove(string id);
        #endregion
    }
}
