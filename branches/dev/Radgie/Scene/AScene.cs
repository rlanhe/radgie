using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Radgie.Util.Collection.Context;
using Radgie.Core.BoundingVolumes;
using Radgie.Core.Collision;

namespace Radgie.Scene
{
	/// <summary>
	/// Abstraccion de una escena.
    /// Una escena contiene todos los elementos que intervienen en el juego. Permite optimizar la gestion de la posicion de los distintos
    /// objetos en la escena, asi como facilitar el calculo de colisiones, activar/desactivar la actualizacion de elementos, mostrar/ocultar 
    /// elementos del juego, etc.
	/// </summary>
	public abstract class AScene<T>: IScene where T: ASceneNode<T>
	{
		#region Properties
		#region IScene Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IActivable.Active"/>
        /// </summary>
		public bool Active { get; set; }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IIdentifiable.Id"/>
        /// </summary>
		public string Id
		{
			get
			{
				return mId;
			}
		}
		private string mId;

        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext"/>
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
        /// Ver <see cref="Radgie.Core.IScene.CollisionsGroups"/>
        /// </summary>
        public List<ICollisionGroup> CollisionGroups
        {
            get
            {
                if (mCollisionGroups != null)
                {
                    return (List<ICollisionGroup>)mCollisionGroups.Values;
                }
                return null;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.IScene.UpdateDelegate"/>
        /// </summary>
        public Job UpdateJob
        {
            get
            {
                return mUpdateDelegate;
            }
        }
        protected Job mUpdateDelegate;
		#endregion

		/// <summary>
		/// Nodo raiz de la escena.
		/// </summary>
		protected T mRoot;

        /// <summary>
        /// Lista de grupos de colision de la escena.
        /// </summary>
        protected IDictionary<string, ICollisionGroup> mCollisionGroups;
        
		#endregion

		#region Constructors
		/// <summary>
		/// Construye una nueva escena.
		/// </summary>
		/// <param name="id">Identificador de la escena.</param>
		public AScene(string id)
		{
			mId = id;
			Active = false;
            mContext = new Context(this);
            mUpdateDelegate = new Job(delegate(){ this.Update(null); });
		}
		#endregion

		#region Methods
		#region IScene Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
		public virtual void Update(GameTime time)
		{
            if (Active)
			{
				mRoot.Update(time);
			}
		}

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.AddComponent"/>
        /// </summary>
        public virtual bool AddComponent(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if ((gc.Scene != null) && (gc.Scene != this))
            {
                gc.Scene.RemoveComponent(gc);
            }
            
            // Si el GC estaba annadido a otro GC, se quita de la lista de hijos de su padre.
            // TODO: esta comprobacion no es necesaria
            if (gc.Parent != null)
            {
                gc.Parent.RemoveGameComponent(gc);
            }
            return mRoot.AddComponent(gc);
		}

        /// <summary>
        /// Ver <see cref="Radgie.Scene.IScene.CanCollide"/>
        /// </summary>
        public bool CanCollide(ISceneNode node1, ISceneNode node2)
        {
            // TODO: cache
            if ((node1 == node2) || ((node1 == mRoot) || (node2 == mRoot)) || (node1.Parent == node2.Parent) || ((IsParentOfNode(node1, node2)) ||(IsParentOfNode(node2, node1))))
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Determina si un nodo es padre de otro.
        /// </summary>
        /// <param name="node1">Nodo 1.</param>
        /// <param name="node2">Nodo 2.</param>
        /// <returns>True si lo es, False en caso contrario.</returns>
        private static bool IsParentOfNode(ISceneNode node1, ISceneNode node2)
        {
            if (node2 == node1)
            {
                return true;
            }

            ISceneNode parentNode2 = node2.Parent;
            if (parentNode2 == null)
            {
                return false;
            }

            return IsParentOfNode(node1, parentNode2);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.RemoveComponent"/>
        /// </summary>
        public bool RemoveComponent(Radgie.Core.IGameComponent gc)
		{
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if (mCollisionGroups != null)
            {
                // Elimina el GC de todos los grupos de colision en los que este.
                foreach (KeyValuePair<string, ICollisionGroup> tuple in mCollisionGroups)
                {
                    tuple.Value.RemoveGameComponent(gc);
                }
            }

            return mRoot.RemoveComponent(gc);
		}

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameObjects"/>
        /// </summary>
        public List<Y> GetGameObjects<Y>(bool includeInvisibleObjects)
        {
            return GetGameObjects<Y>(null, includeInvisibleObjects);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(bool includeInvisibleObjects, List<Y> results)
        {
            results.Clear();
            GetGameObjects<Y>(null, includeInvisibleObjects, results);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameObjects"/>
        /// </summary>
        public List<Y> GetGameObjects<Y>(IBoundingVolume frustum, bool includeInvisibleObjects)
        {
            List<Y> results = new List<Y>();

            this.GetGameObjects(frustum, includeInvisibleObjects, results);

            return results;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(IBoundingVolume frustum, bool includeInvisibleObjects, List<Y> results)
        {
            results.Clear();
            mRoot.GetGameObjects<Y>(frustum, results, includeInvisibleObjects);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IActivable.IsActive"/>
        /// </summary>
        public bool IsActive()
        {
            return Active;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameComponents"/>
        /// </summary>
        public List<Radgie.Core.IGameComponent> GetGameComponents(bool includeInvisibleObjects)
        {
            return GetGameComponents(null, includeInvisibleObjects);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameComponents"/>
        /// </summary>
        public void GetGameComponents(bool includeInvisibleObjects, List<Core.IGameComponent> results)
        {
            results.Clear();
            GetGameComponents(null, includeInvisibleObjects, results);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameComponents"/>
        /// </summary>
        public List<Radgie.Core.IGameComponent> GetGameComponents(IBoundingVolume frustum, bool includeInvisibleObjects)
        {
            List<Radgie.Core.IGameComponent> list = new List<Radgie.Core.IGameComponent>();

            this.GetGameComponents(frustum, includeInvisibleObjects, list);

            return list;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetGameComponents"/>
        /// </summary>
        public void GetGameComponents(IBoundingVolume frustum, bool includeInvisibleObjects, List<Core.IGameComponent> results)
        {
            results.Clear();
            mRoot.GetGameComponents(frustum, results, includeInvisibleObjects);
        }        

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.AddNewCollisionGroup"/>
        /// </summary>
        public ICollisionGroup AddNewCollisionGroup(string id)
        {
            if (mCollisionGroups == null)
            {
                mCollisionGroups = new Dictionary<string, ICollisionGroup>();
            }

            ICollisionGroup cGroup = GetCollisionGroup(id);
            if (cGroup == null)
            {
                cGroup = new CollisionGroup(this, id);
                mCollisionGroups.Add(id, cGroup);
                return cGroup;
            }
            return null;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.GetCollisionGroup"/>
        /// </summary>
        public ICollisionGroup GetCollisionGroup(string id)
        {
            if (mCollisionGroups != null)
            {
                ICollisionGroup cGroup;
                mCollisionGroups.TryGetValue(id, out cGroup);
                return cGroup;
            }
            return null;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IScene.RemoveCollisionGroup"/>
        /// </summary>
        public void RemoveCollisionGroup(ICollisionGroup collisionGroup)
        {
            if (mCollisionGroups != null)
            {
                mCollisionGroups.Remove(collisionGroup.Id);
            }
        }

		#endregion

        #region IHasContext Methods

        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext"/>
        /// </summary>
        public Variable GetFromContext(string id)
        {
            return Context.Get(id);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext<T>"/>
        /// </summary>
        public Y GetFromContext<Y>(string id)
        {
            return Context.Get<Y>(id);
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
        public void SetInContext<Y>(string id, Y value)
        {
            Context.Set<Y>(id, value);
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
