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
    /// Controlador del sector de asteroides para el que se muestra en el menu principal.
    /// </summary>
    class MenuAsteroidSectorState: AAsteroidSectorState
    {
        /// <summary>
        /// Tiempo de espera entre dos fases de lanzamiento de asteroides.
        /// </summary>
        private int mSleepTime = 1;

        /// <summary>
        /// Contador para realizar las pausas en los lanzamientos de asteroides.
        /// </summary>
        private BackCounter mCounter;
        /// <summary>
        /// SpaceObjects generados.
        /// </summary>
        private static List<SpaceObject> mGeneratedObjects;

        /// <summary>
        /// Indica si puede lanzar objetos bonus.
        /// </summary>
        public bool HasBonus { get; set; }
        /// <summary>
        /// Tiempo transcurrido desde el ultimo lanzamiento.
        /// </summary>
        private TimeSpan mTimeElapsed;
        /// <summary>
        /// Tiempo transcurrido desde la inicializacion del sector.
        /// </summary>
        private bool mInitializeTimeElapsed;

        /// <summary>
        /// Constructor estatico.
        /// </summary>
        static MenuAsteroidSectorState()
        {
            mGeneratedObjects = new List<SpaceObject>();
        }

        /// <summary>
        /// Construye el sector de asteroides que sera usado desde el menu de la aplicacion.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados principal.</param>
        public MenuAsteroidSectorState(IStateMachine stateMachine)
            : base(stateMachine)
        {
            HasBonus = false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            mCounter = new BackCounter(new TimeSpan(0, 0, mSleepTime));
            mCounter.Start();

            mInitializeTimeElapsed = true;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateableAtRate.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            if (mInitializeTimeElapsed)
            {
                mTimeElapsed = new TimeSpan(time.TotalGameTime.Ticks);
                mInitializeTimeElapsed = false;
            }
            
            if (mCounter.Finished())
            {
                AsteroidsField aField = (AsteroidsField)Owner.Component;

                List<SpaceObject> sObjects = aField.SpaceObjects;

                Radgie.Core.IGameComponent gc = Owner.Component;

                int count = sObjects.Count;
                int newAsteroids = MaxAsteroids - count;
                if (newAsteroids > 0)
                {
                    newAsteroids = newAsteroids > MaxAsteroidsPerSecond ? MaxAsteroidsPerSecond : newAsteroids;

                    mGeneratedObjects.Clear();

                    for (int i = 0; i < newAsteroids; i++)
                    {
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

                mCounter.Start();
            }
            

        }

        /// <summary>
        /// Obtiene el tipo del SpaceObject obtenido de forma aleatoria.
        /// </summary>
        /// <returns>Tipo del SpaceObject.</returns>
        private SpaceObject.Type GetRandomType()
        {
            double value = MathUtil.GetRandomDouble();
            SpaceObject.Type returnType;

            returnType = SpaceObject.Type.Asteroid_Regular;

            return returnType;
        }

        /// <summary>
        /// Chequea la posicion del asteroide, corrigiendo su posicion si esta chocando con otro objeto.
        /// </summary>
        /// <param name="asteroids">Lista de SpaceObjects que maneja el sector de asteroides.</param>
        /// <param name="newAsteroid">Asteroide que va a recolocar.</param>
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
        /// Indica si el asteroide esta colisionando con alguno de los otros SpaceObject que maneja el sector de asteroides.
        /// </summary>
        /// <param name="asteroids">Lista de asteroides que maneja el sector.</param>
        /// <param name="newAsteroid">Asteroides que se va a comprobar si esta colisionando.</param>
        /// <returns>True si colisiona con algun asteroide, False en caso contrario.</returns>
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
        /// Calcula la posicion inicial del asteroide dentro del sector.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde el inicio del sector.</param>
        /// <returns>Posicion del asteroide dentro del sector.</returns>
        private Vector3 GetInitialPosition(Microsoft.Xna.Framework.GameTime time)
        {
            Vector3 result = new Vector3((float)mRandomGenerator.NextDouble() * mWidth - mHalfWidth, (float)mRandomGenerator.NextDouble() * mHeight - mHalfHeight, -mHalfLength);
            
            if (time != null)
            {
                result.Z = (float)((time.TotalGameTime - mTimeElapsed).TotalSeconds * -1.0f);
            }
            return result;
        }
    }
}
