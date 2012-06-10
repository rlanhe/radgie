using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Adapta un control analogico a un control analogico con valores acotados.
    /// </summary>
    public class Analogical2AnalogicalAdapter: IAnalogicalControl
    {
        #region Properties
        #region IAnalogicalControl Properties
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.Value"/>
        /// </summary>
        public float Value
        {
            get
            {
                return GetValue(mAControl.Value);
            }
        }
        private float mValue;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.PreviousValue"/>
        /// </summary>
        public float PreviousValue
        {
            get
            {
                return mPreviousValue;
            }
        }
        private float mPreviousValue;
        #endregion

        /// <summary>
        /// Control analogico que adapta.
        /// </summary>
        private IAnalogicalControl mAControl = null;
        /// <summary>
        /// Valor minimo que puede tomar el control.
        /// </summary>
        private float mMin = 0.0f;
        /// <summary>
        /// Valor maximo que puede tomar el control.
        /// </summary>
        private float mMax = 1.0f;
        private float mGain;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un adaptador.
        /// </summary>
        /// <param name="aControl">Contorl analogico que adapta.</param>
        /// <param name="min">Valor minimo.</param>
        /// <param name="max">Valor maximo.</param>
        /// <param name="gain">Variación máxima.</param>
        /// <exception cref="ArgumentNullException">Si aControl es null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si min > max</exception>
        public Analogical2AnalogicalAdapter(IAnalogicalControl aControl, float min, float max): this(aControl, min, max, max)
        {
        }

        /// <summary>
        /// Crea un adaptador.
        /// </summary>
        /// <param name="aControl">Contorl analogico que adapta.</param>
        /// <param name="min">Valor minimo.</param>
        /// <param name="max">Valor maximo.</param>
        /// <param name="gain">Variación máxima.</param>
        /// <exception cref="ArgumentNullException">Si aControl es null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Si min > max</exception>
        public Analogical2AnalogicalAdapter(IAnalogicalControl aControl, float min, float max, float gain)
        {
            if (aControl == null)
            {
                throw new ArgumentNullException("aControl is null");
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min > max");
            }

            mAControl = aControl;
            mMin = min;
            mMax = max;
            mGain = gain;
        }
        #endregion

        #region Methods
        private TimeSpan mLastTimeUpdated;
        private static IInputSystem mInputSystem = (IInputSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IInputSystem));

        /// <summary>
        /// Acota el valor.
        /// </summary>
        /// <param name="value">Valor original.</param>
        /// <returns>Valor acotado.</returns>
        private float GetValue(float value)
        {
            GameTime systemLastTimeUpdated = mInputSystem.LastTimeUpdated;

            // Se volvio a actualizar el dispositivo
            if (systemLastTimeUpdated.TotalGameTime > mLastTimeUpdated)
            {
                mPreviousValue = mValue;

                float diff = mValue - value;
                if (Math.Abs(diff) > mGain)
                {
                    value = diff > 0 ? mValue - mGain : mValue + mGain;
                }

                if (value > mMax)
                {
                    mValue = mMax;
                }
                else if (value < mMin)
                {
                    mValue = mMin;
                }

                mValue = value;
                
                mLastTimeUpdated = systemLastTimeUpdated.TotalGameTime;
            }

            return mValue;
        }
        #endregion
    }
}
