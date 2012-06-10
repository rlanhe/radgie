using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Radgie.Util;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Maquina de estados de la nave espacial.
    /// </summary>
    class SpaceshipStateMachine: AStateMachine<ISpaceshipState>
    {
        /// <summary>
        /// Inicializa la maquina de estados de la nave espacial.
        /// </summary>
        /// <param name="eventSink">Consumidor de los evento de la maquina de estados.</param>
        public SpaceshipStateMachine(IStateMachine eventSink): base(eventSink)
        {
            ISpaceshipState defaultState = new DefaultState(this);
            ISpaceshipState hitState = new RecieveHitState(this);
            ISpaceshipState invincibilityState = new InvincibilityState(this);
            ISpaceshipState finalState = new FinalState(this);
            ISpaceshipState bombState = new BombState(this);

            AddTransition(defaultState, ASpaceshipState.GO_FINAL_STATE, finalState);
            AddTransition(defaultState, ASpaceshipState.GO_INVINCIBILITY, invincibilityState);
            AddTransition(defaultState, ASpaceshipState.GO_RECIEVE_HIT, hitState);
            AddTransition(defaultState, ASpaceshipState.GO_BOMB, bombState);

            AddTransition(hitState, ASpaceshipState.GO_DEFAULT, defaultState);
            AddTransition(hitState, ASpaceshipState.GO_INVINCIBILITY, invincibilityState);
            AddTransition(hitState, ASpaceshipState.GO_BOMB, bombState);

            AddTransition(invincibilityState, ASpaceshipState.GO_DEFAULT, defaultState);

            AddTransition(bombState, ASpaceshipState.GO_DEFAULT, defaultState);

            InitialState = defaultState;
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.Spaceship.State.ISpaceshipState.CollideWithSpaceObject"/>
        /// </summary>
        public void CollideWithAsteroid(SpaceObject asteroid)
        {
            CurrentState.CollideWithSpaceObject(asteroid);
        }
    }
}

