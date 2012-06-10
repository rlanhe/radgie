using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Caja de colision.
    /// Una caja de colision representa un cubo en el espacio 3D. Es un volumen Axis-Aligned, por lo que no puede ser rotado.
    /// </summary>
    public class BoundingBox : ABoundingVolume<Microsoft.Xna.Framework.BoundingBox>
    {
        #region Constructors
        /// <summary>
        /// Crea una caja de colision.
        /// </summary>
        /// <param name="box">Estructura de XNA para definir una caja de colision.</param>
        public BoundingBox(Microsoft.Xna.Framework.BoundingBox box)
        {
            mLocalBoundingVolume = mBoundingVolume = box;
        }

        /// <summary>
        /// Crea una caja de colision.
        /// </summary>
        /// <param name="min">Punto minimo en el espacio 3D de la caja.</param>
        /// <param name="max">Punto maximo en el espacio 3D de la caja.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            mLocalBoundingVolume = mBoundingVolume = new Microsoft.Xna.Framework.BoundingBox(min, max);
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Matrix worldMatrix)
        {
            Vector3[] corners = mLocalBoundingVolume.GetCorners();

            for (int i = 0; i < corners.Length; i++)
            {
                Vector3.Transform(ref corners[i], ref worldMatrix, out corners[i]);
            }

            mBoundingVolume = Microsoft.Xna.Framework.BoundingBox.CreateFromPoints(corners);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new BoundingBox(mLocalBoundingVolume);
        }
        #endregion
        #endregion
    }
}
