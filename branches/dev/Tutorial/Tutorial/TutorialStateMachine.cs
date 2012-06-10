using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;

namespace Tutorial
{
    public class TutorialStateMachine: AStateMachine<IState>
    {
        public TutorialStateMachine()
            : base(null)
        {
            SetInContext("controller", new TutorialController());

            IState intro = new IntroState(this);
            IState main = new MainState(this);

            AddTransition(intro, IntroState.EXIT_INTRO, main);
            AddTransition(main, MainState.EXIT_MAIN, null);

            InitialState = intro;
        }

        protected override void OnFinish()
        {
            base.OnFinish();
            Radgie.Core.RadgieGame.Instance.Exit();
        }
    }
}
