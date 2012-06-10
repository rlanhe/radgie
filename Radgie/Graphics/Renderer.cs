using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Camera;
using System.Diagnostics;
using Radgie.Core;

namespace Radgie.Graphics
{
    // TODO: Especificar esta clase en la configuracion del sistema
    /// <summary>
    /// Renderer del sistema grafico.
    /// Esta clase es la encargada de dirigir todo el proceso del dibujado de los objetos en pantalla.
    /// </summary>
    public class Renderer: IRenderer
    {
        #region Properties
        #region Renderer Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Material"/>
        /// </summary>
        public Material Material
        {
            get
            {
                return mMaterial;
            }
            set
            {
                mMaterial = value;
            }
        }
        private Material mMaterial;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.World"/>
        /// </summary>
        public Matrix World
        {
            get
            {
                return mWorld;
            }
            set
            {
                mWorld = value;
                CalculateWorldViewProj();
            }
        }
        private Matrix mWorld;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.View"/>
        /// </summary>
        public Matrix View
        {
            get
            {
                return mView;
            }
        }
        private Matrix mView;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Projection"/>
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return mProjection;
            }
        }
        private Matrix mProjection;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.WorldViewProjection"/>
        /// </summary>
        public Matrix WorldViewProjection
        {
            get
            {
                return mWorldViewProjection;
            }
        }
        private Matrix mWorldViewProjection;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Device"/>
        /// </summary>
        public GraphicsDevice Device 
        {
            get
            {
                return mDevice;
            }
        }
        private GraphicsDevice mDevice;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.SpriteBatch"/>
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return mSpriteBatch;
            }
        }
        private SpriteBatch mSpriteBatch;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Statistics"/>
        /// </summary>
        public GraphicsSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private GraphicsSystemStatistics mStatistics;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.CurrentCamera"/>
        /// </summary>
        public ICamera CurrentCamera
        {
            get
            {
                return mCurrentCamera;
            }
            set
            {
                mCurrentCamera = value;
                Debug.Assert(mCurrentCamera != null);
                mView = mCurrentCamera.View;
                mProjection = mCurrentCamera.Projection;
                mBackgroundColor = mCurrentCamera.BackgroundColor;

                Rectangle rtBounds = Device.ScissorRectangle;
                Rectangle cvBounds = mCurrentCamera.Viewport.Bounds;

                if((rtBounds.Width >= cvBounds.Width) && (rtBounds.Height >= cvBounds.Height))
                {
                    Device.Viewport = mCurrentCamera.Viewport;
                }
            }
        }
        private ICamera mCurrentCamera;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Viewport"/>
        /// </summary>
        public Viewport Viewport
        {
            get
            {
                if (mCurrentCamera != null)
                {
                    return mCurrentCamera.Viewport;
                }
                else
                {
                    return Device.Viewport;
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.AmbientLightColor"/>
        /// </summary>
        public Color AmbientLightColor
        {
            get
            {
                return mAmbientLightColor;
            }
            set
            {
                mAmbientLightColor = value;
            }
        }
        private Color mAmbientLightColor = Color.White;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.MaxLightsNumber"/>
        /// </summary>
        public int MaxLightsNumber
        {
            get
            {
                return (int)MathHelper.Min(mMaxLighstNumber, mLights == null ? 0 : mLights.Count);
            }
            set
            {
                mMaxLighstNumber = value;
            }
        }

        // TODO: config file
        /// <summary>
        /// Numero maximo de luces por defecto del renderer.
        /// </summary>
        protected int mMaxLighstNumber = 3;

        /// <summary>
        /// Color de fondo con el que se limpiara el renderTarget.
        /// </summary>
        protected Color mBackgroundColor;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Lights"/>
        /// </summary>
        public List<ILight> Lights
        {
            get
            {
                return mLights;
            }
            set
            {
                mLights = value;
            }
        }
        private List<ILight> mLights;

        /// <summary>
        /// Callback para ordenar las luces en funcion de su distancia al objeto que se esta dibujando.
        /// </summary>
        private Comparison<ILight> mSortLightsCallback;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.GraphicSystem"/>
        /// </summary>
        public IGraphicSystem GraphicSystem
        {
            get
            {
                if (mGraphicSystem == null)
                {
                    mGraphicSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
                }
                return mGraphicSystem;
            }
        }
        private IGraphicSystem mGraphicSystem;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.RenderMode"/>
        /// </summary>
        public RenderMode SelectedRenderMode
        {
            get
            {
                return mSelectedRenderMode;
            }
        }
        private RenderMode mSelectedRenderMode;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.InstancesData"/>
        /// </summary>
        public DynamicVertexBuffer InstancesData
        {
            get
            {
                return mInstancesWorlds;
            }
            set
            {
                mSelectedRenderMode = value == null ? RenderMode.NoInstancing : RenderMode.Instancing;

                mInstancesWorlds = value;
            }
        }
        private DynamicVertexBuffer mInstancesWorlds;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo renderer.
        /// </summary>
        /// <param name="device">Dispositivo grafico con el que va a interactuar.</param>
        /// <param name="spriteBatch">SpriteBatch que usara para dibujar sprites.</param>
        /// <param name="statistics">Registro de estadisticas del sistema grafico.</param>
        public Renderer(GraphicsDevice device, SpriteBatch spriteBatch, GraphicsSystemStatistics statistics)
        {
            mDevice = device;
            mSpriteBatch = spriteBatch;
            mStatistics = statistics;
            mSelectedRenderMode = RenderMode.NoInstancing;
            mSortLightsCallback = SortLightsCallback;
        }
        #endregion

        #region Methods
        #region IRenderer Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Render"/>
        /// </summary>
        public void Render(DrawDelegate drawDelegate)
        {
            if (drawDelegate == null)
            {
                throw new ArgumentNullException("DrawDelegate is null");
            }

            if (mMaterial != null)
            {
                SortLights();
                ApplyRenderState(mMaterial.RenderState);
                mMaterial.Bind(this);

                foreach (EffectPass pass in mMaterial.Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    drawDelegate.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderer.Clean"/>
        /// </summary>
        public void Clean()
        {
            mDevice.Clear(mBackgroundColor);
        }
        #endregion

        /// <summary>
        /// Ordena las luces segun su distancia al objeto.
        /// </summary>
        private void SortLights()
        {
            if (mLights != null)
            {
                Vector3 position = World.Translation;

                mLights.Sort(mSortLightsCallback);
            }
        }

        /// <summary>
        /// Metodo para ordenar las luces de una lista.
        /// </summary>
        /// <param name="light1">Luz 1</param>
        /// <param name="light2">Luz 2</param>
        /// <returns>Menor que cero, si light1 esta mas lejos que light2, 0 si estan a igual distancia y >0 para el resto de casos.</returns>
        private int SortLightsCallback(ILight light1, ILight light2)
        {
            float dist1 = Vector3.DistanceSquared(World.Translation, light1.Position);
            float dist2 = Vector3.DistanceSquared(World.Translation, light2.Position);
            return dist1.CompareTo(dist2);
        }
        
        /// <summary>
        /// Calcula la matriz combianda de mundo - vista - proyecion.
        /// </summary>
        private void CalculateWorldViewProj()
        {
            mWorldViewProjection = World * View * Projection;
        }

        /// <summary>
        /// Aplica el render state especificado en el material.
        /// </summary>
        /// <param name="renderState"></param>
        private void ApplyRenderState(RenderState renderState)
        {
            mDevice.BlendState = renderState.BlendState;
            mDevice.DepthStencilState = renderState.DepthStencilState;
            mDevice.RasterizerState = renderState.RasterizerState;
            // TODO: Apply each SamplerState??
            mDevice.SamplerStates[0] = renderState.SamplerState;
            mDevice.BlendFactor = renderState.BlendFactor;
            mDevice.MultiSampleMask = renderState.MultiSampleMask;
            mDevice.ReferenceStencil = renderState.ReferenceStencil;
        }
        #endregion
    }
}
