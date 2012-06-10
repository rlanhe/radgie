using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework;
using Radgie.Util;
using AsteroidsStorm.States.Game;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.AsteroidsField
{
    /// <summary>
    /// Controlador de un sector de asteroides que dispone los SpaceObject de manera aleatoria.
    /// </summary>
    class Random: AAsteroidSectorState
    {
        /// <summary>
        /// Lista con los SpaceObject que maneja el controlador.
        /// </summary>
        private static List<SpaceObject> mGeneratedObjects;

        /// <summary>
        /// Indica si puede generar bonificadores.
        /// </summary>
        public bool HasBonus { get; set; }
        /// <summary>
        /// Tiempo transcurrido desde la ultima actualizacion.
        /// </summary>
        private TimeSpan mTimeElapsed;
        /// <summary>
        /// Indica si mTimeElapsed fue inicializado.
        /// </summary>
        private bool mInitializeTimeElapsed;
        /// <summary>
        /// Numero de asteroides que debe crear.
        /// </summary>
        private float mAsteroidsToCreate;

        /// <summary>
        /// Constructor estatico.
        /// </summary>
        static Random()
        {
            mGeneratedObjects = new List<SpaceObject>();
        }

        /// <summary>
        /// Constructor del sector de asteroides.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados padre.</param>
        public Random(IStateMachine stateMachine)
            : base(stateMachine)
        {
            HasBonus = true;
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            mInitializeTimeElapsed = true;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (mInitializeTimeElapsed)
            {
                mTimeElapsed = new TimeSpan(time.TotalGameTime.Ticks);
                mInitializeTimeElapsed = false;
            }

            AsteroidsField aField = (AsteroidsField)Owner.Component;

            List<SpaceObject> sObjects = aField.SpaceObjects;

            Radgie.Core.IGameComponent gc = Owner.Component;

            int count = sObjects.Count;
            int newAsteroids = MaxAsteroids - count;
            if (newAsteroids > 0)
            {
                float numberOfAsteroidsPerFrame = ((float)MaxAsteroidsPerSecond) * ((float)time.ElapsedGameTime.TotalSeconds);
                numberOfAsteroidsPerFrame += mAsteroidsToCreate;
                int asteroids = (int)numberOfAsteroidsPerFrame / 1;
                mAsteroidsToCreate = numberOfAsteroidsPerFrame - asteroids;

                mGeneratedObjects.Clear();

                for (int i = 0; i < asteroids; i++)
                {
                    if (mGeneratedObjects.Count > newAsteroids)
                    {
                        break;
                    }
                    // Generate random rotation vector
                    Vector3 rotVector = new Vector3((float)mRandomGenerator.NextDouble(), (float)mRandomGenerator.NextDouble() * 2.0f - 1.0f, 0.0f);
                    rotVector.Normalize();

                    Vector3 position = GetInitialPosition(time);
                    SpaceObject sObject = aField.AddSpaceObject(GetRandomType(), position, rotVector, AsteroidsSpeed, AsteroidsRotationSpeed);
                    sObject.Update(null);

                    if (sObject != null)
                    {
                        CheckAsteroidPosition(mGeneratedObjects, sObject);
                        mGeneratedObjects.Add(sObject);
                    }
                }
            }
        }

        /// <summary>
        /// Obtiene el tipo del proximo objeto que va a crear.
        /// </summary>
        /// <returns>Tipo del SpaceObject.</returns>
        private SpaceObject.Type GetRandomType()
        {
            double value = MathUtil.GetRandomDouble();
            SpaceObject.Type returnType;

            if (HasBonus)
            {

                if (value < 0.15d)
                {
                    returnType = SpaceObject.Type.Bonus_Energy;
                }
                else if (value < 0.50d)
                {
                    returnType = SpaceObject.Type.Asteroid_Regular;
                }
                else if (value < 0.70d)
                {
                    returnType = SpaceObject.Type.Apophis_Asteroid;
                }
                else if (value < 0.85d)
                {
                    returnType = SpaceObject.Type.Bonus_Life;
                }
                else if (value < 0.95d)
                {
                    returnType = SpaceObject.Type.Bonus_Bomb;
                }
                else
                {
                    returnType = SpaceObject.Type.Bonus_Invincibility;
                }
            }
            else
            {
                returnType = SpaceObject.Type.Asteroid_Regular;
            }

            return returnType;
        }

        /// <summary>
        /// Indica si debe generar un bonus.
        /// </summary>
        /// <returns>True si debe generar un bonus, False en caso contrario.</returns>
        private bool GenerateBonus()
        {
            double val = mRandomGenerator.NextDouble();
            if (val < 0.6d)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Comprueba la posicion del asteroide y si colisiona con otros, lo reposiciona.
        /// </summary>
        /// <param name="asteroids">Lista de SpaceObjects que maneja el sector de asteroides.</param>
        /// <param name="newAsteroid">Asteroide que se va a recolocar.</param>
        private void CheckAsteroidPosition(List<SpaceObject> asteroids, SpaceObject newAsteroid)
        {
            newAsteroid.Update(null);
            const int MAX_TRIES = 20;
            int counter = 0;
            while((counter < MAX_TRIES) && (CheckIfCollides(asteroids, newAsteroid)))
            {
                newAsteroid.Transformation.Translation = GetInitialPosition(null);
                newAsteroid.Update(null);
                counter++;
            }
        }

        /// <summary>
        /// Comprueba si un asteroide colisiona con el resto.
        /// </summary>
        /// <param name="asteroids">Lista de SpaceObjects.</param>
        /// <param name="newAsteroid">SpaceObject que se va a comprobar.</param>
        /// <returns>True si colisiona, False en caso contrario.</returns>
        private bool CheckIfCollides(List<SpaceObject> asteroids, SpaceObject newAsteroid)
        {
            foreach (SpaceObject asteroid in asteroids)
            {
                if (asteroid.BoundingVolume.Intersects(newAsteroid.BoundingVolume) != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Obtiene la posicion inicial del SpaceObject.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la inicializacion del sector de asteroides.</param>
        /// <returns>Posicion inicial del SpaceObject.</returns>
        private Vector3 GetInitialPosition(Microsoft.Xna.Framework.GameTime time)
        {
            GameData gameData = GetFromContext<GameData>("GameData");
            Vector3 result = new Vector3((float)mRandomGenerator.NextDouble() * mWidth - mHalfWidth, (float)mRandomGenerator.NextDouble() * mHeight - mHalfHeight, -mHalfLength);
            
            if (gameData != null)
            {
                result.Z = gameData.PlayerState.Spaceship.World.Translation.Z + result.Z;
                return result;
            }
            else
            {
                if (time != null)
                {
                    result.Z = (float)((time.TotalGameTime - mTimeElapsed).TotalSeconds * -1.0f);
                }
                return result;
            }
        }
    }
}
