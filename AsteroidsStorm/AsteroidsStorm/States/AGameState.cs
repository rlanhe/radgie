using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using Radgie.Scene.Managers.Simple;
using Radgie.Scene;
using AsteroidsStorm.GameComponents.GUI;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics;
using Radgie.Graphics.RenderPass;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework;
using Radgie.Util.Debug;
using Radgie.Sound;
using AsteroidsStorm.RenderPasses;
using Radgie.File;

namespace AsteroidsStorm.States
{
    /// <summary>
    /// Clase base para los estados del juego.
    /// </summary>
    abstract class AGameState: AState
    {        
        /// <summary>
        /// Controlador del menu.
        /// </summary>
        protected IGUIController Controller
        {
            get
            {
                return GetFromContext<IGUIController>(GUI_CONTROLLER);
            }
        }
        public const string GUI_CONTROLLER = "GuiController";

        /// <summary>
        /// Escena para los elementos del GUI.
        /// </summary>
        protected IScene mGuiScene;
        /// <summary>
        /// Contenedor de los elementos del GUI.
        /// </summary>
        protected IContainer mContainer;
        /// <summary>
        /// Pasada para dibujar los elementos del GUI.
        /// </summary>
        protected SceneRenderPass mGuiRenderPass;
        /// <summary>
        /// Camara para dibujar los elementos del GUI.
        /// </summary>
        protected ICamera mGuiCam;
        /// <summary>
        /// Target donde dejar el resultado de dibujar los elementos del GUI.
        /// </summary>
        protected RenderTarget2D mGuiSceneTarget;

        /// <summary>
        /// Sonido al cambiar de widget seleccionado.
        /// </summary>
        protected static SoundEffect mFocusChangeSound;
        /// <summary>
        /// Sonido al activar un widget.
        /// </summary>
        protected static SoundEffect mFireActionSound;
        /// <summary>
        /// Sonido al cancelar una accion.
        /// </summary>
        protected static SoundEffect mBackActionSound;
        /// <summary>
        /// Sonido al cambiar el valor a un campo.
        /// </summary>
        protected static SoundEffect mChangeValueSound;

        /// <summary>
        /// Job para ejecutar en la salida del estado.
        /// </summary>
        protected Job mOnExitJob;

        /// <summary>
        /// Referencia al sistema de estados.
        /// </summary>
        protected static IStateSystem mStateSystem;

        /// <summary>
        /// Traducciones de la interfaz de usuario.
        /// </summary>
        protected static StringsFile mStrings;
        
        /// <summary>
        /// Inicializador estatico.
        /// </summary>
        static AGameState()
        {
            XmlFile xmlFile = RadgieGame.Instance.ResourceManager.Load<XmlFile>("Translations/" + ResourceManager.LOCALE_KEY, false);
            mStrings = new StringsFile(xmlFile.Content);
            mStateSystem = (IStateSystem)RadgieGame.Instance.GetSystem(typeof(IStateSystem));
            mFocusChangeSound = new SoundEffect("GameComponents/GUI/Sounds/focusChange");
            mFireActionSound = new SoundEffect("GameComponents/GUI/Sounds/forward");
            mBackActionSound = new SoundEffect("GameComponents/GUI/Sounds/back");
            mChangeValueSound = new SoundEffect("GameComponents/GUI/Sounds/changeValue");
        }

        /// <summary>
        /// Crea e inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public AGameState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnInitialize"/>
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            mGuiScene = new SimpleScene("gui_" + this.GetType().Name);

            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            Viewport viewport = gs.Device.Viewport;
            mContainer = new Frame("Main_Frame", viewport.Width, viewport.Height);
            mContainer.X = (int)(mContainer.Width * 0.5f);
            mContainer.Y = (int)(mContainer.Height * 0.5f);
            mGuiScene.AddComponent(mContainer.GameComponent);

#if DEBUG
            // Debug
            mGuiScene.AddComponent(new DebugConsole());
#endif

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(mGuiScene);

            mGuiCam = new Camera2D("gui_camera");
            mGuiCam.BackgroundColor = Color.Transparent;
            lock (gs.Device)
            {
                mGuiSceneTarget = new RenderTarget2D(gs.Device, viewport.Width, viewport.Height);
            }

            // Crea la pasada para dibujar la interfaz, pero no se la asigna al render, para que pueda ser personalizado en las clases hijas.
            mGuiRenderPass = new SceneRenderPass(mGuiScene, mGuiSceneTarget, mGuiCam, false, true);
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(mGuiScene);

