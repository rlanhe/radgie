using Radgie.Core;
using System.Xml;
using Microsoft.Xna.Framework.Audio;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Radgie.Util.Collection.ReferencePool;
using System.Xml.Linq;
using System.Threading;

namespace Radgie.Sound
{
    /// <summary>
    /// Sistema de gestion de sonidos.
    /// Gestiona la carga y reproduccion de sonidos.
    /// </summary>
    public class SoundSystem : ASystem, ISoundSystem
    {
        #region Properties

        #region ISoundSystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.Repeat"/>
        /// </summary>
        public bool Repeat
        {
            get
            {
                return MediaPlayer.IsRepeating;
            }
            set
            {
                MediaPlayer.IsRepeating = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.Volume"/>
        /// </summary>
        public float Volume
        {
            get
            {
                return MediaPlayer.Volume;
            }
            set
            {
                MediaPlayer.Volume = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.FXVolume"/>
        /// </summary>
        public float FXVolume
        {
            get
            {
                return Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume;
            }
            set
            {
                Microsoft.Xna.Framework.Audio.SoundEffect.MasterVolume = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.SoundEntityReferences"/>
        /// </summary>
        public IReferencePool<ISoundEntity> SoundEntityReferences
        {
            get
            {
                return mSoundEntityReferences;
            }
        }
        private IReferencePool<ISoundEntity> mSoundEntityReferences;

        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.Listener"/>
        /// </summary>
        public AudioListener Listener
        {
            get
            {
                return mAudioListener;
            }
        }
        protected AudioListener mAudioListener;

        /// <summary>
        /// Ver <see cref="Radgie.Sound.ISoundSystem.Statistics"/>
        /// </summary>
        public SoundSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private SoundSystemStatistics mStatistics;
        #endregion
        /// <summary>
        /// ultima posicion conocida del listener.
        /// </summary>
        private Vector3 mLastPosition;
        /// <summary>
        /// Indica si ya se registro una posicion del listener y puede comenzar a calcular la velocidad a la que se mueve.
        /// </summary>
        private bool mLastPositionInitialized;
        /// <summary>
        /// ultima vez que fue actualizado.
        /// </summary>
        private GameTime mUpdateTime;

        /// <summary>
        /// Callback para actualizar los elementos del pool.
        /// </summary>
        private PoolAction<ISoundEntity> mUpdatePoolActionCallback;

        #endregion

        #region Constructors
        /// <summary>
        /// Crea una instancia del sistema de sonido.
        /// </summary>
        /// <param name="sc">Seccion del fichero xml de configuracion del motor referente al sistema de sonido</param>
        public SoundSystem(XElement sc): base(sc)
        {
            mAudioListener = new AudioListener();
            mLastPositionInitialized = false;
            mSoundEntityReferences = new ReferencePool<ISoundEntity>(true);
            mStatistics = new SoundSystemStatistics();
            mUpdatePoolActionCallback = UpdatePoolAction;
        }
        #endregion

        #region Methods

        #region ISoundSystem Members
        /// <summary>
        /// Actualiza la posicion del listener.
        /// </summary>
        /// <param name="worldTransform">Matriz de transformacion del listener.</param>
        public void UpdateListener(Matrix worldTransform)
        {
            mAudioListener.Position = worldTransform.Translation;
            mAudioListener.Forward = worldTransform.Forward;
            mAudioListener.Up = worldTransform.Up;

            if (!mLastPositionInitialized)
            {
                mLastPositionInitialized = true;
                mAudioListener.Velocity = Vector3.Zero;
            }
            else
            {
                mAudioListener.Velocity = mAudioListener.Position - mLastPosition;
            }
            mLastPosition = mAudioListener.Position;
        }

        /// <summary>
        /// Comienza la reproduccion de una cancion de una manera progresiva.
        /// </summary>
        /// <param name="attenuation">Valor que representa cuando se aumenta el volumen del sonido por segundo.</param>
        /// <param name="song">Cancion que se quiere reproducir.</param>
        public void Play(float attenuation, Song song)
        {
            mOperationWithAttenuation.Parameters = new object[] { SoundOperation.Play, attenuation, song };
            RadgieGame.Instance.JobScheduler.AddJob(mOperationWithAttenuation);
        }

        /// <summary>
        /// Reproduce una cancion.
        /// </summary>
        /// <param name="song">Cancion que se quiere reproducir.</param>
        public void Play(Song song)
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
            }
        }

        /// <summary>
        /// Para la reproduccion de una cancion de una manera progresiva.
        /// </summary>
        /// <param name="attenuation">Valor que representa cuanto se reduce el volumen del sonido por segundo.</param>
        public void Stop(float attenuation)
        {
            mOperationWithAttenuation.Parameters = new object[] { SoundOperation.Stop, attenuation };
            RadgieGame.Instance.JobScheduler.AddJob(mOperationWithAttenuation);
        }

        public void Stop()
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
            }
        }

        /// <summary>
        /// Operaciones disponibles para el manejo de sonidos.
        /// </summary>
        public enum SoundOperation
        {
            Play,
            Pause,
            Stop,
            Resume
        };

        /// <summary>
        /// Trabajo interno para la reproduccion de sonidos de manera atenuada.
        /// </summary>
        private static ParametrizedJob mOperationWithAttenuation = new ParametrizedJob(DoOperationWithAttenuation);
        /// <summary>
        /// Realiza una operacion aumentando o reduciendo el volumen de manera progresiva.
        /// </summary>
        /// <param name="parameters">Parametros de la operacion. [0] = SoundOperation, [1] = atenuacion</param>
        private static void DoOperationWithAttenuation(Object[] parameters)
        {
            ISoundSystem sSystem = (ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
            SoundOperation op = (SoundOperation)parameters[0];
            float attenuation = (float)parameters[1];
            float time = 100.0f;
            float speed = attenuation / (attenuation / (1000.0f/time));
            Song song = parameters.Length >= 3 ? (Song)parameters[2] : null;
            float finalVolume = sSystem.Volume;
            float startVolume;
            float targetVolume;
            if ((op == SoundOperation.Play) || (op == SoundOperation.Resume))
            {
                speed = Math.Abs(speed);
                startVolume = 0.0f;
                targetVolume = sSystem.Volume;
                sSystem.Volume = startVolume;
                switch (op)
                {
                    case SoundOperation.Play:
                        sSystem.Play(song);
                        break;
                    case SoundOperation.Resume:
                        sSystem.Resume();
                        break;
                }
            }
            else
            {
                speed = -Math.Abs(speed);
                startVolume = sSystem.Volume;
                targetVolume = 0.0f;
                sSystem.Volume = startVolume;
            }
            

            while (targetVolume == 0.0f ? sSystem.Volume > targetVolume : sSystem.Volume < targetVolume)
            {
                sSystem.Volume += speed;
                Thread.Sleep(TimeSpan.FromMilliseconds(time));
            }

            switch (op)
            {
                case SoundOperation.Pause:
                    sSystem.Pause();
                    break;
                case SoundOperation.Stop:
                    sSystem.Stop();
                    break;
            }

            sSystem.Volume = finalVolume;
        }

        /// <summary>
        /// Pausa el sonido de una manera progresiva, reduciendo el volumen.
        /// </summary>
        /// <param name="attenuation">Cuanto se reduce el volumen del sonido por segundo.</param>
        public void Pause(float attenuation)
        {
            mOperationWithAttenuation.Parameters = new object[] { SoundOperation.Pause, attenuation };
            RadgieGame.Instance.JobScheduler.AddJob(mOperationWithAttenuation);
        }

        /// <summary>
        /// Pausa la reproduccion del sonido.
        /// </summary>
        public void Pause()
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Pause();
            }
        }

        /// <summary>
        /// Retoma la reproduccion del sonido de una manera progresiva.
        /// </summary>
        /// <param name="attenuation">Cuanto se aumenta el volumen del sonido por segundo.</param>
        public void Resume(float attenuation)
        {
            mOperationWithAttenuation.Parameters = new object[] { SoundOperation.Resume, attenuation };
            RadgieGame.Instance.JobScheduler.AddJob(mOperationWithAttenuation);
        }

        /// <summary>
        /// Retoma la reproduccion del sonido.
        /// </summary>
        public void Resume()
        {
            if (MediaPlayer.GameHasControl)
            {
                MediaPlayer.Resume();
            }
        }

        #endregion
        /// <summary>
        /// Ver <see cref="Radgie.Core.AUpdateableAtRate.UpdateAction"/>
        /// </summary>
        protected override void UpdateAction(Microsoft.Xna.Framework.GameTime time)
        {
            mStatistics.Reset();
            mStatistics.StartUpdateTimer();
            mUpdateTime = time;
            mSoundEntityReferences.FireActionOverPoolItems(mUpdatePoolActionCallback);
            mStatistics.StopUpdateTimer();
        }

        /// <summary>
        /// Actualiza las entidades de sonido que maneja el sistema.
        /// </summary>
        /// <param name="sEntity">Entidad de sonido a actualizar.</param>
        private void UpdatePoolAction(ISoundEntity sEntity)
        {
            mStatistics.NumberOfItemsInPool++;
            if ((sEntity.Component != null) && (sEntity.IsActive()))
            {
                mStatistics.NumberOfItemsInPoolToUpdate++;
                sEntity.Update(mUpdateTime);
            }
        }
        #endregion
    }
}