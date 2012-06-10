using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Graphics;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Radgie.Scene.Managers.Simple;
using Radgie.Scene;
using Radgie.Graphics.Entity;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.RenderPass;
using Radgie.Graphics.Camera;
using Radgie.Sound;

namespace Tutorial
{
    public class MainState: AState
    {
        public static Event EXIT_MAIN = new Event("Exit_Main");
        private SimpleScene mScene;

        public MainState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        public override void OnEntry()
        {
            base.OnEntry();

            mScene = new SimpleScene("tutorialScene");

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(mScene);

            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("tutorialGC");

            SimpleModel model = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("Spaceship0"));
            gc.AddGameObject(model);
            gc.Transformation.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)(Math.PI / 8.0d));

            SoundEffect sEffect = new SoundEffect("collision");
            sEffect.IsLooped = true;
            gc.AddGameObject(sEffect);
            sEffect.Play();

            mScene.AddComponent(gc);

            ICamera3D camera =  new Camera3D("camera");
            camera.Transformation.Translation = new Vector3(0.0f, 0.0f, 3.0f);
            camera.Target = gc.Transformation.Translation;
            mScene.AddComponent(camera);

            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gSystem.RenderProcess = new SceneRenderPass(mScene, camera, true, true);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            TutorialController controller = GetFromContext<TutorialController>("controller");
            if (controller.OKAction.Pressed)
            {
                SendEvent(EXIT_MAIN);
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.RemoveScene(mScene);
            mScene = null;

            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            gSystem.RenderProcess = null;
        }
    }
}
