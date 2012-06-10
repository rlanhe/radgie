using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    /// <summary>
    /// Configuracion del sistema de particulas.
    /// </summary>
    public class ParticleSystemSettings
    {
        #region Properties
        /// <summary>
        /// Numero maximo de particulas que pueden existir a la vez en el sistema de particulas.
        /// </summary>
        public int MaxParticles { get; set; }
        /// <summary>
        /// Duracion media de cada particula.
        /// </summary>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Factor de aleatoriedad de la duracion de cada particula.
        /// </summary>
        public float DurationRandomness { get; set; }
        /// <summary>
        /// Factor por el que se multiplica la velocidad de las particulas creadas.
        /// </summary>
        public float EmitterVelocitySensitivity { get; set; }
        /// <summary>
        /// Velocidad minima horizontal.
        /// </summary>
        public float MinHorizontalVelocity { get; set; }
        /// <summary>
        /// Velocidad maxima horizontal.
        /// </summary>
        public float MaxHorizontalVelocity { get; set; }
        /// <summary>
        /// Velocidad minima vertical.
        /// </summary>
        public float MinVerticalVelocity { get; set; }
        /// <summary>
        /// Velocidad maxima vertical.
        /// </summary>
        public float MaxVerticalVelocity { get; set; }
        /// <summary>
        /// Direccion de la gravedad.
        /// </summary>
        public Vector3 Gravity { get; set; }
        /// <summary>
        /// Velocidad final.
        /// </summary>
        public float EndVelocity { get; set; }
        /// <summary>
        /// Color inicial.
        /// </summary>
        public Color MinColor { get; set; }
        /// <summary>
        /// Color final.
        /// </summary>
        public Color MaxColor { get; set; }
        /// <summary>
        /// Velocidad de rotacion minima.
        /// </summary>
        public float MinRotateSpeed { get; set; }
        /// <summary>
        /// Velocidad de rotacion maxima.
        /// </summary>
        public float MaxRotateSpeed { get; set; }
        /// <summary>
        /// Tamanno inicial minimio.
        /// </summary>
        public float MinStartSize { get; set; }
        /// <summary>
        /// Tamanno inicial maximo.
        /// </summary>
        public float MaxStartSize { get; set; }
        /// <summary>
        /// Tamanno final minimo.
        /// </summary>
        public float MinEndSize { get; set; }
        /// <summary>
        /// Tamanno final maximo.
        /// </summary>
        public float MaxEndSize { get; set; }
        /// <summary>
        /// Material que se usara para dibujar las particulas.
        /// </summary>
        public Material Material { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Configuracion por defecto de un sistema de particulas.
        /// </summary>
        public ParticleSystemSettings()
        {
            MaxParticles = 100;
            Duration = TimeSpan.FromSeconds(2);
            DurationRandomness = 0;
            EmitterVelocitySensitivity = 1;
            MinHorizontalVelocity = 0;
            MaxHorizontalVelocity = 0;
            MinVerticalVelocity = 0;
            MaxVerticalVelocity = 0;
            Gravity = Vector3.Zero;
            EndVelocity = 1;
            MinColor = Color.White;
            MaxColor = Color.White;
            MinRotateSpeed = 0;
            MaxRotateSpeed = 0;
            MinStartSize = 100;
            MaxStartSize = 100;
            MinEndSize = 100;
            MaxEndSize = 100;
        }
        #endregion
    }
}
