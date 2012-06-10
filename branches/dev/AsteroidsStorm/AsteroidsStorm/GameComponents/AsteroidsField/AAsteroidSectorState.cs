using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework;
using Radgie.Util.Collection.Context;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.AsteroidsField
{
    /// <summary>
    /// Estado de la maquina de estados del sector de asteroides.
    /// Controla la creacion de los asteroides asi como su disposicion en el espacio.
    /// </summary>
    abstract class AAsteroidSectorState: AState
    {
        /// <summary>
        /// Evento para cambiar al siguiente estado.
        /// </summary>
        public static readonly Event GO_NEXT = new Event("GO_NEXT_AAsteroidSectorState");

        /// <summary>
        /// Numero maximo de asteroides que puede manejar.
        /// </summary>
        public int MaxAsteroids
        {
            get
            {
                return mMaxAsteroids;
            }
            set
            {
                if (value > 0)
                {
                    mMaxAsteroids = value;
                }
            }
        }
        private int mMaxAsteroids = 50;//100

        /// <summary>
        /// Numero maximo de asteroides por segundo que puede crear.
        /// </summary>
        public int MaxAsteroidsPerSecond
        {
            get
            {
                return mMaxAsteroidsPerSecond;
            }
            set
            {
                if (mMaxAsteroidsPerSecond > 0)
                {
                    mMaxAsteroidsPerSecond = value;
                }
            }
        }
        private int mMaxAsteroidsPerSecond = 3;//3

        /// <summary>
        /// Velocidad de los asteroides.
        /// </summary>
        public float AsteroidsSpeed
        {
            get
            {
                return mAsteroidsSpeed;
            }
            set
            {
                mAsteroidsSpeed = value;
            }
        }
        private float mAsteroidsSpeed = 17.0f;

        /// <summary>
        /// Velocidad de rotacion de los asteroides.
        /// </summary>
        public float AsteroidsRotationSpeed
        {
            get
            {
                return mAsteroidsRotationSpeed;
            }
            set
            {
                mAsteroidsRotationSpeed = value;
            }
        }
        private float mAsteroidsRotationSpeed = 5.0f;

        /// <summary>
        /// Largo del sector de asteroides.
        /// </summary>
        protected int mLength;
        protected float mHalfLength;
        /// <summary>
        /// Ancho del sector de asteroides.
        /// </summary>
        protected int mWidth;
        protected float mHalfWidth;
        /// <summary>
        /// Altura del sector de asteroides.
        /// </summary>
        protected int mHeight;
        protected float mHalfHeight;

        /// <summary>
        /// Generador de numeros aleatorios que usara para annadir cierto 
        /// </summary>
        protected static System.Random mRandomGenerator = new System.Random();

        /// <summary>
        /// Crea el nuevo estado del sector de asteroides.
        /// </summary>
        /// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
        public AAsteroidSectorState(IStateMachine stateMachine)
            : base(stateMachine)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnInitialize"/>
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            mLength = GetFromContext<int>("Length");
            mHalfLength = mLength / 2.0f;
            mWidth = GetFromContext<int>("Width");
            mHalfWidth = mWidth / 2.0f;
            mHeight = GetFromContext<int>("Height");
            mHalfHeight = mHeight / 2.0f;
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.Update"/>
        /// </summary>
        /// <param name="time">Tiempo transcurrido.</param>
        public override void Update(GameTime time)
        {
            base.Update(time);

            AsteroidsField aField = ((AsteroidsField)Owner.Component);
            IContext context = aField.Context;
            List<SpaceObject> sObjects = aField.SpaceObjects;
            
            // Comprueba los asteroides que estan fuera del sector
            for (int i = sObjects.Count - 1; i >= 0; i--)
            {
                SpaceObject asteroid = sObjects[i];
                if(aField.ContainsSpaceObject(asteroid))
                {
                    // Quita asteroide de sector
                    aField.RemoveSpaceObject(asteroid);
                }
            }
        }
    }
}
