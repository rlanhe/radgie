using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Util;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Abstraccion de un volumen de colision.
    /// Contiene la funcionalidad comun a todos los volumenes de colision.
    /// </summary>
    /// <typeparam name="T">Tipo de la colision. Se corresponde con la estructura XNA que define la colision.</typeparam>
    public abstract class ABoundingVolume<T>: IBoundingVolume
    {
        #region Properties
        /// <summary>
        /// Volumen de colision en coordenadas de mundo.
        /// Se usa el valor de esta estructura para comprobar colisiones con otros volumenes de colision.
        /// </summary>
        public T BoundingVolume
        {
            get
            {
                return mBoundingVolume;
            }
        }
        protected T mBoundingVolume;

        /// <summary>
        /// Volumen de colision en coordenadas locales al GameComponet al que va asociado.
        /// A partir de este se calcula BoundingVolume (metodo Update).
        /// </summary>
        public T LocalBoundingVolume
        {
            get
            {
                return mLocalBoundingVolume;
            }
        }
        protected T mLocalBoundingVolume;

        #region IBoundingVolume Properties
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.IBoundingVolume.ChildVolume"/>
        /// </summary>
        public IBoundingVolume ChildVolume
        {
            get
            {
                return mChildVolume;
            }
            set
            {
                mChildVolume = value;
            }
        }
        protected IBoundingVolume mChildVolume;
        #endregion
        #endregion

        #region Methods
        #region IBoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.IBoundingVolume.Update"/>
        /// </summary>
        public abstract void Update(ref Matrix worldMatrix);

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.IBoundingVolume.Intersects"/>
        /// </summary>
        public float? Intersects(IBoundingVolume bv)
        {
            float? result = BoundingUtil.Intersects(this, bv);

            if ((result != null) && ((ChildVolume != null) || (bv.ChildVolume != null)))
            {
                // Si alguno tiene un volumen hijo, vuelve a realizar la comprobacion con este
                IBoundingVolume bv1 = ChildVolume != null ? ChildVolume : this;
                IBoundingVolume bv2 = bv.ChildVolume != null ? bv.ChildVolume : bv;

                return bv1.Intersects(bv2);
            }

            return result;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.IBoundingVolume.Contains"/>
        /// </summary>
        public virtual ContainmentType Contains(IBoundingVolume bv)
        {
            ContainmentType result = BoundingUtil.Contains(this, bv);

            if ((result == ContainmentType.Intersects) && ((ChildVolume != null) || (bv.ChildVolume != null)))
            {
                // Si alguno tiene un volumen hijo, vuelve a realizar la comprobacion con este
                IBoundingVolume bv1 = ChildVolume != null ? ChildVolume : this;
                IBoundingVolume bv2 = bv.ChildVolume != null ? bv.ChildVolume : bv;

                return bv1.Contains(bv2);
            }

            return result;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.IBoundingVolume.Clone"/>
        /// </summary>
        public abstract IBoundingVolume Clone();
        #endregion
        #endregion
    }
}
