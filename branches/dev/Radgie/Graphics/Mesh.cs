using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core.BoundingVolumes;
using Microsoft.Xna.Framework;
using Radgie.Util;
using Radgie.Core;

namespace Radgie.Graphics
{
    /// <summary>
    /// Clase donde se almacena la geometria y materiales de un modelo estatico cargado desde fichero.
    /// </summary>
    public class Mesh
    {
        #region Properties
        /// <summary>
        /// Partes de la geometria de un modelo. Aqui se carga los datos de la geometria del fichero.
        /// La geometria de un modelo puede estar dividida en varias partes, para aplicar un material distinto a cada una de ellas.
        /// </summary>
        [ContentSerializer]
        public List<Geometry> GeometryParts;

        /// <summary>
        /// Lista con los shaders a aplicar a cada una de las entradas de la lista GeometryParts. Aqui se carga la informacion sobre efectos del fichero.
        /// </summary>
        [ContentSerializer]
        public List<Effect> Effects;

        #region Class
        /// <summary>
        /// Clase empleada para cargar los datos del fichero sobre la geometria del modelo.
        /// </summary>
        public class Geometry
        {
            /// <summary>
            /// Numero de triangulos de la geometria.
            /// </summary>
            public int TriangleCount;
            /// <summary>
            /// Numero de vertices de la geometria.
            /// </summary>
            public int VertexCount;
            /// <summary>
            /// Buffer con los datos de los vertices.
            /// </summary>
            public VertexBuffer VertexBuffer;
            /// <summary>
            /// Buffer con los indices sobre los vertices.
            /// </summary>
            public IndexBuffer IndexBuffer;
        }
        #endregion

        /// <summary>
        /// A partir de los datos de GeometryParts, se generan los objetos StaticGeometry correspondientes.
        /// Estos son los datos de la geometria que se dibujaran al dibujar el modelo.
        /// </summary>
        [ContentSerializerIgnore]
        public List<StaticGeometry> MeshParts;
        /// <summary>
        /// Lista con los materiales que se deben aplicar a cada una de las entradas de MeshParts.
        /// Estos materiales seran los que se usaran a la hora de dibujar el modelo.
        /// </summary>
        [ContentSerializerIgnore]
        public List<Material> Materials;
        /// <summary>
        /// Material por defecto para asignar a cada una de las partes del modelo.
        /// Asignada por el usuario en las properties del modelo.
        /// </summary>
        private Material mDefaultMaterial;
        /// <summary>
        /// Identificador del material por defecto.
        /// Aqui es donde deja el lector del modelo el material por defecto asignado por el usuario.
        /// </summary>
        public string DefaultMaterialId;
        /// <summary>
        /// Indica si ya se inicializaron las propiedades internas del modelo.
        /// </summary>
        private bool mConfigured = false;

        /// <summary>
        /// BoundingVolume del modelo.
        /// </summary>
        [ContentSerializerIgnore]
        public IBoundingVolume BoundingVolume
        {
            get
            {
                return mBoundingVolume.Clone();
            }
        }
        private IBoundingVolume mBoundingVolume;
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa la geometria y los meteriales del modelo.
        /// </summary>
        public void Configure()
        {
            if (!mConfigured)
            {
                mDefaultMaterial = RadgieGame.Instance.ResourceManager.Load<Material>(DefaultMaterialId);
                MeshParts = new List<StaticGeometry>();
                foreach (Geometry g in GeometryParts)
                {
                    MeshParts.Add(new StaticGeometry(g.VertexBuffer, g.IndexBuffer, PrimitiveType.TriangleList));// TODO: PrimitiveType -> manera de inferirlo?
                }

                Materials = new List<Material>();
                foreach (Effect e in Effects)
                {
                    Material mat = mDefaultMaterial.Clone();
                    mat.LoadParametersFromEffect(e);
                    Materials.Add(mat);
                }
                Effects = null;

                CalculateBoundingVolume();

                // Libera las estructuras de datos usadas durante la carga
                GeometryParts = null;
                Effects = null;

                mConfigured = true;
            }
        }

        /// <summary>
        /// Calcula el volumen del modelo mediante un cubo de colision.
        /// </summary>
        private void CalculateBoundingVolume()
        {
            float minX = 0.0f;
            float minY = 0.0f;
            float minZ = 0.0f;
            float maxX = 0.0f;
            float maxY = 0.0f;
            float maxZ = 0.0f;

            foreach (Geometry g in GeometryParts)
            {
                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[g.VertexCount];
                g.VertexBuffer.GetData<VertexPositionColorTexture>(vertices);
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
            }

            mBoundingVolume = new Radgie.Core.BoundingVolumes.BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
        }
        #endregion
    }
}
