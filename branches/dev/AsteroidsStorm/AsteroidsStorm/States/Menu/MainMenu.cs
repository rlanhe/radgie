using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.GameComponents.GUI;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Radgie.Graphics;
using Radgie.Scene;
using Radgie.Graphics.RenderPass;
using Radgie.Scene.Managers.Simple;
using Radgie.Graphics.Camera;
using AsteroidsStorm.GameComponents.Skybox;
using Radgie.Util;
using AsteroidsStorm.GameComponents;
using AsteroidsStorm.RenderPasses;
using Radgie.Sound;
using Microsoft.Xna.Framework.Media;
using AsteroidsStorm.GameComponents.AsteroidsField;

namespace AsteroidsStorm.States.Menu
{
    /// <summary>
    /// Menun principal de la aplicacion.
    /// </summary>
    class MainMenu: AGameState
    {
        /// <summary>
        /// Evento para comenzar el juego.
        /// </summary>
        public static Event GO_START_GAME = new Event("MainMenu_Start_Game");
        /// <summary>
        /// Evento para ir al menu de opciones.
        /// </summary>
        public static Event GO_OPTIONS = new Event("MainMenu_Options");
        /// <summary>
        /// Evento para finalizar la aplicacion.
        /// </summary>
        public static Event GO_EXIT = new Event("MainMenu_Exit");

        private MultipleRenderPass2 mBatchRenderPass;
        private TextureRenderPass mFadeRenderPass;
        private BackCounter mFadeCounter;

        private const string MENU_BACKGROUND_SCENE = "MenuBackgroundScene";
        private const string MENU_BACKGROUND_SCENE_RENDER_PASS = "MenuBackgroundSceneRenderPass";
        private static Color FADE_START_COLOR = new Color(0.0f,0.0f,0.0f,255.0f);
        private static Color FADE_END_COLOR = new Color(0.0f,0.0f,0.0f,0.0f);

        private ARenderPass mBackgroundScenePass;
        private Song mMenuSoundTrack;

        ICamera3D camera;

        /// <summary>
        /// Crea e inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public MainMenu(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Inicializa las opciones configurables a sus valores por defecto.
        /// </summary>
        private void InitializeOptions()
        {
            Options.SetInvertAxes(false);
            Options.SetVolume(80);
            Options.SetFXVolume(80);
        }

