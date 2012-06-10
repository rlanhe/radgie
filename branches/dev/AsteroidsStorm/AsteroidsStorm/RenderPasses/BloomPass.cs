using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;
using Radgie.Core;
using Radgie.Graphics.RenderPass;

namespace AsteroidsStorm.RenderPasses
{
    /// <summary>
    /// Pasada para aplicar un efecto de bloom sobre la imagen del juego.
    /// </summary>
    public class BloomPass: ARenderPass
    {
        #region Properties
        private Material mExtractMaterial;
        private Material mCombineMaterial;

        /// <summary>
        /// Pasada con la lista de pasadas de este efecto.
        /// </summary>
        public MultipleRenderPass2 Passes
        {
            get
            {
                return mPasses;
            }
        }
        private MultipleRenderPass2 mPasses;

        /// <summary>
        /// Pasada para el efecto de blur.
        /// </summary>
        private BlurPass mBlurPass;

        /// <summary>
        /// Intensidad del efecto de blur.
        /// </summary>
        public float BlurIntensity
        {
            get
            {
                return mBlurPass.Intensity;
            }
            set
            {
                mBlurPass.Intensity = value;
            }
        }

        /// <summary>
        /// Valor a partir del cual se considera las partes brillantes de una imagen.
        /// </summary>
        public float Threshold
        {
            get
            {
                return mExtractMaterial[THRESHOLD_ID].EffectParameter.GetValueSingle();
            }
            set
            {
                mExtractMaterial[THRESHOLD_ID].SetValue(value);
            }
        }
        private const string THRESHOLD_ID = "BloomThreshold";
        private const float THRESHOLD_MIN_VALUE = 0.0f;

        /// <summary>
        /// Intensidad del efecto bloom.
        /// </summary>
        public float BloomIntensity
        {
            get
            {
                return mCombineMaterial[BLOOM_INTENSITY_ID].EffectParameter.GetValueSingle();
            }
            set
            {
                mCombineMaterial[BLOOM_INTENSITY_ID].SetValue(value);
            }
        }
        private const string BLOOM_INTENSITY_ID = "BloomIntensity";
        private const float BLOOM_INTENSITY_MIN_VALUE = 0.0f;

        /// <summary>
        /// Intensidad de la imagen base.
        /// </summary>
        public float BaseIntensity
        {
            get
            {
                return mCombineMaterial[BASE_INTENSITY_ID].EffectParameter.GetValueSingle();
            }
            set
            {
                mCombineMaterial[BASE_INTENSITY_ID].SetValue(value);
            }
        }
        private const string BASE_INTENSITY_ID = "BaseIntensity";
        private const float BASE_INTENSITY_MIN_VALUE = 0.0f;

        /// <summary>
        /// Saturacion del efecto bloom.
        /// </summary>
        public float BloomSaturation
        {
            get
            {
                return mCombineMaterial[BLOOM_SATURATION_ID].EffectParameter.GetValueSingle();
            }
            set
            {
                mCombineMaterial[BLOOM_SATURATION_ID].SetValue(value);
            }
        }
        private const string BLOOM_SATURATION_ID = "BloomSaturation";
        private const float BLOOM_SATURATION_MIN_VALUE = 0.0f;

        /// <summary>
        /// Saturacion de la imagen base.
        /// </summary>
        public float BaseSaturation
        {
            get
            {
                return mCombineMaterial[BASE_SATURATION_ID].EffectParameter.GetValueSingle();
            }
            set
            {
                mCombineMaterial[BASE_SATURATION_ID].SetValue(value);
            }
        }
        private const string BASE_SATURATION_ID = "BaseSaturation";
        private const float BASE_SATURATION_MIN_VALUE = 0.0f;

        private int mHeight;
        private int mWidth;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el efecto.
        /// </summary>
        /// <param name="texture">Textura base.</param>
        /// <param name="target">Target donde dejar el resultado.</param>
        public BloomPass(Texture2D texture, RenderTarget2D target)
            : base(target, null, false, false)
        {
            GraphicsDevice device = ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Device;
            mExtractMaterial = RadgieGame.Instance.ResourceManager.Load<Material>("Radgie/Graphics/Materials/bloomExtract").Clone();
            mCombineMaterial = RadgieGame.Instance.ResourceManager.Load<Material>("Radgie/Graphics/Materials/bloomCombine").Clone();

            Threshold = THRESHOLD_MIN_VALUE;
            BloomIntensity = BLOOM_INTENSITY_MIN_VALUE;
            BloomSaturation = BLOOM_SATURATION_MIN_VALUE;
            BaseIntensity = BASE_INTENSITY_MIN_VALUE;
            BaseSaturation = BASE_SATURATION_MIN_VALUE;

            RenderTarget2D tempRT1 = null;
            RenderTarget2D tempRT2 = null;

            PresentationParameters pp = device.PresentationParameters;
            mWidth = (target == null ? pp.BackBufferWidth : target.Width) / 2;
            mHeight = (target == null ? pp.BackBufferHeight : target.Height) / 2;

            lock (device)
            {
                tempRT1 = new RenderTarget2D(device, mWidth, mHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None);
                tempRT2 = new RenderTarget2D(device, mWidth, mHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None);
            }

            if ((tempRT1 == null) || (tempRT2 == null))
            {
                throw new Exception("Can't create Rendertargets");
            }

            List<ARenderPass> passes = new List<ARenderPass>();
            passes.Add(new FullScreenQuadPass(texture, tempRT1, mExtractMaterial));
            
            mBlurPass = new BlurPass(tempRT1, tempRT2);
            mBlurPass.Intensity = 4.0f;
            passes.Add(mBlurPass);
            
            mCombineMaterial["BaseTexture"].SetValue(texture);
            passes.Add(new FullScreenQuadPass(tempRT2, target, mCombineMaterial));

            mPasses = new MultipleRenderPass2(passes);
        }
        #endregion

        #region Methods

        #region ARenderPass Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.RenderAction"/>
        /// </summary>
        public override void RenderAction(IRenderer renderer)
        {
            mPasses.Render(renderer);
        }
        #endregion
        #endregion
    }
}
