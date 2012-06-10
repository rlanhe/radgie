using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Graphics.RenderPass;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using AsteroidsStorm.GameComponents.GUI;
using Microsoft.Xna.Framework;
using Radgie.Scene;
using Radgie.Util;
using AsteroidsStorm.RenderPasses;
using Radgie.Sound;

namespace AsteroidsStorm.States.Game.States
{
    /// <summary>
    /// Estado para terminar la ejecucion del juego.
    /// </summary>
    class End:AGameState
    {
        public static Event FINISH = new Event("End_Finish");

        private MultipleRenderPass2 mRenderPasses;
        private ARenderPass mFinalPass;
        private Label mScoreLabel;
        private BloomPass mBloomPass;
        private TextureRenderPass mFadeRenderPass;

        private const string GAME_BACKGROUND_SCENE = "GameBackgroundScene";
        private const string GAME_BACKGROUND_SCENE_PASS = "GameBackgroundScenePass";
        private BackCounter mKeysTimer;

        private static Color mFaceEndColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        private static Color mFaceStartColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public End(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            mRenderPasses = new MultipleRenderPass2(new List<ARenderPass>());

            // Monta la escena
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("End_Title", 800, 100);
            label.Text = mStrings["GameOver"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 10.0f);

            // Opciones del menu
            Frame frame = new Frame("End_frame", 600, 600);
            mContainer.AddWidget(frame);
            LayaoutUtil.PlaceInX(frame, 50.0f);
            LayaoutUtil.PlaceInY(frame, 80.0f);

            SpriteFont smallFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk_very_small", false);
            Label scoreLabel = new Label("Score_Label", 600, 400);
            scoreLabel.Font = smallFont;
            scoreLabel.Text = "You've scored\n 000000 points\nPress any key\n to continue";
            frame.AddWidget(scoreLabel);
            LayaoutUtil.Center(scoreLabel);
            mScoreLabel = scoreLabel;

            mBloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            mFinalPass = new TextureRenderPass(mBloomPass.Target, null, false);
            mKeysTimer = new BackCounter(TimeSpan.FromSeconds(1.0f));

            // Pasada para realizar fadeIn del menu
            Texture2D transparentTexture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/transparent", false);
            mFadeRenderPass = new TextureRenderPass(transparentTexture, null, false);
            mFadeRenderPass.ExpandToDestinationTarget = true;
            mFadeRenderPass.Color = mFaceStartColor;
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            mRenderPasses.Passes.Clear();
            mRenderPasses.Passes.Add(mGuiRenderPass);
            mRenderPasses.Passes.Add(mBloomPass);        
            mRenderPasses.Passes.Add(GetFromContext<ARenderPass>(GAME_BACKGROUND_SCENE_PASS));
            mRenderPasses.Passes.Add(mFadeRenderPass);
            mRenderPasses.Passes.Add(mFinalPass);
            

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mRenderPasses;

            mScoreLabel.Text = mStrings["Message_1"] + "\n" + this.GetScoreString(GetFromContext<GameData>("GameData").PlayerState.Score.ToString()) + mStrings["Message_2"] + "\n" + mStrings["Message_3"] + "\n" + mStrings["Message_4"];

            mKeysTimer.Start();
        }

        private string GetScoreString(string value)
        {
            string tmp = value;
            if (value.Length < 6)
            {
                int times = 6-value.Length;
                for (int i = 0; i < times; i++)
                {
                    tmp = "0" + tmp;
                }
            }
            return tmp;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (mKeysTimer.Finished())
            {
                GameController controller = GetFromContext<GameData>("GameData").PlayerState.GameController;
                if (controller.ActionFired())
                {
                    SendEvent(FINISH);
                }
            }
            else
            {
                mFadeRenderPass.Color = Color.Lerp(mFaceStartColor, mFaceEndColor, (float)(mKeysTimer.TimeElapsed.TotalSeconds / mKeysTimer.Duration.TotalSeconds));
            }

            UpdateGUIActions();
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            SetInContext("GameData", null);

            //ISoundSystem soundSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            //soundSystem.Stop(0.7f);

            // Quita la escena del sistema de escenas.
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            IScene scene = GetFromContext<GameData>("GameData").GameScene;
            sSystem.RemoveScene(scene);

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = null;
        }
    }
}
