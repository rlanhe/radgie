using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Radgie.Sound;
using Radgie.Util;
using Radgie.Graphics.Entity;
using Radgie.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Util.Collection.Pool;
using AsteroidsStorm.States.Game;

namespace AsteroidsStorm.GameComponents.SpaceObjects
{
    /// <summary>
    /// Controlador de la destruccion de bonificadores.
    /// </summary>
    class BonusDestructionController:AState
    {
        
        private BackCounter mAsteroidBackCounter;
        
        /// <summary>
        /// Efecto de sonido asociado a su destruccion.
        /// </summary>
        protected SoundEffect mSoundEffect;

        private static Pool<SoundEffect> mCollisionEfectPool = new Pool<SoundEffect>(10, true, 3, InitCollisionSound);

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public BonusDestructionController()
            : base(null)
        {
            mAsteroidBackCounter = new BackCounter(TimeSpan.FromSeconds(0));
        }

        /// <summary>
        /// Inicializa el sonido de colision.
        /// </summary>
        /// <returns>Sonido de colision.</returns>
        private static SoundEffect InitCollisionSound()
        {
            SoundEffect soundEffect = new SoundEffect("GameComponents/Bonus/Sounds/Bonus");
            soundEffect.IsLooped = false;
            return soundEffect;
        }

        /// <summary>
        /// Ver <see cref="Radgie.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            GameData gData = GetFromContext<GameData>("GameData");
            gData.AsteroidsField.CollisionGroup.RemoveGameComponent(Owner.Component);

            // Sonido colision
            mSoundEffect = mCollisionEfectPool.Get();
            if (mSoundEffect != null)
            {
                Owner.Component.AddGameObject(mSoundEffect);
                mSoundEffect.Play();
            }

            // Timers
            mAsteroidBackCounter.Start();

        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            // Elimina el asteroide
            if (mAsteroidBackCounter.Finished())
            {
                SpaceObject sObject = (SpaceObject)Owner.Component;
                sObject.HideAsteroid();
                if (mSoundEffect != null)
                {
                    Owner.Component.RemoveGameObject(mSoundEffect);
                    mCollisionEfectPool.Release(mSoundEffect);
                }

                sObject.Reset();
            }
        }
    }
}
