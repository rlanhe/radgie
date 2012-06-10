using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework.Graphics;

namespace Radgie.Graphics.RenderPass
{
    /// <summary>
    /// Pasada del render para mezclar varias pasadas sobre el target final.
    /// </summary>
    public class MultipleRenderPass : ARenderPass
    {
        #region Properties
        /// <summary>
        /// Lista de pasadas.
        /// </summary>
        public List<ARenderPass> Passes
        {
            get
            {
                return mPasses;
            }
            set
            {
                mPasses = new List<ARenderPass>(value);
            }
        }
        private List<ARenderPass> mPasses;
        /// <summary>
        /// Targets intermedios de cada una de las pasadas.
        /// </summary>
        private List<RenderTarget2D> mTargets;
        /// <summary>
        /// Pasada final para dejar el resultado en el target final.
        /// </summary>
        private MultipleTexturesRenderPass mFinalPass;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una pasada multiple a partir de una lista de pasadas y deja el resultado en el backbuffer.
        /// </summary>
        /// <param name="passes">Lista de pasadas.</param>
        public MultipleRenderPass(List<ARenderPass> passes, ICamera camera)
            : this(passes, camera, null)
        {
        }

        /// <summary>
        /// Crea una pasada multiple a partir de una lista de pasadas.
        /// </summary>
        /// <param name="passes">Lista de pasadas.</param>
        /// <param name="target">Target final. Null para dejar el resultado en el backbuffer.</param>
        public MultipleRenderPass(List<ARenderPass> passes, ICamera camera, RenderTarget2D target)
            : base(null, null, false, false)
        {
            Passes = passes;
            mFinalPass = new MultipleTexturesRenderPass(target, camera, true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa los targets intermedios de las pasadas.
        /// </summary>
        /// <param name="count">Numero de targets.</param>
        /// <param name="renderer">Renderer que se usara a la hora de dibujar.</param>
        private void InitRenderTargets(int count, IRenderer renderer)
        {
            if (mTargets != null)
            {
                foreach (RenderTarget2D target in mTargets)
                {
                    target.Dispose();
                }
                mTargets.Clear();
            }
            else
            {
                mTargets = new List<RenderTarget2D>();
            }
            PresentationParameters pp = renderer.Device.PresentationParameters;
            DisplayMode mode = renderer.Device.DisplayMode;
            for (int i = 0; i < mPasses.Count; i++)
            {
                mTargets.Add(new RenderTarget2D(renderer.Device, pp.BackBufferWidth, pp.BackBufferHeight, true, pp.BackBufferFormat, pp.DepthStencilFormat));
            }
        }

        #region ARenderPass Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.PreRenderAction"/>
        /// </summary>
        public override void PreRenderAction(IRenderer renderer)
        {
            base.PreRenderAction(renderer);

            if ((mTargets == null) || (mTargets.Count != mPasses.Count))
            {
                InitRenderTargets(mPasses.Count, renderer);
            }

            for (int i = 0; i < mPasses.Count; i++)
            {
                mPasses[i].Target = mTargets[i];
                mPasses[i].Render(renderer);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.RenderAction"/>
        /// </summary>
        public override void RenderAction(IRenderer renderer)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.PosRenderAction"/>
        /// </summary>
        public override void PosRenderAction(IRenderer renderer)
        {
            base.PosRenderAction(renderer);
            
            // Vuelca el contenido del RT al Backbuffer
            mFinalPass.Sources.Clear();
            foreach(RenderTarget2D rt in mTargets)
            {
                mFinalPass.Sources.Add((Texture2D)rt);
            }
            mFinalPass.Render(renderer);
        }
        #endregion
        #endregion
    }
}
