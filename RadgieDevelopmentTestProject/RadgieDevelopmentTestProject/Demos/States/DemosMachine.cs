using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using RadgieDevelopmentTestProject.Demos.Input.States;
using RadgieDevelopmentTestProject.Demos.Scene.States;
using RadgieDevelopmentTestProject.Demos.Graphics.States;
using RadgieDevelopmentTestProject.Demos.Sound.States;

namespace RadgieDevelopmentTestProject.Demos.States
{
    public class DemosMachine : AStateMachine<IState>
    {
        IDemoController dController;

        ADemoState input;
		ADemoState scene;
        ADemoState graphics;
        ADemoState sound;

        public DemosMachine(): base(null)
        {
            dController = new DemoController();

            graphics = new GraphicsTestState(this, dController);
			scene = new SceneTestState(this, dController);
            input = new InputTestState(this, dController);
            sound = new SoundTestState(this, dController);

            AddTransition(graphics, ADemoState.GO_NEXT, input);
            AddTransition(graphics, ADemoState.GO_PREVIOUS, input);
            AddTransition(graphics, ADemoState.EXIT, null);// Termina la ejecucion de la maquina

			AddTransition(scene, ADemoState.GO_NEXT, input);
			AddTransition(scene, ADemoState.GO_PREVIOUS, input);
			AddTransition(scene, ADemoState.EXIT, null);// Termina la ejecucion de la maquina

            AddTransition(input, ADemoState.GO_NEXT, sound);
            AddTransition(input, ADemoState.GO_PREVIOUS, sound);
            AddTransition(input, ADemoState.EXIT, null);// Termina la ejecucion de la maquina

            AddTransition(sound, ADemoState.GO_NEXT, scene);
            AddTransition(sound, ADemoState.GO_PREVIOUS, scene);
            AddTransition(sound, ADemoState.EXIT, null);// Termina la ejecucion de la maquina

            InitialState = graphics;
        }

        protected override void OnFinish()
        {
            Radgie.Core.RadgieGame.Instance.Exit();
        }
    }
}
