using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Volumen de colision formado por varios volumenes de colision.
    /// Es util para representar el volumen de objetos que no se asemejen a los volumenes primitivos de colision.
    /// </summary>
    public class CompositeBoundingVolume : ABoundingVolume<IBoundingVolume>
    {
        #region Properties
        /// <summary>
        /// Lista de volumenes de colision.
        /// </summary>
        public List<IBoundingVolume> BoundingVolumes
        {
            get
            {
                return mBoundingVolumes;
            }
        }
        List<IBoundingVolume> mBoundingVolumes;

        /// <summary>
        /// Volumen de colision que engloba al resto.
        /// </summary>
        public IBoundingVolume CompositeVolume
        {
            get
            {
                return mCompositeVolume;
            }
        }
        IBoundingVolume mCompositeVolume;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un compositeBoundingVolume.
        /// </summary>
        public CompositeBoundingVolume()
        {
            mBoundingVolumes = new List<IBoundingVolume>();
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Microsoft.Xna.Framework.Matrix worldMatrix)
        {
            mCompositeVolume = null;
            foreach (IBoundingVolume volume in BoundingVolumes)
            {
                volume.Update(ref worldMatrix);

                if (mCompositeVolume == null)
                {
                    mCompositeVolume = volume.Clone();
                }
                else
                {
                    IBoundingVolume res = BoundingUtil.Merge(mCompositeVolume, volume);
                    if (res != null)
                    {
                        mCompositeVolume = res;
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            CompositeBoundingVolume cbv = new CompositeBoundingVolume();

            foreach (IBoundingVolume v in mBoundingVolumes)
            {
                cbv.mBoundingVolumes.Add(v.Clone());
            }

            cbv.mCompositeVolume = mCompositeVolume.Clone();
            return cbv;
        }
        #endregion
        #endregion
    }
}
