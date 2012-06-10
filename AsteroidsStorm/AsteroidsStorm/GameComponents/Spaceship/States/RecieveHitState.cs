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
    /// Estado de la nave tras recibir un impacto de un asteroide.
    /// </summary>
    class RecieveHitState: ASpaceshipState
    {
        private BackCounter mBackCounter;

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public RecieveHitState(IStateMachine stateMachine)
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
                mBackCounter = new BackCounter(new TimeSpan(0, 0, 3));
            }
            mBackCounter.Start();
            
            ApplyTechnique("HitRecieved");
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
                case SpaceObject.Type.Bonus_Bomb:
                    CheckBombBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Invincibility:
                    CheckInvincibilityBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Life:
                    CheckLifeBonus(spaceObject);
                    break;
                case SpaceObject.Type.Bonus_Energy:
                    CheckEnergyBonus(spaceObject);
                    break;
            }
        }
    }
}
