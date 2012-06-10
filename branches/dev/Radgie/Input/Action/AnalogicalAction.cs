using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
#if WIN32
using log4net;
#endif

namespace Radgie.Input.Action
{
    /// <summary>
    /// Accion analogica a la que asociar acciones.
    /// </summary>
    public class AnalogicalAction: AAction<IAnalogicalControl>, IAnalogicalControl
    {
        #region Properties

        /// <summary>
        /// Logger de la clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(AnalogicalAction));
        #endif

        #region IAnalogicalControl Properties
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.Value"/>
        /// </summary>
        public float  Value
        {
	        get
            {
                bool method = Math.Abs(mPreviousValue) > mMinDelta;
                float newValue = 0.0f;
                float diff = float.MaxValue;
                // Devuelve el control cuyo valor sea mayor que minDelta y este mas proximo a PreviousValue
                foreach (IAnalogicalControl control in mBindings)
                {
                    float value = control.Value;
                    if (method)
                    {
                        // Se queda con el que este mas proximo a el valor anterior
                        float newDiff = Math.Abs(mPreviousValue - value);
                        if(newDiff <= diff)
                        {
                            diff = newDiff;
                            newValue = value;
                        }
                    }
                    else
                    {
                        // Se queda con el mayor
                        if (Math.Abs(value) >= Math.Abs(newValue))
                        {
                            newValue = value;
                        }
                    }
                }

                mPreviousValue = Math.Abs(mDefaultValue) > Math.Abs(newValue) ? mDefaultValue : newValue;
                return mPreviousValue;
            }
        }

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
        /// Por debajo de este valor se considera que ningun control ha sido activado.
        /// </summary>
        private float mMinDelta = 0.0f;
        /// <summary>
        /// Valor por defecto para la accion cuando ningun control ha sido activado.
        /// </summary>
        private float mDefaultValue = 0.0f;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva accion.
        /// </summary>
        /// <param name="minDelta">Valor por debajo del cual no se considera que un control haya sido activado.</param>
        /// <param name="defaultValue">Valor por defecto cuando ningun control ha sido activado.</param>
        public AnalogicalAction(float minDelta, float defaultValue)
        {
            if (defaultValue > minDelta)
            {
                throw new ArgumentException("defaultValue > minDelta");
            }

            mMinDelta = Math.Abs(minDelta);
            mDefaultValue = defaultValue;
        }
        #endregion
    }
}
