using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Mouse
{
    /// <summary>
    /// Interfaz del dispositivo Mouse
    /// </summary>
    public interface IMouse: IDevice
    {
        #region Properties
        #region Buttons
        IDigitalControl LeftButton { get; }
        IDigitalControl MiddleButton { get; }
        IDigitalControl RightButton { get; }
        IDigitalControl X1Button { get; }
        IDigitalControl X2Button { get; }
        #endregion

        #region Position
        IPositionControl Position { get; }
        #endregion

        #region Scroll
        IScrollControl Wheel { get; }
        #endregion

        /// <summary>
        /// Estado actual del control.
        /// </summary>
        MouseState State { get; }

        /// <summary>
        /// Estado del control en la actualizacion anterior.
        /// </summary>
        MouseState PreviousState { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Establece la posicion del cursor en pantalla.
        /// </summary>
        /// <param name="x">Coordenada X</param>
        /// <param name="y">Coordenada Y</param>
        void SetPosition(int x, int y);
        /// <summary>
        /// Establece la posicion del cursor en pantalla.
        /// </summary>
        /// <param name="position">Vector con las coordenadas X,Y</param>
        void SetPosition(Vector2 position);
        #endregion
    }
}
