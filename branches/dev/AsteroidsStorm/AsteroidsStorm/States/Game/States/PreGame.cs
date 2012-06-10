using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using AsteroidsStorm.GameComponents.Spaceship;
using AsteroidsStorm.GameComponents.Axes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsStorm.GameComponents.GUI;
using Radgie.Util;
using AsteroidsStorm.GameComponents.Skybox;
using AsteroidsStorm.GameComponents.Box;
using AsteroidsStorm.GameComponents.ChaseCamera;
using Radgie.Scene;
using Radgie.Graphics;
using Radgie.Graphics.RenderPass;
using System.Threading;
using AsteroidsStorm.GameComponents;
using Radgie.Sound;
using AsteroidsStorm.RenderPasses;

namespace AsteroidsStorm.States.Game.States
{
    /// <summary>
    /// Estado anterior al lanzamiento del juego.
    /// Carga la partida.
    /// </summary>
    class PreGame: AGameState
    {
        /// <summary>
        /// Evento para dar comienzo al juego.
        /// </summary>
        public static Event GO_GAME = new Event("Go_Game");

        private Job mGameLoadJob;
        private int mDoneProgress;

        private MultipleRenderPass2 mBatchRenderPass;

        private TextureRenderPass mFadeRenderPass;
        private TextureRenderPass mFadeRenderPass2;
        private BackCounter mFadeOutCounter;
        private BackCounter mFadeOutCounter2;
        private static Color mFaceStartColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        private static Color mFaceEndColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        private Bar mLoadBar;

        private const string MENU_BACKGROUND_SCENE_RENDER_PASS = "MenuBackgroundSceneRenderPass";
        private const string MENU_BACKGROUND_SCENE = "MenuBackgroundScene";

        private int mBackgroundScenePassIndex;
        
        /// <summary>
        /// Crea e inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public PreGame(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnInitialize"/>
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Monta la escena 2D
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("PreGame_Title", 800, 100);
            label.Text = mStrings["PleaseWait"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 5.0f);


            mLoadBar = new Bar("load_barr", ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).Device.Viewport.Width, 30);
            mContainer.AddWidget(mLoadBar);
            mLoadBar.Texture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/gui_texture_white", false); ;
            LayaoutUtil.SetPositionInX(mLoadBar, 0);
            LayaoutUtil.PlaceInY(mLoadBar, 80.0f);
            
            // Pasada para realizar fadeIn del menu
            Texture2D transparentTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/transparent", false);
            mFadeRenderPass = new TextureRenderPass(transparentTexture, null, false);
            mFadeRenderPass.ExpandToDestinationTarget = true;
            mFadeRenderPass.Color = mFaceStartColor;

            mFadeRenderPass2 = new TextureRenderPass(transparentTexture, null, false);
            mFadeRenderPass2.ExpandToDestinationTarget = true;
            mFadeRenderPass2.Color = mFaceStartColor;

            List<ARenderPass> passes = new List<ARenderPass>();
            passes.Add(mGuiRenderPass);
            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            passes.Add(bloomPass);
            mBackgroundScenePassIndex = passes.Count;
            passes.Add(mGuiRenderPass);
            passes.Add(mFadeRenderPass);
            passes.Add(new TextureRenderPass(bloomPass.Target, null, false));
            passes.Add(mFadeRenderPass2);
            mBatchRenderPass = new MultipleRenderPass2(passes);

            mGameLoadJob = new Job(this.GameLoad);
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            mBatchRenderPass.Passes.RemoveAt(mBackgroundScenePassIndex);
            mBatchRenderPass.Passes.Insert(mBackgroundScenePassIndex, GetFromContext<ARenderPass>(MENU_BACKGROUND_SCENE_RENDER_PASS));

            mFadeOutCounter = new BackCounter(TimeSpan.FromSeconds(2.0f));
            mFadeOutCounter2 = new BackCounter(TimeSpan.FromSeconds(2.0f));
            
            mFadeRenderPass.Color = mFaceStartColor;
            mFadeRenderPass2.Color = mFaceStartColor;

            mDoneProgress = 0;
            mLoadBar.Value = mDoneProgress;

