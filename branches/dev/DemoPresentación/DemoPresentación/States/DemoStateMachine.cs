using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;

namespace DemoPresentacion.States
{
    class DemoStateMachine: AStateMachine<IState>
    {
        public DemoStateMachine()
            : base(null)
        {
            IState mainState = new MainState(this);
            this.InitialState = mainState;
            this.AddTransition(mainState, MainState.MAIN_STATE_END, null);
        }
    }
}
