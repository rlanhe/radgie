using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using AsteroidsStorm.GameComponents.GUI;
using Microsoft.Xna.Framework;
using Radgie.Graphics;
using Radgie.Scene;
using AsteroidsStorm.GameComponents.Spaceship;
using System.IO;
using Radgie.Scene.Managers.Simple;
using Radgie.Graphics.Camera;
using Radgie.Graphics.RenderPass;
using System.Threading;
using Radgie.Graphics.Entity;
using AsteroidsStorm.RenderPasses;

namespace AsteroidsStorm.States.Menu
{
    /// <summary>
    /// Estado de seleccion de la nave.
    /// </summary>
    class SelectShip : AGameState
    {
        /// <summary>
        /// Evento para volver al menu de la aplicacion.
        /// </summary>
        public static Event GO_MENU = new Event("SelectShip_Menu");
        /// <summary>
        /// Evento para ir al estado del juego.
        /// </summary>
        public static Event GO_GAME = new Event("SelectShip_Game");

        private List<string> mSpaceshipsIdList;
        private List<Spaceship> mSpaceshipsList;
        private IScene m3DScene;
        private ICamera3D m3DCamera;
        private SceneRenderPass m3DSceneRenderPass;
        private MultipleRenderPass2 mFinalPass;

        private Radgie.Core.GameComponent mRotationNode;
        private int mCurrentShipIndex;
        private Spaceship mCurrentShip;
        private int mBackgroundScenePassIndex;

        private const int BAR_SIZE = 200;
        private const int BAR_SPEED = 100;
        private Bar mArmorBar;
        private Bar mSteerBar;

        private const string MENU_BACKGROUND_SCENE_RENDER_PASS = "MenuBackgroundSceneRenderPass";

        private const string SPACESHIPS_PATH = "GameComponents/Spaceship/";

        private static Quaternion ROTATION_SPEED = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Pi/80.0f);

        private Job mSpaceshipLoadJob;

        private bool mShipAutoSelected;

