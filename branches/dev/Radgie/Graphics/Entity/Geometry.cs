using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Instance;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad para dibujar geometrias.
    /// </summary>
    public class Geometry: ASimpleGraphicEntity
    {
        #region Properties
        /// <summary>
        /// Referencia a la geometria que se quiere dibujar.
        /// </summary>
        protected StaticGeometry mGeometry;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva entidad para dibujar una geometria.
        /// </summary>
        /// <param name="geometry">Datos de la geometria.</param>
        public Geometry(StaticGeometry geometry)
        {
            mGeometry = geometry;
        }
        #endregion

        #region Methods
        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.DrawAction"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            base.DrawAction(renderer);
            mGeometry.Draw(renderer);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.CalculateBoundingVolume"/>
        /// </summary>
        public override IBoundingVolume CalculateBoundingVolume()
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[mGeometry.VertexBuffer.VertexCount];
            mGeometry.VertexBuffer.GetData<VertexPositionColorTexture>(vertices);
            float minX = 0.0f;
            float minY = 0.0f;
            float minZ = 0.0f;
            float maxX = 0.0f;
            float maxY = 0.0f;
            float maxZ = 0.0f;

            bool firstVertex = true;
            foreach (VertexPositionColorTexture vertex in vertices)
            {
                Vector3 pos = vertex.Position;
                if (firstVertex)
                {
                    firstVertex = false;
                    maxX = minX = pos.X;
                    maxY = minY = pos.Y;
                    maxZ = minZ = pos.Z;
                }
                else
                {
                    if (pos.X < minX)
                    {
                        minX = pos.X;
                    }
                    if (pos.X > maxX)
                    {
                        maxX = pos.X;
                    }
                    if (pos.Y < minY)
                    {
                        minY = pos.Y;
                    }
                    if (pos.Y > maxY)
                    {
                        maxY = pos.Y;
                    }
                    if (pos.Z < minZ)
                    {
                        minZ = pos.Z;
                    }
                    if (pos.Z > maxZ)
                    {
                        maxZ = pos.Z;
                    }
                }
            }

            if (minZ == maxZ)
            {
                maxZ += 0.1f;
            }

            return new Radgie.Core.BoundingVolumes.BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
        }
        #endregion

        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override IInstance CreateSpecificInstance()
        {
            return new SimpleGraphicInstance(this);
        }
        #endregion
        #endregion
    }
}
