using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Radgie.Graphics
{
    /// <summary>
    /// Geometria estatica.
    /// </summary>
    public class StaticGeometry: IDraw
    {
        #region Properties
        /// <summary>
        /// Tipo de primitiva.
        /// </summary>
        public PrimitiveType PrimitiveType
        {
            get
            {
                return mPrimitiveType;
            }
        }
        protected PrimitiveType mPrimitiveType;

        /// <summary>
        /// VertexBuffer.
        /// Buffer de vertices en la GPU.
        /// </summary>
        public VertexBuffer VertexBuffer
        {
            get
            {
                return mVertexBuffer;
            }
        }
        protected VertexBuffer mVertexBuffer;

        /// <summary>
        /// IndexBuffer.
        /// Buffer de indices en la GPU.
        /// </summary>
        public IndexBuffer IndexBuffer
        {
            get
            {
                return mIndexBuffer;
            }
        }
        protected IndexBuffer mIndexBuffer;

        /// <summary>
        /// Numero de primitivas que contiene la geometria.
        /// </summary>
        public int PrimitiveCount
        {
            get
            {
                return mPrimitiveCount;
            }
        }
        protected int mPrimitiveCount;

        /// <summary>
        /// Referencia al sistema grafico.
        /// </summary>
        protected static IGraphicSystem mGraphicSystem;

        /// <summary>
        /// Delegado usado para dibujar la geometria por el Renderer.
        /// </summary>
        private DrawDelegate mDrawCallback;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa una geometria vacia.
        /// </summary>
        public StaticGeometry()
        {
            if (mGraphicSystem == null)
            {
                mGraphicSystem = ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem)));
            }
            mDrawCallback = DrawAction;
        }

        /// <summary>
        /// Inicializa una geometria con la informacion pasada por parametro.
        /// </summary>
        /// <param name="vertexBuffer">Buffer de vertices.</param>
        /// <param name="indexBuffer">Buffer de indices.</param>
        /// <param name="primitiveType">Tipo de primitiva.</param>
        public StaticGeometry(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, PrimitiveType primitiveType)
            :this()
        {
            Initialize(vertexBuffer, indexBuffer, primitiveType);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el tamanno de los indices.
        /// </summary>
        /// <param name="device">Dispositivo grafico.</param>
        /// <returns>Tamanno de los indices.</returns>
        protected static IndexElementSize GetIndexElementSize(GraphicsDevice device)
        {
            IndexElementSize size = IndexElementSize.ThirtyTwoBits;
            switch (device.GraphicsProfile)
            {
                case GraphicsProfile.HiDef:
                    size = IndexElementSize.ThirtyTwoBits;
                    break;
                case GraphicsProfile.Reach:
                    size = IndexElementSize.SixteenBits;
                    break;
            }
            return size;
        }

        /// <summary>
        /// Calcula el numero de primivitivas en funcion del numero de indices y el tipo de primitiva.
        /// </summary>
        protected virtual void CalculatePrimitiveCount()
        {
            switch (mPrimitiveType)
            {
                case PrimitiveType.LineList:
                    mPrimitiveCount = mIndexBuffer.IndexCount / 2;
                    break;
                case PrimitiveType.LineStrip:
                    mPrimitiveCount = mIndexBuffer.IndexCount - 1;
                    break;
                case PrimitiveType.TriangleStrip:
                    mPrimitiveCount = mIndexBuffer.IndexCount - 2;
                    break;
                case PrimitiveType.TriangleList:
                    mPrimitiveCount = mIndexBuffer.IndexCount / 3;
                    break;
            }
        }

        /// <summary>
        /// Inicializa los vertices e indices de la geometria.
        /// </summary>
        /// <typeparam name="T">Tipo de los vertices.</typeparam>
        /// <typeparam name="Y">Tipo de los indices.</typeparam>
        /// <param name="vertices">Array de vertices.</param>
        /// <param name="indices">Array de indices.</param>
        /// <param name="primitiveType">Tipo de primitiva.</param>
        public virtual void SetData<T, Y>(T[] vertices, Y[] indices, PrimitiveType primitiveType) where T : struct
                                                                                          where Y : struct
        {
            // TODO: booleano que impida llamara a este metodo mas de una vez
            GraphicsDevice device = mGraphicSystem.Device;
            
            lock (device)
            {
                VertexBuffer vBuffer = new VertexBuffer(device, typeof(T), vertices.Length, BufferUsage.None);
                IndexBuffer iBuffer = new IndexBuffer(device, GetIndexElementSize(device), indices.Length, BufferUsage.None);
                vBuffer.SetData(vertices);
                iBuffer.SetData(indices);

                Initialize(vBuffer, iBuffer, primitiveType);
            }
        }
        
        /// <summary>
        /// Inicializa la geometria a partir de los buffer de vertices e indices.
        /// </summary>
        /// <param name="vertexBuffer">Buffer de vertices.</param>
        /// <param name="indexBuffer">Buffer de indices.</param>
        /// <param name="primitiveType">Tipo de primitiva.</param>
        protected virtual void Initialize(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, PrimitiveType primitiveType)
        {
            mVertexBuffer = vertexBuffer;
            mIndexBuffer = indexBuffer;
            mPrimitiveType = primitiveType;

            CalculatePrimitiveCount();
        }

        #region IDraw Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDraw.Draw"/>
        /// </summary>
        public virtual void Draw(IRenderer renderer)
        {
            switch (renderer.SelectedRenderMode)
            {
                case RenderMode.NoInstancing:
                    renderer.Device.SetVertexBuffer(mVertexBuffer);
                    break;
                case RenderMode.Instancing:
                    renderer.Device.SetVertexBuffers(new VertexBufferBinding(mVertexBuffer, 0, 0), new VertexBufferBinding(renderer.InstancesData, 0, 1));
                    break;
            }

            renderer.Device.Indices = mIndexBuffer;

            renderer.Render(mDrawCallback);

            switch (renderer.SelectedRenderMode)
            {
                case RenderMode.NoInstancing:
                    renderer.Device.SetVertexBuffer(null);
                    break;
                case RenderMode.Instancing:
                    renderer.Device.SetVertexBuffers(null);
                    break;
            }

            renderer.Device.Indices = null;
        }
        #endregion

        /// <summary>
        /// Metodo para dibujar la geometria.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        protected virtual void DrawAction(IRenderer renderer)
        {
            renderer.Statistics.NumberOfDrawCalls++;
            renderer.Statistics.NumberOfTrianglesDrawed += mPrimitiveCount;

            switch (renderer.SelectedRenderMode)
            {
                case RenderMode.NoInstancing:
                    renderer.Device.DrawIndexedPrimitives(mPrimitiveType, 0, 0, mVertexBuffer.VertexCount, 0, mPrimitiveCount);
                    break;
                case RenderMode.Instancing:
                    renderer.Device.DrawInstancedPrimitives(mPrimitiveType, 0, 0, mVertexBuffer.VertexCount, 0, mPrimitiveCount, renderer.InstancesData.VertexCount);
                    break;
            }
        }
        #endregion
    }
}
