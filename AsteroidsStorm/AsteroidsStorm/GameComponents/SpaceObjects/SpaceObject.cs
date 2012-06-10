using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.State;
using Radgie.Util.Collection.Pool;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Entity;
using Radgie.Sound;
using AsteroidsStorm.States.Game;

namespace AsteroidsStorm.GameComponents.SpaceObjects
{
    /// <summary>
    /// Cualquier objeto que pertenezca al sector de asteroides.
    /// </summary>
    public class SpaceObject: Radgie.Core.GameComponent
    {
        /// <summary>
        /// Tipos de SpaceObjects.
        /// </summary>
        public enum Type
        {
            Asteroid_Regular,
            Apophis_Asteroid,
            Bonus_Life,
            Bonus_Invincibility,
            Bonus_Bomb,
            Bonus_Energy
        }

        private static int mCounter;

        /// <summary>
        /// Tipo del space object.
        /// </summary>
        public Type ObjectType
        {
            get
            {
                return mObjectType;
            }
        }
        private Type mObjectType;

        /// <summary>
        /// Indica si se trata de un bonificador o no.
        /// </summary>
        public bool IsBonus
        {
            get
            {
                if ((mObjectType == Type.Apophis_Asteroid) || (mObjectType == Type.Asteroid_Regular))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private Radgie.Core.GameComponent mGraphicComponent;
        private Behaviour mController;

        private Vector3 mRotDirection;
        private float mLinearVelocity;
        private float mAngularVelocity;

        private static Pool<Radgie.Core.GameComponent> mRegularAsteroidPool;
        private static Pool<Radgie.Core.GameComponent> mApophisAsteroidPool;
        private static Pool<Radgie.Core.GameComponent> mLifeBonusPool;
        private static Pool<Radgie.Core.GameComponent> mInvincibilityBonusPool;
        private static Pool<Radgie.Core.GameComponent> mBombBonusPool;
        private static Pool<Radgie.Core.GameComponent> mEnergyBonusPool;
        private static SimpleModel mRegularAsteroidModel;
        private static SimpleModel mApophisAsteroidModel;
        private static SimpleModel mLifeBonusModel;
        private static SimpleModel mInvincibilityBonusModel;
        private static SimpleModel mBombBonusModel;
        private static SimpleModel mEnergyBonusModel;
        
        private static Pool<Behaviour> mApophisAsteroidControllerPool;
        private static Pool<Behaviour> mAsteroidDestructionControllerPool;
        private static Pool<Behaviour> mBonusDestructionControllerPool;
        private static Pool<Behaviour> mAsteroidControllerPool;

        /// <summary>
        /// Datos de un SpaceObject.
        /// </summary>
        private struct SpaceObjectData
        {
            /// <summary>
            /// Matriz de transformacion.
            /// </summary>
            Matrix World;
            /// <summary>
            /// Eje en el que rota.
            /// </summary>
            Vector3 Axis;

            /// <summary>
            /// Datos de un spaceObject.
            /// </summary>
            /// <param name="world">Matriz de transformacion.</param>
            /// <param name="axis">Eje en el que rota.</param>
            public SpaceObjectData(Matrix world, Vector3 axis)
            {
                World = world;
                Axis = axis;
            }
        }

        /// <summary>
        /// Obtiene los datos de un space object a partir de los datos de una instancia.
        /// </summary>
        /// <param name="instance">Instancia.</param>
        /// <returns>Estructura SpaceObjectData rellenada.</returns>
        private static SpaceObjectData GetData(IGraphicInstance instance)
        {
            return new SpaceObjectData(instance.Component.World, instance.Component.GetFromContext<Vector3>("Axis"));
        }

        /// <summary>
        /// Declaracion de los vertices de un SpaceObject.
        /// </summary>
        private static VertexDeclaration SpaceObjectInstanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3),
            new VertexElement(64, VertexElementFormat.Vector3, VertexElementUsage.BlendIndices, 0)
        );

        /// <summary>
        /// Configura el material del modelo del objeto que representa al spaceobject para rellenar los parametros personalizados.
        /// </summary>
        /// <param name="model"></param>
        private static void ConfigureSimpleModelMaterial(SimpleModel model)
        {
            IEnumerator<Material> enumerator = model.Materials;
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                Material current = enumerator.Current;
                MaterialParameter parameter = current["targetPosition"];
                parameter.UpdateFreq = UpdateFreq.PerFrame;
                parameter.UpdateCallback = GetTargetPosition;
            }
        }

