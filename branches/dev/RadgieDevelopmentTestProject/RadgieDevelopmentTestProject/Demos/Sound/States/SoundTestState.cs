using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadgieDevelopmentTestProject.Demos.States;
using Radgie.State;
using Radgie.Core;
using Radgie.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace RadgieDevelopmentTestProject.Demos.Sound.States
{
    public class SoundTestState: ADemoState
    {
        private TestGameComponent mGC;
        private Matrix mListenerWorldTransform = Matrix.Identity;
        private ISoundSystem mSoundSystem;

        public SoundTestState(IStateMachine eventSink, IDemoController dController)
            : base(eventSink, dController)
        {
            mSoundSystem = (ISoundSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            mGC = new TestGameComponent("");
            SoundEffect sEffect = new SoundEffect("en/Sound/Bip");
            mGC.AddGameObject(sEffect);
            sEffect.Is3D = true;
            //sEffect.IsLooped = true;
            mSoundSystem.UpdateListener(mListenerWorldTransform);
            //sEffect.Play();

            Song song = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Song>("en/Sound/Songs/Ambient", false);
            //mSoundSystem.Play(song);
        }

        private class TestGameComponent : Radgie.Core.GameComponent
        {
            public TestGameComponent(string id)
                : base(id)
            {
            }
        }
    }
}
