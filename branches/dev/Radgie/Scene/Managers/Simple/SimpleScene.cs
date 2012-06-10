using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Scene.Managers.Simple
{
    /// <summary>
    /// Una escena SimpleScene almacena todos los GameComponents en su nodo Root.
    /// Es aconsejable su uso en escenas sencillas, con un bajo numero de elementos, por ejemplo GUIs
    /// </summary>
    public class SimpleScene: AScene<SimpleSceneNode>
    {
        #region Constructors
        /// <summary>
        /// Crea una escena SimpleScene
        /// </summary>
        /// <param name="id">Identificador</param>
        public SimpleScene(string id)
            : base(id)
        {
            mRoot = new SimpleSceneNode(this);
        }
        #endregion
    }
}
