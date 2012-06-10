using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de un GameComponent.
    /// Componente del juego. Un componente del juego agrupa varios objetos del juego (grafico, sonido, fisica, etc) en un mismo objeto que se considera una entidad.
    /// A su ver puede contener jerarquias de componentes de juego. Cualquier objeto que quiera ser dibujado debe annadirse a un componente y este annadirse a una
    /// escena para que sea tenido en cuenta.
    /// </summary>
    public interface IGameComponent: IUpdateable, IActivable, IHasContext, IIdentifiable
    {
        #region Properties
        /// <summary>
        /// Escena a la que pertence el GameComponent.
        /// </summary>
        IScene Scene { get; }

        /// <summary>
        /// Component padre.
        /// Si es null, se trata de un GameComponent root.
        /// </summary>
        IGameComponent Parent { get; }

        /// <summary>
        /// Transformacion de mundo.
        /// </summary>
        Matrix World { get; }

        /// <summary>
        /// Transformacion local.
        /// </summary>
        Transformation Transformation { get; set; }

        /// <summary>
        /// GameObjects que maneja este GameComponent
        /// </summary>
        IEnumerator<IGameObject> GameObjects { get; }

        /// <summary>
        /// GameComponents que maneja este GameComponent
        /// </summary>
        IEnumerator<IGameComponent> GameComponents { get; }

        /// <summary>
        /// Volumen de colision.
        /// </summary>
        IBoundingVolume BoundingVolume { get; set; }

        /// <summary>
        /// Indica si el objeto es visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Indica el nodo al que pertenece en la escena el GameComponent.
        /// </summary>
        ISceneNode ParentNode { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Annade un observador de este GameComponent.
        /// Un observador es avisado cuando se produce un cambio en la posicion del componente o en el padre de este.
        /// </summary>
        /// <param name="observer">Observador.</param>
        void AddObserver(IGameComponentObserver observer);
        /// <summary>
        /// Quita un observador del GameComponent.
        /// </summary>
        /// <param name="observer">Observador a quitar.</param>
        void RemoveObserver(IGameComponentObserver observer);

        /// <summary>
        /// Annade un GameComponent como hijo del actual.
        /// </summary>
        /// <param name="child">Componente hijo.</param>
        /// <returns>True si se inserta, False en caso contrario.</returns>
        bool AddGameComponent(IGameComponent child);
        /// <summary>
        /// Quita un GameComponent hijo del actual.
        /// </summary>
        /// <param name="child">Componente hijo.</param>
        /// <returns>True si lo quita, False en caso contrario.</returns>
        bool RemoveGameComponent(IGameComponent child);

        /// <summary>
        /// Annade un GameObject a este GameComponent.
        /// Si el GameObject ya estaba annadido a otro GameComponent, se desvincula de este antes de annadirse a este.
        /// </summary>
        /// <param name="gameObject">GameObject.</param>
        /// <returns>True si se annade, False si ya estaba annadido.</returns>
        bool AddGameObject(IGameObject gameObject);
        /// <summary>
        /// Quita un GameObject de este GameComponent.
        /// </summary>
        /// <param name="gameObject">GameObject.</param>
        /// <returns>True si se annade, False si ya esta annadido.</returns>
        bool RemoveGameObject(IGameObject gameObject);

        /// <summary>
        /// Obtiene los GameObjects de un tipo determinado que contiene el nodo de escena.
        /// </summary>
        /// <param name="results">Lista donde dejar las referencias encontradas</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        void GetGameObjects<Y>(List<Y> results, bool includeInvisibleObjects);

        /// <summary>
        /// Obtiene todos los GameObjects que cuelgan del GameComponent que estan dentro del frustum de la camara.
        /// </summary>
        /// <typeparam name="Y">Tipo de objetos buscados</typeparam>
        /// <param name="frustum">Frustum de la camara.</param>
        /// <param name="results">Lista donde dejar las referencias encontradas.</param>
        /// <param name="includeInvisibleObjects">Indica si debe ignorar los objetos invisibles</param>
        void GetGameObjects<Y>(IBoundingVolume frustum, List<Y> results, bool includeInvisibleObjects);

        /// <summary>
        /// Inicializa el gamecomponent cuando se actualiza por primera vez.
        /// </summary>
        void Initialize();
        #endregion
	}
}
