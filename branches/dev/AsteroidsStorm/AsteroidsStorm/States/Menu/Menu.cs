using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.States.Game.States;
using AsteroidsStorm.States.Game;

namespace AsteroidsStorm.States.Menu
{
    /// <summary>
    /// Maquina de estados del menu del juego.
    /// </summary>
    class Menu : AStateMachine<IState>
    {
        /// <summary>
        /// Crea e inicializa la maquina de estados del menu del juego.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public Menu(IStateMachine stateMachine)
            : base(stateMachine)
        {
            InitialState = new MainMenu(this);
            Radgie.Core.Job job = new Radgie.Core.Job(new Radgie.Core.Job.JobDelegate(BuildMachine));
            Radgie.Core.RadgieGame.Instance.JobScheduler.AddJob(job);
#if !XBOX360
            job.Wait();
#endif
        }

        /// <summary>
        /// Inicializa la maquina de estados.
        /// </summary>
        private void BuildMachine()
        {
            IState selectShip = new SelectShip(this);
            IState preGame = new PreGame(this);
            IState game = new GameStateMachine(this);
            IState options = new Options(this);

            AddTransition(InitialState, MainMenu.GO_START_GAME, selectShip);
            AddTransition(InitialState, MainMenu.GO_OPTIONS, options);
            AddTransition(InitialState, MainMenu.GO_EXIT, null);

            AddTransition(options, Options.GO_BACK, InitialState);

            AddTransition(selectShip, SelectShip.GO_MENU, InitialState);
            AddTransition(selectShip, SelectShip.GO_GAME, preGame);

            AddTransition(preGame, PreGame.GO_GAME, game);
            AddTransition(game, GameStateMachine.FINISH, InitialState);   
        }

        /// <summary>
        /// Ver <see cref="Radgie.States.IState.OnFinish"/>
        /// </summary>
        protected override void OnFinish()
        {
            base.OnFinish();
            Radgie.Core.RadgieGame.Instance.Exit();
        }
    }
}
