using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Scene
{
	public abstract class ASceneNode<T>: IComplexSceneNode<ASceneNode<T>> where T: ASceneNode<T>
	{
		#region Properties
        /// <summary>
        /// Escena a la que pertenece el nodo.
        /// </summary>
        protected AScene<T> mScene;
		/// <summary>
		/// Nodo padre.
		/// Si es null, se trata del nodo root de una escena.
		/// </summary>
        public ISceneNode Parent
        {
            get
            {
                return mParent;
            }
        }
		protected T mParent;
		/// <summary>
		/// Nodos hijos.
		/// </summary>
		protected List<T> mChilds;

		/// <summary>
		/// Componentes del juego que cuelgan de este nodo
		/// </summary>
        protected List<Radgie.Core.IGameComponent> mComponents;
        
        /// <summary>
        /// Posicion del nodo de escena.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
        }
        protected Vector3 mPosition;

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode"/>
        /// </summary>
        public int ComponentsCount
        {
            get
            {
                int count = 0;

                if (mComponents != null)
                {
                    count += mComponents.Count;
                }

                if (mChilds != null)
                {
                    foreach (ASceneNode<T> child in mChilds)
                    {
                        count += child.ComponentsCount;
                    }
                }

                return count;
            }
        }
		#endregion

		#region Constructors
		/// <summary>
		/// Crea un nodo de escena.
		/// </summary>
        /// <param name="scene">Escena a la que pertenece este nodo</param>
		/// <param name="parent">Padre del nodo, null en caso de ser el nodo root de la escena.</param>
		public ASceneNode(AScene<T> scene, T parent)
		{
            if (scene == null)
            {
                throw new ArgumentNullException("Scene is null");
            }
            mScene = scene;
			mParent = parent;
		}
		#endregion

		#region Methods
		#region IComplexSceneNode
        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
		public virtual void Update(GameTime time)
		{
			if (mComponents != null)
			{
                // Esta actualizacion puede dar lugar a que un GameComponent se pase a otro nodo, por lo que no se puede recorrer a traves de un iterador
                for (int i = mComponents.Count - 1; i >= 0; i--)
                {
                    mComponents.ElementAt(i).Update(time);
                }
			}

			if (mChilds != null)
			{
				foreach (T node in mChilds)
				{
					node.Update(time);
				}
			}
		}

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.AddComponent"/>
        /// </summary>
        public bool AddComponent(Radgie.Core.IGameComponent gc)
		{
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if (CouldBePlacedGCIntoNode(gc))
            {
                bool inserted = false;
                // Trata de insertarlo primero en los hijos
                if (mChilds != null)
                {
                    foreach (T child in mChilds)
                    {
                        inserted = child.AddComponent(gc);
                        if (inserted)
                        {
                            break;
                        }
                    }
                }

                // Si no pudo insertarlo en sus hijos, hay que insertarlo en el propio nodo
                if (!inserted)
                {
                    if (mComponents == null)
                    {
                        mComponents = new List<Radgie.Core.IGameComponent>();
                    }

                    mComponents.Add(gc);
                    Radgie.Core.GameComponent tmpGC = (Radgie.Core.GameComponent)gc;
                    tmpGC.Scene = mScene;
                    tmpGC.ParentNode = this;
                    // Pone al nodo como observer del GameComponent
                    gc.AddObserver(this);
                }
                return true;
            }
            else
            {
                return false;
            }
		}

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.RemoveComponent"/>
        /// </summary>
        public bool RemoveComponent(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            bool result = RemoveComponentFromComponentsList(gc);

            // Si el nodo no tiene el component, busca en los hijos recursivamente
            if (!result)
            {
                result = RemoveComponentFromChilds(gc);
            }
            else
            {
                Radgie.Core.GameComponent tmpGC = (Radgie.Core.GameComponent)gc;
                tmpGC.Scene = null;
                tmpGC.ParentNode = null;
            }
            return result;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.NotifyUpdateFromComponent"/>
        /// </summary>
        public void NotifyUpdateFromComponent(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            RemoveComponentFromComponentsList(gc);
            mScene.AddComponent(gc);
            /*
            bool isInNode = CouldBePlacedGCIntoNode(gc);
            if (!isInNode)
            {
                
            }
            */
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.NotifyParentChangeFromComponent"/>
        /// </summary>
        public void NotifyParentChangeFromComponent(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if (gc.Parent != null)
            {
                RemoveComponentFromComponentsList(gc);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(IBoundingVolume frustum, List<Y> results, bool includeInvisibleObjects)
        {
            if ((frustum == null) || (IsNodeVisible(frustum)))
            {
                if (mChilds != null)
                {
                    foreach (ISceneNode node in mChilds)
                    {
                        node.GetGameObjects<Y>(frustum, results, includeInvisibleObjects);
                    }
                }

                if (mComponents != null)
                {
                    foreach (Radgie.Core.IGameComponent gc in mComponents)
                    {
                        if (/*gc.IsActive() && */(includeInvisibleObjects || gc.Visible))
                        {
                            if ((frustum != null) && (gc.BoundingVolume != null))
                            {
                                if (frustum.Contains(gc.BoundingVolume) != ContainmentType.Disjoint)
                                {
                                    gc.GetGameObjects<Y>(frustum, results, includeInvisibleObjects);
                                }
                            }
                            else
                            {
                                gc.GetGameObjects<Y>(results, includeInvisibleObjects);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.GetGameObjects"/>
        /// </summary>
        public void GetGameObjects<Y>(List<Y> results, bool includeInvisibleObjects)
        {
            GetGameObjects<Y>(null, results, includeInvisibleObjects);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.GetGameComponents"/>
        /// </summary>
        public void GetGameComponents(IBoundingVolume frustum, List<Radgie.Core.IGameComponent> results, bool includeInvisibleObjects)
        {
            if ((frustum == null) || (IsNodeVisible(frustum)))
            {
                if (mChilds != null)
                {
                    foreach (ISceneNode node in mChilds)
                    {
                        node.GetGameComponents(frustum, results, includeInvisibleObjects);
                    }
                }

                if (mComponents != null)
                {
                    foreach (Radgie.Core.IGameComponent gc in mComponents)
                    {
                        if (/*gc.IsActive() && */(includeInvisibleObjects || gc.Visible))
                        {
                            if ((frustum != null) && (gc.BoundingVolume != null))
                            {
                                if (frustum.Contains(gc.BoundingVolume) != ContainmentType.Disjoint)
                                {
                                    results.Add(gc);
                                }
                            }
                            else
                            {
                                results.Add(gc);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.GetGameComponents"/>
        /// </summary>
        public void GetGameComponents(List<Radgie.Core.IGameComponent> results, bool includeInvisibleObjects)
        {
            GetGameComponents(null, results, includeInvisibleObjects);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneNode.IsNodeVisible"/>
        /// </summary>
        public abstract bool IsNodeVisible(IBoundingVolume frustum);
        #endregion

        /// <summary>
        /// Quita un GameComponent de la lista de components del nodo
        /// </summary>
        /// <param name="gc">GameComponent</param>
        /// <returns>True si lo elimina, False en caso contrario</returns>
        private bool RemoveComponentFromComponentsList(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if (mComponents != null)
            {
                if (mComponents.Contains(gc))
                {
                    Radgie.Core.GameComponent tmpGC = (Radgie.Core.GameComponent)gc;
                    mComponents.Remove(tmpGC);
                    // El nodo ya no necesita estar de observador del GameComponent
                    tmpGC.RemoveObserver(this);
                    tmpGC.ParentNode = null;
                    //((Radgie.Core.GameComponent)gc).Scene = null;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Quita un component de uno de los hijos de este nodo.
        /// </summary>
        /// <param name="gc">GameComponent</param>
        /// <returns>True si lo encuentra y lo elimina, False en caso contrario.</returns>
        private bool RemoveComponentFromChilds(Radgie.Core.IGameComponent gc)
        {
            if (gc == null)
            {
                throw new ArgumentNullException("GameComponent is null");
            }

            if (mChilds != null)
            {
                foreach (ASceneNode<T> child in mChilds)
                {
                    if (child.RemoveComponent(gc))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Indica si se puede meter el GameComponent en el nodo.
        /// </summary>
        /// <param name="gc">GameComponent</param>
        /// <returns>True en caso afirmativo, False en caso contrario.</returns>
        protected virtual bool CouldBePlacedGCIntoNode(Radgie.Core.IGameComponent gc)
        {
            // TODO: Tener en cuenta posicion, etc...
            return true;
        }

        #endregion
    }
}
