using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.State;
using AsteroidsStorm.GameComponents.Spaceship.States;
using Radgie.File;
using System.Xml;
using Radgie.Graphics;
using Radgie.Graphics.Entity;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using Radgie.Sound;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Util.Collection.Context;
using System.Xml.Linq;
using System.IO;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship
{
    /// <summary>
    /// Nave espacial.
    /// </summary>
    class Spaceship : Radgie.Core.GameComponent
    {
        private const string CONTROLLER = "Controller";
        private const string ID = "Id";
        private const string MODEL = "Model";
        private const string PARTICLE_SYSTEM = "ParticleSystem";
        private const string SCALE = "Scale";
        private const string ROTATION = "Rotation";
        private const string CENTER = "Center";
        private const string ENGINE_SOUND = "EngineSound";
        
        /// <summary>
        /// Factor de giro de la nave.
        /// </summary>
        public float Steer
        {
            get
            {
                return mSteer;
            }
            private set
            {
                mSteer = value;
            }
        }
        private float mSteer;
        public const string STEER = "Steer";

        /// <summary>
        /// Factor de armadura de la nave.
        /// </summary>
        public float Armor
        {
            get
            {
                return mArmor;
            }
            private set
            {
                mArmor = value;
            }
        }
        private float mArmor;
        public const string ARMOR = "Armor";
        
        /// <summary>
        /// Limites en el movimiento de la nave.
        /// </summary>
        public Vector4 PositionLimits
        {
            get
            {
                return GetFromContext<Vector4>(POSITION_LIMITS);
            }
            set
            {
                SetInContext(POSITION_LIMITS, value);
            }
        }
        public const string POSITION_LIMITS = "PositionLimits";

        /// <summary>
        /// Velocidad de la nave.
        /// </summary>
        public float Speed
        {
            get
            {
                return GetFromContext<float>(SPEED);
            }
            set
            {
                SetInContext(SPEED, value);
            }
        }

        private float mScale = 1.0f;

        private SoundEffect mEngineEffect;

        private SpaceshipStateMachine mStateMachine;

        /// <summary>
        /// Id de la variable con la lista de observadores de la nave.
        /// </summary>
        public const string OBSERVERS = "Observers";

        private Radgie.Util.Collection.ObserverList.ObserverList<Vector3> mObservers;

        private Radgie.Core.IGameComponent mSpaceshipModel;
        private List<Radgie.Core.IGameComponent> mPsChilds;
        /// <summary>
        /// Crea la nave espacial.
        /// </summary>
        /// <param name="id">Identificador de la nave.</param>
        /// <param name="controller">Controlador de la nave.</param>
        /// <param name="configFile">Fichero de configuracion de la nave.</param>
        /// <param name="controlled">Indica si la nave va a ser controlada por el jugador.</param>
        public Spaceship(string id, ISpaceshipController controller, string configFile, bool controlled):this(id, controller, configFile, controlled, Vector4.Zero)
        {
        }

        /// <summary>
        /// Crea la nave espacial.
        /// </summary>
        /// <param name="id">Identificador de la nave.</param>
        /// <param name="controller">Controlador de la nave.</param>
        /// <param name="configFile">Fichero de configuracion de la nave.</param>
        /// <param name="controlled">Indica si la nave va a ser controlada por el jugador.</param>
        /// <param name="positionLimits">Limites de posicion de la nave.</param>
        public Spaceship(string id, ISpaceshipController controller, string configFile, bool controlled, Vector4 positionLimits): base(id)
        {
            InitFromConfigFile(configFile, controlled);

            mSpaceshipModel.Transformation.Scale = new Vector3(mScale, mScale, mScale);

            if (controlled)
            {
                Context.Set(CONTROLLER, controller);
                mStateMachine = new SpaceshipStateMachine(null);
                AddGameObject(new Behaviour(mStateMachine));
                AddGameObject(new Behaviour(new SpaceshipMovementController(null)));
                mObservers = new Radgie.Util.Collection.ObserverList.ObserverList<Vector3>();
                SetInContext(OBSERVERS, mObservers);
                PositionLimits = positionLimits;
            }
        }

        /// <summary>
        /// Annade un observador el movimiento de la nave.
        /// </summary>
        /// <param name="observer">Nuevo observer.</param>
        /// <param name="delegateMethod"></param>
        public void AddObserver(object observer, Radgie.Util.Collection.ObserverList.ObserverList<Vector3>.NotifyObserverDelegate delegateMethod)
        {
            mObservers.AddObserver(observer, delegateMethod);
        }

        /// <summary>
        /// Quita un observador de la nave.
        /// </summary>
        /// <param name="observer">Observer a quitar.</param>
        public void RemoveObserver(object observer)
        {
            mObservers.RemoveObserver(observer);
        }

        /// <summary>
        /// Desencadena el efecto de colisionar la nave con un SpaceObject.
        /// </summary>
        /// <param name="asteroid">SpaceObject con el que ha colisionado la nave.</param>
        public void CollideWithAsteroid(SpaceObject asteroid)
        {
            mStateMachine.CollideWithAsteroid(asteroid);
        }

        /// <summary>
        /// Apaga los motores de la nave.
        /// </summary>
        public void TurnOff()
        {
            foreach (Radgie.Core.IGameComponent gc in mPsChilds)
            {
                gc.Active = false;
            }
            if (mEngineEffect != null)
            {
                mEngineEffect.Stop(true);
            }
        }

        /// <summary>
        /// Enciende los motores de la nave.
        /// </summary>
        public void TurnOn()
        {
            foreach (Radgie.Core.IGameComponent gc in mPsChilds)
            {
                gc.Active = true;
            }
            if (mEngineEffect != null)
            {
                mEngineEffect.Play();
            }
        }

        /// <summary>
        /// Inicializa la configuracion de la nave desde fichero.
        /// </summary>
        /// <param name="file">Fichero con la configuracion de la nave.</param>
        /// <param name="controlled">Indica si va a ser controlada por el jugador.</param>
        private void InitFromConfigFile(string file, bool controlled)
        {
            XmlFile configFile = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<XmlFile>(file);
            XDocument doc = XDocument.Load(new StringReader(configFile.Content));
            mSpaceshipModel = new Radgie.Core.GameComponent("ShipModel");
            AddGameComponent(mSpaceshipModel);
            //AddGameComponent(new Axes.Axes());

            mPsChilds = new List<Radgie.Core.IGameComponent>();

            XElement modelElement = doc.Root.Element(MODEL);
            if (modelElement != null)
            {
                SimpleModel model = new SimpleModel(Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Mesh>(modelElement.Value));
                mSpaceshipModel.AddGameObject(model);
                BoundingVolume = new Radgie.Core.BoundingVolumes.BoundingSphere(new Vector3(0.0f, 0.0f, 0.0f), 0.5f);
                SetInContext(MODEL, model);
            }

            XElement steerElement = doc.Root.Element(STEER);
            if (steerElement != null)
            {
                Steer = XmlFileReader.GetFloat(steerElement);
            }

            XElement armorElement = doc.Root.Element(ARMOR);
            if (armorElement != null)
            {
                Armor = XmlFileReader.GetFloat(armorElement);
            }

            XElement engineSoundElement = doc.Root.Element(ENGINE_SOUND);
            if (engineSoundElement != null)
            {
                mEngineEffect = new SoundEffect(engineSoundElement.Value);
                mEngineEffect.Is3D = true;
                mEngineEffect.IsLooped = true;
                mEngineEffect.Volume = 0.03f;
                AddGameObject(mEngineEffect);
            }

            XElement scaleElement = doc.Root.Element(SCALE);
            if (scaleElement != null)
            {
                mScale = XmlFileReader.GetFloat(scaleElement);
            }
            
            var particleElements = doc.Root.Elements(PARTICLE_SYSTEM);
            foreach (var particleElement in particleElements)
            {
                if (controlled)
                {
                    GetParticleSystem(particleElement);
                }
            }
            
            XElement rotationElement = doc.Root.Element(ROTATION);
            if (rotationElement != null)
            {
                mSpaceshipModel.Transformation.Rotation *= XmlFileReader.GetQuaternion(rotationElement);
            }
            
            XElement centerElement = doc.Root.Element(CENTER);
            if (centerElement != null)
            {
                Transformation.Center = XmlFileReader.GetVector3(centerElement);
            }
        }
        /// <summary>
        /// Id de la seccion de parametros de la nave.
        /// </summary>
        public const string SETTINGS = "Settings";
        /// <summary>
        /// Id de la seccion de emisores de particulas.
        /// </summary>
        public const string EMITTER = "Emitter";
        /// <summary>
        /// Id de la seccion con la posicion de las particulas..
        /// </summary>
        public const string POSITION = "Position";
        /// <summary>
        /// Id de la seccion de la velocidad de las particulas.
        /// </summary>
        public const string SPEED = "Speed";

        /// <summary>
        /// Crea un sistema de paticulas a partir de un nodo xml del fichero de configuracion.
        /// </summary>
        /// <param name="node">Nodo del fichero xml.</param>
        public void GetParticleSystem(XElement node)
        {
            ParticleSystem pSystem = null;
            Vector3 gcPosition = Vector3.Zero;
            Vector3 position = Vector3.Zero;
            float speed = 0.0f;

            XElement settingsElement = node.Element(SETTINGS);
            if (settingsElement != null)
            {
                ParticleSystemSettings settings = RadgieGame.Instance.ResourceManager.Load<ParticleSystemSettings>(settingsElement.Value, false);
                pSystem = new ParticleSystem(settings);
            }

            XElement positionElement = node.Element(POSITION);
            if (positionElement != null)
            {
                gcPosition = XmlFileReader.GetVector3(positionElement);
            }

            XElement element = node.Element(EMITTER);
            if (element != null)
            {
                foreach (XElement child in element.Nodes())
                {
                    switch (child.Name.LocalName)
                    {
                        case POSITION:
                            position = XmlFileReader.GetVector3(child);
                            break;
                        case SPEED:
                            speed = XmlFileReader.GetFloat(child);
                            break;
                    }
                }
            }

            ParticleEmitter emitter = new ParticleEmitter(pSystem, speed, position);

            Radgie.Core.GameComponent childGC = new Radgie.Core.GameComponent("");
            childGC.AddGameObject(emitter);
            childGC.AddGameObject(pSystem);
            AddGameComponent(childGC);
            childGC.Transformation.Translation = gcPosition;

            mPsChilds.Add(childGC);
        }
    }
}
