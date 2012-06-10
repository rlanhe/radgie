using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Util;

namespace Radgie.Graphics
{
    /// <summary>
    /// Estadisticas del sistema grafico.
    /// </summary>
    public class GraphicsSystemStatistics: SystemStatistics
    {
        #region Properties
        /// <summary>
        /// Numero de objetos dibujados en la ultima pasada.
        /// </summary>
        public long NumberOfObjectsDrawed { get; set; }
        /// <summary>
        /// Numero de triangulos dibujados en la ultima pasada.
        /// </summary>
        public long NumberOfTrianglesDrawed { get; set; }
        /// <summary>
        /// Numero de llamadas Draw.
        /// </summary>
        public long NumberOfDrawCalls { get; set; }

        /// <summary>
        /// Tiempo empleado en dibujar en el ultimo frame.
        /// </summary>
        public TimeSpan DrawTime
        {
            get
            {
                return mDrawTimer.GetTotalTime();
            }
        }
        private Timer mDrawTimer;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un registro de estadisticas para el sistema de graficos.
        /// </summary>
        public GraphicsSystemStatistics()
        {
            mDrawTimer = Timer.StartNew();
            mDrawTimer.Stop();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Arranca el temporizador para medir el tiempo que emplea en dibujar.
        /// </summary>
        public void StartDrawTimer()
        {
            if (mDrawTimer.State == Timer.TimerState.STOPPED)
            {
                mDrawTimer.Start();
            }
        }

        /// <summary>
        /// Para el temporizador utilizado para medir el tiempo empleado en dibujar.
        /// </summary>
        public void StopDrawTimer()
        {
            if (mDrawTimer.State == Timer.TimerState.RUNNING)
            {
                mDrawTimer.Stop();
            }
        }
        
        /// <summary>
        /// Reinicializa los contadores.
        /// </summary>
        public void Reset()
        {
            NumberOfDrawCalls = 0;
            NumberOfTrianglesDrawed = 0;
            NumberOfObjectsDrawed = 0;
        }
        #endregion
    }
}
