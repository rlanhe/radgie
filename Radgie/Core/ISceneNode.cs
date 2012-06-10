using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Core
{
	/// <summary>
	/// Interfaz del nodo de una escena.
	/// </summary>
	public interface ISceneNode: Radgie.Core.IUpdateable, IGameComponentObserver
    {
        #region Properties
        /// <summary>
        /// Numero de componentes que cuelgan del nodo y de sus hijos
        /// </summary>
        int ComponentsCount { get; }
        /// <summary>
        /// Nodo padre en la escena.
        /// </summary>
        ISceneNode Parent { get; }
        #endregion
        #region Methods
        /// <summary>
		/// Annade un componente al nodo.
		/// </summary>
		/// <param name="gc">Componente a annadir</param>
		/// <returns>True o False en funcion del resultado de la operacion.</returns>
        bool AddComponent(Radgie.Core.IGameComponent gc);
		/// <summary>
		/// Quita un componente del nodo.
		/// </summary>
		/// <param name="gc">Componente a quitar</param>
		/// <returns>True o False en funcion del resultado de la operacion.</returns>
        bool RemoveComponent(Radgie.Core.IGameComponent gc);
        /// <summary>
        /// Obtiene los GameObjects de un tipo determinado que contiene el nodo de escena.
        /// </summary>
        /// <param name="results">Lista donde dejar las referencias encontradas</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        void GetGameObjects<Y>(List<Y> results, bool includeInvisibleObjects);
        /// <summary>
        /// Obtiene los GameObjects de un tipo determinado que contiene el nodo de escena.
        /// </summary>
        /// <param name="frustum">frustum de la camara</param>
        /// <param name="results">Lista donde dejar las referencias encontradas</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        void GetGameObjects<Y>(IBoundingVolume frustum, List<Y> results, bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena.
        /// </summary>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con todos los GameComponents.</returns>
        void GetGameComponents(List<Radgie.Core.IGameComponent> results, bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena que son visibles
        /// </summary>
        /// <param name="frustum">frustum de la camara</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con todos los GameComponents que son visibles.</returns>
        void GetGameComponents(IBoundingVolume frustum, List<Radgie.Core.IGameComponent> results, bool includeInvisibleObjects);
        /// <summary>
        /// Indica si el nodo de escena es visible
        /// </summary>
        /// <param name="frustum">frustum de la camara</param>
        /// <returns>true si es visible, false en caso contrario</returns>
        bool IsNodeVisible(IBoundingVolume frustum);
		#endregion
	}
}
