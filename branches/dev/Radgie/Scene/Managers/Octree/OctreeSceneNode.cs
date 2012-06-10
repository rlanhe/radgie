using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using Radgie.Util;

namespace Radgie.Scene.Managers.Octree
{
    /// <summary>
    /// Nodo de una escena basada en un octree.
    /// </summary>
    public class OctreeSceneNode : ASceneNode<OctreeSceneNode>
    {
        #region Properties
        /// <summary>
        /// Volumen de colision que delimita la region del espacio cubierta por el scene node.
        /// </summary>
        private Radgie.Core.BoundingVolumes.BoundingBox mBoundingBox;
        /// <summary>
        /// Coordenadas minimas cubiertas por el nodo de escena.
        /// </summary>
        private Vector3 mMin;
        /// <summary>
        /// Coordenadas maximas cubiertas por el nodo de escena.
        /// </summary>
        private Vector3 mMax;

        /// <summary>
        /// Tamanno de la region cubierta por el nodo de escena.
        /// </summary>
        public float Size
        {
            get
            {
                return mSize;
            }
        }
        private float mSize;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye un nodo de escena.
        /// </summary>
        /// <param name="scene">Escena a la que pertenece.</param>
        /// <param name="size">Tamanno del nodo de escena.</param>
        /// <param name="min">Posicion minima que cubre el nodo de escena.</param>
        /// <param name="max">Posicion maxima que cubre el nodo de escena.</param>
        public OctreeSceneNode(OctreeScene scene, float size, Vector3 min, Vector3 max)
            : base(scene, null)
        {
            Init(size, min, max);
        }

        /// <summary>
        /// Construye un nodo de escena a partir del nodo de escena que contiene un game component y del volumen del propio game component.
        /// </summary>
        /// <param name="child">Nodo de escena.</param>
        /// <param name="gc">Game Component.</param>
        public OctreeSceneNode(OctreeSceneNode child, Radgie.Core.IGameComponent gc)
            : base(child.mScene, null)
        {
            IBoundingVolume bv = BoundingUtil.Merge(child.mBoundingBox, gc.BoundingVolume);

            Vector3 nodeRef = new Vector3();
            if(bv is Radgie.Core.BoundingVolumes.BoundingSphere)
            {
                nodeRef = ((Radgie.Core.BoundingVolumes.BoundingSphere)bv).BoundingVolume.Center;
            }
            else if(bv is Radgie.Core.BoundingVolumes.BoundingBox)
            {
                nodeRef = ((Radgie.Core.BoundingVolumes.BoundingBox)bv).BoundingVolume.Min;
            }

            // Determina donde esta el gc respecto al nodo de escena actual
            int i = child.mMin.X < nodeRef.X ? 0 : 1;
            int j = child.mMin.Y < nodeRef.Y ? 0 : 1;
            int k = child.mMin.Z < nodeRef.Z ? 0 : 1;

            float size = child.mSize;
            
            Vector3 min = child.mMin - new Vector3(i * size, j * size, k * size);
            Vector3 max = child.mMin + new Vector3((2 - i) * size, (2 - j) * size, (2 - k) * size);

            float parentSize = child.mSize * 2.0f;
            Init(parentSize, min, max);
            CreateChildNodes();

            for (i = 0; i < 8; i++)
            {
                OctreeSceneNode childNode = mChilds[i];
                if ((childNode.mMin == child.mMin) && (childNode.mMax == child.mMax))
                {
                    // Reemplaza nodo por antiguo
                    mChilds[i] = child;
                    break;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa el nodo de escena.
        /// </summary>
        /// <param name="size">Tamanno del nodo de escena.</param>
        /// <param name="min">Posicion minima cubierta por el nodo de escena.</param>
        /// <param name="max">Posicion maxima cubierta por el nodo de escena.</param>
        private void Init(float size, Vector3 min, Vector3 max)
        {
            mMin = min;
            mMax = max;
            mSize = size;
            mBoundingBox = new Radgie.Core.BoundingVolumes.BoundingBox(min, max);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ASceneNode.IsNodeVisible"/>
        /// </summary>
        public override bool IsNodeVisible(IBoundingVolume frustum)
        {
            ContainmentType result = frustum.Contains(mBoundingBox);
            return ((result == ContainmentType.Contains) || (result == ContainmentType.Intersects));
        }

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ASceneNode.CouldBePlacedGCIntoNode"/>
        /// </summary>
        protected override bool CouldBePlacedGCIntoNode(Core.IGameComponent gc)
        {
            if (gc.BoundingVolume == null)
            {
                return true;
            }

            ContainmentType containmentType = mBoundingBox.Contains(gc.BoundingVolume);
            return containmentType == ContainmentType.Contains;
        }

        /// <summary>
        /// Crea los nodos hijos del nodo actual, que dividen el espacio que gestiona el padre en ocho partes iguales.
        /// </summary>
        private void CreateChildNodes()
        {
            if (mChilds == null)
            {
                mChilds = new List<OctreeSceneNode>(8);
            }

            const int MAX = 2;
            float size = Size / 2.0f;
            for (int i = 0; i < MAX; i++)
            {
                for (int j = 0; j < MAX; j++)
                {
                    for (int k = 0; k < MAX; k++)
                    {
                        Vector3 min = mMin + new Vector3(i * size, j * size, k * size);
                        Vector3 max = mMax - new Vector3((1 - i) * size, (1 - j) * size, (1 - k) * size);
                        mChilds.Add(new OctreeSceneNode((OctreeScene)mScene, size, min, max));
                    }
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);

            if ((mChilds == null) && (mComponents != null) && (mComponents.Count > ((OctreeScene)mScene).MaxObjectsPerPartition))
            {
                // Si el numero de elementos dentro del nodo supera el maximo establecido, reparte los nodos entre sus hijos
                // Crea los nodos hijos
                CreateChildNodes();

                // Trata de recolocar cada componente hijo en los nodos recien creados
                for (int i = mComponents.Count - 1; i >= 0; i--)
                {
                    mScene.AddComponent(mComponents[i]);
                }
            }
            else if (mChilds != null)
            {
                int count = ComponentsCount;
                int partitionCount = mComponents != null ? mComponents.Count : 0;
                OctreeScene octScene = (OctreeScene)mScene;
                if ((count < octScene.MinObjectsPerPartition) && (count + partitionCount < octScene.MaxObjectsPerPartition))
                {
                    // Los nodos hijos no son necesarios
                    // Elimina los nodos de escena hijos y se queda con sus componentes
                    for (int i = 0; i < 8; i++)
                    {
                        List<Radgie.Core.IGameComponent> components = mChilds[i].mComponents;

                        if (components != null)
                        {
                            mChilds.RemoveAt(i);
                            for (int j = components.Count - 1; j >= 0; j--)
                            {
                                mScene.AddComponent(components[j]);
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
