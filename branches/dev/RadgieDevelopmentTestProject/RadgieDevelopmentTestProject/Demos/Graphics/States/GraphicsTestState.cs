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
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Entity;
using Radgie.Graphics.RenderPass;
using Radgie.Graphics.Camera;

namespace RadgieDevelopmentTestProject.Demos.Graphics.States
{
    public class GraphicsTestState : ADemoState
    {
        private static readonly ILog mLog = LogManager.GetLogger(typeof(GraphicsTestState));
        private IGraphicSystem gSystem;
        private Sprite mSprite;
        private Text mText;
        private Mesh testModel;
        private IScene scene;
        private TestGameComponent mGC;
        private ICamera mCamera;

        public GraphicsTestState(IStateMachine eventSink, IDemoController dController)
            : base(eventSink, dController)
        {
            gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));

            scene = new SimpleScene("scene01");

            mSprite = new Sprite(100.0f, 100.0f);
            Material material;// = new Material();
            /*material.RenderState = new RenderState();
            material.Effect = Radgie.Core.RadgieGame.Instance.Content.Load<Effect>(@"Effects/BasicEffect");
            mSprite.Material = material;
            mQuadInstance = (IGraphicInstance)mSprite.CreateInstance();*/
            mGC = new TestGameComponent("");
            //mGC.AddGameObject(mQuadInstance);

            mText = new Text();
            mText.Value = "De prueba";
            mText.Font = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<SpriteFont>(@"en/Fonts/TestFont", false);
            material = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Material>(@"en/Graphic/Materials/default");
            //material.RenderState = Radgie.Core.RadgieGame.Instance.Content.Load<RenderState>(@"Graphic/RenderStates/default");
            //material.EffectId = "Effects/BasicEffect";
            mText.Material = material;
            //mTextInstance = (IGraphicInstance)mText.CreateInstance();
            //mGC.AddGameObject(mTextInstance);
            mGC.AddGameObject(mText);
            mGC.Transformation = new Transformation(new Vector3(500.0f, 300.0f, -500.0f));
            mGC.AddGameObject(mText);
            testModel = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Mesh>(@"en/Graphic/Models/Tank/tank");
            testModel.Configure();
            SimpleModel model = new SimpleModel(testModel);
            //mGC.AddGameObject(model);

            scene.AddComponent(mGC);

            mCamera = new Camera2D("");
            scene.AddComponent(mCamera);

            ISceneSystem sSystem = (ISceneSystem)RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(scene);

            //gSystem.RenderProcess = new RenderGameComponent(mGC);
            gSystem.RenderProcess = new SceneRenderPass(scene, mCamera, true, true);

            SetInContext("pepe", 2);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            int valor = GetFromContext<int>("pepe");
            mLog.Debug("valor de la variable pepe: " + valor);

            mGC.Update(time);
            base.Update(time);
        }

        private class TestGameComponent : Radgie.Core.GameComponent
        {
            public TestGameComponent(string id)
                : base(id)
            {
            }
        }

        
        /*
        private class RenderGameComponent : ARenderPass
        {
            Radgie.Core.IGameComponent mGC;

            public RenderGameComponent(Radgie.Core.IGameComponent gc)
                : base(null)
            {
                mGC = gc;
            }

            public override void RenderAction(IRenderer renderer)
            {
                IEnumerator<IGameObject> gos = mGC.GameObjects;
                gos.Reset();
                while (gos.MoveNext())
                {
                    IGameObject go = gos.Current;
                    if (go is IDraw)
                    {
                        IDraw gi = (IDraw)go;
                        gi.Draw(renderer);
                    }
                }
            }
        }

        private class RenderInstance : ARenderPass
        {
            IGraphicInstance mQuad;

            public RenderInstance(IGraphicInstance quad)
                : base(null)
            {
                mQuad = quad;
            }

            public override void RenderAction(IRenderer renderer)
            {
                mQuad.Draw(renderer);
            }
        }
         * */
    }
}
