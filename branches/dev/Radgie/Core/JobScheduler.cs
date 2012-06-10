using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radgie.Core
{
    /// <summary>
    /// Gestor de trabajos para la ejecucion de trabajos de forma paralela.
    /// </summary>
    public class JobScheduler: IDisposable
    {
        #region Properties
        /// <summary>
        /// Numero de hilos hardware soportados.
        /// </summary>
        public int NumberOfThreads
        {
            get
            {
                return mNumberOfThreads;
            }
        }
        private int mNumberOfThreads;

        /// <summary>
        /// Trabajos encolados.
        /// </summary>
        private Queue<Job> mQueue;

        /// <summary>
        /// Sennal para despertar a los hilos cuando llega un nuevo trabajo.
        /// </summary>
        private EventWaitHandle mWakeUpSignal;

        /// <summary>
        /// Flag para indicar a los hilos que deben terminar.
        /// </summary>
        private bool mDispose;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el gestor de trabajos.
        /// </summary>
        public JobScheduler()
        {
            mQueue = new Queue<Job>();
            //mWakeUpSignal = new EventWaitHandle(false, EventResetMode.AutoReset);
            mWakeUpSignal = new AutoResetEvent(false);
#if XBOX360
            int[] xboxThreads = new int[]{3, 4, 5};
            mNumberOfThreads = 3;
            for(int i = 0; i < mNumberOfThreads; i++)
            {
                int threadId = xboxThreads[i];
                Thread thread = new Thread(new ThreadStart(
                    delegate ()
                    {
                        Thread.CurrentThread.SetProcessorAffinity(new int[]{threadId});
                        this.ThreadWork();
                    }
                ));
                thread.Start();
            }
#else
            mNumberOfThreads = System.Environment.ProcessorCount;
            for (int i = 0; i < mNumberOfThreads; i++)
            {
                Thread thread = new Thread(new ThreadStart(this.ThreadWork));
                thread.Start();
            }
#endif
        }
        #endregion

        #region Methods
        
        #region API methods
        /// <summary>
        /// Encola un nuevo trabajo.
        /// </summary>
        /// <param name="job">Trabajo.</param>
        public void AddJob(Job job)
        {
            lock (mQueue)
            {
                job.Reset();
                mQueue.Enqueue(job);
                mWakeUpSignal.Set();
            }
        }

        public void AddJob(Job.JobDelegate jobDelegate)
        {
            AddJob(new Job(jobDelegate));
        }
        #endregion

        #region Thread methods
        /// <summary>
        /// Comportamiento de cada hilo de ejecucion.
        /// </summary>
        private void ThreadWork()
        {
            while (true)
            {
                if (mDispose)
                {
                    // Despierta a otro hilo y termina.
                    mWakeUpSignal.Set();
                    return;
                }
                if (mQueue.Count == 0)
                {
                    // Duerme a la espera de una nueva sennal.
                    mWakeUpSignal.WaitOne();
                }
                // Obtiene un trabajo para ejecutar.
                Job job = GetJob();
                if (job != null)
                {
                    job.Execute();
                }
            }
        }

        /// <summary>
        /// Obtiene un nuevo trabajo de la cola.
        /// </summary>
        /// <returns>El trabajo a ejecutar o null en caso de que no haya nuevos.</returns>
        private Job GetJob()
        {
            Job job = null;
            lock (mQueue)
            {
                if (mQueue.Count > 0)
                {
                    job = mQueue.Dequeue();
                }
            }
            return job;
        }
        #endregion

        #region IDisposable Methods
        /// <summary>
        /// Ver <see cref="system.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            mDispose = true;
            mWakeUpSignal.Set();
        }

        #endregion
        #endregion
    }
}
