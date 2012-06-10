using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;

namespace ParticleEditor.States
{
    class MainStateMachine: AStateMachine<AState>
    {
        public MainStateMachine():base(null)
        {
            InitialState = new MainState(this);
        }
    }
}