        /// <summary>
        /// Crea la pasada de render para dibujar la escena 3D.
        /// </summary>
        private void CreateRenderPass()
        {
            // Escena3d de fondo
            IScene scene = new SimpleScene("AsteroidsScene");
            SetInContext(MENU_BACKGROUND_SCENE, scene);

            // Camara
            camera = new Camera3D("AsteroidsScene_Camera");
            camera.Transformation.Translation = Vector3.Backward * 15.0f;

            // Componentes escena
            AsteroidsStorm.GameComponents.AsteroidsField.MenuAsteroidSectorState asteroidController = new AsteroidsStorm.GameComponents.AsteroidsField.MenuAsteroidSectorState(null);
            asteroidController.MaxAsteroidsPerSecond = 1;
            //AsteroidsField sector = new AsteroidsField(scene, 400, 40, 20, 10, Vector3.Backward, asteroidController);
            Skybox skybox = new Skybox(100.0f, "GameComponents/Skybox/Graphics/Textures/top", "GameComponents/Skybox/Graphics/Textures/back", "GameComponents/Skybox/Graphics/Textures/front", "GameComponents/Skybox/Graphics/Textures/bottom", "GameComponents/Skybox/Graphics/Textures/left", "GameComponents/Skybox/Graphics/Textures/right");
            scene.AddComponent(skybox);
            skybox.Transformation.Rotation *= new Quaternion(Vector3.Up, (float)MathUtil.GetRandomDouble());
            scene.AddComponent(camera);
            //sector.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.PiOver2);

            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            PresentationParameters pp = gSystem.Device.PresentationParameters;
            
            // Pasadas del render
            List<ARenderPass> passes = new List<ARenderPass>();

            // Pasada controles interfaz
            passes.Add(mGuiRenderPass);

            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            RenderTarget2D guiTarget = bloomPass.Target;
            passes.Add(bloomPass);

            // Pasada para dibujar la escena y aplicar un efecto de bloom
            RenderTarget2D rt = null;
            lock (gSystem.Device)
            {
                rt = new RenderTarget2D(gSystem.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            }
            SceneRenderPass pass = new SceneRenderPass(scene, rt, camera, true, true);

            bloomPass = Create3DSceneBloomPass(rt, null);

            mBackgroundScenePass = new MultipleRenderPass2(new List<ARenderPass>() { pass, bloomPass });
            passes.Add(mBackgroundScenePass);

            // Comparte la pasada que dibuja la escena para que sea utilizada por el resto de estados del menu
            SetInContext(MENU_BACKGROUND_SCENE_RENDER_PASS, mBackgroundScenePass);

            // Pasada para dibujar por encima de la escena los controles del menu
            passes.Add(new TextureRenderPass(guiTarget, null, false));

            // Pasada para realizar fadeIn del menu
            Texture2D transparentTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/transparent", false);
            mFadeRenderPass = new TextureRenderPass(transparentTexture, null, false);
            mFadeRenderPass.ExpandToDestinationTarget = true;
            passes.Add(mFadeRenderPass);

            // Establece el proceso de render
            mBatchRenderPass = new MultipleRenderPass2(passes);
        }

        /// <summary>
        /// Crea el HUD del estado.
        /// </summary>
        private void CreateHUD()
        {
            // Monta la escena
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("MainMenu_Title", 800, 100);
            label.Text = mStrings["AsteroidsStorm"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 10.0f);

            // Opciones del menu
            Frame frame = new Frame("Menu_ops", 1, 400);
            mContainer.AddWidget(frame);
            LayaoutUtil.PlaceInX(frame, 50.0f);
            LayaoutUtil.PlaceInY(frame, 80.0f);

            frame.AddWidget(CreateMenuOption(mStrings["StartGame"], 10, StartGameFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));
            frame.AddWidget(CreateMenuOption(mStrings["Options"], 20, OptionsFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));
            frame.AddWidget(CreateMenuOption(mStrings["Exit"], 30, ExitFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));

            LayaoutUtil.SpreadWidgetsInY(frame);

            // Selecciona el primer widget
            mContainer.NextWidget();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeOptions();
            mMenuSoundTrack = RadgieGame.Instance.ResourceManager.Load<Song>("GameComponents/GUI/Sounds/Music/01");
            mFadeCounter = new BackCounter(TimeSpan.FromSeconds(1));
            CreateHUD();
            CreateRenderPass();
        }

        public override void OnEntry()
        {
            base.OnEntry();

            // Annade la escena de fondo a la lista de escenas a actualizar
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(GetFromContext<IScene>(MENU_BACKGROUND_SCENE));

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mBatchRenderPass;

            if (mStateMachine.LastEvent != Options.GO_BACK)
            {
                // Arranca el counter para efecto fade
                mFadeRenderPass.Color = FADE_START_COLOR;
                mFadeCounter.Start();
            }

            ISoundSystem soundSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            if (mStateMachine.LastEvent != Options.GO_BACK)
            {
                soundSystem.Play(0.7f, mMenuSoundTrack);
                soundSystem.Repeat = true;
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (!mFadeCounter.Finished())
            {
                mFadeRenderPass.Color = Color.Lerp(FADE_START_COLOR, FADE_END_COLOR, (float)(mFadeCounter.TimeElapsed.TotalSeconds / mFadeCounter.Duration.TotalSeconds));
            }
            UpdateGUIActions();
        }

        public override void OnExit()
        {
            base.OnExit();

            // Quita la escena del sistema de escenas. No destruye los componentes, ya que se volvera a este estado.
            // No modifica el renderpass que debe dibujarse.
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(mGuiScene);
        }

        /// <summary>
        /// Ejecuta la accion relacionada con la opcion de menu de comenzar el juego.
        /// </summary>
        private void StartGameFireDelegate(IWidget widget)
        {
            SendEvent(GO_START_GAME);
        }

        /// <summary>
        /// Ejecuta la accion relacionada con la opcion de menu de mostrar las opciones del juego.
        /// </summary>
        private void OptionsFireDelegate(IWidget widget)
        {
            SendEvent(GO_OPTIONS);
        }

        /// <summary>
        /// Ejecuta la accion relacionada con la opcion de menu de salir del juego.
        /// </summary>
        private void ExitFireDelegate(IWidget widget)
        {
            SendEvent(GO_EXIT);
        }
    }
}
