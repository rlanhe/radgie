using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;

namespace AsteroidsStorm.GameComponents.FreeCamera
{
    /// <summary>
    /// Controlador para manejar la camara FreeCamera.
    /// </summary>
    public interface IFreeCameraInputController
    {
        /// <summary>
        /// Mover lateralmente a la izquierda.
        /// </summary>
        DigitalAction Left { get; }
        /// <summary>
        /// Mover lateralmente a la derecha.
        /// </summary>
        DigitalAction Right { get; }
        /// <summary>
        /// Mover hacia delante.
        /// </summary>
        DigitalAction Forward { get; }
        /// <summary>
        /// Mover hacia atras
        /// </summary>
        DigitalAction Backward { get; }

        /// <summary>
        /// Activa/Desactiva la rotacion de la camara.
        /// </summary>
        DigitalAction RotationActivated { get; }
        /// <summary>
        /// Resetea la rotacion aplicada a la camara.
        /// </summary>
        DigitalAction RotationReset { get; }
        /// <summary>
        /// Rotacion de la camara.
        /// </summary>
        PositionAction Rotation { get; }
    }
}
