using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Util.Collection.Context;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Core
{
    /// <summary>
    /// Clase base para un GameComponent.
    /// Un GameComponent es un objeto que representa una posicion en el mundo, dentro de la escena. Puede contener GameObjects (graficos, sonidos, etc). 
    /// Todos los GameObjects de un GameComponent se consideran que ocupan el mismo espacio que este.
    /// 
    /// A su vez, tambien puede contener otros GameComponents, lo que posibilita crear jerarquias de GameComponents para representar los elementos del juego.
    /// </summary>
    public class GameComponent: IGameComponent
    {
        #region Properties

        #region IGameComponet Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IActivable.Active"/>
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.Scene"/>
        /// </summary>
        public IScene Scene
        {
            get
            {
                return mScene;
            }
            internal set
            {
                if (mScene != value)
                {
                    mScene = value;
                    
                    if (mChilds != null)
                    {
                        // Establece en todos los GameComponents hijos la escena a la que pertenecen.
                        foreach(IGameComponent gc in mChilds)
                        {
                            ((GameComponent)gc).Scene = value;
                        }
                    }
                }
            }
        }
        protected IScene mScene;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.Parent"/>
        /// </summary>
        public IGameComponent Parent
        {
            get
            {
                return mParent;
            }
        }
        private IGameComponent mParent;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.World"/>
        /// </summary>
        public Matrix World
        {
            get
            {
                return mWorldTransformation.Matrix;
            }
        }
        protected Transformation mWorldTransformation;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.Transformation"/>
        /// </summary>
        public Transformation Transformation
        {
            get
            {
                return mTransformation;
            }
            set
            {
                if (!mTransformation.Equals(value))
                {
                    mTransformation.Set(value);
                    // Debe actualizar su transformacion de mundo.
                    mTransformUpdate = true;
                }
            }
        }
        protected Transformation mTransformation;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.GameObjects"/>
        /// </summary>
        public IEnumerator<IGameObject> GameObjects
        {
            get
            {
                if (mGameObjects != null)
                {
                    return mGameObjects.GetEnumerator();
                }
                return mEmptyGameObjects.GetEnumerator();
            }
        }
        protected List<IGameObject> mGameObjects;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.GameComponents"/>
        /// </summary>
        public IEnumerator<IGameComponent> GameComponents
        {
            get
            {
                if (mChilds != null)
                {
                    return mChilds.GetEnumerator();
                }
                return mEmptyGameComponents.GetEnumerator();
            }
        }

        /// <summary>
        /// Identificador del GameComponent.
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }
        private string mId;

        /// <summary>
        /// Contexto del GameComponent
        /// </summary>
        public IContext Context
        {
            get
            {
                return mContext;
            }
        }
        private IContext mContext;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.BoundingVolume"/>
        /// </summary>
        public IBoundingVolume BoundingVolume
        {
            get
            {
                return mBoundingVolume;
            }
            set
            {
                mBoundingVolume = value;
            }
        }
        protected IBoundingVolume mBoundingVolume;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.Visible"/>
        /// </summary>
        public bool Visible
        {
            get
            {
                return mVisible;
            }
            set
            {
                mVisible = value;
            }
        }
        protected bool mVisible;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.ParentNode"/>
        /// </summary>
        public ISceneNode ParentNode
        {
            get
            {
                if (mParent == null)
                {
                    return mParentNode;
                }
                else
                {
                    return mParent.ParentNode;
                }
            }
            internal set
            {
                mParentNode = value;
            }
        }
        protected ISceneNode mParentNode;
        #endregion

        /// <summary>
        /// Indica si el estado del componente cambio durante la ultima actualizacion. Si esta a true, se notifica a los observers.
        /// </summary>
        protected bool mStateChanged;

        /// <summary>
        /// Indica si la posicion del componente cambio durante la ultima actualizacion. Si esta a true, se notifica a los observers.
        /// </summary>
        protected bool mTransformUpdate;

        /// <summary>
        /// Lista de objetos observers de este GameComponent.
        /// </summary>
        private List<IGameComponentObserver> mObservers;

        /// <summary>
        /// Lista de GameComponents hijos.
        /// </summary>
        protected List<IGameComponent> mChilds;

        /// <summary>
        /// Lista de GameObjects vacia.
        /// Se utiliza para devolver un Enumerator vacio cuando el GameComponent no tiene GameObjects.
        /// </summary>
        private static List<IGameObject> mEmptyGameObjects = new List<IGameObject>();
        /// <summary>
        /// Lista de GameComponents vacia.
        /// Se utiliza para devolver un Enumerator vacio cuando el GameComponent no tiene GameComponents hijos.
        /// </summary>
        private static List<IGameComponent> mEmptyGameComponents = new List<IGameComponent>();

        /// <summary>
        /// Flag para determinar si ya se llamo al metodo Initialize.
        /// </summary>
        private bool mInitialized;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo GameComponent.
        /// </summary>
        public GameComponent(string id)
        {
            mId = id;
            Active = true;
            mTransformUpdate = false;
            mTransformation = new Transformation();
            mWorldTransformation = new Transformation();
            mContext = new Context(this);
            Visible = true;
            mInitialized = false;
        }

        /// <summary>
        /// Crea un nuevo GameComponent.
        /// </summary>
        public GameComponent():this("")
        {
        }
        #endregion

        #region Methods

        #region IGameComponent Members

        #region IUpdateable Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public virtual void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (Active)
            {
                if (!mInitialized)
                {
                    Initialize();
                    mInitialized = true;
                }

                CalculateTransformation();
                CalculateWorldTransformation();

                if (mChilds != null)
                {
                    foreach (IGameComponent child in mChilds)
                    {
                        child.Update(time);
                    }
                }

                // TODO: Bounding Volume deberia de representar el volumen de todos los GameComponents hijos??
                if (mBoundingVolume != null)
                {
                    Matrix world = World;
                    mBoundingVolume.Update(ref world);
                }

                if (mStateChanged)
                {
                    // Informa a todos los objetos que tiene como observers de un cambio en su estado.
                    if (mObservers != null)
                    {
                        for (int i = mObservers.Count - 1; i >= 0; i--)
                        {
                            mObservers.ElementAt(i).NotifyUpdateFromComponent(this);
                        }
                    }
                    mStateChanged = false;
                }
            }
        }

        #endregion

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.AddObserver"/>
        /// </summary>
        public void AddObserver(IGameComponentObserver observer)
        {
            if (mObservers == null)
            {
                mObservers = new List<IGameComponentObserver>();
            }

            if (!mObservers.Contains(observer))
            {
                mObservers.Add(observer);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.RemoveObserver"/>
        /// </summary>
        public void RemoveObserver(IGameComponentObserver observer)
        {
            if (mObservers != null)
            {
                mObservers.Remove(observer);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.AddGameComponent"/>
        /// </summary>
        public bool AddGameComponent(IGameComponent child)
        {
            GameComponent childGC = (GameComponent)child;
            if (mChilds == null)
            {
                mChilds = new List<IGameComponent>();
            }

            if (childGC.mParent != null)
            {
                childGC.mParent.RemoveGameComponent(childGC);
            }

            if (!mChilds.Contains(childGC))
            {
                mChilds.Add(childGC);
                childGC.SetParent(this);
                childGC.Scene = Scene;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.RemoveGameComponent"/>
        /// </summary>
        public bool RemoveGameComponent(IGameComponent child)
        {
            GameComponent childGC = (GameComponent)child;
            if (mChilds != null)
            {
                bool result = mChilds.Remove(childGC);
                if (result)
                {
                    childGC.SetParent(null);
                    childGC.Scene = null;
                }
                return result;
            }
            return false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.AddGameObject"/>
        /// </summary>
        public bool AddGameObject(IGameObject gameObject)
        {
            if (mGameObjects == null)
            {
                mGameObjects = new List<IGameObject>();
            }

            if (!mGameObjects.Contains(gameObject))
            {
                if (gameObject.Component != null)
                {
                    gameObject.Component.RemoveGameObject(gameObject);
                }
                mGameObjects.Add(gameObject);
                gameObject.Component = this;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.RemoveGameObject"/>
        /// </summary>
        public bool RemoveGameObject(IGameObject gameObject)
        {
            if (mGameObjects != null)
            {
                if (mGameObjects.Contains(gameObject))
                {
                    bool result = mGameObjects.Remove(gameObject);
                    gameObject.Component = null;
                    return result;
                }
            }
            return false;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(List<Y> results, bool includeInvisibleObjects)
        {
            if ((includeInvisibleObjects) || (Visible))
            {
                if (mChilds != null)
                {
                    foreach (IGameComponent gComponent in mChilds)
                    {
                        gComponent.GetGameObjects<Y>(results, includeInvisibleObjects);
                    }
                }

                if (mGameObjects != null)
                {
                    foreach (IGameObject gObject in mGameObjects)
                    {
                        if (gObject is Y)
                        {
                            results.Add((Y)gObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(IBoundingVolume frustum, List<Y> results, bool includeInvisibleObjects)
        {
            if ((includeInvisibleObjects) || (Visible))
            {
                if (frustum != null)
                {
                    if (mChilds != null)
                    {
                        foreach (IGameComponent gComponent in mChilds)
                        {
                            if (gComponent.BoundingVolume != null)
                            {
                                if (frustum.Contains(gComponent.BoundingVolume) != ContainmentType.Disjoint)
                                {
                                    gComponent.GetGameObjects<Y>(frustum, results, includeInvisibleObjects);
                                }
                            }
                            else
                            {
                                gComponent.GetGameObjects<Y>(results, includeInvisibleObjects);
                            }
                        }
                    }

                    if ((mGameObjects != null) && (includeInvisibleObjects || Visible))
                    {
                        foreach (IGameObject gObject in mGameObjects)
                        {
                            if (gObject is Y)
                            {
                                results.Add((Y)gObject);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameComponent"/>
        /// </summary>
        public virtual void Initialize()
        {
        }

        #region IActivable Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IActivable.IsActive"/>
        /// </summary>
        public bool IsActive()
        {
            return Active && (Scene == null ? false : Scene.IsActive()) && (Parent == null ? true : Parent.IsActive());
        }
        #endregion
        #endregion

        /// <summary>
        /// Calcula la transformacion local del GameComponent.
        /// </summary>
        private void CalculateTransformation()
        {
            mTransformation.Update();
        }

        /// <summary>
        /// Calcula la transformacion de mundo del GameComponent.
        /// </summary>
        private void CalculateWorldTransformation()
        {
            Matrix lastWorld = mWorldTransformation.Matrix;
            
            if (Parent == null)
            {
                // Root node
                mWorldTransformation.Set(mTransformation);
            }
            else
            {
                Matrix worldMatrix = mTransformation.Matrix * Parent.World;
                mWorldTransformation.Init(ref worldMatrix);
            }

            if (lastWorld != mWorldTransformation.Matrix)
            {
                mStateChanged = true;
            }
        }

        /// <summary>
        /// Notifica un cambio de GameComponent padre a los objetos observers.
        /// </summary>
        private void NotifyParentChangeToObservers()
        {
            if (mObservers != null)
            {
                // Se recorre al reves ya que puede que se modifique la lista de observers durante la actualizacion de los mismos.
                for (int i = mObservers.Count - 1; i >= 0; i--)
                {
                    IGameComponentObserver observer = mObservers[i];
                    observer.NotifyParentChangeFromComponent(this);
                }
            }
        }

        /// <summary>
        /// Establece el GameComponent padre.
        /// </summary>
        /// <param name="parent">Component padre.</param>
        protected void SetParent(IGameComponent parent)
        {
            mParent = parent;
            mStateChanged = true;// Marca tambien el estado, ya que al cambiar de padre es posible que tambien lo haga su posicion.
            NotifyParentChangeToObservers();
        }

        #region IHasContext Methods

        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext"/>
        /// </summary>
        public Variable GetFromContext(string id)
        {
            Variable result = null;

            result = Context.Get(id);
            if (result == null)
            {
                if (Parent != null)
                {
                    result = Parent.GetFromContext(id);
                }
                else
                {
                    result = Scene.GetFromContext(id);
                }
            }

            return result;
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext<T>"/>
        /// </summary>
        public T GetFromContext<T>(string id)
        {
            T result = default(T);

            result = Context.Get<T>(id);
            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                if (Parent != null)
                {
                    result = Parent.GetFromContext<T>(id);
                }
                else
                {
                    result = Scene.GetFromContext<T>(id);
                }
            }

            return result;
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext"/>
        /// </summary>
        public void SetInContext(string id, Variable value)
        {
            Context.Set(id, value);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext<T>"/>
        /// </summary>
        public void SetInContext<T>(string id, T value)
        {
            Context.Set<T>(id, value);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.RemoveFromContext"/>
        /// </summary>
        public bool RemoveFromContext(string id)
        {
            return Context.Remove(id);
        }
        #endregion

        #endregion
    }
}
