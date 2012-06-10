using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Core.Collision
{
    /// <summary>
    /// Registro que almacena la informacion de una colision producida dentro de un grupo de colision.
    /// </summary>
    public struct CollisionRecord
    {
        #region Properties
        /// <summary>
        /// GameComponentA
        /// </summary>
        public IGameComponent GameComponentA;
        /// <summary>
        /// GameComponentB
        /// </summary>
        public IGameComponent GameComponentB;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo registro de colision.
        /// </summary>
        /// <param name="gameComponentA">GameComponentA</param>
        /// <param name="gameComponentB">GameComponentB</param>
        public CollisionRecord(IGameComponent gameComponentA, IGameComponent gameComponentB)
        {
            GameComponentA = gameComponentA;
            GameComponentB = gameComponentB;
        }
        #endregion
    }
}
