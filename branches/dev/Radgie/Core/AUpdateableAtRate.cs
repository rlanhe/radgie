using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if WIN32
using log4net;
#endif

namespace Radgie.Core
{
    /// <summary>
    /// Abstrae la funcionalidad comun de objetos que necesitan actualizarse con una frecuencia constante en el tiempo.
    /// </summary>
    public abstract class AUpdateableAtRate: IUpdateable
    {
        #region Properties

        /// <summary>
        /// Logger de la clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(AUpdateableAtRate));
        #endif

        /// <summary>
        /// Si actualiza o no en funcion del valor de este atributo.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Tiempo minimo en ms entre dos actualizaciones consecutivas.
        /// </summary>
        public double UpdateRate
        {
            get
            {
                return mUpdateRate;
            }
            set
            {
				if (value <= 0.0d)
				{
					throw new ArgumentException("UpdateRate must be greater than 0.0f");
				}
                mUpdateRate = value;
                mMSBetweenUpdates = 1000.0d / mUpdateRate;
            }
        }
        private double mUpdateRate = 0.0d;

        /// <summary>
        /// ms entre dos actualizaciones consecutivas.
        /// </summary>
        public double MSBetweenUpdates
        {
            get
            {
                return mMSBetweenUpdates;
            }
        }
        private double mMSBetweenUpdates = 1000.0d/60.0d;

        /// <summary>
        /// ms desde la ultima actualizacion.
        /// </summary>
        public double MSFromLastUpdateActionCall
        {
            get
            {
                return mMSFromLastUpdateActionCall;
            }
        }
        private double mMSFromLastUpdateActionCall = 0;

		/// <summary>
		/// Estrutura de tiempo que recibio como argumento la ultima vez que fue actualizado.
		/// </summary>
        public GameTime LastTimeUpdated
        {
            get
            {
                return mLastTimeUpdated;
            }
        }
        private GameTime mLastTimeUpdated = null;

		/// <summary>
		/// Valor por defecto usado para inicializar LastTimeUpdated al inicio
		/// </summary>
		private static GameTime START_TIME = new GameTime();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor por defecto.
        /// Establece que el sistema se actualice 60 veces por segundo.
        /// </summary>
        protected AUpdateableAtRate(): this(60.0d)
        {
        }

        /// <summary>
        /// Constructor.
        /// Establece el rate de actualizacion del sistema.
        /// </summary>
        /// <param name="rate">Numero de veces que se quiere actualizar el sistema por segundo</param>
        public AUpdateableAtRate(double rate)
        {
			mLastTimeUpdated = START_TIME;
            UpdateRate = rate;
            Active = true;
        }

        #endregion

        #region Methods

        #region IUpdateable Methods
        /// <summary>
        /// Actualiza el sistema si ha pasado el tiempo suficiente.
        /// </summary>
        /// <param name="time">Informacion sobre el tiempo transcurrido desde la ultima actualizacion.</param>
        public void Update(GameTime time)
        {
            double msFromLastUpdateCall = time.ElapsedGameTime.TotalMilliseconds;
            mMSFromLastUpdateActionCall += msFromLastUpdateCall;

            if (Active)
            {
                // Si transcurrio el tiempo necesario desde la ultima actualizacion, actualiza el sistema
                if (mMSFromLastUpdateActionCall >= mMSBetweenUpdates)
                {
                    GameTime realTime = new GameTime(time.TotalGameTime, new TimeSpan(0, 0, 0, 0, (int)mMSFromLastUpdateActionCall));
                    mLastTimeUpdated = realTime;
                    // Actualiza el sistema. El tiempo en ms desde la ultima actualizacion se encuentra en msFromLastUpdateActionCall
                    UpdateAction(realTime);

                    mMSFromLastUpdateActionCall = 0;
                }
            }
        }
        #endregion

        /// <summary>
        /// Actualiza el estado del sistema.
        /// Implementado por las clases derivadas de esta.
        /// </summary>
        /// <param name="time">Informacion temporal de la aplicacion.</param>
        protected abstract void UpdateAction(GameTime time);

        #endregion
    }
}
