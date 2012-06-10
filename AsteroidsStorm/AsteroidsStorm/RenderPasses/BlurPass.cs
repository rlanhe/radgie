using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;
using Radgie.Graphics.RenderPass;
using Radgie.Core;
using Microsoft.Xna.Framework;

namespace AsteroidsStorm.RenderPasses
{
    /// <summary>
    /// Pasada para el efecto de Blur.
    /// </summary>
    public class BlurPass: ARenderPass
    {
        #region Properties
        private MultipleRenderPass2 mPasses;

        private Material mMaterial1;
        private Material mMaterial2;

        private int mWidth;
        private int mHeight;

        /// <summary>
        /// Intensidad del efecto de blur.
        /// </summary>
        public float Intensity
        {
            get
            {
                return mIntensity;
            }
            set
            {
                if (value >= INTENSITY_MIN_VALUE)
                {
                    mIntensity = value;
                }
                else
                {
                    mIntensity = INTENSITY_MIN_VALUE;
                }
                Configure();
            }
        }
        private float mIntensity;

        private const float INTENSITY_MIN_VALUE = 0.000001f;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el efecto de blur.
        /// </summary>
        /// <param name="texture">Textura base.</param>
        /// <param name="target">Target donde deja el resultado.</param>
        public BlurPass(Texture2D texture, RenderTarget2D target)
            : base(target, null, false, false)
        {
            mIntensity = INTENSITY_MIN_VALUE;

            GraphicsDevice device = ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Device;
            mMaterial1 = RadgieGame.Instance.ResourceManager.Load<Material>("Radgie/Graphics/Materials/blur").Clone();
            mMaterial2 = mMaterial1.Clone();
            
            RenderTarget2D tempRT1 = null;
            RenderTarget2D tempRT2 = null;

            PresentationParameters pp = device.PresentationParameters;
            mWidth = (target == null ? pp.BackBufferWidth : target.Width) / 2;
            mHeight = (target == null ? pp.BackBufferHeight : target.Height) / 2;

            lock(device)
            {
                tempRT1 = new RenderTarget2D(device, mWidth, mHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None);
                tempRT2 = new RenderTarget2D(device, mWidth, mHeight, false, device.PresentationParameters.BackBufferFormat, DepthFormat.None);
            }

            if((tempRT1 == null) || (tempRT2 == null))
            {
                throw new Exception("Can't create Rendertargets");
            }

            Configure();

            List<ARenderPass> passes = new List<ARenderPass>();
            passes.Add(new FullScreenQuadPass(texture, tempRT1, mMaterial1));
            passes.Add(new FullScreenQuadPass(tempRT1, tempRT2, mMaterial2));
            MultipleTexturesRenderPass finalPass = new MultipleTexturesRenderPass(new List<Texture2D>() { tempRT2 }, null, target, true);
            finalPass.ExpandToDestinationTarget = true;
            passes.Add(finalPass);

            mPasses = new MultipleRenderPass2(passes);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Configura el efecto.
        /// </summary>
        private void Configure()
        {
            // Configure material for vertical pass
            ConfigureMaterial(mMaterial1, 0, 1.0f / (float)mHeight);
            // Configure material for horizontal pass
            ConfigureMaterial(mMaterial2, 1.0f / (float)mWidth, 0);
        }

        /// <summary>
        /// Configura el material que usara para aplicar el efecto.
        /// </summary>
        /// <param name="material">Material.</param>
        /// <param name="dx">Desplazamiento en el eje X.</param>
        /// <param name="dy">Desplazamiento en el eje Y.</param>
        private void ConfigureMaterial(Material material, float dx, float dy)
        {
            MaterialParameter weightsParameter = material["SampleWeights"];
            MaterialParameter offsetsParameter = material["SampleOffsets"];
            
            // How many samples support
            int sampleCount = weightsParameter.EffectParameter.Elements.Count;

            // Blur amount
            float amountValue = Intensity;

            // Create temporary arrays for computing our filter settings.
            float[] sampleWeights = new float[sampleCount];
            Vector2[] sampleOffsets = new Vector2[sampleCount];

            // The first sample always has a zero offset.
            sampleWeights[0] = ComputeGaussian(0, amountValue);
            sampleOffsets[0] = new Vector2(0);

            // Maintain a sum of all the weighting values.
            float totalWeights = sampleWeights[0];

            // Add pairs of additional sample taps, positioned
            // along a line in both directions from the center.
            for (int i = 0; i < sampleCount / 2; i++)
            {
                // Store weights for the positive and negative taps.
                float weight = ComputeGaussian(i + 1, amountValue);

                sampleWeights[i * 2 + 1] = weight;
                sampleWeights[i * 2 + 2] = weight;

                totalWeights += weight * 2;

                // To get the maximum amount of blurring from a limited number of
                // pixel shader samples, we take advantage of the bilinear filtering
                // hardware inside the texture fetch unit. If we position our texture
                // coordinates exactly halfway between two texels, the filtering unit
                // will average them for us, giving two samples for the price of one.
                // This allows us to step in units of two texels per sample, rather
                // than just one at a time. The 1.5 offset kicks things off by
                // positioning us nicely in between two texels.
                float sampleOffset = i * 2 + 1.5f;

                Vector2 delta = new Vector2(dx, dy) * sampleOffset;

                // Store texture coordinate offsets for the positive and negative taps.
                sampleOffsets[i * 2 + 1] = delta;
                sampleOffsets[i * 2 + 2] = -delta;
            }

            // Normalize the list of sample weightings, so they will always sum to one.
            for (int i = 0; i < sampleWeights.Length; i++)
            {
                sampleWeights[i] /= totalWeights;
            }

            // Tell the effect about our new filter settings.
            weightsParameter.SetValue(sampleWeights);
            offsetsParameter.SetValue(sampleOffsets);
        }


        private float ComputeGaussian(float n, float amount)
        {
            float theta = amount;

            return (float)((1.0 / Math.Sqrt(2 * Math.PI * theta)) *
                           Math.Exp(-(n * n) / (2 * theta * theta)));
        }

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
