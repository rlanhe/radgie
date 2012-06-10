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
    public class Digitals2AnalogicalAdapter: IAnalogicalControl
    {
        #region Properties

        /// <summary>
        /// Logger de clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(Digitals2AnalogicalAdapter));
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
        private IDigitalControl mNegativeControl = null;
        /// <summary>
        /// Control que adapta.
        /// </summary>
        private IDigitalControl mPositiveControl = null;
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
        public Digitals2AnalogicalAdapter(IDigitalControl negativeControl, IDigitalControl positiveControl, float minValue, float maxValue, float gain)
        {
            System.Diagnostics.Debug.Assert((negativeControl != null) && (positiveControl != null));
            System.Diagnostics.Debug.Assert(minValue < maxValue);
            System.Diagnostics.Debug.Assert(gain > 0.0f);

            mNegativeControl = negativeControl;
            mPositiveControl = positiveControl;
            mGain = gain;
            mMinValue = minValue;
            mMaxValue = maxValue;
            mLastTimeUpdated = mInputSystem.LastTimeUpdated.TotalGameTime;
        }

		/// <summary>
		/// Construye un nuevo adaptador
		/// </summary>
		/// <param name="dControl">Control digital que adapta</param>
        public Digitals2AnalogicalAdapter(IDigitalControl negativeControl, IDigitalControl positiveControl)
            : this(negativeControl, positiveControl, -0.1f, 1.0f, 0.1f)
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
                // No se usa elapsed time, ya que la consulta del valor se hace desde otros subsistemas que pueden ir a distinto rate, por lo que
                // hay que calcularlo sobre la marcha
                // TODO: Este sistema no es preciso, aunque minimiza problemas. Se necesitaria acceder a el gameTime actual y no al del sistema de input (que pudo actualizarse tiempo atras)
                float delta = CalculateDelta(systemLastTimeUpdated.TotalGameTime.TotalMilliseconds - mLastTimeUpdated.TotalMilliseconds);
                mValue += delta;
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
        protected virtual float CalculateDelta(double timeElapsed)
        {
            float gain = mGain * (float)timeElapsed/1000.0f;
            if ((mPreviousValue != 0.0f) && (!mPositiveControl.Pressed) && (!mNegativeControl.Pressed))
            {
                if (Math.Abs(mPreviousValue) < Math.Abs(gain))
                {
                    return -mPreviousValue;
                }
                else
                {
                    return mPreviousValue > 0.0f ? -gain : gain;
                }
            }
            else if((mPositiveControl.Pressed) && (mNegativeControl.Pressed))
            {
                return 0.0f;
            }
            else
            {
                if (mPositiveControl.Pressed)
                {
                    return gain;
                }
                else if (mNegativeControl.Pressed)
                {
                    return -gain;
                }
            }
            return 0.0f;
        }
        #endregion
    }
}
