using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using AsteroidsStorm.GameComponents.GUI;
using Radgie.Scene.Managers.Simple;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Scene;
using Radgie.Graphics.RenderPass;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework;
using Radgie.Util;
using AsteroidsStorm.RenderPasses;

namespace AsteroidsStorm.States.Menu
{
    /// <summary>
    /// Estado inicial de la aplicacion.
    /// </summary>
    class Intro: AGameState
    {
        /// <summary>
        /// Evento para ir al siguiente estado de la aplicacion.
        /// </summary>
        public static Event GO_NEXT = new Event("Intro_Go_Next");
        
        private BackCounter mFadeInCounter;
        private BackCounter mFadeOutCounter;
        private static Color mFaceStartColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        private static Color mFaceEndColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        private TextureRenderPass mFadePass;

        private float mFadeInDuration = 2.0f;
        private float mFadeOutDuration = 1.2f;

        private IRenderable mRenderPass;
        
        /// <summary>
        /// Crea e inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public Intro(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            mFadeInCounter = new BackCounter(TimeSpan.FromSeconds(mFadeInDuration));
            mFadeOutCounter = new BackCounter(TimeSpan.FromSeconds(mFadeOutDuration));

            // Monta la escena
            Texture2D texture = RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/title", false);
            Image img = new Image("logo", texture.Bounds.Width, texture.Bounds.Height);
            img.Texture = texture;

            mContainer.AddWidget(img);
            LayaoutUtil.Center(img);
            
            // Fade pass
            mFadePass = new TextureRenderPass(RadgieGame.Instance.ResourceManager.Load<Texture2D>("GameComponents/GUI/Graphics/Textures/transparent", false), null, false);
            mFadePass.ExpandToDestinationTarget = true;

            // Especifica como dibujar la escena
            Camera2D cam2d = new Camera2D("intro_cam");
            cam2d.BackgroundColor = Color.Black;
            List<ARenderPass> mRenderPasses = new List<ARenderPass>();
            mRenderPasses.Add(mGuiRenderPass);
            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            mRenderPasses.Add(bloomPass);
            mRenderPasses.Add(new TextureRenderPass(bloomPass.Target, cam2d, true));
            mRenderPasses.Add(mFadePass);
            mRenderPass = new MultipleRenderPass2(mRenderPasses);
        }

        public override void OnEntry()
        {
            base.OnEntry();

            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mRenderPass;

            mFadeInCounter.Start();
        }
        
        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (!mFadeInCounter.Finished())
            {
                mFadePass.Color = Color.Lerp(mFaceStartColor, mFaceEndColor, (float)(mFadeInCounter.TimeElapsed.TotalSeconds / mFadeInCounter.Duration.TotalSeconds));
            }
            else
            {
                // Desencadena el fade out
                if (mFadeOutCounter.State == Timer.TimerState.STOPPED)
                {
                    mFadeOutCounter.Start();
                }

                if (!mFadeOutCounter.Finished())
                {
                    mFadePass.Color = Color.Lerp(mFaceEndColor, mFaceStartColor, (float)(mFadeOutCounter.TimeElapsed.TotalSeconds / mFadeOutCounter.Duration.TotalSeconds));
                }
            }

            if (mFadeOutCounter.Finished())
            {
                SendEvent(GO_NEXT);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            // Como no va a volver a entrar en este estado, libero todas las referencias que tiene
            // Quita la escena del sistema de escenas
            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(mGuiScene);

            mGuiScene = null;
            mContainer = null;
        }
    }
}
