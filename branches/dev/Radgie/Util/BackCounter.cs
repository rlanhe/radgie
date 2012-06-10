using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util
{
    /// <summary>
    /// Timer para realizar cuentas atras.
    /// </summary>
    public class BackCounter
    {
        #region Properties
        /// <summary>
        /// Timer interno.
        /// </summary>
        private Timer mTimer;

        /// <summary>
        /// Tiempo transcurrido.
        /// </summary>
        public TimeSpan TimeElapsed
        {
            get
            {
                if (mTimer == null)
                {
                    return TimeSpan.Zero;
                }
                return mTimer.GetTotalTime();
            }
        }

        /// <summary>
        /// Estado del Timer.
        /// </summary>
        public Radgie.Util.Timer.TimerState State
        {
            get
            {
                if (mTimer != null)
                {
                    return mTimer.State;
                }
                return Timer.TimerState.STOPPED;
            }
        }

        /// <summary>
        /// Duraccion de la cuenta atras.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return mDuration;
            }
        }
        private TimeSpan mDuration;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo BackCounter de una duracion determinada.
        /// </summary>
        /// <param name="duration">Duracion del BackCounter.</param>
        public BackCounter(TimeSpan duration)
        {
            mDuration = duration;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Comienza la cuenta atras.
        /// </summary>
        public void Start()
        {
            mTimer = Timer.StartNew();
        }

        /// <summary>
        /// Consulta si termino la ejecucion de la cuenta atras.
        /// </summary>
        /// <returns>True si termino, False en caso contrario.</returns>
        public bool Finished()
        {
            return mDuration <= TimeElapsed;
        }
        #endregion
    }
}
