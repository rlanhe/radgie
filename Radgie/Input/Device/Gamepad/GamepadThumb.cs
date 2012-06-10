using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Control que representa un Thumb del Gamepad.
    /// </summary>
    public class GamepadThumb : ADeviceControl<IGamepad>, IAnalogicalDirectionControl
    {
        #region Delegates and events
        /// <summary>
        /// Definicion del delegado para la actualizacion del estado del control.
        /// </summary>
        /// <param name="state">Estado del dispositivo.</param>
        /// <returns>Valores de los ejes X e Y del control.</returns>
        public delegate Vector2 GamepadThumbDelegate(GamePadState state);
        #endregion

        #region Properties
        #region Values
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalDirectionControl.X"/>
        /// </summary>
        public float X
        {
            get
            {
                return mGetter(mParent.State).X;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalDirectionControl.Y"/>
        /// </summary>
        public float Y
        {
            get
            {
                return mGetter(mParent.State).Y;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalDirectionControl.PreviousX"/>
        /// </summary>
        public float PreviousX
        {
            get
            {
                return mGetter(mParent.PreviousState).X;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IAnalogicalDirectionControl.PreviousY"/>
        /// </summary>
        public float PreviousY
        {
            get
            {
                return mGetter(mParent.PreviousState).Y;
            }
        }

        #endregion

        protected GamepadThumbDelegate mGetter = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa un control Thumb.
        /// </summary>
        /// <param name="parent">Dispositivo padre del control.</param>
        /// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
        public GamepadThumb(IGamepad parent, GamepadThumbDelegate getter): base(parent)
        {
            mGetter = getter;
        }
        #endregion
    }
}
