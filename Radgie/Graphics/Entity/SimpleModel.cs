using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core.BoundingVolumes;
using Radgie.Graphics.Instance;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad para el manejo de modelos estaticos.
    /// </summary>
    public class SimpleModel: AGraphicEntity
    {
        #region Properties
        #region IDrawable Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDrawable"/>
        /// </summary>
        public override int DrawOrder
        {
            get
            {
                if ((mMaterials != null) && (mMaterials.Count > 0))
                {
                    return mMaterials[0].DrawOrder;
                }
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// Mesh del modelo.
        /// </summary>
        private Mesh mMesh;
        /// <summary>
        /// Lista de materiales de cada una de las MeshParts
        /// </summary>
        public IEnumerator<Material> Materials
        {
            get
            {
                return mMaterials.GetEnumerator();
            }
        }
        private List<Material> mMaterials;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un modelo a partir de una Mesh cargada desde fichero.
        /// </summary>
        /// <param name="mesh">Mesh cargada desde fichero.</param>
        public SimpleModel(Mesh mesh)
        {
            mMesh = mesh;
            mMesh.Configure();
            mMaterials = new List<Material>();
            foreach (Material mat in mMesh.Materials)
            {
                mMaterials.Add(mat.Clone());
            }
        }
        #endregion

        #region Methods
        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphic.AGraphicEntity.DrawAction"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            base.DrawAction(renderer);

            for (int i = 0; i < mMesh.MeshParts.Count; i++)
            {
                renderer.Material = mMaterials[i];
                mMesh.MeshParts[i].Draw(renderer);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphic.AGraphicEntity.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphic.AGraphicEntity.CalculateBoundingVolume"/>
        /// </summary>
        public override IBoundingVolume CalculateBoundingVolume()
        {
            return mMesh.BoundingVolume;
        }
        #endregion

        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphic.AGraphicEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override Core.IInstance CreateSpecificInstance()
        {
            return new SimpleGraphicInstance(this);
        }
        #endregion

        /// <summary>
        /// Cambia el material indicado.
        /// </summary>
        /// <param name="index">Indice en la lista de materiales.</param>
        /// <param name="material">Nuevo material.</param>
        public void SetMaterial(int index, Material material)
        {
            mMaterials[index] = material;
        }
        #endregion
    }
}
