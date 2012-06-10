using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using System.Xml;
using Radgie.File;

namespace Radgie.Graphics.Entity
{
    /// <summary>
    /// Sistema de particulas.
    /// </summary>
    public class ParticleSystem: ASimpleGraphicEntity
    {
        #region Properties
        /// <summary>
        /// Buffer de particulas.
        /// </summary>
        private ParticlesBuffer mParticlesBuffer;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo sistema de particulas.
        /// </summary>
        /// <param name="settings">Configuracion del sistema de particulas.</param>
        public ParticleSystem(ParticleSystemSettings settings)
        {
            ((IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem))).GraphicEntityReferences.Add(this);

            mParticlesBuffer = new ParticlesBuffer(settings);

            Material = settings.Material;

            Init(settings);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa el sistema de particulas a partir de la configuracion pasada por parametro.
        /// Este metodo puede sobrecargarse en clases hijas si se requiere usar un shader distinto.
        /// </summary>
        /// <param name="settings">Configuracion del sistema de particulas.</param>
        protected virtual void Init(ParticleSystemSettings settings)
        {
            // Configura los parametros del sistema de particulas
            mMaterial["Duration"].SetValue((float)settings.Duration.TotalSeconds);
            mMaterial["DurationRandomness"].SetValue(settings.DurationRandomness);
            mMaterial["Gravity"].SetValue(settings.Gravity);
            mMaterial["EndVelocity"].SetValue(settings.EndVelocity);
            mMaterial["MinColor"].SetValue(settings.MinColor.ToVector4());
            mMaterial["MaxColor"].SetValue(settings.MaxColor.ToVector4());
            mMaterial["RotateSpeed"].SetValue(new Vector2(settings.MinRotateSpeed, settings.MaxRotateSpeed));
            mMaterial["StartSize"].SetValue(new Vector2(settings.MinStartSize, settings.MaxStartSize));
            mMaterial["EndSize"].SetValue(new Vector2(settings.MinEndSize, settings.MaxEndSize));
        }

        #region AGraphicEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            mParticlesBuffer.Update((float)time.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.PreDraw"/>
        /// </summary>
        protected override void PreDraw(IRenderer renderer)
        {
            base.PreDraw(renderer);

            // Establece valores del material
            mMaterial["CurrentTime"].SetValue(mParticlesBuffer.Time);
            mMaterial["ViewportScale"].SetValue(new Vector2(0.5f / renderer.Device.Viewport.AspectRatio, -0.5f));
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.DrawAction"/>
        /// </summary>
        protected override void DrawAction(IRenderer renderer)
        {
            base.DrawAction(renderer);

            mParticlesBuffer.Draw(renderer);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Entity.AGraphicEntity.CalculateBoundingVolume"/>
        /// </summary>
        public override Core.BoundingVolumes.IBoundingVolume CalculateBoundingVolume()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            if (mParticlesBuffer != null)
            {
                mParticlesBuffer.Init();
            }
        }
        #endregion

        #region IEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override Core.IInstance CreateSpecificInstance()
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Annade una nueva particula al sistema.
        /// </summary>
        /// <param name="position">Posicion inicial del sistema de particulas.</param>
        /// <param name="velocity">Velocidad inicial de la particula.</param>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            mParticlesBuffer.AddParticle(position, velocity);
        }
        #endregion
    }
}
