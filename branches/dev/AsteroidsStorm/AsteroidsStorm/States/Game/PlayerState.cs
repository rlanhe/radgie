using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AsteroidsStorm.GameComponents.Spaceship;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Camera;
using AsteroidsStorm.GameComponents;
using AsteroidsStorm.GameComponents.SpaceObjects;

namespace AsteroidsStorm.States.Game
{
    /// <summary>
    /// Clase que encapsula el estado del jugador.
    /// </summary>
    class PlayerState
    {
        /// <summary>
        /// Vida del jugador.
        /// </summary>
        public float Life { get; set; }
        /// <summary>
        /// Energia del jugador.
        /// </summary>
        public float Energy { get; set; }
        /// <summary>
        /// Indica si el jugador tiene un bonus.
        /// </summary>
        public bool HasBonus { get; set; }
        /// <summary>
        /// Bonus que posee el jugador.
        /// </summary>
        public SpaceObject.Type Bonus { get; set; }
        /// <summary>
        /// Index del jugador.
        /// </summary>
        public PlayerIndex PlayerIndex { get; set; }
        /// <summary>
        /// Nave que controla el jugador.
        /// </summary>
        public Spaceship Spaceship { get; set; }
        /// <summary>
        /// Controlador que usa el jugador para controlar el juego.
        /// </summary>
        public GameController GameController { get; set; }
        /// <summary>
        /// Camara seleccionada por el jugador.
        /// </summary>
        public ICamera SelectedCamera { get; set; }
        /// <summary>
        /// Puntuacion obtenida por el jugador.
        /// </summary>
        public long Score { get; set; }

        /// <summary>
        /// Crea e inicializa el estado del jugador.
        /// </summary>
        public PlayerState()
        {
            Life = 100.0f;
            Energy = 100.0f;
            HasBonus = false;
            Score = 0;
        }
    }
}
