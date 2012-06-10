using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radgie.Core
{
    /// <summary>
    /// Trabajo para ejecutar por JobScheduler al que se le pueden pasar parametros.
    /// </summary>
    public class ParametrizedJob: Job
    {
        #region Delegates
        /// <summary>
        /// Definicion del delegado donde se definen las tareas a ejecutar.
        /// </summary>
        public delegate void ParametrizedJobDelegate(params object[] parameters);
        #endregion

        #region Properties
        /// <summary>
        /// Trabajo que debe ejecutar.
        /// </summary>
        private ParametrizedJobDelegate mWork;

        /// <summary>
        /// Parametros de invocacion del trabajo.
        /// </summary>
        public object[] Parameters
        {
            get
            {
                return mParameters;
            }
            set
            {
                mParameters = value;
            }
        }
        private object[] mParameters;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo trabajo.
        /// </summary>
        /// <param name="job">Tareas a ejecutar dentro del trabjo.</param>
        public ParametrizedJob(ParametrizedJobDelegate job)
        {
            mWork = job;
        }

        /// <summary>
        /// Ejecuta el trabajo.
        /// </summary>
        public override void Execute()
        {
            mWork(mParameters);

            MarkAsFinished();
        }
        #endregion
    }
}
