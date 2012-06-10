using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;

namespace Radgie.Input.Adapters
{
    /// <summary>
    /// Implementa un control digital que no admite repeticion de pulsaciones.
    /// </summary>
    public class DigitalOneShotAdapter: IDigitalControl
    {
        #region Properties
        #region IDigitalControl member
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.Pressed"/>
        /// </summary>
        public virtual bool Pressed
        {
            get
            {
                if(mDControl.Pressed)
                {
                    return !mDControl.PreviousValue;
                }
                return false;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IDigitalControl.PreviousValue"/>
        /// </summary>
        public virtual bool PreviousValue
        {
            get
            {
                return !Pressed;
            }
        }
        #endregion
        /// <summary>
        /// Control que adapta.
        /// </summary>
        protected IDigitalControl mDControl = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el adaptador.
        /// </summary>
        /// <param name="dControl">Control digital que encapsula</param>
        /// <exception cref="ArgumentNullException">Si dControl es null</exception>
        public DigitalOneShotAdapter(IDigitalControl dControl)
        {
            if (dControl == null)
            {
                throw new ArgumentNullException("dControl is null");
            }

            mDControl = dControl;
        }

        #endregion
    }
}
