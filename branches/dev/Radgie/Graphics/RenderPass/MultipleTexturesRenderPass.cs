using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;
using Radgie.Graphics.Entity;
using Microsoft.Xna.Framework;
using Radgie.Core;

namespace Radgie.Graphics.RenderPass
{
    /// <summary>
    /// RenderPass para dibujar texturas sobre el backbuffer.
    /// </summary>
    public class MultipleTexturesRenderPass: ARenderPass
    {
        #region Properties
        /// <summary>
        /// Textura fuente.
        /// </summary>
        public List<Texture2D> Sources
        {
            get
            {
                return mSources;
            }
            set
            {
                mSources = value;
            }
        }
        private List<Texture2D> mSources;

        /// <summary>
        /// Color con el que se tinta el resultado final.
        /// Usar Color.White para dibujar normalmente.
        /// </summary>
        public Color Color
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

        /// <summary>
        /// Expande la textura al tamanno del target.
        /// </summary>
        public bool ExpandToDestinationTarget
        {
            get
            {
                return mExpandToDestinationTarget;
            }
            set
            {
                mExpandToDestinationTarget = value;
            }
        }
        private bool mExpandToDestinationTarget;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="camera">Camara a usar en el dibujado.</param>
        /// <param name="cleanRT">Indica si debe limpiar el render target sobre el que va a dibujar.</param>
        public MultipleTexturesRenderPass(ICamera camera, bool cleanRT)
            : this(null, camera, null, cleanRT)
        {
        }

        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="sources">Lista de texturas de entrada.</param>
        /// <param name="camera">Camara a usar en el dibujado.</param>
        /// <param name="cleanRT">Indica si debe limpiar el render target sobre el que va a dibujar.</param>
        public MultipleTexturesRenderPass(List<Texture2D> sources, ICamera camera, bool cleanRT)
            : this(sources, camera, null, cleanRT)
        {
        }

        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="target">Target sobre el que dibujar.</param>
        /// <param name="camera">Camara a usar en el dibujado.</param>
        /// <param name="cleanRT">Limpia el render target sobre el que va a dibujar.</param>
        public MultipleTexturesRenderPass(RenderTarget2D target, ICamera camera, bool cleanRT)
            : this(null, camera, target, cleanRT)
        {
            mSources = new List<Texture2D>();
        }

        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="sources">Lista de texturas de entrada.</param>
        /// <param name="camera">Camara a usar en el dibujado.</param>
        /// <param name="target">Target sobre el que dibujar.</param>
        /// <param name="cleanRT">Indica si se debe limpiar el render target sobre el que va a dibujar.</param>
        public MultipleTexturesRenderPass(List<Texture2D> sources, ICamera camera, RenderTarget2D target, bool cleanRT)
            : base(target, camera, false, cleanRT)
        {
            mSources = sources;
            mColor = Color.White;
            mExpandToDestinationTarget = false;
        }
        #endregion

        #region Methods
        #region ARenderPass Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass"/>
        /// </summary>
        public override void RenderAction(IRenderer renderer)
        {
            if (mSources != null)
            {
                lock (renderer.Device)
                {
                    renderer.SpriteBatch.Begin();
                    foreach (Texture2D texture in mSources)
                    {
                        if (ExpandToDestinationTarget)
                        {
                            Rectangle bounds;
                            if (mTarget != null)
                            {
                                bounds = mTarget.Bounds;
                            }
                            else
                            {
                                bounds = renderer.Device.Viewport.Bounds;
                            }
                            renderer.SpriteBatch.Draw(texture, bounds, mColor);
                        }
                        else
                        {
                            renderer.SpriteBatch.Draw(texture, Vector2.Zero, mColor);
                        }
                    }
                    renderer.SpriteBatch.End();
                }
            }
        }
        #endregion
        #endregion
    }
}
