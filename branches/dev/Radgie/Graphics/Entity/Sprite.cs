using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics.Instance;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad para dibujar sprites.
    /// </summary>
    public class Sprite : Geometry
    {
        /// <summary>
        /// Quad sobre el que se dibuja el sprite.
        /// </summary>
        public Quad Quad
        {
            get
            {
                return (Quad)mGeometry;
            }
            set
            {
                if (value != null)
                {
                    mGeometry = value;
                }
            }
        }

        #region Constructors
        /// <summary>
        /// Constructor de un sprite.
        /// </summary>
        /// <param name="width">Anchura del sprite.</param>
        /// <param name="height">Altura del sprite.</param>
        public Sprite(float width, float height)
            : base(new Quad(width, height))
        {
        }
        #endregion
    }
}
