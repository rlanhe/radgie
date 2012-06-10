using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Rayo en el espacio.
    /// </summary>
    public class Ray : ABoundingVolume<Microsoft.Xna.Framework.Ray>
    {
        #region Constructors
        /// <summary>
        /// Crea un rayo.
        /// </summary>
        /// <param name="ray">Estructura para representar un rayo de XNA.</param>
        public Ray(Microsoft.Xna.Framework.Ray ray)
        {
            mLocalBoundingVolume = mBoundingVolume = ray;
        }

        /// <summary>
        /// Crea un rayo.
        /// </summary>
        /// <param name="position">Punto origen del rayo.</param>
        /// <param name="direction">Direccion del rayo.</param>
        public Ray(Vector3 position, Vector3 direction)
        {
            mLocalBoundingVolume = mBoundingVolume = new Microsoft.Xna.Framework.Ray(position, direction);
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
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
            Vector3 position;
            Vector3 direction;

            Vector3.Transform(ref mLocalBoundingVolume.Position, ref worldMatrix, out position);
            Vector3.Transform(ref mLocalBoundingVolume.Direction, ref worldMatrix, out direction);
            mBoundingVolume = new Microsoft.Xna.Framework.Ray(position, direction);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new Ray(mLocalBoundingVolume);
        }
        #endregion
        #endregion
    }
}
