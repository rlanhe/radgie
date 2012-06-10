using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Util;
using Radgie.Core.BoundingVolumes;
using Radgie.Core;
using Radgie.Core.Collision;
using AsteroidsStorm.States.Game;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Estado de la nave durante el lanzamiento de una bomba de energia.
    /// </summary>
    class BombState: ASpaceshipState
    {
        private BackCounter mBackCounter;
        private const float SIZE_INCREMENT_PER_SECOND = 30.0f;
        private IGameComponent mBombGC;

        private static List<CollisionRecord> mResults;

        /// <summary>
        /// Constructor estatico.
        /// </summary>
        static BombState()
        {
            mResults = new List<CollisionRecord>();
        }

        /// <summary>
        /// Inicializa el estado.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public BombState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            mBombGC = new GameComponent("BombGC");
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            if (mBackCounter == null)
            {
                mBackCounter = new BackCounter(new TimeSpan(0, 0, 2));
            }
            mBackCounter.Start();

            mBombGC.BoundingVolume = new BoundingSphere(new Microsoft.Xna.Framework.Vector4(Microsoft.Xna.Framework.Vector3.Zero, 0.1f));
            AsteroidsField.AsteroidsField asteroidsField = this.GameData.AsteroidsField;
            asteroidsField.Scene.AddComponent(mBombGC);
            asteroidsField.CollisionGroup.AddGameComponent(mBombGC);
            mBombGC.Transformation.Translation = this.GameData.PlayerState.Spaceship.World.Translation;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            float sizeIncrement = (float)(time.ElapsedGameTime.TotalSeconds*SIZE_INCREMENT_PER_SECOND);
            BoundingSphere bs = (BoundingSphere)mBombGC.BoundingVolume;
            mBombGC.BoundingVolume = new BoundingSphere(new Microsoft.Xna.Framework.Vector4(Microsoft.Xna.Framework.Vector3.Zero, bs.BoundingVolume.Radius + sizeIncrement));
            // Actualiza la posicion del boundingVolume del objeto
            mBombGC.Update(null);

            // Destruye asteroides en la onda expansiva de la bomba
            AsteroidsField.AsteroidsField asteroidsField = this.GameData.AsteroidsField;
            asteroidsField.GetCollisions(mBombGC, mResults);
            foreach (CollisionRecord record in mResults)
            {
                IGameComponent other = record.GameComponentA == mBombGC ? record.GameComponentB : record.GameComponentA;
                if (other is SpaceObject)
                {
                    SpaceObject sObject = (SpaceObject)other;
                    if ((sObject.ObjectType == SpaceObject.Type.Apophis_Asteroid) || (sObject.ObjectType == SpaceObject.Type.Asteroid_Regular))
                    {
                        sObject.Destroy();
                    }
                }
            }

            if (mBackCounter.Finished())
            {
                SendEvent(GO_DEFAULT);
            }
        }
        
        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            AsteroidsField.AsteroidsField asteroidsField = this.GameData.AsteroidsField;
            asteroidsField.Scene.RemoveComponent(mBombGC);
        }
    }
}
