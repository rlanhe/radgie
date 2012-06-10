using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Core;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad para dibujar texto en pantalla.
    /// </summary>
    public class Text: Sprite
    {
        #region Properties
        /// <summary>
        /// Texto que se dibujara.
        /// </summary>
        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                if ((value != null) && (!value.Equals(mValue)))
                {
                    mValue = value;
                    mDirty = true;
                }
            }
        }
        private string mValue;

        /// <summary>
        /// Fuente para dibujar el texto.
        /// </summary>
        public SpriteFont Font
        {
            get
            {
                return mFont;
            }
            set
            {
                if ((value != null) && (!value.Equals(mFont)))
                {
                    mFont = value;
                    mDirty = true;
                }
            }
        }
        private SpriteFont mFont;

        /// <summary>
        /// Color de la fuente.
        /// </summary>
        public Color Color
        {
            get
            {
                return mColor;
            }
            set
            {
                if ((value != null) && (!value.Equals(mColor)))
                {
                    mColor = value;
                    mDirty = true;
                }
            }
        }
        private Color mColor;

        /// <summary>
        /// Escala del texto.
        /// </summary>
        public float Scale
        {
            get
            {
                return mScale;
            }
            set
            {
                if (value > 0)
                {
                    mScale = value;
                    mDirty = true;
                }
            }
        }
        private float mScale;

        /// <summary>
        /// Marca para saber que tiene que recalcular la textura del texto.
        /// </summary>
        private bool mDirty;

        private RenderTarget2D mRenderTarget;

        #endregion

        #region Constructors
        /// <summary>
        /// Crea una entidad de tipo texto.
        /// </summary>
        public Text()
            : base(100.0f, 100.0f)
        {
            mDirty = true;
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gSystem.GraphicEntityReferences.Add(this);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recalcula la textura en funcion del texto, fuente y color.
        /// </summary>
        private void Update()
        {
            if ((mDirty) && (mFont != null) && (mValue != null))
            {
                
                IRenderer renderer = ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Renderer;
                Vector2 size = mFont.MeasureString(mValue);

                float width = size.X * mScale;
                float height = size.Y * mScale;
                Quad oldQuad = (Quad)mGeometry;
                bool resize = (oldQuad.Width != width) && (oldQuad.Height != height);

				// TODO: Construir nueva geometria solo si es necesario
                // Solo construye una nueva geometria si cambia el tamanno del texto.
                if((oldQuad == null) || (resize))
                {
                    mGeometry = new Quad(width, height);
                }
                
                // Bloquea el dispositivo grafico para evitar conflictos con otros hilos de ejecucion.
                lock (((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Device)
                {
                	// Crea solo rendertarget nuevo si es necesario, si no reaprovechar existente.
                    if((mRenderTarget == null) || (resize))
                    {
                        if (mRenderTarget != null)
                        {
                            mRenderTarget.Dispose();
                        }
                        mRenderTarget = new RenderTarget2D(renderer.Device, (int)size.X, (int)size.Y, false, SurfaceFormat.Alpha8, DepthFormat.None);
                    }
                    SpriteBatch sbatch = renderer.SpriteBatch;
                    renderer.Device.SetRenderTarget(mRenderTarget);
                    renderer.Device.Clear(Color.Transparent);

                    sbatch.Begin();
                    sbatch.DrawString(mFont, mValue, Vector2.Zero, mColor, 0.0f, Vector2.Zero, mScale, SpriteEffects.None, 0.0f);
                    sbatch.End();

                    renderer.Device.SetRenderTarget(null);
                    mMaterial[Semantic.Texture0].SetValue(mRenderTarget);
                }

                mDirty = false;
            }
        }

        #region AGraphicEntity
        /// <summary>
        /// Ver <see cref="Radgie.Graphic.AGraphicEntity.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            if (mDirty)
            {
                // Se realiza aqui y no en durante el dibujado de la escena para evitar flickering
                Update();
            }
        }
        #endregion
        #endregion
    }
}
