using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Scene.Managers.Simple
{
    /// <summary>
    /// Nodo de una escena SimpleScene.
    /// </summary>
    public class SimpleSceneNode: ASceneNode<SimpleSceneNode>
    {
        #region Constructors
        /// <summary>
        /// Crea un nodo de una escena SimpleScene.
        /// </summary>
        /// <param name="scene">Escena a la que pertenece</param>
        public SimpleSceneNode(SimpleScene scene)
            : base(scene, null)
        {
        }
        #endregion

        #region Methods
        #region ASceneNode Methods
        /// <summary>
        /// Ver <see cref="Radgie.Scene.ASceneNode.IsNodeVisible"/>
        /// </summary>
        public override bool IsNodeVisible(Core.BoundingVolumes.IBoundingVolume frustum)
        {
            // SimpleScene no descarta nodos de la escena
            return true;
        }
        #endregion
        #endregion
    }
}
