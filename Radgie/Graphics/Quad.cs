using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    // TODO: Hacer posible LowProfile (short)
    /// <summary>
    /// Un quad es una geometria en forma de rectangulo, formada por la union de dos triangulos.
    /// Se usa principalmente para dibujar imagenes 2d (Textos, sprites, etc).
    /// </summary>
    public class Quad : StaticGeometry
    {
        #region Properties
        /// <summary>
        /// Anchura del quad.
        /// </summary>
        public float Width
        {
            get
            {
                return mWidth;
            }
        }
        protected float mWidth;

        /// <summary>
        /// Altura del quad.
        /// </summary>
        public float Height
        {
            get
            {
                return mHeight;
            }
        }
        protected float mHeight;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo quad de las dimensiones especificadas.
        /// </summary>
        /// <param name="width">Ancho del quad.</param>
        /// <param name="height">Alto del quad.</param>
        public Quad(float width, float height)
        {
            mWidth = width;
            mHeight = height;

            base.SetData(new VertexPositionNormalTexture[]{ new VertexPositionNormalTexture (new Vector3(-width/2.0f, -height/2.0f, 0.0f), Vector3.Backward, new Vector2(0.0f, 1.0f)),
                                                            new VertexPositionNormalTexture (new Vector3(-width/2.0f, height/2.0f, 0.0f), Vector3.Backward, new Vector2(0.0f, 0.0f)),
                                                            new VertexPositionNormalTexture (new Vector3(width/2.0f, -height/2.0f, 0.0f), Vector3.Backward, new Vector2(1.0f, 1.0f)),
                                                            new VertexPositionNormalTexture (new Vector3(width/2.0f, height/2.0f, 0.0f), Vector3.Backward, new Vector2(1.0f, 0.0f))},
                                                            new int[] { 0, 1, 2, 2, 1, 3 },
                                                            PrimitiveType.TriangleList);
        }
        #endregion
    }
}
