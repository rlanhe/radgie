using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util.Collection.ReferencePool
{
    /// <summary>
    /// Metodo que se llama por cada elemento de IReferencePool.
    /// </summary>
    /// <typeparam name="T">Tipo de los elementos de la coleccion.</typeparam>
    /// <param name="target">Elemento de la coleccion.</param>
    public delegate void PoolAction<T>(T target);

    /// <summary>
    /// Interfaz de la coleccion IReferencePool.
    /// Almacena WeakReferences de los elementos que se annaden.
    /// Estas referencias son eliminadas cuando el objeto al que referencia ya no existe.
    /// </summary>
    /// <typeparam name="T">Tipo de objetos que se introduciran en la coleccion.</typeparam>
    public interface IReferencePool<T>
    {
        #region Methods
        /// <summary>
        /// Annade un item a la coleccion.
        /// </summary>
        /// <param name="item">Nuevo item.</param>
        void Add(T item);
        /// <summary>
        /// Llama a action por cada uno de los elementos de la coleccion.
        /// </summary>
        /// <param name="action">Delegado para actualizar los elementos de la coleccion.</param>
        void FireActionOverPoolItems(PoolAction<T> action);
        #endregion
    }
}
