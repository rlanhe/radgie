using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Graphics;
using Radgie.Core;
using Radgie.Graphics.Entity;
using Radgie.Scene.Managers.Simple;
using Radgie.Scene;
using Radgie.Graphics.Camera;
using Radgie.Graphics.RenderPass;
using Microsoft.Xna.Framework;

namespace ParticleEditor.States
{
    class MainState : AState
    {
        private Editor mEditor;
        private IScene mScene;
        private Radgie.Core.IGameComponent mGC;
        private ParticleSystem mPSystem;
        private ICamera3D mCam;
        private float mReservedTime;

        public MainState(IStateMachine stateMachine)
            : base(stateMachine)
        {

        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            mEditor = new Editor(ApplyChanges);
            mEditor.Visible = true;

            mScene = new SimpleScene("MainScene");
            ISceneSystem sSystem = (ISceneSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISceneSystem));
            sSystem.AddScene(mScene);

            mGC = new Radgie.Core.GameComponent("PSystemComponent");
            mScene.AddComponent(mGC);

            mCam = new Camera3D("main");
            mCam.Transformation.Translation = new Vector3(0.0f, 0.0f, 10.0f);
            mScene.AddComponent(mCam);

            mEditor.ParticlesPerSecond = 50;
            mEditor.EmitterVelocity = new Vector3(0.0f, 0.0f, 0.0f);

            ((IGraphicSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).RenderProcess = new SceneRenderPass(mScene, mCam, true, true);

            mEditor.Settings = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<ParticleSystemSettings>("GameComponents/Spaceship/Graphics/Particles/Engine/engine", false);
        }

        private void ApplyChanges(ParticleSystemSettings settings)
        {
            if (mPSystem != null)
            {
                mGC.RemoveGameObject(mPSystem);
            }

            mPSystem = new ParticleSystem(settings);
            mGC.AddGameObject(mPSystem);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (mPSystem != null)
            {
                Vector3 v = mEditor.EmitterVelocity;
                int max = (int)((mReservedTime + time.ElapsedGameTime.TotalSeconds)*mEditor.ParticlesPerSecond);
                if (max == 0.0f)
                {
                    mReservedTime += (float)time.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    for (int i = 0; i < max; i++)
                    {
                        mPSystem.AddParticle(Vector3.Zero, v);
                    }
                    mReservedTime = 0.0f;
                }
            }
        }
    }
}
