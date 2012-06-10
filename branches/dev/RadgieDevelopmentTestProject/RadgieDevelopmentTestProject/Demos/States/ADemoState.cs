using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using log4net;

namespace RadgieDevelopmentTestProject.Demos.States
{
    public abstract class ADemoState: AState
    {
        private static readonly ILog mLog = LogManager.GetLogger(typeof(ADemoState));

        public static readonly Event GO_NEXT = new Event("GO_NEXT_InputTestState");
        public static readonly Event GO_PREVIOUS = new Event("GO_PREVIOUS_InputTestState");
        public static readonly Event EXIT = new Event("EXIT_InputTestState");

        private IDemoController mDemoController;

        public ADemoState(IStateMachine eventSink, IDemoController dController)
            : base(eventSink)
        {
            mLog.Debug("Crea el control del jugador");
            mDemoController = dController;
        }

        public override void OnEntry()
        {
        }

        public override void OnExit()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            mLog.Debug("Update " + this);

            if (mDemoController.ExitDemo.Pressed)
            {
                mLog.Debug("Fin demo");
                SendEvent(EXIT);
            }
            else if (mDemoController.NextDemo.Pressed)
            {
                mLog.Debug("Siguiente demo");
                SendEvent(GO_NEXT);
            }
            else if (mDemoController.PreviousDemo.Pressed)
            {
                mLog.Debug("Demo anterior");
                SendEvent(GO_PREVIOUS);
            }
        }
    }
}
