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

namespace AsteroidsStorm.States.Menu
{
    /// <summary>
    /// Menu de opciones del juego.
    /// </summary>
    class Options: AGameState
    {
        /// <summary>
        /// Evento para volver al menu anterior.
        /// </summary>
        public static Event GO_BACK = new Event("Options_Back");

        private MultipleRenderPass2 mBatchRenderPass;
        //private ARenderPass mFinalPass;

        private Label mInvertAxes;
        private Label mVolume;
        private Label mFXVolume;

        private const string GAME_BACKGROUND_SCENE_PASS = "GameBackgroundScenePass";
        private const string MENU_BACKGROUND_SCENE_RENDER_PASS = "MenuBackgroundSceneRenderPass";

        private const string INVERT_AXES = "InvertAxes";
        private const string VOLUME = "Volume";
        private const string FX_VOLUME = "FXVolume";

        public static bool GetInvertAxes()
        {
            return mStateSystem.Context.Get<bool>(INVERT_AXES);
        }

        public static void SetInvertAxes(bool value)
        {
            mStateSystem.Context.Set<bool>(INVERT_AXES, value);
            
        }

        public static int GetVolume()
        {
            return Convert.ToInt32(sSystem.Volume * 100.0f);
        }

        public static void SetVolume(int value)
        {
            sSystem.Volume = value / 100.0f;
        }

        public static int GetFXVolume()
        {
            return Convert.ToInt32(sSystem.FXVolume * 100.0f);
        }

        public static void SetFXVolume(int value)
        {
            sSystem.FXVolume = value / 100.0f;
        }


        private static ISoundSystem sSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));

        /// <summary>
        /// Crea e inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece./param>
        public Options(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            

            // Monta la escena
            SpriteFont bigFont = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/motorwerk", false);

            Label label = new Label("Options_Title", 800, 100);
            label.Text = mStrings["Options"];
            label.Font = bigFont;
            mContainer.AddWidget(label);
            LayaoutUtil.CenterInX(label);
            LayaoutUtil.PlaceInY(label, 10.0f);

            // Opciones del menu
            Frame frame = new Frame("Options_ops", 1, 500);
            mContainer.AddWidget(frame);
            LayaoutUtil.PlaceInX(frame, 50.0f);
            LayaoutUtil.PlaceInY(frame, 80.0f);

            SetInvertAxes(false);

            mInvertAxes = CreateMenuOption(mStrings["InvertAxes"] + " - " + mStrings["False"], 10, null, SelectWidgetDelegate, DeselectWidgetDelegate);
            frame.AddWidget(mInvertAxes);
            mVolume = CreateMenuOption(mStrings["Volume"] + " - 100", 20, null, SelectWidgetDelegate, DeselectWidgetDelegate);
            frame.AddWidget(mVolume);
            mFXVolume = CreateMenuOption(mStrings["FXVolume"] + " - 100", 30, null, SelectWidgetDelegate, DeselectWidgetDelegate);
            frame.AddWidget(mFXVolume);
            frame.AddWidget(CreateMenuOption(mStrings["Return"], 40, ReturnFireDelegate, SelectWidgetDelegate, DeselectWidgetDelegate));

            LayaoutUtil.SpreadWidgetsInY(frame);

            // Selecciona el primer widget
            mContainer.NextWidget();

            // RenderPass
            List<ARenderPass> passes = new List<ARenderPass>();
            mBatchRenderPass = new MultipleRenderPass2(passes);

            mBatchRenderPass.Passes.Clear();
            mBatchRenderPass.Passes.Add(mGuiRenderPass);

            BloomPass bloomPass = CreateHUDBloomPass(mGuiSceneTarget);
            mBatchRenderPass.Passes.Add(bloomPass);

            ARenderPass renderPass = GetFromContext<ARenderPass>(GAME_BACKGROUND_SCENE_PASS);
            if (renderPass == null)
            {
                renderPass = GetFromContext<ARenderPass>(MENU_BACKGROUND_SCENE_RENDER_PASS);
            }
            mBatchRenderPass.Passes.Add(renderPass);
            mBatchRenderPass.Passes.Add(new TextureRenderPass(bloomPass.Target, null, false));
        }

        public override void OnEntry()
        {
            base.OnEntry();

            // Especifica como dibujar la escena
            IGraphicSystem gs = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gs.RenderProcess = mBatchRenderPass;

            // Actualiza las opciones de configuración con los valores actuales
            mInvertAxes.Text = mStrings["InvertAxes"] + " - " + mStrings[GetInvertAxes().ToString()] + "  ";
            mVolume.Text = mStrings["Volume"] + " - " + GetVolume() + "  ";
            mFXVolume.Text = mStrings["FXVolume"] + " - " + GetFXVolume() + "  ";
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            UpdateGUIActions();

            // Reacciona ante las acciones del usuario
            IGUIController controller = Controller;

            if ((mContainer.CurrentWidget() == mVolume) || (mContainer.CurrentWidget() == mFXVolume))
            {
                UpdateVolumeControl(controller, (Label)mContainer.CurrentWidget());
            }
            else if (mContainer.CurrentWidget() == mInvertAxes)
            {
                if ((controller.Left.Pressed) || (controller.Right.Pressed))
                {
                    bool value = GetInvertAxes();
                    value = !value;
                    mInvertAxes.Text = mStrings["InvertAxes"] + " - " + mStrings[value.ToString()];
                    SetInvertAxes(value);
                }
            }
        }

        private void UpdateVolumeControl(IGUIController controller, Label volumeControl)
        {
            if ((controller.Left.Pressed) || (controller.Right.Pressed))
            {
                string label = volumeControl == mVolume ? mStrings["Volume"] + " - " : mStrings["FXVolume"] + " - ";
                int volume = volumeControl == mVolume ? GetVolume() : GetFXVolume();
                if (controller.Left.Pressed)
                {
                    volume--;
                    if (volume < 0)
                    {
                        volume = 0;
                    }
                }
                else if (controller.Right.Pressed)
                {
                    volume++;
                    if (volume > 100)
                    {
                        volume = 100;
                    }
                }

                if (volumeControl == mVolume)
                {
                    SetVolume(volume);
                }
                else
                {
                    SetFXVolume(volume);
                }

                volumeControl.Text = label + volume;
                mChangeValueSound.Play();
            }
        }

        /// <summary>
        /// Sale del menu de opciones.
        /// </summary>
        /// <param name="widget">Widget.</param>
        private void ReturnFireDelegate(IWidget widget)
        {
            SendEvent(GO_BACK);
            mBackActionSound.Play();
        }
    }
}
