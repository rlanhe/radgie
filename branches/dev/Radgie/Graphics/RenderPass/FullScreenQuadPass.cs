using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.RenderPass
{
    /// <summary>
    /// Pasada para dibujar un quad a pantalla completa.
    /// </summary>
    public class FullScreenQuadPass: ARenderPass
    {
        #region Properties
        /// <summary>
        /// Material a utilizar cuando dibuje a pantalla completa.
        /// </summary>
        private Material mMaterial;
        /// <summary>
        /// Textura a dibujar a pantalla completa.
        /// </summary>
        private Texture2D mTexture;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye la pasada.
        /// </summary>
        /// <param name="texture">Textura de origen.</param>
        /// <param name="target">Target sobre el que dibuja.</param>
        /// <param name="material">Material que usara al dibujar.</param>
        public FullScreenQuadPass(Texture2D texture, RenderTarget2D target, Material material)
            : base(target, null, false, false)
        {
            mTexture = texture;
            mMaterial = material;
        }
        #endregion

        #region Methods
        #region ARenderPass Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.RenderAction"/>
        /// </summary>
        /// <param name="renderer">Renderer en uso.</param>
        public override void RenderAction(IRenderer renderer)
        {
            PresentationParameters pp = renderer.Device.PresentationParameters;
            lock (renderer.Device)
            {
                renderer.SpriteBatch.Begin(0, BlendState.Opaque, null, null, null, mMaterial.Effect);
                renderer.SpriteBatch.Draw(mTexture, new Microsoft.Xna.Framework.Rectangle(0, 0, Target == null ? pp.BackBufferWidth : Target.Width, Target == null ? pp.BackBufferHeight : Target.Height), Color.White);
                renderer.SpriteBatch.End();
            }
        }
        #endregion
        #endregion
    }
}
