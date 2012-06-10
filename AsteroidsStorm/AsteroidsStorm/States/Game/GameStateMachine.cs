using Radgie.State;
using AsteroidsStorm.States.Menu;

namespace AsteroidsStorm.States.Game
{
    /// <summary>
    /// Maquina de estados de la aplicacion.
    /// </summary>
    class GameStateMachine: AStateMachine<AState>
    {
        /// <summary>
        /// Evento para finalizar la ejecucion de la maquina de estados.
        /// </summary>
        public static Event FINISH = new Event("Game_Finish");

        /// <summary>
        /// Crea e inicializa la maquina de estados.
        /// </summary>
        /// <param name="eventSink">Consumidor de los eventos de esta maquina de estados.</param>
        public GameStateMachine(IStateMachine eventSink)
            : base(eventSink)
        {
            Radgie.Core.Job job = new Radgie.Core.Job(new Radgie.Core.Job.JobDelegate(BuildMachine));
            Radgie.Core.RadgieGame.Instance.JobScheduler.AddJob(job);
#if !XBOX360
            job.Wait();
#endif
        }

        /// <summary>
        /// Inicializa la maquina de estados en un hilo de ejecucion separado.
        /// </summary>
        private void BuildMachine()
        {
            States.Game gameState = new States.Game(this);
            InitialState = gameState;

            States.End endState = new States.End(this);
            States.Pause pauseState = new States.Pause(this);
            Options optionsState = new Options(this);

            AddTransition(gameState, States.Game.GO_PAUSE, pauseState);
            AddTransition(gameState, States.Game.GO_GAME_OVER, endState);

            AddTransition(pauseState, States.Pause.GO_GAME, gameState);
            AddTransition(pauseState, States.Pause.GO_OPTIONS, optionsState);
            AddTransition(pauseState, States.Pause.FINISH, endState);

            AddTransition(optionsState, Options.GO_BACK, pauseState);

            AddTransition(endState, States.End.FINISH, null);
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            SetInContext("SelectedShip", GetFromContext<string>("SelectedShip"));

            base.OnEntry();
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnFinish"/>
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();

            SendEvent(FINISH);
        }
    }
}

