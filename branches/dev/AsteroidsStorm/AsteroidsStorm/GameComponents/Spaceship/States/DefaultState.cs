using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Estado por defecto de la nave espacial.
    /// </summary>
    class DefaultState: ASpaceshipState
    {
        /// <summary>
        /// Inicializa el estado de la nave.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece el estado.</param>
        public DefaultState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            ApplyTechnique("Default");
        }
        
        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            UseBombBonus();
            UseInvincibilityBonus();
            CheckLife();
        }
    }
}
