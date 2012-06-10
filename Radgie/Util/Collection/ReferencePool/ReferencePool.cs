using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Util.Collection.ReferencePool
{
    /// <summary>
    /// Pool de referencias a objetos.
    /// </summary>
    /// <typeparam name="T">Tipo de los objetos.</typeparam>
    public class ReferencePool<T>: IReferencePool<T>
    {
        #region Properties
        /// <summary>
        /// Referencias de la coleccion.
        /// </summary>
        private IDictionary<int, WeakReference> mReferences;
        /// <summary>
        /// Lista de trabajos para actualizar los items del pool.
        /// </summary>
        private Job[] mUpdateJobs;
        /// <summary>
        /// Items del pool.
        /// </summary>
        private KeyValuePair<int, WeakReference>[] mItems;
        private int mLastIndex;
        private int mItemsPerThread;
        private PoolAction<T> mAction;
        private bool mThreaded;
        private object mSync = new object();
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa un nuevo pool de referencias.
        /// </summary>
        public ReferencePool(bool threaded)
        {
            mThreaded = threaded;
            mReferences = new Dictionary<int, WeakReference>();

            if (mThreaded)
            {
                mUpdateJobs = new Job[RadgieGame.Instance.JobScheduler.NumberOfThreads];
                for (int i = 0; i < mUpdateJobs.Length; i++)
                {
                    mUpdateJobs[i] = new Job(FireActionOverItems);
                }
            }
        }
        #endregion

        #region Methods
        #region IReferencePool Methods
        
        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.ReferencePool.IReferencePool.Add"/>
        /// </summary>
        public void Add(T item)
        {
            lock (mSync)
            {
                int itemHash = item.GetHashCode();
                WeakReference wRef;
                mReferences.TryGetValue(itemHash, out wRef);
                if (wRef == null)
                {
                    mReferences.Add(itemHash, new WeakReference(item));
                }
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.ReferencePool.IReferencePool.FireActionOverPoolItems"/>
        /// </summary>
        public void FireActionOverPoolItems(PoolAction<T> action)
        {
            lock (mSync)
            {
                mAction = action;
                if (mThreaded)
                {
                    mItems = mReferences.ToArray();
                    mLastIndex = 0;
                    mItemsPerThread = mItems.Length / RadgieGame.Instance.JobScheduler.NumberOfThreads;

                    if (mItems.Length != 0)
                    {
                        foreach (Job job in mUpdateJobs)
                        {
                            RadgieGame.Instance.JobScheduler.AddJob(job);
                        }

                        foreach (Job job in mUpdateJobs)
                        {
                            job.Wait();
                        }
                    }
                }
                else
                {
                    List<KeyValuePair<int, WeakReference>> referencesToUpdate = mReferences.ToList();
                    foreach (KeyValuePair<int, WeakReference> tuple in referencesToUpdate)
                    {
                        WeakReference wRef = tuple.Value;
                        if ((wRef.IsAlive) && (wRef.Target != null))
                        {
                            action((T)wRef.Target);
                        }
                        else
                        {
                            mReferences.Remove(tuple.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para la actualizacion parcial de los items del pool.
        /// </summary>
        private void FireActionOverItems()
        {
            int start;
            int end;

            lock (mItems)
            {
                start = mLastIndex;
                end = start + mItemsPerThread;
                
                // Si se pasa, o si no quedan mas hilos, procesa hasta el ultimo del array
                int leftCount = mItems.Length - end;
                if((end >= mItems.Length) || (leftCount != 0))
                {
                    end = mItems.Length;
                }

                mLastIndex = end;
            }

            for (int i = start; i < end; i++)
            {
                WeakReference wRef = mItems[i].Value;
                if ((wRef.IsAlive) && (wRef.Target != null))
                {
                    mAction((T)wRef.Target);
                }
                else
                {
                    mReferences.Remove(mItems[i].Key);
                }
            }
        }
        #endregion
        #endregion
    }
}
