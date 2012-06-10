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
    // TODO: Cada vez que se accede a el valor se actualiza. Deberia hacerlo solo cuando se ha modificado el estado del controlador
    /// <summary>
    /// Accion de posicion a la que asociar acciones.
    /// </summary>
    public class PositionAction: AAction<IPositionControl>, IPositionControl
    {
        #region Properties
        /// <summary>
        /// Logger de clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(PositionAction));
        #endif

        #region IPositionControl Properties
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.X"/>
        /// </summary>
        public int X
        {
            get
            {
                Update();
                return mX;
            }
        }
        private int mX = 0;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.Y"/>
        /// </summary>
        public int Y
        {
            get
            {
                Update();
                return mY;
            }
        }
        private int mY = 0;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.PreviousX"/>
        /// </summary>
        public int PreviousX
        {
            get
            {
                Update();
                return mPreviousX;
            }
        }
        private int mPreviousX = 0;

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.PreviousY"/>
        /// </summary>
        public int PreviousY
        {
            get
            {
                Update();
                return mPreviousY;
            }
        }
        private int mPreviousY = 0;
        #endregion
        /// <summary>
        /// Referencia al sistema de input.
        /// </summary>
        protected static IInputSystem mInputSystem;
        /// <summary>
        /// ultima vez que se actualizo.
        /// </summary>
        protected TimeSpan mLastTimeUpdated;
        #endregion

        #region Constructors
        /// <summary>
        /// Accion de posicion por defecto.
        /// </summary>
        public PositionAction()
        {
            mInputSystem = (IInputSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IInputSystem));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Actualiza el estado del controlador.
        /// </summary>
        private void Update()
        {
            if (mLastTimeUpdated < mInputSystem.LastTimeUpdated.TotalGameTime)
            {
                mLastTimeUpdated = mInputSystem.LastTimeUpdated.TotalGameTime;
                
                int maxX = 0;
                int maxY = 0;

                foreach (IPositionControl control in mBindings)
                {
                    if(control.X > maxX)
                    {
                        maxX = control.X;
                    }

                    if(control.Y > maxY)
                    {
                        maxY = control.Y;
                    }
                }

                mPreviousX = mX;
                mX = maxX;

                mPreviousY = mY;
                mY = maxY;
            }
        }
        #endregion
    }
}
