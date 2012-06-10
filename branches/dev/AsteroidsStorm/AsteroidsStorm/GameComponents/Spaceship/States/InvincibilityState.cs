using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Util;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Estado de la nave durante el uso de un bonificador de invencibilidad.
    /// </summary>
    class InvincibilityState: ASpaceshipState
    {
        private BackCounter mBackCounter;

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public InvincibilityState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            if (mBackCounter == null)
            {
                mBackCounter = new BackCounter(new TimeSpan(0, 0, 7));
            }
            mBackCounter.Start();

            ApplyTechnique("Invincibility");
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        /// <param name="time"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            CheckLife();

            if (mBackCounter.Finished())
            {
                SendEvent(GO_DEFAULT);
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.Spaceship.State.ISpaceshipState.CollideWithSpaceObject"/>
        /// </summary>
        public override void CollideWithSpaceObject(SpaceObject spaceObject)
        {
            switch (spaceObject.ObjectType)
            {
                case SpaceObject.Type.Apophis_Asteroid:
                case SpaceObject.Type.Asteroid_Regular:
                    CheckCollisions(spaceObject, false);
                    break;
                case SpaceObject.Type.Bonus_Bomb:
                    CheckBombBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Energy:
                    CheckEnergyBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Invincibility:
                    CheckInvincibilityBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Life:
                    CheckLifeBonus(spaceObject);
                    break;
            }
        }
    }
}
