using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Entity;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Instance
{
    /// <summary>
    /// Instancia de una entidad SimpleGraphicEntity.
    /// </summary>
    public class SimpleGraphicInstance: AInstance, IGraphicInstance
    {
        #region Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.DrawOrder"/>
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return ((IGraphicEntity)Entity).DrawOrder;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva instancia.
        /// Solo para uso interno.
        /// </summary>
        /// <param name="entity">Entidad grafica.</param>
        internal SimpleGraphicInstance(IGraphicEntity entity)
            : base(entity)
        {
        }
        #endregion

        #region Methods

        #region IGraphicInstance
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDraw"/>
        /// </summary>
        public void Draw(IRenderer renderer)
        {
            // Instance properties
            renderer.World = Component.World;

            IGraphicEntity entity = (IGraphicEntity)Entity;
            entity.DrawInstance(renderer);
        }
        #endregion

        #region AGameObject Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.AGameObject"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IGraphicInstance Methods
        /// <summary>
        /// Metodo para ordenar los objetos a la hora de dibujar.
        /// </summary>
        /// <param name="other">Otro objeto para dibujar con el que comparar.</param>
        /// <returns>-1 si es menor, 0 si es igual y 1 si es mayor.</returns>
        public int CompareTo(IDrawable other)
        {
            return DrawOrder.CompareTo(other.DrawOrder);
        }
        #endregion
        #endregion
    }
}
