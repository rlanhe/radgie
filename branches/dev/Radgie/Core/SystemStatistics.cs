using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util;

namespace Radgie.Core
{
    /// <summary>
    /// Clase para registrar estadisticas de los sistemas con el proposito de mejorar su rendimiento.
    /// </summary>
    public class SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Tiempo desde que comienza a actualizarse hasta que termina.
        /// </summary>
        public TimeSpan UpdateTime
        {
            get
            {
                return mUpdateTimer.GetTotalTime();
            }
        }

        /// <summary>
        /// Timer para registrar el tiempo de actualizacion.
        /// </summary>
        private Timer mUpdateTimer;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el objeto para registrar estadisticas.
        /// </summary>
        public SystemStatistics()
        {
            mUpdateTimer = Timer.StartNew();
            mUpdateTimer.Stop();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Pone a cero el temporizador y lo arranca para tomar tiempos.
        /// </summary>
        public void StartUpdateTimer()
        {
            if (mUpdateTimer.State == Timer.TimerState.STOPPED)
            {
                mUpdateTimer.Start();
            }
        }

        /// <summary>
        /// Para el temporizador.
        /// </summary>
        public void StopUpdateTimer()
        {
            if (mUpdateTimer.State == Timer.TimerState.RUNNING)
            {
                mUpdateTimer.Stop();
            }
        }
        #endregion
    }
}
