using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Adapta un control analogico a un control digital.
    /// </summary>
    public class Analogical2DigitalAdapter: IDigitalControl
    {
        #region Properties
        #region IDigitalControl member
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.Pressed"/>
        /// </summary>
        public bool Pressed
        {
            get
            {
                return CalculateValue(mAControl.Value);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.PreviousValue"/>
        /// </summary>
        public bool PreviousValue
        {
            get
            {
                return CalculateValue(mAControl.PreviousValue);
            }
        }
        #endregion
        /// <summary>
        /// Control que adapta.
        /// </summary>
        protected IAnalogicalControl mAControl = null;
        /// <summary>
        /// Valor a partir del cual se considera que el control digital esta activado.
        /// </summary>
        protected float mTrueValue = 0.0f;
        /// <summary>
        /// Indica si el valor a partir del cual se considera que el control digital esta activado es positivo o negativo.
        /// </summary>
        private bool mPositiveValue = true;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el adaptador.
        /// </summary>
        /// <param name="aControl">Control analogico que encapsula</param>
        public Analogical2DigitalAdapter(IAnalogicalControl aControl): this(aControl, 0.5f)
        {
        }

        /// <summary>
        /// Crea e inicializa el adaptador.
        /// </summary>
        /// <param name="aControl">Control analogico que encapsula</param>
        /// <param name="trueValue">Valor del controlador analogico a partir del cual se considera el valor digital true</param>
        /// <exception cref="ArgumentNullException">Si aControl es null</exception>
        public Analogical2DigitalAdapter(IAnalogicalControl aControl, float trueValue)
        {
            if (aControl == null)
            {
                throw new ArgumentNullException("aControl is null");
            }

            mAControl = aControl;
            mTrueValue = trueValue;
            mPositiveValue = mTrueValue > 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Determina el valor del control digital a partir del valor del control analogico.
        /// </summary>
        /// <param name="value">Valor del control analogico.</param>
        /// <returns>Valor del control digital.</returns>
        private bool CalculateValue(float value)
        {
            if (((mPositiveValue) && (value >= mTrueValue)) || ((!mPositiveValue) && (value <= mTrueValue)))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