            mGuiScene.Update(null);
        }
        
        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(mGuiScene);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        /// <summary>
        /// Crea una opcion de menu.
        /// </summary>
        /// <param name="text">Texto del boton.</param>
        /// <param name="tabOrder">Orden de tabulacion.</param>
        /// <param name="fireDelegate">Delegado que se ejecutara cuando se active el widget.</param>
        /// <param name="activateDelegate">Delegado que se ejecutara cuando se seleccione el widget.</param>
        /// <param name="deactivationDelegate">Delegado que se ejecutara cuando se deseleccione el widget.</param>
        /// <returns>Etiqueta que representa la opcion de menu.</returns>
        protected Label CreateMenuOption(string text, byte tabOrder, WidgetActionDelegate fireDelegate, WidgetActionDelegate activateDelegate, WidgetActionDelegate deactivationDelegate)
        {
            Label label = new Label(text, 250, 50);
            label.Text = text;
            label.TabOrder = tabOrder;
            label.Font = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk_small", false);
            label.OnActivateDelegate = activateDelegate;
            label.OnDeactivateDelegate = deactivationDelegate;
            label.FireDelegate = fireDelegate;
            return label;
        }

        /// <summary>
        /// Delegado que se ejecutara al seleccionar un widget.
        /// </summary>
        /// <param name="widget">Widget.</param>
        protected static void SelectWidgetDelegate(IWidget widget)
        {
            if (widget is Label)
            {
                ((Label)widget).Color = Color.Red;
            }
        }

        /// <summary>
        /// Delegado que se ejecutara al deseleccionar un widget.
        /// </summary>
        /// <param name="widget">Widget.</param>
        protected static void DeselectWidgetDelegate(IWidget widget)
        {
            if (widget is Label)
            {
                ((Label)widget).Color = Color.White;
            }
        }

        /// <summary>
        /// Crea una pasada bloompass para aplicar dicho efecto.
        /// </summary>
        /// <param name="texture">Textura base.</param>
        /// <returns>Pasada para aplicar el efecto bloompass.</returns>
        protected static BloomPass Create3DSceneBloomPass(Texture2D texture)
        {
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            PresentationParameters pp = gSystem.Device.PresentationParameters;

            RenderTarget2D rt = null;
            lock (gSystem.Device)
            {
                rt = new RenderTarget2D(gSystem.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            }

            return Create3DSceneBloomPass(texture, rt);
        }

        /// <summary>
        /// Crea una pasada bloompass para aplicar dicho efecto.
        /// </summary>
        /// <param name="texture">Textura base.</param>
        /// <param name="rt">Target donde dejar el resultado.</param>
        /// <returns>Pasada para aplicar el efecto bloompass.</returns>
        protected static BloomPass Create3DSceneBloomPass(Texture2D texture, RenderTarget2D rt)
        {
            BloomPass bloomPass = new BloomPass(texture, rt);
            bloomPass.BlurIntensity = 1.5f;
            bloomPass.BaseIntensity = 1.0f;
            bloomPass.BaseSaturation = 1.0f;
            bloomPass.BloomIntensity = 1.5f;
            bloomPass.BloomSaturation = 1.0f;
            bloomPass.Threshold = 0.25f;

            return bloomPass;
        }

        /// <summary>
        /// Crea una pasada para aplicar el efecto bloom a los componentes de la GUI.
        /// </summary>
        /// <param name="texture">Textura de entrada.</param>
        /// <returns>Pasada para aplicar el efecto.</returns>
        protected static BloomPass CreateHUDBloomPass(Texture2D texture)
        {
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            PresentationParameters pp = gSystem.Device.PresentationParameters;

            RenderTarget2D rt = null;
            lock (gSystem.Device)
            {
                rt = new RenderTarget2D(gSystem.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            }

            return CreateHUDBloomPass(texture, rt);
        }

        /// <summary>
        /// Crea una pasada para aplicar el efecto bloom a los componentes de la GUI.
        /// </summary>
        /// <param name="texture">Textura de entrada.</param>
        /// <param name="rt">Target donde dejar el resultado.</param>
        /// <returns>Pasada para aplicar el efecto.</returns>
        protected static BloomPass CreateHUDBloomPass(Texture2D texture, RenderTarget2D rt)
        {
            BloomPass bloomPass = new BloomPass(texture, rt);
            bloomPass.BlurIntensity = 1.0f;
            bloomPass.BaseIntensity = 1.0f;
            bloomPass.BaseSaturation = 1.0f;
            bloomPass.BloomIntensity = 1.0f;
            bloomPass.BloomSaturation = 1.0f;
            bloomPass.Threshold = 0.25f;

            return bloomPass;
        }

        /// <summary>
        /// Actualiza las acciones del GUI.
        /// </summary>
        protected void UpdateGUIActions()
        {
            // Reacciona ante las acciones del usuario
            IGUIController controller = Controller;

            if (controller.Down.Pressed)
            {
                mFocusChangeSound.Play();
                mContainer.NextWidget();
            }
            else if (controller.Up.Pressed)
            {
                mFocusChangeSound.Play();
                mContainer.PreviousWidget();
            }

            if (controller.Ok.Pressed)
            {
                mFireActionSound.Play();
                mContainer.FireAction();
            }

            // TODO: Go back??
        }
    }
}
