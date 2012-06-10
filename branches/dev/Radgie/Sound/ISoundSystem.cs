using Radgie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Radgie.Util.Collection.ReferencePool;

namespace Radgie.Sound
{
    /// <summary>
    /// Interfaz del sistema de sonido.
    /// </summary>
    public interface ISoundSystem: ISystem
    {
        #region Properties
        /// <summary>
        /// Repite la cancion actual.
        /// </summary>
        bool Repeat { get; set; }
        /// <summary>
        /// Volumen de reproduccion de las canciones.
        /// Entre 0 y 1.
        /// </summary>
        float Volume { get; set; }
        /// <summary>
        /// Volumen de reproduccion de los efectos de sonido.
        /// Entre 0 y 1.
        /// </summary>
        float FXVolume { get; set; }
        /// <summary>
        /// Entidades que el sistema tiene que actualizar.
        /// </summary>
        IReferencePool<ISoundEntity> SoundEntityReferences { get; }
        /// <summary>
        /// Listener.
        /// Determina la posicion del oyente para calcular los efectos aplicados a los sonidos segun su posicion y velocidad.
        /// </summary>
        AudioListener Listener { get; }
        /// <summary>
        /// Estadisticas del sistema.
        /// </summary>
        SoundSystemStatistics Statistics { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Actualiza la posicion del listener.
        /// </summary>
        /// <param name="worldTransform">Matriz de mundo del listener.</param>
        void UpdateListener(Matrix worldTransform);

        /// <summary>
        /// Reproduce una cancion de fondo.
        /// </summary>
        /// <param name="song">Cancion.</param>
        void Play(Song song);
        /// <summary>
        /// Reproduce una cancion de fondo de manera atenuada
        /// </summary>
        /// <param name="attenuation">salto de volumen por segundo</param>
        /// <param name="song">Cancion a reproducir.</param>
        void Play(float attenuation, Song song);
        /// <summary>
        /// Para la reproduccion de la cancion actual.
        /// </summary>
        void Stop();
        /// <summary>
        /// Para la reproduccion de la cancion actual.
        /// <param name="attenuation">salto de volumen por segundo</param>
        /// </summary>
        void Stop(float attenuation);
        /// <summary>
        /// Pausa la reproduccion de la cancion actual.
        /// </summary>
        void Pause();
        /// <summary>
        /// Pausa la reproduccion de la cancion actual.
        /// <param name="attenuation">salto de volumen por segundo</param>
        /// </summary>
        void Pause(float attenuation);
        /// <summary>
        /// Continua la reproduccion de la cancion actual si esta esta pausada.
        /// </summary>
        void Resume();
        /// <summary>
        /// Continua la reproduccion de la cancion actual si esta esta pausada.
        /// <param name="attenuation">salto de volumen por segundo</param>
        /// Para la reproduccion de la cancion actual.
        /// </summary>
        void Resume(float attenuation);
        #endregion
    }
}