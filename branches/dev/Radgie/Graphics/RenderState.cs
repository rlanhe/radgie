using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;

namespace Radgie.Graphics
{
    /// <summary>
    /// Configuracion del modo de dibujado a usar por parte del dispositivo grafico.
    /// </summary>
    public class RenderState
    {
        #region Properties
        /// <summary>
        /// Blend render state.
        /// </summary>
        public BlendState BlendState { get; set; }
        /// <summary>
        /// Depth Stencil render state.
        /// </summary>
        public DepthStencilState DepthStencilState { get; set; }
        /// <summary>
        /// Rasterizer render state.
        /// </summary>
        public RasterizerState RasterizerState { get; set; }
        /// <summary>
        /// Sampler render state.
        /// </summary>
        public SamplerState SamplerState { get; set; }
        /// <summary>
        /// Color usado como constante para realizar Alpha-Blending.
        /// </summary>
        public Color BlendFactor { get; set; }
        /// <summary>
        /// Cada bit indica si el sample esta habilitado. Solo tiene efecto si se renderiza en varios buffer a la vez.
        /// </summary>
        public int MultiSampleMask { get; set; }
        /// <summary>
        /// Valor para usar en el test de stencil.
        /// </summary>
        public int ReferenceStencil { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// RenderState vacio.
        /// </summary>
        public RenderState()
        {
        }

        /// <summary>
        /// Crea e inicializa un nuevo RenderState.
        /// </summary>
        /// <param name="blendState">BlendState</param>
        /// <param name="depthStencilState">DepthStencilState</param>
        /// <param name="rasterizerState">RasterizerState</param>
        /// <param name="samplerState">SamplerState</param>
        /// <param name="blendFactor">BlendFactor</param>
        /// <param name="multiSampleMask">MultiSampleMask</param>
        /// <param name="referenceStencil">ReferenceStencil</param>
        public RenderState(BlendState blendState, DepthStencilState depthStencilState, RasterizerState rasterizerState, SamplerState samplerState, Color blendFactor, int multiSampleMask, int referenceStencil)
        {
            BlendState = blendState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            SamplerState = samplerState;
            BlendFactor = blendFactor;
            MultiSampleMask = multiSampleMask;
            ReferenceStencil = referenceStencil;
        }
        #endregion
    }
}
