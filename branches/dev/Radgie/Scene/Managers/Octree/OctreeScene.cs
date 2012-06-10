using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Radgie.Scene.Managers.Octree
{
    /// <summary>
    /// Escena basada en un octree.
    /// Gestiona los objetos de la escena dividiendo el espacio en 8 cubos del mismo tamanno. A su vez, cada cubo se divide en otros 8 cubos mas
    /// pequennos. Los objetos se situan en uno de estos cubos en funcion de su posicion en el espacio. Esta distribucion le permite optimizar 
    /// las operaciones tipicas sobre una escena al poder descartar de una forma rapida una gran cantidad de objetos que no estan involucrados 
    /// en la operacion que se quiere realizar, fundamentalmente para saber si un objeto es visible o el calculo de colisiones entre objetos 
    /// de la escena.
    /// </summary>
    public class OctreeScene: AScene<OctreeSceneNode>
    {
        #region Properties
        /// <summary>
        /// Numero de objetos maximo por particion del espacio.
        /// Si se supera este numero se divide nuevamente el espacio para distribuir mejor los objetos.
        /// </summary>
        public int MaxObjectsPerPartition
        {
            get
            {
                return mMaxObjectsPerPartition;
            }
        }
        protected int mMaxObjectsPerPartition;

        /// <summary>
        /// Numero de objetos minimo por particion del espacio.
        /// Si el numero de objetos es menor, se deshace la ultima division del espacio y los objetos se reagrupan en la particion del espacio padre.
        /// </summary>
        public int MinObjectsPerPartition
        {
            get
            {
                return mMinObjectsPerPartition;
            }
        }
        protected int mMinObjectsPerPartition;

        private const float DEFAULT_PARTITION_SIZE = 1000.0f;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye una escena basada en un octree.
        /// </summary>
        /// <param name="id">Identificador.</param>
        /// <param name="maxObjectsPerPartition">Numero de objetos maximo por particion.</param>
        /// <param name="minObjectsPerPartition">Numero de objetos minimo por particion.</param>
        public OctreeScene(string id, int maxObjectsPerPartition, int minObjectsPerPartition)
            : base(id)
        {
            Debug.Assert(maxObjectsPerPartition > 0, "maxObjectsPerPartion is lower than zero");
            Debug.Assert(minObjectsPerPartition > 0, "minObjectsPerPartition is lower than zero");
            Debug.Assert(maxObjectsPerPartition > minObjectsPerPartition, "maxObjectsPerPartion is lower or equal to minObjectsPerPartition");

            mMaxObjectsPerPartition = maxObjectsPerPartition;
            mMinObjectsPerPartition = minObjectsPerPartition;

            float halfSize = DEFAULT_PARTITION_SIZE / 2.0f;
            mRoot = new OctreeSceneNode(this, DEFAULT_PARTITION_SIZE, Vector3.One * (-halfSize), Vector3.One * halfSize);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Annade un componente a la escena.
        /// </summary>
        /// <param name="gc">Nuevo game component que se annade a la escena.</param>
        /// <returns>True si se annade, false si no. P.E. si ya esta annadido.</returns>
        public override bool AddComponent(Core.IGameComponent gc)
        {
            bool result = base.AddComponent(gc);

            while (!result)
            {
                // Es necesario ampliar el area que cubre el octree
                mRoot = new OctreeSceneNode(mRoot, gc);
                result = base.AddComponent(gc);
            }

            return result;
        }
        #endregion
    }
}
