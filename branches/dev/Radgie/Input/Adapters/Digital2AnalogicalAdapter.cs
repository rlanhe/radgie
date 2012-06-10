using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;
#if WIN32
using log4net;
#endif

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Adapta un control digital a un control analogico.
    /// </summary>
    public class Digital2AnalogicalAdapter: IAnalogicalControl
    {
        #region Properties

        /// <summary>
        /// Logger de clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(Digital2AnalogicalAdapter));
        #endif

        #region IAnalogicalControl Properties
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.Value"/>
        /// </summary>
        public float Value
        {
            get
            {
                Update();
                return mValue;
            }
        }
        private float mValue = 0.0f;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.PreviousValue"/>
        /// </summary>
        public float PreviousValue
        {
            get
            {
                Update();
                return mPreviousValue;
            }
        }
        private float mPreviousValue = 0.0f;
        #endregion

        /// <summary>
        /// Control que adapta.
        /// </summary>
        private IDigitalControl mDControl = null;
        /// <summary>
        /// Constante que representa la ganancia del control por cada segundo que esta pulsado.
        /// </summary>
        private float mGain = 1.0f;
        /// <summary>
        /// Referencia al sistema de entrada/salida.
        /// </summary>
        private IInputSystem mInputSystem = (IInputSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IInputSystem));
        /// <summary>
        /// ultima vez que el control fue actualizado.
        /// </summary>
        private TimeSpan mLastTimeUpdated;
        /// <summary>
        /// El valor del control analogico es positivo.
        /// </summary>
        private bool mPositive = true;
        /// <summary>
        /// Valor minimo que puede tomar el control analogico.
        /// </summary>
        private float mMinValue = 0.0f;
        /// <summary>
        /// Valor maximo que puede tomar el control analogico.
        /// </summary>
        private float mMaxValue = 1.0f;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye un nuevo adapatador.
        /// </summary>
        /// <param name="dControl">Control digital que adapta.</param>
        /// <param name="positive">Indica si el valor de la ganancia debe ser positivo o negativo</param>
        /// <param name="minValue">Valor minimo que puede tomar el control.</param>
        /// <param name="maxValue">Valor maximo que puede tomar el control.</param>
        /// <param name="gain">Ganancia del control cuando esta pulsado.</param>
        public Digital2AnalogicalAdapter(IDigitalControl dControl, bool positive, float minValue, float maxValue, float gain)
        {
            System.Diagnostics.Debug.Assert(dControl != null);
            System.Diagnostics.Debug.Assert(minValue < maxValue);
            System.Diagnostics.Debug.Assert(gain > 0.0f);

            mDControl = dControl;
            mGain = gain;
            mPositive = positive;
            mMinValue = minValue;
            mMaxValue = maxValue;
            mLastTimeUpdated = mInputSystem.LastTimeUpdated.TotalGameTime;
        }

		/// <summary>
		/// Construye un nuevo adaptador
		/// </summary>
		/// <param name="dControl">Control digital que adapta</param>
        public Digital2AnalogicalAdapter(IDigitalControl dControl): this(dControl, true, 0.0f, 1.0f, 0.1f)
        {
        }
        #endregion

        #region Methods
		/// <summary>
		/// Actualiza los valores del adaptador.
		/// </summary>
        private void Update()
        {
            GameTime systemLastTimeUpdated = mInputSystem.LastTimeUpdated;

			// Se volvio a actualizar el dispositivo
            if (systemLastTimeUpdated.TotalGameTime > mLastTimeUpdated)
            {
                mPreviousValue = mValue;
                float delta = CalculateDelta(systemLastTimeUpdated.ElapsedGameTime.Milliseconds);
                mValue = mPositive ? mValue + delta : mValue - delta;
                if (mValue > mMaxValue)
                {
                    mValue = mMaxValue;
                }
                if (mValue < mMinValue)
                {
                    mValue = mMinValue;
                }
                mLastTimeUpdated = systemLastTimeUpdated.TotalGameTime;
            }
        }

		//TODO: Usar delegado en lugar de sobrecargar
		/// <summary>
		/// Calcula el delta en funcion de la ganancia y el tiempo.
		/// Puede sobrecargarse si se desea un comportamiento distinto.
		/// </summary>
		/// <param name="timeElapsed">ms transcurridos desde la ultima actualizacion.</param>
		/// <returns>Ganancia del control.</returns>
        protected virtual float CalculateDelta(float timeElapsed)
        {
            float gain = mDControl.Pressed ? mGain : -mGain;
            return gain * (timeElapsed/ 1000.0f);
        }
        #endregion
    }
}
