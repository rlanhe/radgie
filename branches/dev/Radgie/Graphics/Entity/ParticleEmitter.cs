using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Emisor de particulas.
    /// Annade particulas a un sistema de particulas teniendo en cuenta los puntos intermedios por donde ha pasado el GameComponent que lo contiene.
    /// </summary>
    public class ParticleEmitter: AGraphicEntity
    {
        #region Properties
        /// <summary>
        /// Sistema de particulas del que es emisor.
        /// </summary>
        private WeakReference mParticleSystemRef;
        /// <summary>
        /// Tiempo en segundos entre particulas.
        /// </summary>
        private float mTimeBetweenParticles;
        /// <summary>
        /// Tiempo restante para emitir particulas.
        /// </summary>
        private float mTimeLeftOver;
        /// <summary>
        /// Posicion cuando se actualizo por ultima vez.
        /// </summary>
        private Vector3 mLastPosition;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo sistema de particulas.
        /// </summary>
        /// <param name="particleSystem">Sistema de particulas del que es emisor.</param>
        /// <param name="particlesPerSecond">Particulas por segundo.</param>
        /// <param name="initialPosition">Posicion inicial.</param>
        public ParticleEmitter(ParticleSystem particleSystem, float particlesPerSecond, Vector3 initialPosition)
        {
            ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).GraphicEntityReferences.Add(this);

            mParticleSystemRef = new WeakReference(particleSystem);
            mTimeBetweenParticles = 1.0f / particlesPerSecond;
            mLastPosition = initialPosition;
        }
        #endregion

        #region Methods
        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.DrawAction"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            // OK. Do nothing
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if ((mParticleSystemRef.IsAlive) && (mParticleSystemRef.Target != null))
            {
                ParticleSystem particleSystem = (ParticleSystem)mParticleSystemRef.Target;

                float elapsedTime = (float)time.ElapsedGameTime.TotalSeconds;
                Vector3 newPosition = Component.World.Translation;
                Vector3 velocity = (newPosition - mLastPosition) / elapsedTime;

                // If we had any time left over that we didn't use during the
                // previous update, add that to the current elapsed time.
                float timeToSpend = mTimeLeftOver + elapsedTime;

                // Counter for looping over the time interval.
                float currentTime = -mTimeLeftOver;

                // Create particles as long as we have a big enough time interval.
                while (timeToSpend > mTimeBetweenParticles)
                {
                    currentTime += mTimeBetweenParticles;
                    timeToSpend -= mTimeBetweenParticles;

                    // Work out the optimal position for this particle. This will produce
                    // evenly spaced particles regardless of the object speed, particle
                    // creation frequency, or game update rate.
                    float mu = currentTime / elapsedTime;

                    Vector3 position = Vector3.Lerp(mLastPosition, newPosition, mu);

                    // Create the particle.
                    particleSystem.AddParticle(position, velocity);
                }

                // Store any time we didn't use, so it can be part of the next update.
                mTimeLeftOver = timeToSpend;
                mLastPosition = newPosition;
            }
            else
            {
                // Quitar emitter de GC que lo contiene
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.CalculateBoundingVolume"/>
        /// </summary>
        public override Core.BoundingVolumes.IBoundingVolume CalculateBoundingVolume()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override IInstance CreateSpecificInstance()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
