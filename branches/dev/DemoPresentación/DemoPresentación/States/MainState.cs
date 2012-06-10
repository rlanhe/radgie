using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using Radgie.Scene.Managers.Simple;
using Radgie.Graphics;
using Radgie.Graphics.Entity;
using Radgie.Graphics.Camera;
using Radgie.Scene;
using Radgie.Graphics.RenderPass;
using Radgie.Input.Action;
using Radgie.Input.Device.Keyboard;
using Radgie.Input.Adapters;

namespace DemoPresentacion.States
{
    class MainState: AState
    {
        public static Event MAIN_STATE_END = new Event("MainStateEnd");
        public static ISceneSystem sceneSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
        public static IGraphicSystem graphicSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));

        private IGameComponent gc;
        private IScene mScene;
        private IRenderable mRenderProcess;
        public AnalogicalAction Steer;

        public MainState(IStateMachine parent)
            : base(parent)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Crea la escena
            mScene = new SimpleScene("DemoScene");

            // Crea la nave y la añade a la escena
            gc = new GameComponent("spaceship");
            gc.AddGameObject(new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("SpaceShip0")));
            mScene.AddComponent(gc);

            // Crea la cámara, la situa y la añade a la escena
            ICamera3D cam3d = new Camera3D("cam3d");
            cam3d.Transformation.Translation += (Microsoft.Xna.Framework.Vector3.Up - Microsoft.Xna.Framework.Vector3.Forward * 3.0f);
            mScene.AddComponent(cam3d);

            // Indica como se debe dibujar
            mRenderProcess = new SceneRenderPass(mScene, cam3d, true, true);

            // Crea la acción para girar la nave
            IKeyboard keyboard = Keyboard.Get(Microsoft.Xna.Framework.PlayerIndex.One);
            Steer = new AnalogicalAction(0.0f, 0.0f);
            Steer.AddBinding(new Digitals2AnalogicalAdapter(keyboard.LeftKey, keyboard.RightKey, -1.0f, 1.0f, 1.0f));
        }

        public override void OnEntry()
        {
            base.OnEntry();

            sceneSystem.AddScene(mScene);
            graphicSystem.RenderProcess = mRenderProcess;
        }

        public override void OnExit()
        {
            base.OnExit();

            sceneSystem.RemoveScene(mScene);
            graphicSystem.RenderProcess = null;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            gc.Transformation.Rotation = Microsoft.Xna.Framework.Quaternion.CreateFromAxisAngle(Microsoft.Xna.Framework.Vector3.Up, Steer.Value);
        }
    }
}
