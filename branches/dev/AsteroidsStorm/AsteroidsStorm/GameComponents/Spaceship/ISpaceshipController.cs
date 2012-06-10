using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;

namespace AsteroidsStorm.GameComponents.Spaceship
{
    /// <summary>
    /// Controlador de la nave espacial.
    /// </summary>
    interface ISpaceshipController
    {
        /// <summary>
        /// Control para mover la nave a izquierda y derecha.
        /// </summary>
        AnalogicalAction LeftRight { get; }
        /// <summary>
        /// Control para mover la nave arriba y abajo.
        /// </summary>
        AnalogicalAction UpDown { get; }
        /// <summary>
        /// Control para usar el bonificador.
        /// </summary>
        DigitalAction UseBonus { get; }
        /// <summary>
        /// Invierte el comportamiento de la nave en el eje Y.
        /// </summary>
        bool InvertAxes { get; }
    }
}
