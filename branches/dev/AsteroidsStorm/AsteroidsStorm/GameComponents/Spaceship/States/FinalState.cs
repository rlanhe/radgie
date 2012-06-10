using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Estado final de la nave.
    /// Representa el fin del juego, cuando la nave es destruida.
    /// </summary>
    class FinalState: ASpaceshipState
    {
        /// <summary>
        /// Inicializa el estado de la nave.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public FinalState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.Spaceship.State.ISpaceshipState.CollideWithSpaceObject"/>
        /// </summary>
        public override void CollideWithSpaceObject(SpaceObject asteroid)
        {
            
        }
    }
}
