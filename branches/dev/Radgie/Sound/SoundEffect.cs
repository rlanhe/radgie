using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Radgie.Sound
{
    public class SoundEffect: ASoundEntity
    {
        #region Properties
        /// <summary>
        /// Referencia al sistema de sonido.
        /// </summary>
        protected static ISoundSystem SoundSystem;

        /// <summary>
        /// Referenica al objeto XNA que representa el efecto de sonido.
        /// </summary>
        protected Microsoft.Xna.Framework.Audio.SoundEffectInstance mSoundEffectInstance;

        #region Microsoft.Xna.Framework.Audio.SoundEffectInstance Properties
        /// <summary>
        /// Indica si debe reproducirse en bucle.
        /// </summary>
        public bool IsLooped
        {
            get
            {
                return mSoundEffectInstance.IsLooped;
            }
            set
            {
                mSoundEffectInstance.IsLooped = value;
            }
        }

        /// <summary>
        /// Indica si el sonido esta desplazado a la izquierda o la derecha.
        /// -1.0f desplazado totalmente a la izquierda. 1.0f desplazado totalmente a la derecha.
        /// </summary>
        public float Pan
        {
            get
            {
                return mSoundEffectInstance.Pan;
            }
            set
            {
                mSoundEffectInstance.Pan = value;
            }
        }

        /// <summary>
        /// Sube o baja una octava el sonido.
        /// -1.0f para bajar una, 1.0f para subir una.
        /// </summary>
        public float Pitch
        {
            get
            {
                return mSoundEffectInstance.Pitch;
            }
            set
            {
                mSoundEffectInstance.Pitch = value;
            }
        }

        /// <summary>
        /// Estado en el que se encuentra el sonido.
        /// </summary>
        public Microsoft.Xna.Framework.Audio.SoundState State
        {
            get
            {
                return mSoundEffectInstance.State;
            }
        }

        /// <summary>
        /// Volumen del sonido.
        /// Entre 0.0f y 1.0f.
        /// </summary>
        public float Volume
        {
            get
            {
                return mSoundEffectInstance.Volume;
            }
            set
            {
                mSoundEffectInstance.Volume = value;
            }
        }
        #endregion

        /// <summary>
        /// Indica si se aplican efectos 3d sobre el sonido.
        /// </summary>
        public bool Is3D
        {
            get
            {
                return mIs3D;
            }
            set
            {
                mIs3D = value;
            }
        }
        private bool mIs3D;

        /// <summary>
        /// Emisor del sonido.
        /// </summary>
        protected AudioEmitter mAudioEmitter;

        /// <summary>
        /// ultima posicion.
        /// Necesaria para calcular la velocidad del objeto del que cuelga el sonido en efectos 3D.
        /// </summary>
        private Vector3 mLastPosition;
        /// <summary>
        /// Indica si el valor de mLastPosition debe tenerse en cuenta.
        /// </summary>
        private bool mLastPositionInitialized;

        /// <summary>
        /// Indica si el sonido estaba desactivado.
        /// </summary>
        private bool mWasDeactivated;
        /// <summary>
        /// Almacena el estado del sonido antes de desactivarlo para restaurar su estado original
        /// </summary>
        private SoundState mLastState;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructor vacio.
        /// </summary>
        public SoundEffect()
        {
        }

        /// <summary>
        /// Crea un nuevo efecto de sonido.
        /// </summary>
        /// <param name="soundEffectId">Identificador del sonido.</param>
        public SoundEffect(string soundEffectId): this(soundEffectId, false)
        {
        }

        /// <summary>
        /// Crea un nuevo efecto de sonido.
        /// </summary>
        /// <param name="soundEffectId">Identificador del sonido.</param>
        /// <param name="stream">Indica si se trata de un stream.</param>
        public SoundEffect(string soundEffectId, bool stream)
        {
            if (SoundSystem == null)
            {
                SoundSystem = (ISoundSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(ISoundSystem));
                if (SoundSystem == null)
                {
                    throw new Exception("ISoundSystem not found");
                }
            }

            if (stream)
            {
                // TODO:
            }
            else
            {
                mSoundEffectInstance = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(soundEffectId, false).CreateInstance();
            }
            mAudioEmitter = new AudioEmitter();
        }
        #endregion

        #region Methods

        #region Microsoft.Xna.Framework.Audio.SoundEffectInstance Methods

        /// <summary>
        /// Pausa la reproduccion del sonido.
        /// </summary>
        public void Pause()
        {
            mSoundEffectInstance.Pause();
        }

        /// <summary>
        /// Para la reproduccion del sonido.
        /// </summary>
        public void Stop()
        {
            Stop(true);
        }

        /// <summary>
        /// Para la reproduccion del sondio.
        /// </summary>
        /// <param name="inmediate">True en el momento, False cuando termine.</param>
        public void Stop(bool inmediate)
        {
            mSoundEffectInstance.Stop(inmediate);
        }

        /// <summary>
        /// Comienza la reproduccion del sonido.
        /// </summary>
        public void Play()
        {
            if (mIs3D)
            {
                mSoundEffectInstance.Apply3D(SoundSystem.Listener, mAudioEmitter);
            }
            mSoundEffectInstance.Play();
        }

        /// <summary>
        /// Continua la reproduccion del sonido.
        /// </summary>
        public void Resume()
        {
            mSoundEffectInstance.Resume();
        }

        #endregion

        #region IGameObject members
        
        /// <summary>
        /// Ver <see cref="Radgie.Core.IGameObject.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            if (IsActive())
            {
                if (mWasDeactivated)
                {
                    // Recobra el estado anterior si se estaba reproduciendo
                    if (mLastState == SoundState.Playing)
                    {
                        Resume();
                    }
                    mWasDeactivated = false;
                }

                UpdateAction(time);
            }
            else
            {
                if (!mWasDeactivated)
                {
                    mWasDeactivated = true;
                    mLastState = State;
                    if (State == SoundState.Playing)
                    {
                        Pause();
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// Accion de actualizacion del sistema.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la ultima actualizacion.</param>
        private void UpdateAction(GameTime time)
        {
            if (Active)
            {
                if ((mIs3D) && (mSoundEffectInstance.State == SoundState.Playing))
                {
                    mAudioEmitter.Position = Component.World.Translation;
                    mAudioEmitter.Forward = Component.World.Forward;
                    mAudioEmitter.Up = Component.World.Up;

                    if (!mLastPositionInitialized)
                    {
                        mLastPositionInitialized = true;
                        mAudioEmitter.Velocity = Vector3.Zero;
                    }
                    else
                    {
                        mAudioEmitter.Velocity = mAudioEmitter.Position - mLastPosition;
                    }
                    mLastPosition = mAudioEmitter.Position;

                    mSoundEffectInstance.Apply3D(SoundSystem.Listener, mAudioEmitter);
                }
            }
            else
            {
                mSoundEffectInstance.Stop();
            }
        }

        #region IEntity members
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override IInstance CreateSpecificInstance()
        {
            // No tiene sentido tener instancias de sonidos, ya que el objeto SoundEffect ya representa una instancia de sonido
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
