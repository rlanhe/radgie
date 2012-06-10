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
    public class RadgieStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Tiempo desde que comienza a actualizarse hasta que termina.
        /// </summary>
        public int FPS
        {
            get
            {
                return mAvgFPS;
            }
        }
        private int mAvgFPS;
        private int mFPS;

        public double mFPS_Timer;
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa el objeto para registrar estadisticas.
        /// </summary>
        public RadgieStatistics()
        {
        }
        #endregion

        #region Methods
        public void AddFrame(double elapsedTime)
        {
            mFPS++;

            mFPS_Timer += elapsedTime;
            if(1.0d < mFPS_Timer)
            {
                mAvgFPS = mFPS;
                mFPS = 0;
                mFPS_Timer = 0.0d;
            }
        }
        #endregion
    }
}
