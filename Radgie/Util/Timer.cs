using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util
{
    /// <summary>
    /// Clase para medir intervalos de tiempo.
    /// </summary>
    public class Timer
    {
        #region Properties
        /// <summary>
        /// Posibles estados del timer.
        /// </summary>
        public enum TimerState
        {
            RUNNING,
            STOPPED
        }

        /// <summary>
        /// Estado del timer.
        /// </summary>
        public TimerState State
        {
            get
            {
                return mState;
            }
        }
        protected TimerState mState;

        /// <summary>
        /// Tiempo en el que se arranco el timer.
        /// </summary>
        protected DateTime mStartTotalTime;
        /// <summary>
        /// Tiempo en el que se consulto por ultima vez el timer.
        /// </summary>
        protected DateTime mLastTotalTime;
        /// <summary>
        /// Intervalo de tiempo medido hasta ahora.
        /// </summary>
        protected DateTime mLastTime;
        #endregion

        #region Constructors
        /// <summary>
        /// Timer por defecto.
        /// </summary>
        private Timer()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Arranca el timer.
        /// </summary>
        public void Start()
        {
            mStartTotalTime = DateTime.Now;
            mLastTime = mStartTotalTime;
            mState = TimerState.RUNNING;
        }

        /// <summary>
        /// Para el timer.
        /// </summary>
        public void Stop()
        {
            mLastTotalTime = DateTime.Now;
            mState = TimerState.STOPPED;
        }

        /// <summary>
        /// Obtiene el tiempo desde que se arranco hasta ahora.
        /// </summary>
        /// <returns>Tiempo transcurrido hasta este momento.</returns>
        public TimeSpan GetTotalTime()
        {
            if (mState == TimerState.RUNNING)
            {
                mLastTotalTime = DateTime.Now;
            }
            return mLastTotalTime - mStartTotalTime;
        }

        /// <summary>
        /// Tiempo transcurrido desde la ultima vez que se consulto.
        /// </summary>
        /// <returns>Tiempo transcurrio desde la ultima vez que se consulto.</returns>
        public TimeSpan GetTime()
        {
            if (mState == TimerState.RUNNING)
            {
                mLastTotalTime = DateTime.Now;
            }
            TimeSpan diff = mLastTotalTime - mLastTime;
            mLastTime = mLastTotalTime;
            return diff;
        }

        /// <summary>
        /// Crea un nuevo timer y lo arranca.
        /// </summary>
        /// <returns>Nuevo timer ya incializado.</returns>
        public static Timer StartNew()
        {
            Timer timer = new Timer();
            timer.Start();
            return timer;
        }
        #endregion
    }
}
