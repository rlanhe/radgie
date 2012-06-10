using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Action;

namespace AsteroidsStorm.States
{
    /// <summary>
    /// Interfaz del controlador de menu.
    /// </summary>
    interface IGUIController
    {
        /// <summary>
        /// Mueve hacia arriba la opcion de menu selecionada.
        /// </summary>
        DigitalAction Up { get; }
        /// <summary>
        /// Mueve hacia abajo la opcion de menu seleccionada.
        /// </summary>
        DigitalAction Down { get; }
        /// <summary>
        /// Disminuye el valor de la opcion actual de menu.
        /// </summary>
        DigitalAction Left { get; }
        /// <summary>
        /// Aumenta el valor de la opcion actual del menu.
        /// </summary>
        DigitalAction Right { get; }
        /// <summary>
        /// Acepta la opcion actual de menu.
        /// </summary>
        DigitalAction Ok { get; }
        /// <summary>
        /// Cancela la opcion actual de menu.
        /// </summary>
        DigitalAction Cancel { get; }
        /// <summary>
        /// Pausa la ejecucion del juego.
        /// </summary>
        DigitalAction Pause { get; }
    }
}
