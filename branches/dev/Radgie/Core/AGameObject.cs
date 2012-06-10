using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Un GameObject es cualquier elemento (grafico, sonido, etc) que se puede asociar a un GameComponent dentro de una escena.
    /// </summary>
    public abstract class AGameObject: IGameObject
    {
        #region Properties
        #region IGameObject Properties
        /// <summary>
        /// Indica si el GameObject esta activo.
        /// Si el GameObject no esta activo, la llamada a su metodo Update no tendra efecto.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Identificador del GameObject.
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }
        private string mId;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameObject.Component"/>
        /// </summary>
        public IGameComponent Component
        {
            get
            {
                return mComponent;
            }
            set
            {
                mComponent = value;
            }
        }
        private IGameComponent mComponent;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Indica si el GameObject esta activo.
        /// Para que un GameObject se considere activo, esde debe tener Active == true y ademas el GameComponent al que esta asociado debe estar activo.
        /// </summary>
        /// <returns>True si lo esta, False en caso contrario.</returns>
        public bool IsActive()
        {
            return Active & (Component == null ? true : Component.IsActive());
        }

        #region IGameObject Methods
        /// <summary>
        /// Actualiza el estado del GameObject.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la ultima actualizacion.</param>
        public abstract void Update(GameTime time);
        #endregion
        #endregion
    }
}