        public SelectShip(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Carga los modelos de las naves espaciales.
        /// </summary>
        private void LoadSpaceships()
        {
            mSpaceshipsIdList = new List<string>();

            // Obtiene la lista de naves disponibles
            DirectoryInfo dir = new DirectoryInfo(RadgieGame.Instance.ResourceManager.ContentDirPath + "/GameComponents/Spaceship");

            if (!dir.Exists)
            {
                throw new Exception("Spaceship folder not found");
            }

            FileInfo[] shipFiles = dir.GetFiles("*.*");
            foreach (FileInfo shipFile in shipFiles)
            {
                string name = shipFile.FullName.Replace(shipFile.Extension, string.Empty);
                name = name.Substring(name.LastIndexOf('\\') + 1);
                mSpaceshipsIdList.Add(name);
            }

            // Carga las naves en un hilo secundario
            mSpaceshipLoadJob = new Job(
                delegate()
                {
                    // Carga la lista de naves
                    mSpaceshipsList = new List<Spaceship>();
                    foreach (string id in mSpaceshipsIdList)
                    {
                        Spaceship ship = new Spaceship(id, null, SPACESHIPS_PATH + id, false);
                        mSpaceshipsList.Add(ship);
                    }
                });
            RadgieGame.Instance.JobScheduler.AddJob(mSpaceshipLoadJob);
        }

        /// <summary>
        /// Crea e inicializa el HUD del estado.
        /// </summary>
        private void CreateHUD()
        {
            // Monta la escena 2D
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("SelectShip_Title", 800, 100);
            label.Text = mStrings["SelectYourShip"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 5.0f);

            // Datos nave
            Frame frame = new Frame("SpaceshipInfo", 200, 400);
            mContainer.AddWidget(frame);

            LayaoutUtil.PlaceInX(frame, 90.0f);
            LayaoutUtil.PlaceInY(frame, 70.0f);

            SpriteFont regularFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk_small", false);

            Texture2D texture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/gui_texture", false);

            label = new Label("Steer", 200, 20);
            label.TabOrder = 20;
            frame.AddWidget(label);
            label.Text = mStrings["Steer"];
            label.Font = regularFont;
            LayaoutUtil.PlaceInX(label, 0.0f);

            mSteerBar = new Bar("Steer_bar", BAR_SIZE, 30);
            mSteerBar.TabOrder = 30;
            frame.AddWidget(mSteerBar);
            mSteerBar.Texture = texture;
            LayaoutUtil.PlaceInX(mSteerBar, 0.0f);

            label = new Label("Armor", 200, 20);
            label.TabOrder = 40;
            frame.AddWidget(label);
            label.Text = mStrings["Armor"];
            label.Font = regularFont;
            LayaoutUtil.PlaceInX(label, 0.0f);
            
            mArmorBar = new Bar("Armor_bar", BAR_SIZE, 30);
            mArmorBar.TabOrder = 50;
            frame.AddWidget(mArmorBar);
            mArmorBar.Texture = texture;
            LayaoutUtil.PlaceInX(mArmorBar, 0.0f);

            LayaoutUtil.SpreadWidgetsInY(frame);
        }

        /// <summary>
        /// Crea la pasada para dibujar la escena 3D.
        /// </summary>
        private void CreateRenderPass()
        {
            // Monta la escena 3D
            m3DScene = new SimpleScene("selectShip3DScene");
            mRotationNode = new Radgie.Core.GameComponent("RotationNode");
            m3DScene.AddComponent(mRotationNode);

            // Camara 3D
            m3DCamera = new Camera3D("selectShip3DSceneCamera");
            m3DCamera.FarPlaneDistance = 1000.0f;
            m3DCamera.Viewport = new Viewport(0, 100, (int)(mContainer.Width*0.8f), (int)(mContainer.Height*0.8f));
            m3DCamera.Transformation.Translation = new Vector3(0.0f, 1.0f, -1.5f);

            //m3DCamera.BackgroundColor = Color.Tomato;
            m3DScene.AddComponent(m3DCamera);
            m3DSceneRenderPass = new SceneRenderPass(m3DScene, m3DCamera, false, false);

            // RenderPass
            List<ARenderPass> passes = new List<ARenderPass>();
            passes.Add(mGuiRenderPass);

            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            passes.Add(bloomPass);

            // Se introduce una pasada ficiticia que sera sustituida en onEntry con la pasada que se usen desde el resto de estados del menu
            mBackgroundScenePassIndex = passes.Count;
            passes.Add(mGuiRenderPass);

            passes.Add(m3DSceneRenderPass);

            passes.Add(new TextureRenderPass(bloomPass.Target, null, false));


            mFinalPass = new MultipleRenderPass2(passes);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            LoadSpaceships();

            CreateHUD();

            CreateRenderPass();
        }

        public override void OnEntry()
        {
            base.OnEntry();

            // Modifica la pasada que dibuja la escena de fondo por la actual que se este utilizando
            mFinalPass.Passes.RemoveAt(mBackgroundScenePassIndex);
            mFinalPass.Passes.Insert(mBackgroundScenePassIndex, GetFromContext<ARenderPass>(MENU_BACKGROUND_SCENE_RENDER_PASS));

            // Annade la escena 3d a la lista de escenas
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(m3DScene);

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mFinalPass;
        }

        /// <summary>
        /// Selecciona la nave actual como nave para usar en el juego.
        /// </summary>
        /// <param name="index">indice de la nave.</param>
        private void SelectShipAt(int index)
        {
            if (index < 0)
            {
                mCurrentShipIndex = mSpaceshipsIdList.Count - 1;
            }
            else if (index >= mSpaceshipsIdList.Count)
            {
                mCurrentShipIndex = 0;
            }

            if (mCurrentShip != null)
            {
                mRotationNode.RemoveGameComponent(mCurrentShip);
            }

            mCurrentShip = mSpaceshipsList[mCurrentShipIndex];
            mRotationNode.AddGameComponent(mCurrentShip);
            // Fuerza la actualización de la rotación de la nave
            mRotationNode.Update(null);
            // Actualiza valores nave
            mArmorBar.Target = (int)(mSpaceshipsList[mCurrentShipIndex].Armor);
            mSteerBar.Target = (int)(mSpaceshipsList[mCurrentShipIndex].Steer);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (mSpaceshipLoadJob.Finished)
            {
                if (!mShipAutoSelected)
                {
                    SelectShipAt(0);
                    m3DCamera.Target = mCurrentShip.World.Translation;
                    mShipAutoSelected = true;
                }

                // Rota la nave
                mRotationNode.Transformation.Rotation *= ROTATION_SPEED;

                UpdateGUIActions();

                // Reacciona ante las acciones del usuario
                IGUIController controller = Controller;

                if (controller.Left.Pressed)
                {
                    SelectShipAt(--mCurrentShipIndex);
                    mChangeValueSound.Play();
                }
                else if (controller.Right.Pressed)
                {
                    SelectShipAt(++mCurrentShipIndex);
                    mChangeValueSound.Play();
                }

                if (controller.Ok.Pressed)
                {
                    SetInContext("SelectedShip", SPACESHIPS_PATH + mSpaceshipsIdList[mCurrentShipIndex]);
                    SendEvent(GO_GAME);
                }
                else if (controller.Cancel.Pressed)
                {
                    SendEvent(GO_MENU);
                }

                UpdateHUD(time);
            }
        }

        private void UpdateHUD(GameTime time)
        {
            mArmorBar.Update(time);
            mSteerBar.Update(time);
        }

        public override void OnExit()
        {
            base.OnExit();

            // Quita la escena del sistema de escenas. No destruye los componentes, ya que se volvera a este estado
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(mGuiScene);
            sSystem.RemoveScene(m3DScene);
        }
    }
}
