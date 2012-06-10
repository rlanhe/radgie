using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using log4net;
using Microsoft.Xna.Framework;
using RadgieDevelopmentTestProject.Demos.States;
using Radgie.Scene;
using Radgie.Scene.Managers.Simple;
using Radgie.Core;

namespace RadgieDevelopmentTestProject.Demos.Scene.States
{
    public class SceneTestState: ADemoState
    {
        private static readonly ILog mLog = LogManager.GetLogger(typeof(SceneTestState));

        private StateController Controller;
        private SimpleScene scene1, scene2, scene3_1, scene3_2, scene4_1, scene4_2, scene5_1, scene5_2;
        private TestGameComponent gc1_1, gc1_2, gc2_1, gc2_2, gc3_1, gc3_2, gc4_1, gc4_2, gc5_1, gc5_2;
        private bool changed = false;

		public SceneTestState(IStateMachine eventSink, IDemoController dController)
            : base(eventSink, dController)
        {
            mLog.Debug("Crea una escena");
            Controller = new StateController(PlayerIndex.One);

            ISceneSystem sSystem = (ISceneSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISceneSystem));

            scene1 = new SimpleScene("scene1");
            scene2 = new SimpleScene("scene2");
            scene3_1 = new SimpleScene("scene3_1");
            scene3_2 = new SimpleScene("scene3_2");
            scene4_1 = new SimpleScene("scene4_1");
            scene4_2 = new SimpleScene("scene4_2");
            scene5_1 = new SimpleScene("scene5_1");
            scene5_2 = new SimpleScene("scene5_2");

            sSystem.AddScene(scene1);
            sSystem.AddScene(scene2);
            sSystem.AddScene(scene3_1);
            sSystem.AddScene(scene3_2);
            sSystem.AddScene(scene4_1);
            sSystem.AddScene(scene4_2);
            sSystem.AddScene(scene5_1);
            sSystem.AddScene(scene5_2);

            gc1_1 = new TestGameComponent("");
            gc1_2 = new TestGameComponent("");
            gc2_1 = new TestGameComponent("");
            gc2_2 = new TestGameComponent("");
            gc3_1 = new TestGameComponent("");
            gc3_2 = new TestGameComponent("");
            gc4_1 = new TestGameComponent("");
            gc4_2 = new TestGameComponent("");
            gc5_1 = new TestGameComponent("");
            gc5_2 = new TestGameComponent("");

            gc1_1.AddGameComponent(gc1_2);
            gc3_1.AddGameComponent(gc3_2);

            scene1.AddComponent(gc1_1);
            scene2.AddComponent(gc2_1);
            scene2.AddComponent(gc2_2);
            scene3_1.AddComponent(gc3_1);
            scene4_1.AddComponent(gc4_1);
            scene4_2.AddComponent(gc4_2);
            scene5_1.AddComponent(gc5_1);
            scene5_2.AddComponent(gc5_2);

            mLog.Debug("Configuracion escenas inicial");
            mLog.Debug(ScenesToString());
        }

        private class TestGameComponent : Radgie.Core.GameComponent
        {
            public TestGameComponent(string id)
                : base(id)
            {
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (Controller.Action.Pressed)
            {
                if (!changed)
                {
                    scene1.AddComponent(gc1_2);
                    gc2_1.AddGameComponent(gc2_2);
                    scene3_2.AddComponent(gc3_2);
                    scene4_1.AddComponent(gc4_2);
                    gc5_1.AddGameComponent(gc5_2);
                    scene4_1.AddComponent(gc5_1);

                    mLog.Debug("Configuracion escenas final");
                    mLog.Debug(ScenesToString());

                    changed = true;
                }
            }
        }

        private string ScenesToString()
        {
            string result = "";

            result += scene1.ToString();
            result += scene2.ToString();
            result += scene3_1.ToString();
            result += scene3_2.ToString();
            result += scene4_1.ToString();
            result += scene4_2.ToString();
            result += scene5_1.ToString();
            result += scene5_2.ToString();

            return result;
        }
    }
}
