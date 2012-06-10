using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Gamepad
{
    /// <summary>
    /// Interfaz del dispositivo Gamepad
    /// </summary>
    public interface IGamepad: IDevice
    {
        #region Properties
        #region Buttons
        IDigitalControl AButton { get; }
        IDigitalControl BButton { get; }
        IDigitalControl XButton { get; }
        IDigitalControl YButton { get; }
        IDigitalControl BackButton { get; }
        IDigitalControl StartButton { get; }
        IDigitalControl LeftShoulderButton { get; }
        IDigitalControl RightShoulderButton { get; }
        IDigitalControl LeftStickButton { get; }
        IDigitalControl RightStickButton { get; }
        IDigitalControl BigButton { get; }
        #endregion

        #region Triggers
        IAnalogicalControl LeftTrigger { get; }
        IAnalogicalControl RightTrigger { get; }
        #endregion

        #region Directional Pad
        GamepadDPad DPad { get; }
        #endregion

        #region Thumbs
        IAnalogicalDirectionControl LeftThumb { get; }
        IAnalogicalDirectionControl RightThumb { get; }
        #endregion

        /// <summary>
        /// Estado actual del dispositivo.
        /// </summary>
        GamePadState State { get; }

        /// <summary>
        /// Estado del dispositivo en el frame anterior.
        /// </summary>
        GamePadState PreviousState { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Establece la vibracion de los motores del gamepad.
        /// </summary>
        /// <param name="leftMotor">Valor entre 0.0f y 1.0f para el motor de baja frecuencia.</param>
        /// <param name="rightMotor">Valor entre 0.0f y 1.0f para el motor de alta frecuencia.</param>
        /// <returns>True si se establecieron los valores correctamente, False en caso contrario./returns>
        bool SetVibration(float leftMotor, float rightMotor);
        #endregion
    }
}
