using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Representa una luz dentro de la escena.
    /// </summary>
    public class Light : AEntity, ILight
    {
        #region Properties
        #region ILight Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ILight.Position"/>
        /// </summary>
        public Microsoft.Xna.Framework.Vector3 Position
        {
            get
            {
                Radgie.Core.IGameComponent gc = Component;
                if (gc != null)
                {
                    return gc.World.Translation;
                }
                else
                {
                    return Vector3.Zero;
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ILight.Radius"/>
        /// </summary>
        public float Radius
        {
            get
            {
                return mRadius;
            }
            set
            {
                mRadius = value;
            }
        }
        private float mRadius;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ILight.FallOff"/>
        /// </summary>
        public float FallOff
        {
            get
            {
                return mFallOff;
            }
            set
            {
                mFallOff = value;
            }
        }
        private float mFallOff;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ILight.Color"/>
        /// </summary>
        public Microsoft.Xna.Framework.Color Color
        {
            get
            {
                return mColor;
            }
            set
            {
                mColor = value;
            }
        }
        private Color mColor;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una luz con los parametros por defecto.
        /// Radio 1, FallOff 0.1 y color Blanco.
        /// </summary>
        public Light(): this(1.0f, 0.1f, Color.White)
        {
        }

        /// <summary>
        /// Crea una luz.
        /// </summary>
        /// <param name="radius">Radio de accion</param>
        /// <param name="fallOff">Factor de atenuacion</param>
        /// <param name="color">Color</param>
        public Light(float radius, float fallOff, Color color)
        {
            mRadius = radius;
            mFallOff = fallOff;
            mColor = color;
        }
        #endregion

        #region Methods
        #region IEntityMethods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override IInstance CreateSpecificInstance()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
        }
        #endregion
        #endregion
    }
}
