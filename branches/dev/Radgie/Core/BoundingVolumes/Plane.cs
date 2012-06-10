using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Representa la superficie de un plano en el espacio.
    /// </summary>
    public class Plane : ABoundingVolume<Microsoft.Xna.Framework.Plane>
    {
        #region Constructors
        /// <summary>
        /// Crea un plano a partir de otro.
        /// </summary>
        /// <param name="plane">Plano de referencia.</param>
        public Plane(Microsoft.Xna.Framework.Plane plane)
        {
            mLocalBoundingVolume = mBoundingVolume = plane;
        }

        /// <summary>
        /// Crea un plano a partir de un vector de 4 elementos.
        /// </summary>
        /// <param name="value">Las componentes x,y,z definen la normal del plano. La componente w describe la distancia del origen al plano siguiendo la normal del plano.</param>
        public Plane(Vector4 value)
        {
            mLocalBoundingVolume = mBoundingVolume = new Microsoft.Xna.Framework.Plane(value);
        }

        /// <summary>
        /// Crea un plano a partir de la normal del plano 
        /// </summary>
        /// <param name="normal">Normal del plano.</param>
        /// <param name="value">Distancia del origen al plano siguiendo la normal del plano.</param>
        public Plane(Vector3 normal, float value)
        {
            mLocalBoundingVolume = mBoundingVolume = new Microsoft.Xna.Framework.Plane(normal,  value);
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Contains"/>
        /// </summary>
        public override ContainmentType Contains(IBoundingVolume bv)
        {
            // Un plano no tiene volumen, por lo que no puede contener ningun otro.
            return ContainmentType.Disjoint;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Matrix worldMatrix)
        {
            Vector3 newValue;
            Vector3.Transform(ref mLocalBoundingVolume.Normal, ref worldMatrix, out newValue);
            mBoundingVolume = new Microsoft.Xna.Framework.Plane(newValue, mLocalBoundingVolume.D);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new Plane(mLocalBoundingVolume);
        }
        #endregion
        #endregion
    }
}
