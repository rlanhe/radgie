using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util.Collection.Pool
{
    /// <summary>
    /// Pool de objetos.
    /// Almacen de objetos para se utilizados con posterioridad sin necesidad de crear nuevos objetos.
    /// </summary>
    /// <typeparam name="T">Tipo de los objetos que contiene el pool. Debe tener un constructor vacio.</typeparam>
    public class Pool<T>
    {
        #region Delegates & Events
        /// <summary>
        /// Delegado para crear los objetos del pool.
        /// </summary>
        /// <returns>Objeto del pool creado.</returns>
        public delegate T InitDelegate();
        #endregion

        #region Properties
        /// <summary>
        /// Numero de elementos disponibles en el pool.
        /// </summary>
        public int Count
        {
            get
            {
                return mPool.Count;
            }
        }
        /// <summary>
        /// Numero inicial de elementos en el pool.
        /// </summary>
        private int mInitialSize;
        /// <summary>
        /// Indica si el numero maximo de elementos del pool es ampliable.
        /// </summary>
        private bool mFixedSize;
        /// <summary>
        /// Numero de elementos que se crearan con cada nueva generacion si se queda sin objetos el pool.
        /// </summary>
        private int mGenerationSize;
        /// <summary>
        /// Queue interna para almacenar los objetos.
        /// </summary>
        private Queue<T> mPool;
        /// <summary>
        /// Referencia al delegado que debe usar para crear los objetos del pool.
        /// </summary>
        private InitDelegate mInitDelegate;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el pool.
        /// </summary>
        /// <param name="initialSize">Numero inicial de elementos.</param>
        /// <param name="fixedSize">True si no puede ampliar su tamanno, False en caso contrario.</param>
        /// <param name="generationSize">Numero de elementos que se crearan si el pool puede incrementar su numero de elementos.</param>
        /// <param name="initDelegate">Delegado para inicializar los objetos del pool.s</param>
        public Pool(int initialSize, bool fixedSize, int generationSize, InitDelegate initDelegate)
        {
            mInitialSize = initialSize;
            mFixedSize = fixedSize;
            mGenerationSize = generationSize;
            mInitDelegate = initDelegate;

            mPool = new Queue<T>(mInitialSize);
            CreateNewGeneration(mInitialSize);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Crea una generacion de nuevos objetos del tamanno especificado.
        /// </summary>
        /// <param name="size">Numero de elementos de la nueva generacion.</param>
        private void CreateNewGeneration(int size)
        {
            for (int i = 0; i < size; i++)
            {
                mPool.Enqueue(mInitDelegate());
            }
        }
        /// <summary>
        /// Obtiene un elemento del pool.
        /// </summary>
        /// <returns>Elemento del pool si quedan objetos dentro (o si puede crecer de tamanno), default(T) en caso contrario.</returns>
        public T Get()
        {
            lock (this)
            {
                if (Count == 0)
                {
                    if (mFixedSize)
                    {
                        return default(T);
                    }
                    else
                    {
                        CreateNewGeneration(mGenerationSize);
                    }
                }

                return mPool.Dequeue();
            }
        }

        /// <summary>
        /// Devuelve el objeto al pool.
        /// </summary>
        /// <param name="item">Objeto a devolver.</param>
        public void Release(T item)
        {
            lock (this)
            {
                mPool.Enqueue(item);
            }
        }
        #endregion
    }
}
