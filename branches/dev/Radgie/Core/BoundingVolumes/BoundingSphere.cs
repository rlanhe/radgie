using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Esfera de colision.
    /// </summary>
    public class BoundingSphere: ABoundingVolume<Microsoft.Xna.Framework.BoundingSphere>
    {
        #region Constructors
        /// <summary>
        /// Crea una esfera de colision.
        /// </summary>
        /// <param name="sphere">Estructura para representar la esfera de XNA</param>
        public BoundingSphere(Microsoft.Xna.Framework.BoundingSphere sphere)
        {
            mLocalBoundingVolume = sphere;
            mBoundingVolume = mLocalBoundingVolume;
        }

        /// <summary>
        /// Crea una esfera de colision.
        /// </summary>
        /// <param name="dimensions">Las componentes x,y,z representa el centro de la espera. La componente w el radio.</param>
        public BoundingSphere(Vector4 dimensions)
        {
            mLocalBoundingVolume = new Microsoft.Xna.Framework.BoundingSphere(new Vector3(dimensions.X, dimensions.Y, dimensions.Z), dimensions.W);
        }

        /// <summary>
        /// Crea una esfera de colision.
        /// </summary>
        /// <param name="center">Centro de la esfera.</param>
        /// <param name="radius">Radio de la esfera.</param>
        public BoundingSphere(Vector3 center, float radius)
        {
            mLocalBoundingVolume = new Microsoft.Xna.Framework.BoundingSphere(center, radius);
            mBoundingVolume = mLocalBoundingVolume;
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Matrix worldMatrix)
        {
            mLocalBoundingVolume.Transform(ref worldMatrix, out mBoundingVolume);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new BoundingSphere(mBoundingVolume);
        }
        #endregion
        #endregion
    }
}
