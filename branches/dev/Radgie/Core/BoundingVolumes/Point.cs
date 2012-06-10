using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Punto en el espacio.
    /// </summary>
    public class Point : ABoundingVolume<Vector3>
    {
        #region Constructors
        /// <summary>
        /// Crea un punto.
        /// </summary>
        /// <param name="point">Coordenadas del punto en el espacio.</param>
        public Point(Vector3 point)
        {
            mLocalBoundingVolume = mBoundingVolume = point;
        }
        #endregion

        #region Methods
        #region ABoundingVolume
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Contains"/>
        /// </summary>
        public override ContainmentType Contains(IBoundingVolume bv)
        {
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Matrix worldMatrix)
        {
            Vector3.Transform(ref mLocalBoundingVolume, ref worldMatrix, out mBoundingVolume);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new Point(mLocalBoundingVolume);
        }
        #endregion
        #endregion
    }
}
