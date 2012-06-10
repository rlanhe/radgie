using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework;
using Radgie.Sound;
using Radgie.Util.Collection.Pool;
using AsteroidsStorm.States.Game;

namespace AsteroidsStorm.GameComponents.SpaceObjects
{
    /// <summary>
    /// Controlador del movimiento de los SpaceObjects.
    /// </summary>
    public class SimpleSpaceObjectController : AState
    {
        private static Pool<SoundEffect> mSoundPool;
        private SoundEffect mSoundEffect;
        private const float SOUND_DISTANCE = 5.0f;

        /// <summary>
        /// Constructor estatico.
        /// </summary>
        static SimpleSpaceObjectController()
        {
            mSoundPool = new Pool<SoundEffect>(3, true, 0, CreateSound);
        }

        /// <summary>
        /// Crea el sonido que hace un SpaceObject al avanzar por el espacio.
        /// </summary>
        /// <returns>Efecto de sonido.</returns>
        private static SoundEffect CreateSound()
        {
            SoundEffect effect = new SoundEffect("GameComponents/SpaceObject/Sounds/Fiuu");
            effect.Is3D = true;
            effect.IsLooped = false;
            effect.Volume = 1.0f;
            return effect;
        }

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public SimpleSpaceObjectController()
            : base(null)
        {
        }

        /// <summary>
        /// Libera los elementos asociados a este controlador.
        /// </summary>
        public void Release()
        {
            mSoundPool.Release(mSoundEffect);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            GameData gameData = GetFromContext<GameData>("GameData");
            if (gameData != null)
            {
                float distance = Vector3.Distance(Owner.Component.World.Translation, gameData.PlayerState.Spaceship.World.Translation);
                if (distance <= SOUND_DISTANCE)
                {
                    if (mSoundEffect == null)
                    {
                        mSoundEffect = mSoundPool.Get();
                        if (mSoundEffect != null)
                        {
                            Owner.Component.AddGameObject(mSoundEffect);
                            mSoundEffect.Stop();
                            mSoundEffect.Play();
                        }
                    }
                }
                else
                {
                    if ((mSoundEffect != null) && (mSoundEffect.State == Microsoft.Xna.Framework.Audio.SoundState.Stopped))
                    {
                        mSoundPool.Release(mSoundEffect);
                        mSoundEffect.Stop();
                        Owner.Component.RemoveGameObject(mSoundEffect);
                        mSoundEffect = null;
                    }
                }
            }
        }
    }
}
