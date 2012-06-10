using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Util.Collection.Pool;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Entity;
using Radgie.Core.Collision;
using Radgie.State;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.AsteroidsField
{
    /// <summary>
    /// Sector de asteroides.
    /// Controla el lanzamiento de asteroides hacia la nave.
    /// </summary>
    public class AsteroidsField: Radgie.Core.GameComponent
    {
        /// <summary>
        /// Pool de SpaceObjects que maneja el sector de asteroides.
        /// </summary>
        private Pool<SpaceObject> mSpaceObjects;

        /// <summary>
        /// Ancho del sector de asteroides.
        /// </summary>
        private int mWidth;
        /// <summary>
        /// Alto del sector de asteroides.
        /// </summary>
        private int mHeight;
        /// <summary>
        /// Longitud del sector de asteroides.
        /// </summary>
        private int mLength;
        private Vector3 mDirection;

        /// <summary>
        /// Lista para almacenar los objetos que se estan mostrando dentro del AsteroidsField.
        /// </summary>
        public List<SpaceObject> SpaceObjects
        {
            get
            {
                return mSpaceObjectsList;
            }
        }
        private List<SpaceObject> mSpaceObjectsList;

        /// <summary>
        /// Grupo de colision en el que estan los asteroides y la nave espcial, que se usara para detectar las colisiones.
        /// </summary>
        public ICollisionGroup CollisionGroup
        {
            get
            {
                return mCollisionGroup;
            }
        }
        private ICollisionGroup mCollisionGroup;

        /// <summary>
        /// Volumen de colision del sector de asteroides para identificar cuando un asteroide esta fuera y puede ser destruido.
        /// </summary>
        private Radgie.Core.IGameComponent mBoundingVolumenComponent;

        /// <summary>
        /// Crea un sector de asteroides.
        /// </summary>
        /// <param name="scene">Escena a la que debe annadirse.</param>
        /// <param name="numberOfObjects">Numero maximo de asteroides que puede manejar.</param>
        /// <param name="width">Ancho del sector de asteroides.</param>
        /// <param name="height">Altura del sector de asteroides.</param>
        /// <param name="length">Largo del sector de asteroides.</param>
        /// <param name="direction">Direccion en la que se enviaran los asteroides.</param>
        /// <param name="controller">Objeto para controlar el lanzamiento de los asteroides.</param>
        public AsteroidsField(IScene scene, int numberOfObjects, int width, int height, int length, Vector3 direction, AState controller)
            : base("AsteroidField")
        {
            mSpaceObjectsList = new List<SpaceObject>();

            float percentageEnlarge = 1.0f;
            mWidth = (int)(width * percentageEnlarge);
            mHeight = (int)(height * percentageEnlarge);
            mLength = length;
            mDirection = direction;

            float halfWidth = mWidth / 2.0f;
            float halfHeight = mHeight / 2.0f;
            float halfLength = mLength / 2.0f;

            SetInContext("Width", mWidth);
            SetInContext("Height", mHeight);
            SetInContext("Length", mLength);

            mSpaceObjects = new Pool<SpaceObject>(numberOfObjects, true, 0, CreateSpaceObject);

            mBoundingVolumenComponent = new Radgie.Core.GameComponent("AsteroidsField_BV_GC");
            mBoundingVolumenComponent.BoundingVolume = new Radgie.Core.BoundingVolumes.BoundingBox(new Vector3(-halfWidth, -halfHeight, -halfLength), new Vector3(halfWidth, halfHeight, halfLength));
            AddGameComponent(mBoundingVolumenComponent);

            scene.AddComponent(this);
            mCollisionGroup = scene.AddNewCollisionGroup("AsteroidsField_CollisionGroup");
            mCollisionGroup.AddGameComponent(mBoundingVolumenComponent);
            AddGameObject(new Behaviour(controller));
        }

        /// <summary>
        /// Crea un nuevo SpaceObject.
        /// </summary>
        /// <returns>SpaceObject creado.</returns>
        private static SpaceObject CreateSpaceObject()
        {
            return new SpaceObject();
        }

        /// <summary>
        /// Annade un nuevo SpaceObject al sector de asteroides.
        /// </summary>
        /// <param name="type">Tipo del SpaceObject.</param>
        /// <param name="position">Posicion en la que se va a annadir.</param>
        /// <param name="rotDirection">Eje de rotacion.</param>
        /// <param name="linearVelocity">Velocidad lineal.</param>
        /// <param name="angularVelocity">Velocidad angular.</param>
        /// <returns></returns>
        public SpaceObject AddSpaceObject(SpaceObject.Type type, Vector3 position, Vector3 rotDirection, float linearVelocity, float angularVelocity)
        {
            SpaceObject spaceObject = mSpaceObjects.Get();

            if (spaceObject != null)
            {
                Scene.AddComponent(spaceObject);
                mCollisionGroup.AddGameComponent(spaceObject);

                spaceObject.Init(type, position, Vector3.Transform(mDirection, Transformation.Rotation), rotDirection, linearVelocity, angularVelocity);
                mSpaceObjectsList.Add(spaceObject);
            }

            return spaceObject;
        }

        /// <summary>
        /// Quita un SpaceObject del sector de asteroides.
        /// </summary>
        /// <param name="spaceObject">SpaceObject que se va a quitar.</param>
        public void RemoveSpaceObject(SpaceObject spaceObject)
        {
            Scene.RemoveComponent(spaceObject);
            spaceObject.Reset();
            mSpaceObjects.Release(spaceObject);
            mSpaceObjectsList.Remove(spaceObject);
        }

        /// <summary>
        /// Mueve el sector de asteroides para mantener la distancia con la nave del jugador.
        /// </summary>
        /// <param name="translation">Posicion de la nave del jugador.</param>
        public void UpdatePosition(Vector3 translation)
        {
            mBoundingVolumenComponent.Transformation.Translation = new Vector3(0.0f, 0.0f, translation.Z - mLength/2.0f + 7.0f);
        }

        /// <summary>
        /// Indica si el sector de asteroides contiene un objeto.
        /// </summary>
        /// <param name="spaceObject">SpaceObject que se quiere consultar.</param>
        /// <returns>True si lo esta, False en caso contrario.</returns>
        public bool ContainsSpaceObject(SpaceObject spaceObject)
        {
            return mBoundingVolumenComponent.BoundingVolume.Contains(spaceObject.BoundingVolume) == ContainmentType.Disjoint;
        }

        /// <summary>
        /// Calcula las colisiones que se estan produciendo dentro del sector de asteroides contra un objeto determinado.
        /// </summary>
        /// <param name="target">Objeto contra el que se van a calcular las colisiones.</param>
        /// <param name="results">Lista con las colisiones que se estan produciendo.</param>
        public void GetCollisions(Radgie.Core.IGameComponent target, List<CollisionRecord> results)
        {
            if (mBoundingVolumenComponent.BoundingVolume.Contains(target.BoundingVolume) != ContainmentType.Disjoint)
            {
                ICollisionGroup cGroup = CollisionGroup;
                if (cGroup != null)
                {
                    cGroup.GetCollisions(target, results);
                }
            }
        }
    }
}
