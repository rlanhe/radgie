using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Funcionalidad comun a todoas las entidades de Radgie.
    /// </summary>
    public abstract class AEntity: AGameObject, IEntity
    {
        #region Properties
        /// <summary>
        /// Lista interna de instancias.
        /// </summary>
        protected List<IInstance> mInstances;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una entidad.
        /// </summary>
        public AEntity()
        {
            Active = true;
            mInstances = new List<IInstance>();
        }
        #endregion

        #region Methods
        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected abstract IInstance CreateSpecificInstance();

        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateInstance"/>
        /// </summary>
        public IInstance CreateInstance()
        {
            IInstance instance = CreateSpecificInstance();
            mInstances.Add(instance);
            return instance;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.RemoveInstance"/>
        /// </summary>
        public void RemoveInstance(IInstance instance)
        {
            mInstances.Remove(instance);
        }
        #endregion
        #endregion
    }
}
