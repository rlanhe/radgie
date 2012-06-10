using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Core;
using System.Xml;
#if WIN32
using log4net;
#endif
using Radgie.Util.Collection.ReferencePool;
using System.Xml.Linq;

namespace Radgie.Graphics
{
    /// <summary>
    /// Sistema de graficos.
    /// </summary>
    public class GraphicSystem: ASystem, IGraphicSystem
    {
        #region Properties

        /// <summary>
        /// Identificador para el parametro anchura del backbuffer.
        /// </summary>
        private const string KEY_BACKBUFFER_WIDTH = "backbufferWidth";

        /// <summary>
        /// Identificador para el parametro altura del backbuffer.
        /// </summary>
        private const string KEY_BACKBUFFER_HEIGHT = "backbufferHeight";

        /// <summary>
        /// Identificador para el parametro pantalla completa.
        /// </summary>
        private const string KEY_FULLSCREEN = "fullscreen";

        /// <summary>
        /// Callback para actualizar los elementos del pool.
        /// </summary>
        private PoolAction<IGraphicEntity> mUpdatePoolActionCallback;

        #region IGraphicSystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.RenderProcess"/>
        /// </summary>
        public IRenderable RenderProcess
        {
            get
            {
                return mRenderProcess;
            }
            set
            {
                lock(Device)
                {
                    mRenderProcess = value;
                }
            }
        }
        private IRenderable mRenderProcess;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.Renderer"/>
        /// </summary>
        public IRenderer Renderer
        {
            get
            {
                return mRenderer;
            }
        }
        private IRenderer mRenderer;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.Device"/>
        /// </summary>
        public GraphicsDevice Device
        {
            get
            {
                return RadgieGame.Instance.GraphicsDevice;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.DeviceManager"/>
        /// </summary>
        public GraphicsDeviceManager DeviceManager
        {
            get
            {
                return RadgieGame.Instance.GraphicsDeviceManager;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.Statistics"/>
        /// </summary>
        public GraphicsSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        protected GraphicsSystemStatistics mStatistics;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.GraphicEntityReferences"/>
        /// </summary>
        public IReferencePool<IGraphicEntity> GraphicEntityReferences
        {
            get
            {
                return mGraphicEntityReferences;
            }
        }
        private IReferencePool<IGraphicEntity> mGraphicEntityReferences;

        /// <summary>
        /// Anchura del backbuffer.
        /// </summary>
        private int mBackbufferWidth = 800;

        /// <summary>
        /// Altura del backbuffer.
        /// </summary>
        private int mBackbufferHeight = 600;

        /// <summary>
        /// Pantalla completa.
        /// </summary>
        private bool mFullscreen = false;

        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el sistema grafico.
        /// </summary>
        /// <param name="sc">Seccion del fichero xml donde se especifica su configuracion.</param>
        public GraphicSystem(XElement sc)
            : base(sc)
        {
            // TODO: Configurar esto en el fichero de radgie
            mStatistics = new GraphicsSystemStatistics();

            DeviceManager.PreferredBackBufferHeight = mBackbufferHeight;
            DeviceManager.PreferredBackBufferWidth = mBackbufferWidth;
            DeviceManager.IsFullScreen = mFullscreen;
            DeviceManager.ApplyChanges();

            mRenderer = new Renderer(Device, new SpriteBatch(Device), mStatistics);
            mGraphicEntityReferences = new ReferencePool<IGraphicEntity>(true);

            mUpdatePoolActionCallback = UpdatePoolAction;
        }
        #endregion

        #region Methods
        #region IGraphicSystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicSystem.Draw"/>
        /// </summary>
        public void Draw()
        {
            mStatistics.Reset();
            mStatistics.StartDrawTimer();
            // Limpia la pantalla
            Device.Clear(Color.Black);
            if (mRenderProcess != null)
            {
                mRenderProcess.Render(mRenderer);
            }
            mStatistics.StopDrawTimer();
        }
        #endregion

        #region ASystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.UpdateAction"/>
        /// </summary>
        protected override void UpdateAction(GameTime time)
        {
            mStatistics.StartUpdateTimer();
            mGraphicEntityReferences.FireActionOverPoolItems(mUpdatePoolActionCallback);
            mStatistics.StopUpdateTimer();
        }
        #endregion

        /// <summary>
        /// Delegado para actualizar un elemento del pool de objetos del sistema grafico.
        /// </summary>
        /// <param name="sEntity">Elemento del pool de objetos.</param>
        private void UpdatePoolAction(IGraphicEntity sEntity)
        {
            if ((sEntity.Component != null) && (sEntity.IsActive()))
            {
                sEntity.Update(LastTimeUpdated);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.LoadParameters"/>
        /// </summary>
        protected override bool LoadParameters(string name, string value)
        {
            bool result = base.LoadParameters(name, value);
            switch (name)
            {
                case KEY_BACKBUFFER_HEIGHT:
                    mBackbufferHeight = int.Parse(value);
                    result = true;
                    break;
                case KEY_BACKBUFFER_WIDTH:
                    mBackbufferWidth = int.Parse(value);
                    result = true;
                    break;
                case KEY_FULLSCREEN:
                    mFullscreen = bool.Parse(value);
                    result = true;
                    break;
            }
            return result;
        }
        #endregion
    }
}
