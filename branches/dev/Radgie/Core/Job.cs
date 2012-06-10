using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radgie.Core
{
    /// <summary>
    /// Trabajo para ejecutar por JobScheduler.
    /// </summary>
    public class Job
    {
        #region Delegates
        /// <summary>
        /// Definicion del delegado donde se definen las tareas a ejecutar.
        /// </summary>
        public delegate void JobDelegate();
        #endregion

        #region Properties
        /// <summary>
        /// Trabajo que debe ejecutar.
        /// </summary>
        private JobDelegate mWork;

        /// <summary>
        /// Evento de espera a que termine el trabajo.
        /// </summary>
        protected EventWaitHandle mWakeUpSignal;

        /// <summary>
        /// Indica si la ejecucion del trabajo ha finalizado.
        /// </summary>
        public bool Finished
        {
            get
            {
                return mFinished;
            }
        }
        protected bool mFinished;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor interno de job para usar en clases derivadas.
        /// </summary>
        internal Job()
        {
            mWakeUpSignal = new AutoResetEvent(false);
        }
        /// <summary>
        /// Crea un nuevo trabajo.
        /// </summary>
        /// <param name="job">Tareas a ejecutar dentro del trabjo.</param>
        public Job(JobDelegate job): this()
        {
            mWork = job;
        }

        /// <summary>
        /// Resetea el estado del job para poder ser lanzado de nuevo.
        /// </summary>
        internal void Reset()
        {
            mFinished = false;
            mWakeUpSignal.Reset();
        }

        /// <summary>
        /// Ejecuta el trabajo.
        /// </summary>
        public virtual void Execute()
        {
            mWork();
            MarkAsFinished();
        }

        /// <summary>
        /// Marca el trabajo como finalizado.
        /// </summary>
        protected void MarkAsFinished()
        {
            mFinished = true;
            // Si hay algun hilo esperando, notifica la finalizacion.
            mWakeUpSignal.Set();
        }

        /// <summary>
        /// Metodo para esperar por el final del trabajo.
        /// </summary>
        public void Wait()
        {
            if (!Finished)
            {
                mWakeUpSignal.WaitOne();
            }
        }
        #endregion
    }
}
