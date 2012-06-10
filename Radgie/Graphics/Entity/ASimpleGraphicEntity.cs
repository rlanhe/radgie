using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics.Instance;
using Radgie.Core;
using Microsoft.Xna.Framework.Graphics;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad grafica que tiene asociado un material para indicar el modo en el que se quiere dibujar.
    /// </summary>
    public abstract class ASimpleGraphicEntity: AGraphicEntity
    {
        #region Properties
        #region IDrawable Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDrawable.DrawOrder"/>
        /// </summary>
        public override int DrawOrder
        {
            get
            {
                return Material.DrawOrder;
            }
        }
        #endregion

        /// <summary>
        /// Material de la entidad.
        /// Especifica la forma en que se va a dibujar la entidad grafica.
        /// </summary>
        public Material Material
        {
            get
            {
                return mMaterial;
            }
            set
            {
                if (value != null)
                {
                    mMaterial = value.Clone();
                }
            }
        }
        protected Material mMaterial;
        #endregion

        #region Methods
        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            base.DrawAction(renderer);
            renderer.Material = mMaterial;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
