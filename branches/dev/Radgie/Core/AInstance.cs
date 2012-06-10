using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Abstraccion con la funcionalidad basica de las instancias de objetos.
    /// Una instancia mantiene una referencia a la entidad a la que hace referencia, de la misma manera que esta mantiene una referencia a la instancia. 
    /// Esto es debido a que la entidad no debe desparecer mientras existan instancias que apunten a ella. De la misma manera, cuando deje de haber instancias 
    /// hacia la entidad, esta debe poder ser borrada de la memoria.
    /// 
    /// Por todo esto, esta clase implementa la interfaz IDisposable que asegura que al desaparecer la instancia se elimina la referencia que la entidad guardaba.
    /// </summary>
    public abstract class AInstance: AGameObject, IInstance
    {
        #region Properties
        /// <summary>
        /// Entidad a la que hace referencia la instancia.
        /// </summary>
        public IEntity Entity
        {
            get
            {
                return mEntity;
            }
        }
        private IEntity mEntity;

        /// <summary>
        /// Indica si ya se ha llamado al metodo Dispose de la instancia.
        /// </summary>
        private bool mDisposed = false;
        #endregion

        #region Constructors
        
        private AInstance()
        {
        }

        /// <summary>
        /// Crea una instancia a partir de una entidad.
        /// </summary>
        /// <param name="entity"></param>
        internal AInstance(IEntity entity)
        {
            mEntity = entity;
        }

        #endregion
        
        #region Methods
        #region IInstance
        /// <summary>
        /// Metodo para liberar de manera explicita los recursos de este objeto.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Metodo para liberar los recursos de este objeto.
        /// </summary>
        /// <param name="disposing">True si fue llamado de manera explicita, false en caso contrario.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!mDisposed)
            {
                // Se desvincula de su GameComponent
                if (Component != null)
                {
                    Component.RemoveGameObject(this);
                }
                // Se desvincula de su entity
                mEntity.RemoveInstance(this);
                mDisposed = true;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// Destructor.
        /// Es llamado por el recolector de basura cuando reclama la memoria de este objeto.
        /// </summary>
        ~AInstance()
        {
            Dispose(false);
        }
        #endregion
        #endregion
    }
}
