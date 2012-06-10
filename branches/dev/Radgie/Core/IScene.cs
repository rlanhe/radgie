using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Core.BoundingVolumes;
using Radgie.Core.Collision;

namespace Radgie.Core
{
	/// <summary>
	/// Interfaz de una escena de Radgie.
    /// Una escena es el elemento central del motor. Es la estructura de datos de la cual cuelgan todos los objetos del juego. Facilita la realizacion de tareas tan diversas 
    /// como el calculo del colisiones, el dibujado de la escena o la agrupacion de objetos del juego.
	/// </summary>
    public interface IScene : IIdentifiable, IUpdateable, IActivable, IHasContext
    {
        #region Properties
        /// <summary>
        /// Lista con los grupos de colision de la escena.
        /// </summary>
        List<ICollisionGroup> CollisionGroups { get; }
        /// <summary>
        /// Delegado para la actualizacion de la escena.
        /// </summary>
        Job UpdateJob { get; }
        #endregion

        #region Methods
        /// <summary>
		/// Annade un componente a la escena para que sea gestionado por ella.
		/// </summary>
		/// <param name="gc">Componente a annadir</param>
		/// <returns>True o False en funcion del resultado de la operacion.</returns>
        bool AddComponent(IGameComponent gc);
		/// <summary>
		/// Quita un componente de la escena.
		/// </summary>
		/// <param name="gc">Componente a quitar</param>
		/// <returns>True o False en funcion del resultado de la operacion.</returns>
        bool RemoveComponent(IGameComponent gc);
        /// <summary>
        /// Devuelve una lista con todos los objetos de un tipo determinado que contiene la escena
        /// </summary>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con referencias a los objetos de la escena del tipo pedido</returns>
        List<Y> GetGameObjects<Y>(bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve una lista con todos los objetos de un tipo determinado que contiene la escena
        /// </summary>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <param name="results">Lista donde dejar los resulstados</param>
        void GetGameObjects<Y>(bool includeInvisibleObjects, List<Y> results);
        /// <summary>
        /// Devuelve una lista con todos los objetos de un tipo determinado que contiene la escena
        /// </summary>
        /// <param name="frustum">Frustum de camara</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con referencias a los objetos de la escena del tipo pedido</returns>
        List<Y> GetGameObjects<Y>(IBoundingVolume frustum, bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve una lista con todos los objetos de un tipo determinado que contiene la escena
        /// </summary>
        /// <param name="frustum">Frustum de camara</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <param name="results">Lista de resultados</param>
        /// <returns>Lista con referencias a los objetos de la escena del tipo pedido</returns>
        void GetGameObjects<Y>(IBoundingVolume frustum, bool includeInvisibleObjects, List<Y> results);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena.
        /// </summary>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con todos los GameComponents.</returns>
        List<IGameComponent> GetGameComponents(bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena.
        /// </summary>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <param name="results">Lista de resultados</param>
        /// <returns>Lista con todos los GameComponents.</returns>
        void GetGameComponents(bool includeInvisibleObjects, List<IGameComponent> results);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena que son visibles.
        /// </summary>
        /// <param name="frustum">Frustum de camara</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <returns>Lista con todos los GameComponents que son visibles.</returns>
        List<IGameComponent> GetGameComponents(IBoundingVolume frustum, bool includeInvisibleObjects);
        /// <summary>
        /// Devuelve la lista de todos los GameComponents de la escena que son visibles.
        /// </summary>
        /// <param name="frustum">Frustum de camara</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        /// <param name="results">Lista de resultados</param>
        void GetGameComponents(IBoundingVolume frustum, bool includeInvisibleObjects, List<IGameComponent> results);
        
        /// <summary>
        /// Crea un nuevo grupo de colision a la escena.
        /// </summary>
        /// <param name="id">Id del nuevo grupo de colision.</param>
        /// <returns>Nuevo grupo de colision o null en caso de que ya exista uno con el mismo Id</returns>
        ICollisionGroup AddNewCollisionGroup(string id);

        /// <summary>
        /// Obtiene un grupo de colision a traves del nombre.
        /// </summary>
        /// <param name="id">Identificador del grupo de colision.</param>
        /// <returns>Grupo de colision requerido.</returns>
        ICollisionGroup GetCollisionGroup(string id);

        /// <summary>
        /// Elimina un grupo de colision.
        /// </summary>
        /// <param name="collisionGroup">Grupo de colision a eliminar.</param>
        void RemoveCollisionGroup(ICollisionGroup collisionGroup);

        /// <summary>
        /// Indica si los objetos de dos nodos de escena pueden colisionar entre si.
        /// </summary>
        /// <param name="node1">Nodo 1</param>
        /// <param name="node2">Nodo 2</param>
        /// <returns>True si pueden colisionar, False en caso contrario.</returns>
        bool CanCollide(ISceneNode node1, ISceneNode node2);
		#endregion
	}
}
