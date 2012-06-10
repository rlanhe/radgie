using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Core.Collision
{
    /// <summary>
    /// Interfaz de grupos de colision.
    /// Un grupo de colision es una relacion de objetos que pueden colisionar entre si. La razon de agrupar los objetos en grupos es optimizar el calculo de las colisiones
    /// al tener que considerar un numero de elementos menor. Cuanto mayor es un grupo mayor es el numero de comprobaciones a realizar (por cada objeto hay que comprobar
    /// si colisiona con cada uno del resto del grupo).
    /// </summary>
    public interface ICollisionGroup: IIdentifiable, IUpdateable
    {
        #region Properties
        /// <summary>
        /// Escena a la que esta asociado.
        /// El grupo de colision solo contiene objetos de esta escena.
        /// </summary>
        IScene Scene { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Annade un GameComponent al grupo de colision.
        /// </summary>
        /// <param name="gc">Nuevo GameComponent.</param>
        void AddGameComponent(IGameComponent gc);
        /// <summary>
        /// Quita un GameComponent del grupo de colision.
        /// </summary>
        /// <param name="gc">GameComponent para quitar.</param>
        void RemoveGameComponent(IGameComponent gc);
        /// <summary>
        /// Devuelve una copia de la lista de GameComponents de este grupo de colision.
        /// </summary>
        List<IGameComponent> GameComponents { get; }
        
        /// <summary>
        /// Obtiene una lista con las colisiones producidas en el ultimo frame para un game component.
        /// </summary>
        /// <param name="gc">GameComponent.</param>
        /// <returns>Lista con las colisiones.</returns>
        List<CollisionRecord> GetCollisions(IGameComponent gc);

        /// <summary>
        /// Obtiene una lista con las colisiones producidas en el ultimo frame para un game component.
        /// </summary>
        /// <param name="gc">GameComponent.</param>
        /// <returns>Lista con las colisiones.</returns>
        /// <param name="results">Lista donde dejar los resultados</param>
        void GetCollisions(IGameComponent gc, List<CollisionRecord> results);
        #endregion
    }
}
