using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;

namespace Radgie.Graphics
{
    /// <summary>
    /// Dibuja instancias de una entidad de una manera eficiente.
    /// Este objeto es encargado de dibujar un mismo objeto muchas veces por pantalla en una sola llamada al dispositivo grafico, mejorando el rendimiento de la aplicacion.
    /// </summary>
    /// <typeparam name="T">Datos de la instancia.</typeparam>
    public class EntityInstancesRenderer<T>: IEntityInstancesRenderer where T: struct
    {
        /// <summary>
        /// Delegado para obtener datos de una instancia necesarios para dibujarla.
        /// </summary>
        /// <param name="instance">Instancia de la entidad.</param>
        /// <returns></returns>
        public delegate T InstanceDataDelegate(IGraphicInstance instance);

        #region Properties
        /// <summary>
        /// Entidad de la que se van a dibujar instancias.
        /// </summary>
        public IGraphicEntity Entity
        {
            get
            {
                return mEntity;
            }
        }
        private IGraphicEntity mEntity;

        /// <summary>
        /// Lista con los datos de las instancias.
        /// </summary>
        private List<T> mInstancesData;

        /// <summary>
        /// Delegado por defecto para obtener los datos de las instancias.
        /// </summary>
        /// <param name="instance">Instancia.</param>
        /// <returns>Matriz de transformacion de la instancia.</returns>
        public static Matrix DefaultInstanceDataDelegate(IGraphicInstance instance)
        {
            return instance.Component.World;
        }

        private InstanceDataDelegate mInstanceDataDelegate;

        /// <summary>
        /// Declaracion por defecto del formato de los vertices que se van a dibujar.
        /// </summary>
        public static VertexDeclaration WorldInstanceVertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3)
        );

        private VertexDeclaration mVertexDeclaration;

        private T[] mInstancesArray;

        private DynamicVertexBuffer mInstancesVertexBuffer;

        #region IDrawable Properties
        /// <summary>
        /// Ver <see cref="Radgie.Grahpics.IDrawable.DrawOrder"/>
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return mEntity.DrawOrder;
            }
        }
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un renderizador de instancias de entidades.
        /// </summary>
        /// <param name="vertexDeclaration">Declaracion de los vertices que se van a dibujar.</param>
        /// <param name="instanceDataDelegate">Delegado para obtener datos de la instancia.</param>
        /// <param name="entity">Entidad de la que se van a dibujar instancias.</param>
        public EntityInstancesRenderer(VertexDeclaration vertexDeclaration, InstanceDataDelegate instanceDataDelegate, IGraphicEntity entity)
        {
            mVertexDeclaration = vertexDeclaration;
            mEntity = entity;
            mInstancesData = new List<T>();
            mInstanceDataDelegate = instanceDataDelegate;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Registra una instancia para dibujarse con este Renderer.
        /// </summary>
        /// <param name="instance">Nueva instancia grafica.</param>
        public void AddInstance(IGraphicInstance instance)
        {
            if ((instance.Entity == Entity) && (instance.Component != null))
            {
                mInstancesData.Add(mInstanceDataDelegate(instance));
            }
        }

        /// <summary>
        /// Limpia la lista de instancias.
        /// </summary>
        private void Reset()
        {
            mInstancesData.Clear();
        }

        #region IDraw Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDraw.Draw"/>
        /// </summary>
        /// <param name="renderer"></param>
        public void Draw(IRenderer renderer)
        {
            if (mInstancesData.Count > 0)
            {
                mInstancesArray = mInstancesData.ToArray();

                lock (renderer.Device)
                {
                	// TODO: Rehace vertex buffer en cada frame. Probar a cambiar esto y que solo lo rehaga cuando el numero de elementos es mayor que su tamanno
                    if ((mInstancesVertexBuffer == null) || (mInstancesArray.Length != mInstancesVertexBuffer.VertexCount))
                    {
                        if (mInstancesVertexBuffer != null)
                        {
                            mInstancesVertexBuffer.Dispose();
                        }

                        mInstancesVertexBuffer = new DynamicVertexBuffer(renderer.Device, mVertexDeclaration, mInstancesArray.Length, BufferUsage.WriteOnly);
                    }

                    mInstancesVertexBuffer.SetData(mInstancesArray, 0, mInstancesArray.Length, SetDataOptions.Discard);

                    renderer.InstancesData = mInstancesVertexBuffer;

                    mEntity.Draw(renderer);
                }

                renderer.InstancesData = null;
                Reset();
            }
        }
        #endregion

        #region IDrawable Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDrawable.CompareTo"/>
        /// </summary>
        public int CompareTo(IDrawable other)
        {
            return mEntity.CompareTo(other);
        }
        #endregion

        #endregion
    }
}
