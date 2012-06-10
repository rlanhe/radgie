using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Interfaz de los estados de la nave espacial.
    /// </summary>
    interface ISpaceshipState: IState
    {
        /// <summary>
        /// Determina el comportamiento de la nave al chocar con un objeto.
        /// </summary>
        /// <param name="asteroid">SpaceObject con el que colisiona.</param>
        void CollideWithSpaceObject(SpaceObject asteroid);
    }
}
