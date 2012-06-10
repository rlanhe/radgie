using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Radgie.Util;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Entidad grafica para dibujar volumenes de colision.
    /// </summary>
    public class BoundingVolume : ASimpleGraphicEntity, IDebugObject
    {
        #region Properties
        /// <summary>
        /// Geometria generada a partir de la definicion del volumen de colision.
        /// Un volumen de colision puede estar formado por varios BoundingVolume, por lo que se genera una geometria para cada uno de ellos.
        /// </summary>
        private List<StaticGeometry> mGeometry;
        /// <summary>
        /// Material por defecto para dibujar las geometrias de BoundingVolumes.
        /// </summary>
        private static Material mDefaultMaterial;
        /// <summary>
        /// Color por defecto para dibujar las geometrias de BoundingVolumes.
        /// </summary>
        private static readonly Color mDefaultColor = Color.Yellow;
        /// <summary>
        /// BoundingVolume que se dibuja.
        /// </summary>
        private IBoundingVolume mBoundingVolume;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva entidad grafica para dibujar la geometria del BoundingVolume pasado por parametro.
        /// </summary>
        /// <param name="volume">Especificacion del volumen que se quiere dibujar.</param>
        public BoundingVolume(IBoundingVolume volume)
        {
            mBoundingVolume = volume;
            mGeometry = new List<StaticGeometry>();
            CreateGeometry(volume);

            if (mDefaultMaterial == null)
            {
                mDefaultMaterial = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Material>(@"Radgie/Graphics/Materials/debug");
            }
            Material = mDefaultMaterial;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Genera la geometria del BoundingVolume en funcion del tipo de volumen de colision.
        /// </summary>
        /// <param name="volume">Volumen de colision.</param>
        private void CreateGeometry(IBoundingVolume volume)
        {
            Type vType = volume.GetType();
            if (vType == BoundingUtil.SPHERE)
            {
                CreateGeometry((Radgie.Core.BoundingVolumes.BoundingSphere)volume);
            }
            else if (vType == BoundingUtil.BOX)
            {
                CreateGeometry((Radgie.Core.BoundingVolumes.BoundingBox)volume);
            }
            else if (vType == BoundingUtil.FRUSTUM)
            {
                CreateGeometry((Radgie.Core.BoundingVolumes.BoundingFrustum)volume);
            }
            else if (vType == BoundingUtil.COMPOSITE)
            {
                foreach (IBoundingVolume v in ((Radgie.Core.BoundingVolumes.CompositeBoundingVolume)volume).BoundingVolumes)
                {
                    CreateGeometry(v);
                }
            }

            IBoundingVolume newVolume = volume.ChildVolume;
            if (newVolume != null)
            {
                CreateGeometry(newVolume);
            }
        }

        /// <summary>
        /// Genera la geometria de una esfera.
        /// </summary>
        /// <param name="sphere">Esfera de colision.</param>
        private void CreateGeometry(Radgie.Core.BoundingVolumes.BoundingSphere sphere)
        {
            const int sphereResolution = 16;
            int dim = (sphereResolution+1)*3;
            VertexPositionColor[] vertices = new VertexPositionColor[dim];
            int[] indices = new int[dim];
            int index = 0;
            float step = MathHelper.TwoPi / (float)sphereResolution;
            float radius = sphere.BoundingVolume.Radius;

            for(int i = 0; i<indices.Length; i++)
            {
                indices[i] = i;
            }

            //Circunferencia en el plano XY
            float a = 0.0f;
            for (int i = 0; i <= sphereResolution; i++)
            {
                a += step;
                vertices[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a) * radius, (float)Math.Sin(a) * radius, 0f), mDefaultColor);
            }

            //Plano XZ
            a = 0.0f;
            for (int i = 0; i <= sphereResolution; i++)
            {
                a += step;
                vertices[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a) * radius, 0f, (float)Math.Sin(a) * radius), mDefaultColor);
            }

            //Plano YZ
            a = 0.0f;
            for (int i = 0; i <= sphereResolution; i++)
            {
                a += step;
                vertices[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a) * radius, (float)Math.Sin(a) * radius), mDefaultColor);
            }

            StaticGeometry geometry = new StaticGeometry();
            geometry.SetData(vertices,indices,PrimitiveType.LineStrip);
            mGeometry.Add(geometry);
        }
        
        /// <summary>
        /// Geometria de un cubo de colision.
        /// </summary>
        /// <param name="box">Cubo de colision.</param>
        private void CreateGeometry(Radgie.Core.BoundingVolumes.BoundingBox box)
        {
            CreateGeometryForBox(box.LocalBoundingVolume.GetCorners());
        }

        /// <summary>
        /// Geometria del frustum de la camara.
        /// </summary>
        /// <param name="frustum">Frustum de la camara.</param>
        private void CreateGeometry(Radgie.Core.BoundingVolumes.BoundingFrustum frustum)
        {
            CreateGeometryForBox(frustum.LocalBoundingVolume.GetCorners());
        }

        /// <summary>
        /// Geometria de un cubo de colision.
        /// </summary>
        /// <param name="corners">Vertices del cubo de colision.</param>
        private void CreateGeometryForBox(Vector3[] corners)
        {
            StaticGeometry geometry = new StaticGeometry();
            VertexPositionColor[] vertices = new VertexPositionColor[8];

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = corners[i];
                vertices[i].Color = mDefaultColor;
            }

            int[] indices = new int[]
            {
                0, 1,
                1, 2,
                2, 3,
                3, 0,
                0, 4,
                1, 5,
                2, 6,
                3, 7,
                4, 5,
                5, 6,
                6, 7,
                7, 4,
            };

            geometry.SetData(vertices, indices, PrimitiveType.LineList);
            mGeometry.Add(geometry);
        }

        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.DrawAction"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            base.DrawAction(renderer);
            foreach (StaticGeometry g in mGeometry)
            {
                g.Draw(renderer);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.CalculateBoundingVolume"/>
        /// </summary>
        public override IBoundingVolume CalculateBoundingVolume()
        {
            return mBoundingVolume;
        }
        #endregion

        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override Core.IInstance CreateSpecificInstance()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