        /// <summary>
        /// Obtiene la posicion del objetivo (en este caso la nave).
        /// Metodo a utilizar en la actualizacion del material del objeto que representa el space object.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        /// <param name="materialParameter">Parametro del material donde se almacena la posicion del objetivo.</param>
        private static void GetTargetPosition(IRenderer renderer, MaterialParameter materialParameter)
        {
            IStateSystem sSystem = ((IStateSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IStateSystem)));
            GameData gameData = sSystem.GetFromContext<GameData>("GameData");
            if(gameData != null)
            {
                Radgie.Core.IGameComponent spaceship = gameData.PlayerState.Spaceship;
                if(spaceship != null)
                {
                    materialParameter.SetValue(spaceship.World.Translation);
                }
            }
        }

        /// <summary>
        /// Constructor estatico.
        /// </summary>
        static SpaceObject()
        {
            mRegularAsteroidModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Asteroid/Graphics/Models/Asteroids/asteroid_01"));
            mRegularAsteroidModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mRegularAsteroidModel);
            IEnumerator<Material> materials = mRegularAsteroidModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Asteroid/Graphics/Textures/Asteroid/normal", false));
            ConfigureSimpleModelMaterial(mRegularAsteroidModel);

            mApophisAsteroidModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Asteroid/Graphics/Models/Asteroids/asteroid_02"));
            mApophisAsteroidModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mApophisAsteroidModel);
            materials = mApophisAsteroidModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Asteroid/Graphics/Textures/Asteroid/apophis", false));

            mLifeBonusModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Bonus/Graphics/Models/cube"));
            mLifeBonusModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mLifeBonusModel);
            materials = mLifeBonusModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Bonus/Graphics/Textures/life", false));

            mInvincibilityBonusModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Bonus/Graphics/Models/cube"));
            mInvincibilityBonusModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mInvincibilityBonusModel);
            materials = mInvincibilityBonusModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Bonus/Graphics/Textures/invincibility", false));

            mBombBonusModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Bonus/Graphics/Models/cube"));
            mBombBonusModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mBombBonusModel);
            materials = mBombBonusModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Bonus/Graphics/Textures/bomb", false));

            mEnergyBonusModel = new SimpleModel(RadgieGame.Instance.ResourceManager.Load<Mesh>("GameComponents/Bonus/Graphics/Models/cube"));
            mEnergyBonusModel.InstancesRenderer = new EntityInstancesRenderer<SpaceObjectData>(SpaceObjectInstanceVertexDeclaration, GetData, mEnergyBonusModel);
            materials = mEnergyBonusModel.Materials;
            materials.MoveNext();
            materials.Current[Semantic.Texture0].SetValue(RadgieGame.Instance.ResourceManager.Load<Texture>("GameComponents/Bonus/Graphics/Textures/energy", false));

            // TODO: Mover pool de sonidos y particulas a controllers
            // TODO: Forzar garbaje collector justo antes de menus y justo antes de empezar juego
            // TODO: Cambiar sistema de entrada y movimiento nave para que fisica no se vea afectada
            mAsteroidControllerPool = new Pool<Behaviour>(300, false, 5, CreateSimpleSpaceObjectController);
            mApophisAsteroidControllerPool = new Pool<Behaviour>(100, false, 5, CreateApophisAsteroidController);
            mAsteroidDestructionControllerPool = new Pool<Behaviour>(75, false, 10, CreateAsteroidDestructionController);
            
            mRegularAsteroidPool = new Pool<Radgie.Core.GameComponent>(350, false, 5, CreateRegularAsteroid);
            mApophisAsteroidPool = new Pool<Radgie.Core.GameComponent>(150, false, 5, CreateApophisAsteroid);
            mLifeBonusPool = new Pool<Radgie.Core.GameComponent>(150, false, 5, CreateLifeBonus);
            mBombBonusPool = new Pool<Radgie.Core.GameComponent>(100, false, 5, CreateBombBonus);
            mInvincibilityBonusPool = new Pool<Radgie.Core.GameComponent>(100, false, 5, CreateInvincibilityBonus);
            mEnergyBonusPool = new Pool<Radgie.Core.GameComponent>(250, false, 5, CreateEnergyBonus);

            
            mBonusDestructionControllerPool = new Pool<Behaviour>(75, false, 10, CreateBonusDestructionController);
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public SpaceObject()
            : base("SpaceObject_" + mCounter++)
        {
            BoundingVolume = new Radgie.Core.BoundingVolumes.BoundingSphere(Vector3.Zero, 1.0f);
        }

        /// <summary>
        /// Crea un asteroide de tipo regular.
        /// </summary>
        /// <returns>Asteroide de tipo regular.</returns>
        private static Radgie.Core.GameComponent CreateRegularAsteroid()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("regular");
            gc.AddGameObject(mRegularAsteroidModel.CreateInstance());
            return gc;
        }

        /// <summary>
        /// Crea un asteroide de tipo Apophis.
        /// </summary>
        /// <returns>Asteroide de tipo apophis.</returns>
        private static Radgie.Core.GameComponent CreateApophisAsteroid()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("apophis");
            gc.AddGameObject(mApophisAsteroidModel.CreateInstance());
            return gc;
        }

        /// <summary>
        /// Crea un bonus de tipo Life.
        /// </summary>
        /// <returns>Bonus de tipo life.</returns>
        private static Radgie.Core.GameComponent CreateLifeBonus()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("life");
            gc.AddGameObject(mLifeBonusModel.CreateInstance());
            return gc;
        }

        /// <summary>
        /// Crea un Bonus de tipo Bomb.
        /// </summary>
        /// <returns>Bonus de tipo Bomb.</returns>
        private static Radgie.Core.GameComponent CreateBombBonus()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("bomb");
            gc.AddGameObject(mBombBonusModel.CreateInstance());
            return gc;
        }

        /// <summary>
        /// Crea un Bonus de tipo Invencibility.
        /// </summary>
        /// <returns>Bonus de tipo Invencibility.</returns>
        private static Radgie.Core.GameComponent CreateInvincibilityBonus()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("invincibility");
            gc.AddGameObject(mInvincibilityBonusModel.CreateInstance());
            return gc;
        }

        /// <summary>
        /// Crea un Bonus de tipo Energy.
        /// </summary>
        /// <returns>Bonus de tipo Energy.</returns>
        private static Radgie.Core.GameComponent CreateEnergyBonus()
        {
            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("energy");
            gc.AddGameObject(mEnergyBonusModel.CreateInstance());
            return gc;
        }
        
        /// <summary>
        /// Crea un controlador de spaceobject.
        /// </summary>
        /// <returns>Nuevo controlador de spaceobject.</returns>
        private static Behaviour CreateSimpleSpaceObjectController()
        {
            return new Behaviour(new SimpleSpaceObjectController());
        }
        
        /// <summary>
        /// Crea un controlador de asteroides apophis.
        /// </summary>
        /// <returns>Controlador de asteroides apophis.</returns>
        private static Behaviour CreateApophisAsteroidController()
        {
            return new Behaviour(new ApophisController());
        }

        /// <summary>
        /// Crea un controlador de destruccion de asteroides.
        /// </summary>
        /// <returns>Controlador de destruccion de asteroides.</returns>
        private static Behaviour CreateAsteroidDestructionController()
        {
            return new Behaviour(new AsteroidDestructionController());
        }

        /// <summary>
        /// Crea un controlador de destruccion de bonificadores.
        /// </summary>
        /// <returns>Controlador de destruccion de bonificadores.</returns>
        private static Behaviour CreateBonusDestructionController()
        {
            return new Behaviour(new BonusDestructionController());
        }
        /*
        private static SoundEffect CreateExplosionSound()
        {
            return new SoundEffect("GameComponents/Asteroid/Sounds/Collision");
        }

        private static SoundEffect CreateBounsSound()
        {
            return new SoundEffect("GameComponents/Bonus/Sounds/bonus");
        }

        private static SoundEffect CreateAsteroidSound()
        {
            return new SoundEffect("GameComponents/Bonus/Sounds/bonus");
        }
         * */

        /// <summary>
        /// Inicializa un space object.
        /// </summary>
        /// <param name="type">Tipo del space object.</param>
        /// <param name="position">Posicion inicial.</param>
        /// <param name="direction">Direccion en la que avanza.</param>
        /// <param name="rotDirection">Eje de rotacion.</param>
        /// <param name="linearVelocity">Velocidad lineal.</param>
        /// <param name="angularVelocity">Velocidad angular.</param>
        public void Init(SpaceObject.Type type, Vector3 position, Vector3 direction, Vector3 rotDirection, float linearVelocity, float angularVelocity)
        {
            mObjectType = type;

            Transformation.Translation = position;

            mRotDirection = rotDirection;

            SetInContext<Vector3>("Axis", rotDirection);
            mLinearVelocity = linearVelocity;
            mAngularVelocity = angularVelocity;


            if (type == Type.Asteroid_Regular)
            {
                mGraphicComponent = mRegularAsteroidPool.Get();
                mController = mAsteroidControllerPool.Get();
            }
            else if(type == Type.Apophis_Asteroid)
            {
                mGraphicComponent = mApophisAsteroidPool.Get();
                mController = mApophisAsteroidControllerPool.Get();
            }
            else if (type == Type.Bonus_Energy)
            {
                mGraphicComponent = mEnergyBonusPool.Get();
                mController = mAsteroidControllerPool.Get();
            }
            else if (type == Type.Bonus_Invincibility)
            {
                mGraphicComponent = mInvincibilityBonusPool.Get();
                mController = mAsteroidControllerPool.Get();
            }
            else if (type == Type.Bonus_Bomb)
            {
                mGraphicComponent = mBombBonusPool.Get();
                mController = mAsteroidControllerPool.Get();
            }
            else if (type == Type.Bonus_Life)
            {
                mGraphicComponent = mLifeBonusPool.Get();
                mController = mAsteroidControllerPool.Get();
            }

            if (mGraphicComponent != null)
            {
                AddGameComponent(mGraphicComponent);
            }

            if (mController != null)
            {
                AddGameObject(mController);
            }
        }

        /// <summary>
        /// Oculta el objeto.
        /// </summary>
        public void HideAsteroid()
        {
            if (mGraphicComponent != null)
            {
                RemoveGameComponent(mGraphicComponent);

                switch (mObjectType)
                {
                    case Type.Asteroid_Regular:
                        mRegularAsteroidPool.Release(mGraphicComponent);
                        break;
                    case Type.Apophis_Asteroid:
                        mApophisAsteroidPool.Release(mGraphicComponent);
                        break;
                    case Type.Bonus_Life:
                        mBombBonusPool.Release(mGraphicComponent);
                        break;
                    case Type.Bonus_Invincibility:
                        mInvincibilityBonusPool.Release(mGraphicComponent);
                        break;
                    case Type.Bonus_Bomb:
                        mBombBonusPool.Release(mGraphicComponent);
                        break;
                    case Type.Bonus_Energy:
                        mEnergyBonusPool.Release(mGraphicComponent);
                        break;
                }

                mGraphicComponent = null;
            }
        }

        /// <summary>
        /// Destruye el asteroide.
        /// </summary>
        public void Destroy()
        {
            GameData gameData = GetFromContext<GameData>("GameData");

            if (mController != null)
            {
                if (mController.State is SimpleSpaceObjectController)
                {
                    ((SimpleSpaceObjectController)mController.State).Release();
                }
            }

            switch (mObjectType)
            {
                case Type.Asteroid_Regular:
                    mAsteroidControllerPool.Release(mController);
                    mController = mAsteroidDestructionControllerPool.Get();
                    gameData.PlayerState.Score += 50;
                    break;
                case Type.Apophis_Asteroid:
                    mApophisAsteroidControllerPool.Release(mController);
                    mController = mAsteroidDestructionControllerPool.Get();
                    gameData.PlayerState.Score += 100;
                    break;
                case Type.Bonus_Life:
                case Type.Bonus_Invincibility:
                case Type.Bonus_Bomb:
                case Type.Bonus_Energy:
                    mAsteroidControllerPool.Release(mController);
                    gameData.PlayerState.Score += 10;
                    mController = mBonusDestructionControllerPool.Get();
                    break;
            }

            AddGameObject(mController);
        }

        /// <summary>
        /// Restaura el estado del objeto.
        /// </summary>
        public void Reset()
        {
            HideAsteroid();

            if (mController != null)
            {
                switch (mObjectType)
                {
                    case Type.Asteroid_Regular:
                    case Type.Apophis_Asteroid:
                        if (mController.State is AsteroidDestructionController)
                        {
                            AsteroidDestructionController destructionController = (AsteroidDestructionController)mController.State;
                            destructionController.OnExit();
                        }
                        RemoveGameObject(mController);
                        mAsteroidDestructionControllerPool.Release(mController);
                        break;
                    case Type.Bonus_Life:
                    case Type.Bonus_Invincibility:
                    case Type.Bonus_Bomb:
                    case Type.Bonus_Energy:
                        RemoveGameObject(mController);
                        mBonusDestructionControllerPool.Release(mController);
                        break;
                }
            }
        }
    }
}
