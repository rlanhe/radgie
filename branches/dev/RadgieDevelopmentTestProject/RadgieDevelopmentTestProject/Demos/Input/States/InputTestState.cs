using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using log4net;
using Microsoft.Xna.Framework;
using RadgieDevelopmentTestProject.Demos.States;

namespace RadgieDevelopmentTestProject.Demos.Input.States
{
    public class InputTestState: ADemoState
    {
        private static readonly ILog mLog = LogManager.GetLogger(typeof(InputTestState));

        private CarController mCarController;
        

        public InputTestState(IStateMachine eventSink, IDemoController dController)
            : base(eventSink, dController)
        {
            mLog.Debug("Crea el control del jugador");
            mCarController = new CarController(PlayerIndex.One);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            mLog.Debug("Update " + this);
            mLog.Debug(mCarController.ToString());
        }
    }
}
