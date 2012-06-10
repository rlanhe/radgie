using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Adapta un control analogico para obtener valores incrementales en lugar de absolutos.
    /// </summary>
    public class IncrementalAnalogicalAdpater: IAnalogicalControl
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
                float dif = mAControl.Value - mAControl.PreviousValue;
                if (dif != mPreviousValue)
                {
                    mPreviousValue = dif;
                }
                return dif;
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
        private float mPreviousValue = 0.0f;
        #endregion
        /// <summary>
        /// Control que adapta.
        /// </summary>
        private IAnalogicalControl mAControl = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye un nuevo adaptador.
        /// </summary>
        /// <param name="aControl">Control que adapta.</param>
        /// <exception cref="ArgumentNullException">Si aControl es null</exception>
        public IncrementalAnalogicalAdpater(IAnalogicalControl aControl)
        {
            if (aControl == null)
            {
                throw new ArgumentNullException("aControl is null");
            }
            mAControl = aControl;
            mPreviousValue = 0.0f;
        }

        #endregion
    }
}
