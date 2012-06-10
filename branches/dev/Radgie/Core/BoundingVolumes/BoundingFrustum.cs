using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Volumen de proyeccion.
    /// Es el volumen definido por los planos near y far y el campo de vision de la camara. Es util para determinar que objetos estan fuera de la camara y no necesitan ser dibujados.
    /// </summary>
    public class BoundingFrustum : ABoundingVolume<Microsoft.Xna.Framework.BoundingFrustum>
    {
        #region Constructors
        /// <summary>
        /// Crea un volumen de proyeccion.
        /// </summary>
        /// <param name="frustum">Estructura de XNA para definir el volumen.</param>
        public BoundingFrustum(Microsoft.Xna.Framework.BoundingFrustum frustum)
        {
            mLocalBoundingVolume = mBoundingVolume = frustum;
        }

        /// <summary>
        /// Crea un volumen de proyeccion.
        /// </summary>
        /// <param name="viewProjection">Matrix de vista-proyeccion que define el volumen.</param>
        public BoundingFrustum(Matrix viewProjection)
        {
            mLocalBoundingVolume = mBoundingVolume = new Microsoft.Xna.Framework.BoundingFrustum(viewProjection);
        }
        #endregion

        #region Methods
        #region ABoundingVolume Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Update"/>
        /// </summary>
        public override void Update(ref Matrix worldMatrix)
        {
            // OK. Do noting
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.BoundingVolumes.ABoundingVolume.Clone"/>
        /// </summary>
        public override IBoundingVolume Clone()
        {
            return new BoundingFrustum(mLocalBoundingVolume);
        }
        #endregion
        #endregion
    }
}
