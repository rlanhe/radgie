using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Adapta un control de direccion a un control analogico.
    /// </summary>
    public class AnalogicalDirectionControl2AnalogicalAdapter: IAnalogicalControl
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
                return GetValue(true);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalControl.PreviousValue"/>
        /// </summary>
        public float PreviousValue
        {
            get
            {
                return GetValue(false);
            }
        }
        #endregion

        /// <summary>
        /// Control que adapta.
        /// </summary>
        private IAnalogicalDirectionControl mADControl = null;
        /// <summary>
        /// Componente del control de direccion que adapta.
        /// </summary>
        private Component mComponent = Component.X;

        /// <summary>
        /// Enumeracion para identificar las componentes de un control de direccion.
        /// </summary>
        public enum Component
        {
            X,
            Y
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Construye un nuevo adaptador.
        /// </summary>
        /// <param name="aDControl">Control de direccion que adapta.</param>
        /// <param name="component">Componente del control de direccion que adapta.</param>
        /// <exception cref="ArgumentNullException">Si aDControl es null</exception>
        public AnalogicalDirectionControl2AnalogicalAdapter(IAnalogicalDirectionControl aDControl, Component component)
        {
            if (aDControl == null)
            {
                throw new ArgumentNullException("aDControl is null");
            }

            mADControl = aDControl;
            mComponent = component;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el valor de la componente.
        /// </summary>
        /// <param name="lastValue">Si es True, devuelve el valor actual, si es False el del frame anterior.</param>
        /// <returns>Valor de la componente del control de direccion.</returns>
        private float GetValue(bool lastValue)
        {
            float value = 0.0f;
            switch (mComponent)
            {
                case Component.X:
                    value = lastValue ? mADControl.X : mADControl.PreviousX;
                    break;
                case Component.Y:
                    value = lastValue ? mADControl.Y : mADControl.PreviousY;
                    break;
            }
            return value;
        }
        #endregion
    }
}
