using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz de GameObjects. Es cualquier elemento (grafico, sonido, etc) que se puede asociar a un GameComponent dentro de una escena.
    /// </summary>
    public interface IGameObject : IUpdateable, IActivable, IIdentifiable
    {
        #region Properties
        /// <summary>
        /// GameComponent al que esta asociado el GameObject.
        /// </summary>
        IGameComponent Component { get; set; }
        #endregion
    }
}
