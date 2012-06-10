using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util.Collection.Context;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de los objetos que tienen contexto.
    /// El contexto es un punto de intercambio de informacion para compartir informacion entre varios objetos. Se utiliza para annadir de manera dinamica propiedades a los
    /// objetos de escena.
    /// </summary>
    public interface IHasContext
    {
        #region Properties
        /// <summary>
        /// Contexto del objeto.
        /// </summary>
        IContext Context { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene una variable a partir del nombre.
        /// Si la variable no existe en el contexto de la maquina de estados, busca en las maquinas de estado padre.
        /// </summary>
        /// <param name="id">Id de la variable.</param>
        /// <returns>Variable buscada.</returns>
        Variable GetFromContext(string id);
        /// <summary>
        /// Obtiene un objeto a partir de su nombre.
        /// Si la variable no existe en el contexto de la maquina de estados, busca en las maquinas de estado padre.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto.</typeparam>
        /// <param name="id">Id del objeto.</param>
        /// <returns>Objeto buscado.</returns>
        T GetFromContext<T>(string id);
        /// <summary>
        /// Establece una variable dentro del contexto.
        /// </summary>
        /// <param name="id">Id de la variable.</param>
        /// <param name="value">Valor de la variable.</param>
        void SetInContext(string id, Variable value);
        /// <summary>
        /// Establece un objeto dentro del contexto.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto.</typeparam>
        /// <param name="id">Id del objeto dentro del contexto.</param>
        /// <param name="value">Objeto</param>
        void SetInContext<T>(string id, T value);
        /// <summary>
        /// Elimina una entrada del contexto.
        /// </summary>
        /// <param name="id">Nombre de la entrada del contexto.</param>
        /// <returns>True si la elimina, False en caso contrario.</returns>
        bool RemoveFromContext(string id);
        #endregion
    }
}
