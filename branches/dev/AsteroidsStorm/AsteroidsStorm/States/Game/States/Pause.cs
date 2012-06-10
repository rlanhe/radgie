using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Graphics;
using Radgie.Graphics.RenderPass;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using AsteroidsStorm.GameComponents.GUI;
using Microsoft.Xna.Framework;

namespace AsteroidsStorm.States.Game.States
{
    /// <summary>
    /// Estado de pausa.
    /// Para la ejecucion del juego para mostrar un menu con opciones al jugador.
    /// </summary>
    class Pause : AGameState
    {
        /// <summary>
        /// Evento para volver al juego.
        /// </summary>
        public static Event GO_GAME = new Event("Pause_Go_Game");
        /// <summary>
        /// Termina la ejecucion de la maquina de estados.
        /// </summary>
        public static Event FINISH = new Event("Pause_Finish");
        /// <summary>
        /// Evento para mostrar el menu de opciones.
        /// </summary>
        public static Event GO_OPTIONS = new Event("Pause_Go_Options");

        private MultipleRenderPass2 mRenderPasses;
        private ARenderPass mFinalPass;
        private ARenderPass mBloomPass;

        private const string GAME_BACKGROUND_SCENE = "GameBackgroundScene";
        private const string GAME_BACKGROUND_SCENE_PASS = "GameBackgroundScenePass";

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public Pause(IStateMachine stateMachine)
            : base(stateMachine)
        {
            
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            mRenderPasses = new MultipleRenderPass2(new List<ARenderPass>());

            // Monta la escena
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("Pause_Title", 800, 100);
            label.Text = mStrings["Pause"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 10.0f);

            // Opciones del menu
            Frame frame = new Frame("PauseMenu_ops", 1, 400);
            mContainer.AddWidget(frame);
            LayaoutUtil.PlaceInX(frame, 50.0f);
            LayaoutUtil.PlaceInY(frame, 80.0f);

            frame.AddWidget(CreateMenuOption(mStrings["Return"], 10, ReturnFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));
            frame.AddWidget(CreateMenuOption(mStrings["Options"], 20, OptionsFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));
            frame.AddWidget(CreateMenuOption(mStrings["Exit"], 30, ExitFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));

            LayaoutUtil.SpreadWidgetsInY(frame);

            // Selecciona el primer widget
            mContainer.NextWidget();

            mBloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            mFinalPass = new TextureRenderPass(mBloomPass.Target, null, false);
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
            mRenderPasses.Passes.Add(mFinalPass);

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mRenderPasses;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);

            UpdateGUIActions();
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            //mRenderPasses.Passes.Clear();
        }

        /// <summary>
        /// Fuerza la vuelta al juego.
        /// </summary>
        /// <param name="widget">Widget.</param>
        private void ReturnFireDelegate(IWidget widget)
        {
            SendEvent(GO_GAME);
        }

        /// <summary>
        /// Muestra el menu de opciones.
        /// </summary>
        /// <param name="widget">Widget.</param>
        private void OptionsFireDelegate(IWidget widget)
        {
            SendEvent(GO_OPTIONS);
        }

        /// <summary>
        /// Finaliza la ejecucion del juego.
        /// </summary>
        /// <param name="widget">Widget.</param>
        private void ExitFireDelegate(IWidget widget)
        {
            SendEvent(FINISH);
        }
    }
}
