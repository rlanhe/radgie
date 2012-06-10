using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics.Instance;
using Radgie.Core;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Funcionalidad comun a todas las entidades graficas.
    /// </summary>
    public abstract class AGraphicEntity: AEntity, IGraphicEntity
    {
        #region Properties
        #region IDrawable Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDrawable.DrawOrder"/>
        /// </summary>
        public virtual int DrawOrder
        {
            get
            {
                return 0;
            }
        }
        #endregion

        #region IGraphicEntity Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicEntity.InstancesRenderer"/>
        /// </summary>
        public virtual IEntityInstancesRenderer InstancesRenderer
        {
            get
            {
                if (mInstancesRenderer == null)
                {
                    mInstancesRenderer = new EntityInstancesRenderer<Matrix>(EntityInstancesRenderer<Matrix>.WorldInstanceVertexDeclaration, EntityInstancesRenderer<Matrix>.DefaultInstanceDataDelegate, this);
                }
                return mInstancesRenderer;
            }
            set
            {
                mInstancesRenderer = value;
            }
        }
        private IEntityInstancesRenderer mInstancesRenderer;
        #endregion
        #endregion

        #region Methods
        #region IDraw Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDraw.Draw"/>
        /// </summary>
        public void Draw(IRenderer renderer)
        {
            PreDraw(renderer);
            DrawAction(renderer);
        }
        #endregion

        #region IGraphicInstance Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicInstance.DrawInstance"/>
        /// </summary>
        public void DrawInstance(IRenderer renderer)
        {
            DrawAction(renderer);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IGraphicInstance.CalculateBoundingVolume"/>
        /// </summary>
        public abstract IBoundingVolume CalculateBoundingVolume();
        #endregion

        /// <summary>
        /// Acciones a realizar antes de que el renderer dibuje la entidad.
        /// Carga en el renderer la transformacion de mundo de la entidad.
        /// </summary>
        /// <param name="renderer">Renderer</param>
        protected virtual void PreDraw(IRenderer renderer)
        {
            if (Component != null)
            {
                renderer.World = Component.World;
            }
            else
            {
                renderer.World = Matrix.Identity;
            }
        }

        /// <summary>
        /// Dibujado de la entidad.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        protected virtual void DrawAction(IRenderer renderer)
        {
            // El dibujado se realiza en la sobrecarga de este metodo en las clases hijas.
            renderer.Statistics.NumberOfObjectsDrawed++;
        }

        /// <summary>
        /// Metodo para ordenar las entidades graficas a la hora de dibujarlas.
        /// Ordena de menor a mayor.
        /// </summary>
        /// <param name="other">Elemento con el que comparar</param>
        /// <returns>-1 si es menor, 0 si es igual y 1 si es mayor.</returns>
        public int CompareTo(IDrawable other)
        {
            return DrawOrder.CompareTo(other.DrawOrder);
        }
        #endregion
    }
}
