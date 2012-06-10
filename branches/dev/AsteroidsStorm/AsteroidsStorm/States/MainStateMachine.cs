using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.States.Game;
using AsteroidsStorm.States.Menu;
using AsteroidsStorm.States.Game.States;

namespace AsteroidsStorm.States
{
    /// <summary>
    /// Maquina de estados principal de la aplicacion.
    /// </summary>
    class MainStateMachine: AStateMachine<IState>
    {
        /// <summary>
        /// Crea e inicializa la maquina principal de la aplicacion.
        /// </summary>
        public MainStateMachine()
            : base(null)
        {
            // Controlador para navegar por los menus
            SetInContext("GuiController", new GUIController());

            IState intro = new Intro(this);
            IState menu = new Menu.Menu(this);

            AddTransition(intro, Intro.GO_NEXT, menu);

            InitialState = intro;
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
