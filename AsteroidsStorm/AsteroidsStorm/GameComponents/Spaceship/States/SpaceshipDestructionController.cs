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

namespace AsteroidsStorm.GameComponents.Spaceship.States
{
    /// <summary>
    /// Controlador para la destruccion de la nave.
    /// </summary>
    class SpaceshipDestructionController:AState
    {
        private BackCounter mSpaceshipBackCounter;
        private BackCounter mParticlesBackCounter;
        private BackCounter mSparkParticlesBackCounter;

        private static ParticleSystemSettings mParticleSettings = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<ParticleSystemSettings>("GameComponents/Asteroid/Graphics/Particles/explosion", false);
        private static ParticleSystemSettings mSparkParticleSettings = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<ParticleSystemSettings>("GameComponents/Asteroid/Graphics/Particles/spark_explosion", false);

        private ParticleSystem mPs;
        private ParticleEmitter mPsEmitter;

        private ParticleSystem mSparkPs;
        private ParticleEmitter mSparkPsEmitter;

        /// <summary>
        /// Efecto de la destruccion de la nave.
        /// </summary>
        protected SoundEffect mSoundEffect;


        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public SpaceshipDestructionController()
            : base(null)
        {
            mSpaceshipBackCounter = new BackCounter(TimeSpan.FromSeconds(0.1d));
            mParticlesBackCounter = new BackCounter(mParticleSettings.Duration);
            mSparkParticlesBackCounter = new BackCounter(mSparkParticleSettings.Duration);

            mPs = new ParticleSystem(mParticleSettings);
            mPsEmitter = new ParticleEmitter(mPs, 600.0f, Vector3.Zero);

            mSparkPs = new ParticleSystem(mSparkParticleSettings);
            mSparkPsEmitter = new ParticleEmitter(mSparkPs, 600.0f, Vector3.Zero);

            mSoundEffect = new SoundEffect("GameComponents/Asteroid/Sounds/collision");
            mSoundEffect.IsLooped = false;
        }
        
        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
        public override void OnEntry()
        {
            base.OnEntry();

            GameData gData = GetFromContext<GameData>("GameData");
            gData.AsteroidsField.CollisionGroup.RemoveGameComponent(Owner.Component);

            // Sonido colision
            Owner.Component.AddGameObject(mSoundEffect);
            mSoundEffect.Play();

            // Timers
            mSpaceshipBackCounter.Start();
            mParticlesBackCounter.Start();
            mSparkParticlesBackCounter.Start();

            // Explosion
            Owner.Component.AddGameObject(mPs);
            Owner.Component.AddGameObject(mPsEmitter);

            // Explosion 2
            Owner.Component.AddGameObject(mSparkPs);
            Owner.Component.AddGameObject(mSparkPsEmitter);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            // Desactiva la creacion de sparks
            if (mSparkParticlesBackCounter.Finished())
            {
                Owner.Component.RemoveGameObject(mSparkPsEmitter);
                Owner.Component.RemoveGameObject(mSparkPs);
                mSparkPs = null;
                mSparkPsEmitter = null;
            }

            // Pasados n segundos, oculta nave
            if (mSpaceshipBackCounter.Finished())
            {
                Spaceship spaceship = (Spaceship)Owner.Component;
                spaceship.Visible = false;
            }
 
            if (mParticlesBackCounter.Finished())
            {
                Owner.Component.RemoveGameObject(mSoundEffect);
                mSoundEffect = null;

                Owner.Component.RemoveGameObject(mPs);
                Owner.Component.RemoveGameObject(mPsEmitter);
                mPs = null;
                mPsEmitter = null;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnExit"/>
        /// </summary>
        public override void OnExit()
        {
            base.OnExit();

            if (mSoundEffect != null)
            {
                Owner.Component.RemoveGameObject(mSoundEffect);
                mSoundEffect = null;
            }

            if (mPs != null)
            {
                Owner.Component.RemoveGameObject(mPs);
                Owner.Component.RemoveGameObject(mPsEmitter);
                mPs = null;
                mPsEmitter = null;
            }

            if (mSparkPs != null)
            {
                Owner.Component.RemoveGameObject(mSparkPs);
                Owner.Component.RemoveGameObject(mSparkPsEmitter);
                mSparkPs = null;
                mSparkPsEmitter = null;
            }
        }
    }
}
