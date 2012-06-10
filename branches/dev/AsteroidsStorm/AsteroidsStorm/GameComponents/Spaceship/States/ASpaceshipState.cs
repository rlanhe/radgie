using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Core;
using Radgie.Sound;
using Radgie.Graphics.Entity;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsStorm.States.Game;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Estado base de la nave espacial.
    /// </summary>
    abstract class ASpaceshipState: AState, ISpaceshipState
    {
        /// <summary>
        /// Evento para ir al estado final de la maquina de estados de la nave.
        /// </summary>
        public static Event GO_FINAL_STATE = new Event("To_FinalState");
        /// <summary>
        /// Evento para ir al estado en el que la nave acaba de recibir un impacto.
        /// </summary>
        public static Event GO_RECIEVE_HIT = new Event("To_RevieveHit");
        /// <summary>
        /// Evento para ir al estado de invencibilidad.
        /// </summary>
        public static Event GO_INVINCIBILITY = new Event("To_Invincibility");
        /// <summary>
        /// Evento para ir al estado en el que se lanza una bomba de energia.
        /// </summary>
        public static Event GO_BOMB = new Event("To_Bomb");
        /// <summary>
        /// Evento para volver al estado por defecto de la nave.
        /// </summary>
        public static Event GO_DEFAULT = new Event("To_Default");

        /// <summary>
        /// Controlador para manejar la nave.
        /// </summary>
        protected ISpaceshipController Controller
        {
            get
            {
                return GameData.PlayerState.GameController;
            }
        }

        /// <summary>
        /// Datos de la partida.
        /// </summary>
        protected GameData GameData
        {
            get
            {
                return GetFromContext<GameData>("GameData");
            }
        }

        /// <summary>
        /// Indica si tiene un bonus que pueda lanzar.
        /// </summary>
        protected bool HasBonus
        {
            get
            {
                return GameData.PlayerState.HasBonus;
            }
            set
            {
                GameData.PlayerState.HasBonus = value;
            }
        }

        /// <summary>
        /// Valor de la energia que le queda a la nave.
        /// Valor entre 0 y 100.
        /// </summary>
        protected float Energy
        {
            get
            {
                return GameData.PlayerState.Energy;
            }
            set
            {
                GameData.PlayerState.Energy = value;
            }
        }

        /// <summary>
        /// Vida que le queda a la nave.
        /// Valor entre 0 y 100.
        /// </summary>
        protected float Life
        {
            get
            {
                return GameData.PlayerState.Life;
            }
            set
            {
                GameData.PlayerState.Life = value;
            }
        }

        /// <summary>
        /// Resistencia de la nave a los impactos.
        /// Valor entre 0 y 100.
        /// </summary>
        protected float Armor
        {
            get
            {
                return GameData.PlayerState.Spaceship.Armor;
            }
        }

        /// <summary>
        /// Tipo del bonificador que tiene la nave.
        /// </summary>
        protected SpaceObject.Type Bonus
        {
            get
            {
                return GameData.PlayerState.Bonus;
            }
            set
            {
                GameData.PlayerState.Bonus = value;
            }
        }

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece este estado.</param>
        public ASpaceshipState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.Spaceship.States.ISpaceshipState.CollideWithSpaceObject"/>
        /// </summary>
        public virtual void CollideWithSpaceObject(SpaceObject spaceObject)
        {
            switch (spaceObject.ObjectType)
            {
                case SpaceObject.Type.Apophis_Asteroid:
                case SpaceObject.Type.Asteroid_Regular:
                    CheckCollisions(spaceObject, true);
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

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);
            
            Energy -= (float)time.ElapsedGameTime.TotalSeconds*2;
            if (Energy < 0.0f)
            {
                Energy = 0.0f;
            }
            
            ISoundSystem sSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            sSystem.UpdateListener(mStateMachine.Owner.Component.World);
        }

        /// <summary>
        /// Comprueba si se trata de un bonificador de energia, en cuyo caso aplica los cambios a su estado que considere necesarios.
        /// </summary>
        /// <param name="spaceObject">SpaceObject con el que colisiono.</param>
        protected void CheckEnergyBonus(SpaceObject spaceObject)
        {
            if (spaceObject.ObjectType == SpaceObject.Type.Bonus_Energy)
            {
                float energy = Energy;
                if (energy < 100.0f)
                {
                    energy += 5.0f;
                }
                if (energy > 100.0f)
                {
                    energy = 100.0f;
                }
                Energy = energy;
                spaceObject.Destroy();
            }
        }

        /// <summary>
        /// Comprueba si se trata de un bonificador de vida, en cyo caso aplica los cambios a su estado que considere necesarios.
        /// </summary>
        /// <param name="spaceObject">SpaceObject con el que colisiono.</param>
        protected void CheckLifeBonus(SpaceObject spaceObject)
        {
            if (spaceObject.ObjectType == SpaceObject.Type.Bonus_Life)
            {
                float life = Life;
                if (life < 100.0f)
                {
                    life += 5.0f;
                }
                if (life > 100.0f)
                {
                    life = 100.0f;
                }
                Life = life;
                spaceObject.Destroy();
            }
        }

        /// <summary>
        /// Comprueba si se trata de un bonificador de tipo bomba, en cuyo caso aplica los cambios necesarios a su estado.
        /// </summary>
        /// <param name="spaceObject">SpaceObject con el que colisiono.</param>
        protected void CheckBombBonus(SpaceObject spaceObject)
        {
            if (spaceObject.ObjectType == SpaceObject.Type.Bonus_Bomb)
            {
                HasBonus = true;
                Bonus = spaceObject.ObjectType;
                spaceObject.Destroy();
            }
        }

        /// <summary>
        /// Comprueba si se trata de un bonificador de tipo invencibilidad, en cuyo caso aplica los cambios necesarios.
        /// </summary>
        /// <param name="asteroid">SpaceObject con el que colisiono.</param>
        protected void CheckInvincibilityBonus(SpaceObject asteroid)
        {
            if (asteroid.ObjectType == SpaceObject.Type.Bonus_Invincibility)
            {
                HasBonus = true;
                Bonus = asteroid.ObjectType;
                asteroid.Destroy();
            }
        }

        /// <summary>
        /// Cambia el estado de la nave en funcion del objeto con el que haya colisionado.
        /// </summary>
        /// <param name="spaceObject">SpaceObject con el que colisiono.</param>
        /// <param name="applyDamage">Si el valor es True, aplica dannos a la nave, en caso de ser False no.</param>
        protected void CheckCollisions(SpaceObject spaceObject, bool applyDamage)
        {
            bool goToRecieveHit = Energy != 0.0f;
            float damage = 0.0f;
            if (spaceObject.ObjectType == SpaceObject.Type.Asteroid_Regular)
            {
                if (applyDamage)
                {
                    damage = 10.0f;
                    if (goToRecieveHit)
                    {
                        SendEvent(GO_RECIEVE_HIT);
                    }
                }
                spaceObject.Destroy();
            }
            else if (spaceObject.ObjectType == SpaceObject.Type.Apophis_Asteroid)
            {
                if (applyDamage)
                {
                    damage = 30.0f;
                    if (goToRecieveHit)
                    {
                        SendEvent(GO_RECIEVE_HIT);
                    }
                }
                spaceObject.Destroy();
            }
    
            Life = Life - (1.0f/(Armor/100.0f)) * damage;

            if (Life <= 0.0f)
            {
                SendEvent(GO_FINAL_STATE);
            }
        }

        /// <summary>
        /// Lanza una bomba de energia.
        /// </summary>
        protected void UseBombBonus()
        {
            if ((HasBonus) && (Bonus == SpaceObject.Type.Bonus_Bomb) && (Controller.UseBonus.Pressed))
            {
                SendEvent(GO_BOMB);
                HasBonus = false;
            }
        }

        /// <summary>
        /// Usa el bonificador de la invencibilidad.
        /// </summary>
        protected void UseInvincibilityBonus()
        {
            if ((HasBonus) && (Bonus == SpaceObject.Type.Bonus_Invincibility) && (Controller.UseBonus.Pressed))
            {
                SendEvent(GO_INVINCIBILITY);
                HasBonus = false;
            }
        }

        /// <summary>
        /// Comprueba que el estado de salud de la nave.
        /// </summary>
        protected void CheckLife()
        {
            if (Life <= 0.0f)
            {
                SendEvent(GO_FINAL_STATE);
            }
        }

        /// <summary>
        /// Selecciona una tecnica visual para dibujar el aspecto de la nave.
        /// </summary>
        /// <param name="id">Identificador de la nave.</param>
        protected void ApplyTechnique(string id)
        {
            SimpleModel model = Owner.Component.Context.Get<SimpleModel>("Model");
            IEnumerator<Material> materials = model.Materials;
            materials.Reset();

            while (materials.MoveNext())
            {
                Effect effect = materials.Current.Effect;
                effect.CurrentTechnique = effect.Techniques[id];
            }
        }
    }
}
