using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.GameComponents.Axes;
using Radgie.Core;
using Microsoft.Xna.Framework;
using AsteroidsStorm.GameComponents.Spaceship;
using Radgie.Graphics;
using Radgie.Scene;
using Radgie.Graphics.RenderPass;
using Radgie.Util;
using Radgie.Scene.Managers.Simple;
using AsteroidsStorm.GameComponents.ChaseCamera;
using Radgie.Core.Collision;
using Radgie.Graphics.Entity;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsStorm.GameComponents.Skybox;
using Radgie.Sound;
using AsteroidsStorm.GameComponents.GUI;
using AsteroidsStorm.GameComponents.Box;
using AsteroidsStorm.RenderPasses;
using AsteroidsStorm.GameComponents;
using Radgie.Util.Debug;
using AsteroidsStorm.GameComponents.Spaceship.States;
using Microsoft.Xna.Framework.Media;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.States.Game.States
{
    /// <summary>
    /// Estado principal del juego.
    /// Durante este estado se desarrolla la accion del juego.
    /// </summary>
    class Game: AGameState
    {
        /// <summary>
        /// Evento para pasar al estado de pausa.
        /// </summary>
        public static Event GO_PAUSE = new Event("Game_Go_Pause");
        /// <summary>
        /// Evento para terminar la ejecucion del juego.
        /// </summary>
        public static Event GO_GAME_OVER = new Event("Game_Go_Game_Over");

        private bool mDebug = false;

        Image mLifeIcon;
        Bar mLifeBar;
        Image mEnergyIcon;
        Bar mEnergyBar;
        Image mBonus;
        const int BAR_SIZE_WIDTH = 220;
        const int BAR_SIZE_HEIGHT = 25;
        Label mScore;
        Image mLeftHUDBackground;
        Image mRightHUDBackground;

        private Texture2D mBombTexture;
        private Texture2D mShieldTexture;
        private Texture2D mLifeTexture;
        private Texture2D mEnergyTexture;

        private GameData mGameState;

        private TextureRenderPass mFadeRenderPass;
        private static Color mFaceEndColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        private static Color mFaceStartColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        private BackCounter mFadeOutCounter;

        private static List<CollisionRecord> mResults;

        private MultipleRenderPass2 multiplePass;

        private const string GAME_BACKGROUND_SCENE = "GameBackgroundScene";
        private const string GAME_BACKGROUND_SCENE_PASS = "GameBackgroundScenePass";

        private SceneRenderPass mSceneRenderPass;

        private Song mBackgroundSong;

        private BackCounter mGameOverCounter;
        private Behaviour mSpaceshipDestructionController;
        
        /// <summary>
        /// Inicializador estatico del estado.
        /// </summary>
        static Game()
        {
            mResults = new List<CollisionRecord>();
        }

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public Game(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Crea la interfaz del juego.
        /// </summary>
        private void CreateHUD()
        {
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));

            string fontId = "Radgie/Graphics/Fonts/quartz_medium_monospace";
            float yPos = 5.0f;

            Color color = new Color(10, 224, 244);

            Texture2D hudBackgroundTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/hud_background", false);

            Frame leftHUD = new Frame("leftHUD", hudBackgroundTexture.Bounds.Width, hudBackgroundTexture.Bounds.Height);
            mContainer.AddWidget(leftHUD);
            LayaoutUtil.PlaceInX(leftHUD, yPos);
            LayaoutUtil.PlaceInY(leftHUD, yPos);

            mLeftHUDBackground = new Image("Back", hudBackgroundTexture.Bounds.Width, hudBackgroundTexture.Bounds.Height);
            mLeftHUDBackground.Texture = hudBackgroundTexture;
            leftHUD.AddWidget(mLeftHUDBackground);
            LayaoutUtil.PlaceInX(mLeftHUDBackground, 0);
            LayaoutUtil.PlaceInY(mLeftHUDBackground, 0);

            mLifeTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/life", false); ;
            mLifeIcon = new Image("lifeIcon", mLifeTexture.Bounds.Width, mLifeTexture.Bounds.Height);
            mLifeIcon.Texture = mLifeTexture;
            leftHUD.AddWidget(mLifeIcon);
            LayaoutUtil.SetPositionInX(mLifeIcon, 200);
            LayaoutUtil.SetPositionInY(mLifeIcon, 22);

            Texture2D hudTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/gui_texture", false);
            mLifeBar = new Bar("lifeBar", BAR_SIZE_WIDTH, BAR_SIZE_HEIGHT);
            mLifeBar.Texture = hudTexture;
            mLifeBar.Absolute = true;
            leftHUD.AddWidget(mLifeBar);
            LayaoutUtil.SetPositionInX(mLifeBar, 260);
            LayaoutUtil.SetPositionInY(mLifeBar, 25);

            mEnergyTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/energy", false);
            mEnergyIcon = new Image("energyIcon", mEnergyTexture.Bounds.Width, mEnergyTexture.Bounds.Height);
            mEnergyIcon.Texture = mEnergyTexture;
            mEnergyIcon.TabOrder = 12;
            mLeftHUDBackground.DrawOrder = 0;
            leftHUD.AddWidget(mEnergyIcon);
            LayaoutUtil.SetPositionInX(mEnergyIcon, 200);
            LayaoutUtil.SetPositionInY(mEnergyIcon, 70);

            mEnergyBar = new Bar("energyBar", BAR_SIZE_WIDTH, BAR_SIZE_HEIGHT);
            mEnergyBar.Texture = hudTexture;
            mEnergyBar.Absolute = true;
            leftHUD.AddWidget(mEnergyBar);
            LayaoutUtil.SetPositionInX(mEnergyBar, 260);
            LayaoutUtil.SetPositionInY(mEnergyBar, 72);

            mBombTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/bomb", false);
            mShieldTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/shield", false);

            mBonus = new Image("bonus", mBombTexture.Bounds.Width, mBombTexture.Bounds.Height);
            mBonus.Texture = null;
            mBonus.TabOrder = 20;
            mBonus.DrawOrder = 10;
            mBonus.Visible = false;
            leftHUD.AddWidget(mBonus);
            LayaoutUtil.SetPositionInX(mBonus, 20);
            LayaoutUtil.SetPositionInY(mBonus, 16);

            Texture2D hudScoreBackgroundTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/hud_score_background", false);

            Frame rightHUD = new Frame("rightHUD", hudScoreBackgroundTexture.Bounds.Width, hudScoreBackgroundTexture.Bounds.Height);
            mContainer.AddWidget(rightHUD);
            LayaoutUtil.PlaceInX(rightHUD, 97);
            LayaoutUtil.PlaceInY(rightHUD, yPos);

            mRightHUDBackground = new Image("ScoreBack", hudScoreBackgroundTexture.Bounds.Width, hudScoreBackgroundTexture.Bounds.Height);
            mRightHUDBackground.Texture = hudScoreBackgroundTexture;
            rightHUD.AddWidget(mRightHUDBackground);
            LayaoutUtil.PlaceInX(mRightHUDBackground, 0);
            LayaoutUtil.PlaceInY(mRightHUDBackground, 0);

            mScore = new Label("score", hudScoreBackgroundTexture.Bounds.Width, hudScoreBackgroundTexture.Bounds.Height);
            mScore.Color = color;
            mScore.Font = RadgieGame.Instance.ResourceManager.Load<SpriteFont>(fontId, false);
            mScore.Text = "0000000";
            mScore.TabOrder = 30;
            mScore.DrawOrder = 10;
            rightHUD.AddWidget(mScore);
            LayaoutUtil.SetPositionInX(mScore, 0);
            LayaoutUtil.SetPositionInY(mScore, -7);
        }

        /// <summary>
        /// Crea la pasada de render de la escena 3D.
        /// </summary>
        private void CreateRenderPass()
        {
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            
            PresentationParameters pp = gSystem.Device.PresentationParameters;
            RenderTarget2D rt = null;
            
            lock (gSystem.Device)
            {
                rt = new RenderTarget2D(gSystem.Device, pp.BackBufferWidth, pp.BackBufferHeight, false, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            }
            List<ARenderPass> passes = new List<ARenderPass>();

            passes.Add(mGuiRenderPass);
            
            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            RenderTarget2D rt2 = bloomPass.Target;
            passes.Add(bloomPass);

            bloomPass = Create3DSceneBloomPass(rt, null);

            // Pasada para realizar fadeIn del menu
            Texture2D transparentTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/transparent", false);
            mFadeRenderPass = new TextureRenderPass(transparentTexture, null, false);
            mFadeRenderPass.ExpandToDestinationTarget = true;
            mFadeRenderPass.Color = mFaceStartColor;
            
            mSceneRenderPass = new SceneRenderPass(null, rt, null, true, true);
            
            MultipleRenderPass2 renderPass = new MultipleRenderPass2(new List<ARenderPass>() { mSceneRenderPass, bloomPass });
            passes.Add(renderPass);
            SetInContext(GAME_BACKGROUND_SCENE_PASS, renderPass);
            passes.Add(new TextureRenderPass(rt2, null, false));
            passes.Add(mFadeRenderPass);

            multiplePass = new MultipleRenderPass2(passes, null);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            mBackgroundSong = RadgieGame.Instance.ResourceManager.Load<Song>("GameComponents/GUI/Sounds/Music/02");
            CreateHUD();

            CreateRenderPass();

            mGameOverCounter = new BackCounter(TimeSpan.FromSeconds(2.0d));

            mSpaceshipDestructionController = new Behaviour(new SpaceshipDestructionController());
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            ISoundSystem soundSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            if (mStateMachine.LastEvent == null)
            {
                soundSystem.Play(0.7f, mBackgroundSong);
                soundSystem.Repeat = true;
            }

            LoadGameState();

            InitGame();

            mGameState.PlayerState.GameController.InvertAxes = GetFromContext<bool>("InvertAxes");

            mSceneRenderPass.Scene = mGameState.GameScene;
            mSceneRenderPass.Camera = mGameState.PlayerState.SelectedCamera;
            
            ISceneSystem sSystem = (ISceneSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(mGameState.GameScene);
            mGameState.GameScene.Active = true;

            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gSystem.RenderProcess = multiplePass;
        }

        /// <summary>
        /// Carga el estado de la partida.
        /// </summary>
        private void LoadGameState()
        {
            mGameState = GetFromContext<GameData>("GameData");
        }
        
        /// <summary>
        /// Inicializa el jeugo.
        /// </summary>
        private void InitGame()
        {
            // Fuerza la compactacion de la memoria antes de comenzar el juego
            GC.Collect();

            if (mStateMachine.LastEvent != Pause.GO_GAME)
            {
                mFadeOutCounter = new BackCounter(TimeSpan.FromSeconds(2.0f));
                mFadeOutCounter.Start();
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();
            mGameState.GameScene.Active = false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        /// <param name="time"></param>
        public override void Update(GameTime time)
        {
            base.Update(time);

            Debug();

            UpdateGameState(time);

            UpdateHUD(time);

            if (!mFadeOutCounter.Finished())
            {
                mFadeRenderPass.Color = Color.Lerp(mFaceStartColor, mFaceEndColor, (float)(mFadeOutCounter.TimeElapsed.TotalSeconds / mFadeOutCounter.Duration.TotalSeconds));
            }
        }

        /// <summary>
        /// Actualiza el estado del juego.
        /// </summary>
        /// <param name="time">Tiempo transcurrido.</param>
        private void UpdateGameState(GameTime time)
        {
            if (!mGameState.GameOver)
            {
                // Calcular colisiones nave. Por cada colision, aplicar efecto de colision sobre nave
                mGameState.AsteroidsField.GetCollisions(mGameState.PlayerState.Spaceship, mResults);
                EvaluateCollisions(mResults);

                if (mGameState.PlayerState.Energy <= 0.0f)
                {
                    mGameState.PlayerState.Spaceship.TurnOff();
                }
                else
                {
                    mGameState.PlayerState.Spaceship.TurnOn();
                }

                // Actualizar estado de la partida
                if (mGameState.PlayerState.Life <= 0.0f)
                {
                    // Explosion de la nave
                    mGameState.PlayerState.Spaceship.AddGameObject(mSpaceshipDestructionController);

                    // Marca la partida como finalizada
                    mGameState.GameOver = true;

                    // Comienza la ejecución del counter para retrasar el cambio de estado
                    mGameOverCounter.Start();
                }
                else if ((mFadeOutCounter.Finished()) && (mGameState.PlayerState.GameController.Backward.Pressed))
                {
                    // Pausa
                    SendEvent(GO_PAUSE);
                }
            }
            else
            {
                if (mGameOverCounter.Finished())
                {
                    // Fin de partida
                    SendEvent(GO_GAME_OVER);
                    mGameState.GameOver = false;
                }
            }
        }

        /// <summary>
        /// Actualiza la informacion del HUD.
        /// </summary>
        /// <param name="time">Tiempo transcurrido.</param>
        private void UpdateHUD(GameTime time)
        {
            // Barra de vida
            if (mGameState.PlayerState.Life > 0)
            {
                mLifeBar.Target = (int)mGameState.PlayerState.Life;
            }
            else
            {
                mLifeBar.Value = 0;
            }
            mLifeBar.Update(time);

            // Barra de energia
            mEnergyBar.Target = (int)mGameState.PlayerState.Energy;
            mEnergyBar.Update(time);

            // Bonus seleccionado
            if (mGameState.PlayerState.HasBonus)
            {
                mBonus.Visible = true;
                switch (mGameState.PlayerState.Bonus)
                {
                    case SpaceObject.Type.Bonus_Bomb:
                        mBonus.Texture = mBombTexture;
                        break;
                    case SpaceObject.Type.Bonus_Invincibility:
                        mBonus.Texture = mShieldTexture;
                        break;
                }
            }
            else
            {
                mBonus.Texture = null;
                mBonus.Visible = false;
            }

            // Puntuacion
            mScore.Text = String.Format("{0:0000000}", mGameState.PlayerState.Score);
        }

        /// <summary>
        /// Evalula las colisiones que se produjeron en la ultima iteracion.
        /// </summary>
        /// <param name="collisions">Lista de colisiones.</param>
        private void EvaluateCollisions(List<CollisionRecord> collisions)
        {
            foreach (CollisionRecord record in collisions)
            {
                // Obtiene el gc que no es la nave
                Radgie.Core.IGameComponent gc = record.GameComponentA == mGameState.PlayerState.Spaceship ? record.GameComponentB : record.GameComponentA;

                // Si se trata de un asteroide
                if (gc is SpaceObject)
                {
                    mGameState.PlayerState.Spaceship.CollideWithAsteroid((SpaceObject)gc);
                }
            }
        }

        /// <summary>
        /// Muestra la informacion de debug.
        /// </summary>
        private void Debug()
        {
            if (mGameState.PlayerState.GameController.Debug.Pressed)
            {
                mDebug = !mDebug;

                ISceneSystem sSystem = (ISceneSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
                if (mDebug)
                {
                    DebugSceneUtil.ActivateDebug(mGameState.GameScene);
                }
                else
                {
                    DebugSceneUtil.DeactivateDebug(mGameState.GameScene);
                }
            }
        }
    }
}
