using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework;
using AsteroidsStorm.States.Game;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Controlador del manejo de la nave.
    /// </summary>
    class SpaceshipMovementController: AState
    {
        /// <summary>
        /// Rotacion del eje X.
        /// </summary>
        protected float mXRot = 0.0f;
        /// <summary>
        /// Rotacion del eje Y.
        /// </summary>
        protected float mYZRot = 0.0f;
        /// <summary>
        /// Maxima rotacion en el eje X.
        /// </summary>
        protected float mMaxXRot = MathHelper.Pi/5.0f;
        /// <summary>
        /// Maxima rotacion en el eje Y.
        /// </summary>
        protected float mMaxYZRot = MathHelper.Pi/5.0f;
                
        /// <summary>
        /// Inicializa el controlador.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertence.</param>
        public SpaceshipMovementController(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }
        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();
            GetFromContext<GameData>("GameData").PlayerState.Spaceship.Speed = 15.0f;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            GameData gameData = GetFromContext<GameData>("GameData");

            Radgie.Core.IGameComponent ship = Owner.Component;
            float elapsedTime = (float)time.ElapsedGameTime.TotalSeconds;

            if(!gameData.GameOver)
            {
                if (gameData.PlayerState.Energy > 0.0f)
                {
                    ISpaceshipController controller = GetFromContext<ISpaceshipController>("Controller");
                    float steerFactor = gameData.PlayerState.Spaceship.Steer / 100.0f;

                    // Rotacion nave
                    mYZRot = -controller.LeftRight.Value;
                    mXRot = controller.UpDown.Value;

                    float maxRot = mMaxYZRot * steerFactor;
                    ship.Transformation.Rotation = Quaternion.CreateFromAxisAngle(ship.World.Right, MathHelper.Lerp(0.0f, maxRot, mXRot)) * Quaternion.CreateFromAxisAngle(ship.World.Backward, MathHelper.Lerp(0.0f, maxRot, mYZRot)) * Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.Lerp(0.0f, maxRot, mYZRot));
                    ship.Transformation.Update();
                }

                // Desplazamiento nave
                Vector3 desplz = ship.Transformation.Forward;
                ship.Transformation.Translation += desplz * gameData.PlayerState.Spaceship.Speed * elapsedTime;
            }

            // Controla que no se salga del cuadrilatero - top - bottom - left - right
            Vector4 positionLimits = GetFromContext<Vector4>("PositionLimits");
            Vector3 translation = ship.Transformation.Translation;
            float limit = (ship.BoundingVolume != null && ship.BoundingVolume is Radgie.Core.BoundingVolumes.BoundingSphere) ? ((Radgie.Core.BoundingVolumes.BoundingSphere)ship.BoundingVolume).BoundingVolume.Radius : 1.0f;
            FixValue(ref translation.Y, positionLimits.X - limit);
            FixValue(ref translation.Y, positionLimits.Y + limit);
            FixValue(ref translation.X, positionLimits.Z + limit);
            FixValue(ref translation.X, positionLimits.W - limit);
            ship.Transformation.Translation = translation;

            GetFromContext<Radgie.Util.Collection.ObserverList.ObserverList<Vector3>>("Observers").NotifyObservers(ship.Transformation.Translation);
        }

        /// <summary>
        /// Rectifica el valor para que no pase del limite.
        /// </summary>
        /// <param name="value">Valor original.</param>
        /// <param name="limit">Limite del valor.</param>
        private void FixValue(ref float value, float limit)
        {
            if (limit > 0)
            {
                if (value > limit)
                {
                    value = limit;
                }
            }
            else
            {
                if (value < limit)
                {
                    value = limit;
                }
            }
        }
    }
}
