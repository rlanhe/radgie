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
    /// Ejecuta multiples render pass.
    /// </summary>
    public class MultipleRenderPass2 : ARenderPass
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
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una pasada multiple a partir de una lista de pasadas.
        /// </summary>
        /// <param name="passes">Lista de pasadas.</param>
        public MultipleRenderPass2(List<ARenderPass> passes)
            : this(passes, null)
        {
        }

        /// <summary>
        /// Crea una pasada multiple a partir de una lista de pasadas.
        /// </summary>
        /// <param name="passes">Lista de pasadas.</param>
        /// <param name="target">Target final. Null para dejar el resultado en el backbuffer.</param>
        public MultipleRenderPass2(List<ARenderPass> passes, RenderTarget2D target)
            : base(null, null, false, false)
        {
            Passes = passes;
        }
        #endregion

        #region Methods

        #region ARenderPass Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.RenderAction"/>
        /// </summary>
        public override void RenderAction(IRenderer renderer)
        {
            foreach (ARenderPass pass in mPasses)
            {
                pass.Render(renderer);
            }
        }
        #endregion
        #endregion
    }
}