            // RenderProcess
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mBatchRenderPass;
        }

        /// <summary>
        /// Comienza la carga de la partida.
        /// </summary>
        private void GameLoad()
        {
            mDoneProgress = 2;
            GameData gameState = new GameData();
            SetInContext("GameData", gameState);
            // Almacena en la escena una referencia a la informacion de la partida para que puedan acceder los elementos de la escena.
            gameState.GameScene.SetInContext("GameData", gameState);
            mDoneProgress = 3;
            // Controlador del juego
            gameState.PlayerState.PlayerIndex = Microsoft.Xna.Framework.PlayerIndex.One;
            gameState.PlayerState.GameController = new GameController(gameState.PlayerState.PlayerIndex);
            mDoneProgress = 4;
            // Nave seleccionada por el jugador
            string selectedShip = GetFromContext<string>("SelectedShip");
            mDoneProgress = 5;
            gameState.PlayerState.Spaceship = new Spaceship("ship01", gameState.PlayerState.GameController, selectedShip == null ? "GameComponents/Spaceship/Icaro" : selectedShip, true);
            mDoneProgress = 6;
            gameState.GameScene.AddComponent(gameState.PlayerState.Spaceship);

            mDoneProgress = 20;
            
            // Sector de asteroides
            int width = 11;// 8
            int height = 10;// 7
            int length = 200;

            AsteroidsStorm.GameComponents.AsteroidsField.Random asteroidsFieldController = new AsteroidsStorm.GameComponents.AsteroidsField.Random(null);
            asteroidsFieldController.MaxAsteroidsPerSecond = 5;

            mDoneProgress = 40;

            gameState.AsteroidsField = new AsteroidsStorm.GameComponents.AsteroidsField.AsteroidsField(gameState.GameScene, 1200, width, height, length, Vector3.Backward, asteroidsFieldController);
            gameState.AsteroidsField.CollisionGroup.AddGameComponent(gameState.PlayerState.Spaceship);
            //gameState.AsteroidsField.Transformation.Translation = new Vector3(0.0f, -length/2.0f+10.0f, 0.0f);

            mDoneProgress = 60;
            
            // Caja contenedora
            Box box = new Box(width, height, "GameComponents/Box/Graphics/Textures/grid");
            gameState.GameScene.AddComponent(box);
            gameState.PlayerState.Spaceship.PositionLimits = new Vector4(height / 2.0f, -height / 2.0f, -width / 2.0f, width / 2.0f);

            mDoneProgress = 75;

            // Skybox
            Skybox skybox = new Skybox(3000.0f, "GameComponents/Skybox/Graphics/Textures/top", "GameComponents/Skybox/Graphics/Textures/back", "GameComponents/Skybox/Graphics/Textures/front", "GameComponents/Skybox/Graphics/Textures/bottom", "GameComponents/Skybox/Graphics/Textures/left", "GameComponents/Skybox/Graphics/Textures/right");
            gameState.GameScene.AddComponent(skybox);
            skybox.Transformation.Rotation *= new Quaternion(Vector3.Up, (float)MathUtil.GetRandomDouble());

            mDoneProgress = 90;

            // Camara del juego
            ChaseCamera camera = new ChaseCamera("playerCam", gameState.PlayerState.Spaceship, new Vector3(0.0f, 1.0f, 5.0f), new Vector3(0.0f, 0.0f, -5.0f), new Vector3(-width / 3.0f + 2.0f, -height / 2.0f + 1.0f, 0.0f), new Vector3(width / 2.0f - 3.0f, height / 2.0f - 2.0f, 0.0f));
            camera.NearPlaneDistance = 0.1f;
            camera.FarPlaneDistance = 5000.0f;
            gameState.GameScene.AddComponent(camera);
            gameState.PlayerState.SelectedCamera = camera;

            // Objetos que se mueven con la nave
            gameState.PlayerState.Spaceship.AddObserver(skybox, skybox.UpdatePosition);
            gameState.PlayerState.Spaceship.AddObserver(box, box.UpdatePosition);
            gameState.PlayerState.Spaceship.AddObserver(gameState.AsteroidsField, gameState.AsteroidsField.UpdatePosition);

            mDoneProgress = 100;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (mDoneProgress == 0)
            {
                if (mFadeOutCounter.State == Radgie.Util.Timer.TimerState.STOPPED)
                {
                    mFadeOutCounter.Start();
                }
                else
                {
                    if (!mFadeOutCounter.Finished())
                    {
                        mFadeRenderPass.Color = Color.Lerp(mFaceStartColor, mFaceEndColor, (float)(mFadeOutCounter.TimeElapsed.TotalSeconds / mFadeOutCounter.Duration.TotalSeconds));
                    }
                    else
                    {
                        mDoneProgress = 1;
                        // Lanza la carga del juego en segundo plano
                        RadgieGame.Instance.JobScheduler.AddJob(mGameLoadJob);
                    }
                }
            }
            else
            {
                mLoadBar.Target = mDoneProgress;
                mLoadBar.Update(time);

                if ((mGameLoadJob.Finished) && (mLoadBar.Value == 100))
                {
                    if (mFadeOutCounter2.State == Radgie.Util.Timer.TimerState.STOPPED)
                    {
                        mFadeOutCounter2.Start();

                        // Silencia la musica del juego
                        //ISoundSystem soundSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
                        //soundSystem.Stop(0.7f);
                    }
                    else
                    {
                        mFadeRenderPass2.Color = Color.Lerp(mFaceStartColor, mFaceEndColor, (float)(mFadeOutCounter2.TimeElapsed.TotalSeconds / mFadeOutCounter2.Duration.TotalSeconds));
                        if (mFadeOutCounter2.Finished())
                        {
                            SendEvent(GO_GAME);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            mDoneProgress = 0;
            mLoadBar.Value = mDoneProgress;

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(GetFromContext<IScene>(MENU_BACKGROUND_SCENE));
        }
    }
}
