using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using System.IO;
using Radgie.File;
using System.Reflection;
using Radgie.Graphics;
using System.Threading;

namespace Radgie.Core
{
    /// <summary>
    /// Clase principal del motor.
    /// </summary>
    public class RadgieGame: Game
    {
        #region Properties
        
        /// <summary>
        /// Coleccion de sistemas que se ejecutan con el motor.
        /// </summary>
        private Dictionary<Type, ISystem> mSystems = new Dictionary<Type, ISystem>();

        /// <summary>
        /// Referencia interna al sistema grafico.
        /// </summary>
        private IGraphicSystem mGraphicSystem;

        /// <summary>
        /// Gestor de recursos.
        /// Facilita la carga de recursos de disco.
        /// </summary>
        public ResourceManager ResourceManager
        {
            get
            {
                return mResourceManager;
            }
        }
        private ResourceManager mResourceManager;

        /// <summary>
        /// Obtiene el GraphicsDeviceManager usado.
        /// </summary>
        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get
            {
                return mGraphicsDeviceManager;
            }
        }
        private GraphicsDeviceManager mGraphicsDeviceManager;

        /// <summary>
        /// Obtiene el JobScheduler.
        /// </summary>
        public JobScheduler JobScheduler
        {
            get
            {
                return mJobScheduler;
            }
        }
        private JobScheduler mJobScheduler;
        
        #region Singleton members
        
        /// <summary>
        /// Punto de acceso global a Radgie.
        /// </summary>
        public static RadgieGame Instance
        {
            get
            {
                return mInstance;
            }
        }
        private static RadgieGame mInstance = null;
        #endregion

        /// <summary>
        /// Estadisticas del sistema.
        /// </summary>
        public RadgieStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private RadgieStatistics mStatistics;
        #endregion

        #region Constructors

        /// <summary>
        /// Crea un nuevo juego Radgie.
        /// </summary>
        /// <param name="contentPath">Directorio base del Content del juego.</param>
        public RadgieGame(string contentPath)
        {
            mJobScheduler = new JobScheduler();
            mStatistics = new RadgieStatistics();

            mInstance = this;
            //Content.RootDirectory = contentPath;
            mResourceManager = new ResourceManager(contentPath, "en");
            this.IsFixedTimeStep = false;
            
            // Inicializa el driver grafico
            mGraphicsDeviceManager = new GraphicsDeviceManager(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Annade un nuevo systema para ejecutarse junto con el motor.
        /// </summary>
        /// <param name="type">Tipo del sistema que se annade.</param>
        /// <param name="system">Sistema que se quiere annadir al motor.</param>
        public void AddSystem(Type type, ISystem system)
        {
            mSystems.Add(type, system);
        }

        /// <summary>
        /// Obtiene un sistema en funcion de su tipo.
        /// </summary>
        /// <param name="type">Tipo del sistema.</param>
        /// <returns>El sistema si se ha annadido, null en caso contrario.</returns>
        public ISystem GetSystem(Type type)
        {
            ISystem system;
            mSystems.TryGetValue(type, out system);
            return system;
        }

        #region Game members

        /// <summary>
        /// Configura el motor e inicializa todos los sistemas.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Configura el motor a partir del fichero.
            RadgieGameConfiguration rgc = new RadgieGameConfiguration(ResourceManager.Load<XmlFile>("radgieconfig", false));
            rgc.Configure();

            mGraphicSystem = (IGraphicSystem)GetSystem(typeof(IGraphicSystem));
            if (mGraphicSystem == null)
            {
                throw new Exception("IGraphicSystem not found");
            }
        }

        /// <summary>
        /// Actualiza los sistemas del motor.
        /// </summary>
        /// <param name="gameTime">Tiempo transcurrido desde la ultima actualizacion</param>
        protected override void Update(GameTime gameTime)
        {
            // Actualiza los sistemas del motor
            foreach (KeyValuePair<Type, ISystem> tuple in mSystems)
            {
                tuple.Value.Update(gameTime);
            }

#if WINDOWS && !DEBUG
            // Muestra FPS en barra de titulo
            float fps = 1000.0f / gameTime.ElapsedGameTime.Milliseconds;
            Window.Title = fps + " FPS";
#endif

            base.Update(gameTime);
        }

        /// <summary>
        /// Ver <see cref="Microsoft.Xna.Framework.Game.BeginDraw"/>
        /// </summary>
        protected override bool BeginDraw()
        {
            bool drawFrame = false;

            try
            {
                Monitor.Enter(((IGraphicSystem)GetSystem(typeof(IGraphicSystem))).Device);
                
                drawFrame = base.BeginDraw();

                if (!drawFrame)
                {
                    Monitor.Exit(((IGraphicSystem)GetSystem(typeof(IGraphicSystem))).Device);
                }
            }
            catch
            {
                Monitor.Exit(((IGraphicSystem)GetSystem(typeof(IGraphicSystem))).Device);
            }

            return drawFrame;
        }

        /// <summary>
        /// Dibuja por pantalla.
        /// </summary>
        /// <param name="gameTime">Tiempo transcurrido desde la ultima actualizacion</param>
        protected override void Draw(GameTime gameTime)
        {
            try
            {
                mStatistics.AddFrame(gameTime.ElapsedGameTime.TotalSeconds);
                mGraphicSystem.Draw();
                base.Draw(gameTime);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Ver <see cref="Microsoft.Xna.Framework.Game.EndDraw"/>
        /// </summary>
        protected override void EndDraw()
        {
            base.EndDraw();
            Monitor.Exit(((IGraphicSystem)GetSystem(typeof(IGraphicSystem))).Device);
        }

        /// <summary>
        /// Libera los recursos de RadgieGame.
        /// </summary>
        /// <param name="disposing">Indica si es una llamada del sistema o del usuario.</param>
        protected override void Dispose(bool disposing)
        {
            mJobScheduler.Dispose();

            base.Dispose(disposing);
        }

        #endregion

        #endregion
    }
}
