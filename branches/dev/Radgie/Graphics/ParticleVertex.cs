using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Radgie.Graphics
{
    /// <summary>
    /// Estructura para almacenar los datos de cada particula en la GPU.
    /// Almacena no solo los datos de la geometria de las particulas, si no su posicion, velocidad y edad.
    /// </summary>
    public struct ParticleVertex
    {
        #region Properties
        /// <summary>
        /// Identificador de la esquina del quad de la particula que representa.
        /// </summary>
        public Short2 Corner;

        /// <summary>
        /// Posicion inicial de la particula.
        /// A partir de este dato, la velocidad y la edad de la particula calcula su posicion actual.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Velocidad inicial de la particula.
        /// </summary>
        public Vector3 Velocity;

        /// <summary>
        /// Almacena 4 valores aleatorios utilizados luego por la GPU para cambiar los parametros de cada particula y hacer que parezcan distintas.
        /// </summary>
        public Color Random;

        /// <summary>
        /// Tiempo en segundos en el que fue creada la particula.
        /// </summary>
        public float Time;

        /// <summary>
        /// Especifica la forma de este tipo de Vertice.
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Short2,
                                 VertexElementUsage.Position, 0),

            new VertexElement(4, VertexElementFormat.Vector3,
                                 VertexElementUsage.Position, 1),

            new VertexElement(16, VertexElementFormat.Vector3,
                                  VertexElementUsage.Normal, 0),

            new VertexElement(28, VertexElementFormat.Color,
                                  VertexElementUsage.Color, 0),

            new VertexElement(32, VertexElementFormat.Single,
                                  VertexElementUsage.TextureCoordinate, 0)
        );

        /// <summary>
        /// Tamanno total en bytes de la estructura.
        /// </summary>
        public const int SizeInBytes = 36;
        #endregion
    }
}
