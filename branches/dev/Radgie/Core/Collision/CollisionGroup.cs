using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Core.Collision
{
    /// <summary>
    /// Implementacion de un grupo de colision.
    /// </summary>
    public class CollisionGroup: ICollisionGroup
    {
        #region Properties

        #region ICollisionGroup Properties
        /// <summary>
        /// Identificador del grupo.
        /// </summary>
        public string Id
        {
            get
            {
                return mId;
            }
        }
        protected string mId;

        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.Scene"/>
        /// </summary>
        public IScene Scene
        {
            get
            {
                return mScene;
            }
        }
        protected IScene mScene;

        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.GameComponents"/>
        /// </summary>
        public List<IGameComponent> GameComponents
        {
            get
            {
                return mGameComponents.ToList();
            }
        }
        protected List<IGameComponent> mGameComponents;
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo grupo de colision para una escena.
        /// </summary>
        /// <param name="scene">Escena a la que perteneceran los objetos del grupo de colision.</param>
        /// <param name="id">Identificador que se le asignara al grupo.</param>
        public CollisionGroup(IScene scene, string id)
        {
            mScene = scene;
            mId = id;
        }
        #endregion

        #region Methods
        #region ICollisionGroup Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.AddGameComponent"/>
        /// </summary>
        public void AddGameComponent(IGameComponent gc)
        {
            if (mScene == gc.Scene)
            {
                if (mGameComponents == null)
                {
                    mGameComponents = new List<IGameComponent>();
                }

                if(!mGameComponents.Contains(gc))
                {
                    mGameComponents.Add(gc);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.RemoveGameComponent"/>
        /// </summary>
        public void RemoveGameComponent(IGameComponent gc)
        {
            if (mScene == gc.Scene)
            {
                if (mGameComponents != null)
                {
                    mGameComponents.Remove(gc);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.GetCollisions"/>
        /// </summary>
        public List<CollisionRecord> GetCollisions(IGameComponent gc)
        {
            List<CollisionRecord> mFilteredCollisions = new List<CollisionRecord>();

            this.GetCollisions(gc, mFilteredCollisions);

            return mFilteredCollisions;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.Collision.ICollisionGroup.GetCollisions"/>
        /// </summary>
        public void GetCollisions(IGameComponent gc, List<CollisionRecord> results)
        {
            results.Clear();

            if (mGameComponents.Contains(gc) && gc.IsActive())
            {
                ISceneNode gcNode = gc.ParentNode;
                IBoundingVolume gcBV = gc.BoundingVolume;

                if (gcBV != null)
                {
                    ISceneNode tmpGCNode;
                    IBoundingVolume tmpGCBV;

                    foreach (IGameComponent tmpGC in mGameComponents)
                    {
                        if (tmpGC.IsActive() && (gc != tmpGC))
                        {
                            tmpGCNode = tmpGC.ParentNode;
                            if (gc.Scene.CanCollide(gcNode, tmpGCNode))
                            {
                                tmpGCBV = tmpGC.BoundingVolume;
                                if (tmpGCBV != null)
                                {
                                    float? result = gcBV.Intersects(tmpGCBV);
                                    if (result != null)
                                    {
                                        results.Add(new CollisionRecord(gc, tmpGC));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Actualiza el estado del grupo de colisiones, recalculando las producidas durante el ultimo frame.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la ultima actualizacion.</param>
        public void Update(Microsoft.Xna.Framework.GameTime time)
        {
        }
        #endregion
        #endregion
    }
}
