using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Util;

namespace Radgie.Graphics
{
    /// <summary>
    /// Buffer donde almacena los datos de las particulas de un sistema de particulas.
    /// Estos datos son volcados en la GPU que es quien calcula el movimiento de las particulas para descargar a la CPU de esto.
    /// </summary>
    public class ParticlesBuffer : IDraw
    {
        #region Properties
        /// <summary>
        /// Cola circular donde almacena la informacion de cada particula.
        /// </summary>
        private ParticleVertex[] mParticlesBuffer;

        /// <summary>
        /// Posicion de la primera particula activa.
        /// </summary>
        private int mFirstActiveParticle;
        /// <summary>
        /// Posicion de la primera particula recien creada (todavia no se dibuja).
        /// </summary>
        private int mFirstNewParticle;
        /// <summary>
        /// Posicion de la primera particula que llego al final de su vida (todavia podria dibujarse).
        /// </summary>
        private int mFirstFreeParticle;
        /// <summary>
        /// Posicion de la primera particula que llego al final de su vida y ya no se dibuja.
        /// </summary>
        private int mFirstRetiredParticle;

        /// <summary>
        /// Buffer donde se cargan los datos que se enviaran a la GPU.
        /// </summary>
        private DynamicVertexBuffer mVertexBuffer;
        /// <summary>
        /// Buffer con los indices sobre mVertexBuffer.
        /// </summary>
        private IndexBuffer mIndexBuffer;

        /// <summary>
        /// Tiempo que lleva activo el sistema de particulas.
        /// </summary>
        public float Time
        {
            get
            {
                return mCurrentTime;
            }
        }
        private float mCurrentTime;
        /// <summary>
        /// Numero de veces que se ha dibujado el sistema de particulas.
        /// </summary>
        private int mDrawCounter;

        /// <summary>
        /// Configuracion del sistema de particulas.
        /// </summary>
        private ParticleSystemSettings mSettings;

        /// <summary>
        /// Existen particulas activas
        /// </summary>
        private bool IsAnyParticleActive
        {
            get
            {
                return mFirstActiveParticle != mFirstFreeParticle;
            }
        }

        /// <summary>
        /// Existe alguna particula que se este dibujando (pueden no estar activas pero seguir en el buffer de la GPU)
        /// </summary>
        private bool IsAnyParticleDrawn
        {
            get
            {
                return mFirstRetiredParticle != mFirstActiveParticle;
            }
        }

        /// <summary>
        /// Existen particulas nuevas que no se han subido a la GPU
        /// </summary>
        private bool AreNewParticlesOutOfBuffer
        {
            get
            {
                return mFirstFreeParticle != mFirstNewParticle;
            }
        }

        /// <summary>
        /// Delegado usado para dibujar las particulas por el Renderer.
        /// </summary>
        private DrawDelegate mDrawCallback;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un buffer de particulas segun la configuracion del sistema de particulas.
        /// </summary>
        /// <param name="settings">Configuracion del sistema de particulas.</param>
        public ParticlesBuffer(ParticleSystemSettings settings)
        {
            mSettings = settings;

            // Crea un array para almacenar la informacion de cada particula
            mParticlesBuffer = new ParticleVertex[mSettings.MaxParticles * 4];

            // Inicializa la posicion de los vertices de cada una de ellas
            for (int i = 0; i < mSettings.MaxParticles; i++)
            {
                mParticlesBuffer[i * 4 + 0].Corner = new Short2(-1, -1);
                mParticlesBuffer[i * 4 + 1].Corner = new Short2(1, -1);
                mParticlesBuffer[i * 4 + 2].Corner = new Short2(1, 1);
                mParticlesBuffer[i * 4 + 3].Corner = new Short2(-1, 1);
            }

            IGraphicSystem gSystem = (IGraphicSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));

            lock (gSystem.Device)
            {
                mVertexBuffer = new DynamicVertexBuffer(gSystem.Device, ParticleVertex.VertexDeclaration, mSettings.MaxParticles * 4, BufferUsage.WriteOnly);
            }

            // Crea los indices que definen los quad de cada particula
            ushort[] indices = new ushort[settings.MaxParticles * 6];
            for (int i = 0; i < mSettings.MaxParticles; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);

                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            lock (gSystem.Device)
            {
                mIndexBuffer = new IndexBuffer(gSystem.Device, typeof(ushort), indices.Length, BufferUsage.WriteOnly);
            }
            mIndexBuffer.SetData(indices);

            mDrawCallback = this.DrawCallback;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inicializa el buffer de particulas.
        /// </summary>
        public void Init()
        {
            RetireActiveParticles();
            FreeRetiredParticles();
            mFirstActiveParticle = mFirstNewParticle = mFirstFreeParticle = mFirstRetiredParticle = 0;
        }

        /// <summary>
        /// Actualiza el buffer de particulas.
        /// </summary>
        /// <param name="elapsedTime">Tiempo transcurrido desde la ultima vez que se llamo a este metodo en segundos.</param>
        public void Update(float elapsedTime)
        {
            mCurrentTime += elapsedTime;

            RetireActiveParticles();
            FreeRetiredParticles();

            if (!IsAnyParticleActive)
            {
                mCurrentTime = 0.0f;
            }

            if (!IsAnyParticleDrawn)
            {
                mDrawCounter = 0;
            }
        }

        /// <summary>
        /// Comprueba que particulas han llegado al final de su vida y las retira de la seccion de activas del buffer.
        /// </summary>
        private void RetireActiveParticles()
        {
            float particleDuration = (float)mSettings.Duration.TotalSeconds;

            while (mFirstActiveParticle != mFirstNewParticle)
            {
                // Is this particle old enough to retire?
                // We multiply the active particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                float particleAge = mCurrentTime - mParticlesBuffer[mFirstActiveParticle * 4].Time;

                if (particleAge < particleDuration)
                {
                    break;
                }

                // Remember the time at which we retired this particle.
                mParticlesBuffer[mFirstActiveParticle * 4].Time = mDrawCounter;

                // Move the particle from the active to the retired queue.
                mFirstActiveParticle++;

                if (mFirstActiveParticle >= mSettings.MaxParticles)
                {
                    mFirstActiveParticle = 0;
                }
            }
        }


        /// <summary>
        /// Comprueba dentro de las particulas retiradas cuales seguro que ya no se estan dibujando por la GPU y se pueden quitar del buffer definitivamente.
        /// </summary>
        private void FreeRetiredParticles()
        {
            while (mFirstRetiredParticle != mFirstActiveParticle)
            {
                // Has this particle been unused long enough that
                // the GPU is sure to be finished with it?
                // We multiply the retired particle index by four, because each
                // particle consists of a quad that is made up of four vertices.
                int age = mDrawCounter - (int)mParticlesBuffer[mFirstRetiredParticle * 4].Time;

                // The GPU is never supposed to get more than 2 frames behind the CPU.
                // We add 1 to that, just to be safe in case of buggy drivers that
                // might bend the rules and let the GPU get further behind.
                if (age < 3)
                {
                    break;
                }

                // Move the particle from the retired to the free queue.
                mFirstRetiredParticle++;

                if (mFirstRetiredParticle >= mSettings.MaxParticles)
                {
                    mFirstRetiredParticle = 0;
                }
            }
        }

        #region IDraw Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IDraw.Draw"/>
        /// </summary>
        public void Draw(IRenderer renderer)
        {
            if (mVertexBuffer.IsContentLost)
            {
                mVertexBuffer.SetData(mParticlesBuffer);
            }

            if (AreNewParticlesOutOfBuffer)
            {
                AddNewParticlesToVertexBuffer();
            }

            // If there are any active particles, draw them now!
            if (mFirstActiveParticle != mFirstFreeParticle)
            {
                // Set the particle vertex and index buffer.
                renderer.Device.SetVertexBuffer(mVertexBuffer);
                renderer.Device.Indices = mIndexBuffer;

                renderer.Render(mDrawCallback);
            }

            mDrawCounter++;
        }
        #endregion

        /// <summary>
        /// Realiza las llamadas para dibujar las particulas en la GPU.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        private void DrawCallback(IRenderer renderer)
        {
            if (mFirstActiveParticle < mFirstFreeParticle)
            {
                // If the active particles are all in one consecutive range,
                // we can draw them all in a single call.
                renderer.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                             mFirstActiveParticle * 4, (mFirstFreeParticle - mFirstActiveParticle) * 4,
                                             mFirstActiveParticle * 6, (mFirstFreeParticle - mFirstActiveParticle) * 2);
            }
            else
            {
                // If the active particle range wraps past the end of the queue
                // back to the start, we must split them over two draw calls.
                renderer.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                             mFirstActiveParticle * 4, (mSettings.MaxParticles - mFirstActiveParticle) * 4,
                                             mFirstActiveParticle * 6, (mSettings.MaxParticles - mFirstActiveParticle) * 2);

                if (mFirstFreeParticle > 0)
                {
                    renderer.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                                                 0, mFirstFreeParticle * 4,
                                                 0, mFirstFreeParticle * 2);
                }
            }
        }

        /// <summary>
        /// Annade las particulas nuevas de mParticlesBuffer al VertexBuffer de la GPU.
        /// </summary>
        private void AddNewParticlesToVertexBuffer()
        {
            int stride = ParticleVertex.SizeInBytes;

            if (mFirstNewParticle < mFirstFreeParticle)
            {
                // If the new particles are all in one consecutive range,
                // we can upload them all in a single call.
                mVertexBuffer.SetData(mFirstNewParticle * stride * 4, mParticlesBuffer,
                                     mFirstNewParticle * 4,
                                     (mFirstFreeParticle - mFirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);
            }
            else
            {
                // If the new particle range wraps past the end of the queue
                // back to the start, we must split them over two upload calls.
                mVertexBuffer.SetData(mFirstNewParticle * stride * 4, mParticlesBuffer,
                                     mFirstNewParticle * 4,
                                     (mSettings.MaxParticles - mFirstNewParticle) * 4,
                                     stride, SetDataOptions.NoOverwrite);

                if (mFirstFreeParticle > 0)
                {
                    mVertexBuffer.SetData(0, mParticlesBuffer,
                                         0, mFirstFreeParticle * 4,
                                         stride, SetDataOptions.NoOverwrite);
                }
            }

            // Move the particles we just uploaded from the new to the active queue.
            mFirstNewParticle = mFirstFreeParticle;
        }

        /// <summary>
        /// Crea una nueva particula en el buffer.
        /// </summary>
        /// <param name="position">Posicion inicial.</param>
        /// <param name="velocity">Velocidad inicial.</param>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            // Figure out where in the circular queue to allocate the new particle.
            int nextFreeParticle = mFirstFreeParticle + 1;

            if (nextFreeParticle >= mSettings.MaxParticles)
                nextFreeParticle = 0;

            // If there are no free particles, we just have to give up.
            if (nextFreeParticle == mFirstRetiredParticle)
                return;

            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= mSettings.EmitterVelocitySensitivity;

            // Add in some random amount of horizontal velocity.
            float horizontalVelocity = MathHelper.Lerp(mSettings.MinHorizontalVelocity,
                                                       mSettings.MaxHorizontalVelocity,
                                                       (float)MathUtil.GetRandomDouble());

            double horizontalAngle = MathUtil.GetRandomDouble() * MathHelper.TwoPi;

            velocity.X += horizontalVelocity * (float)Math.Cos(horizontalAngle);
            velocity.Z += horizontalVelocity * (float)Math.Sin(horizontalAngle);

            // Add in some random amount of vertical velocity.
            velocity.Y += MathHelper.Lerp(mSettings.MinVerticalVelocity,
                                          mSettings.MaxVerticalVelocity,
                                          (float)MathUtil.GetRandomDouble());

            // Choose four random control values. These will be used by the vertex
            // shader to give each particle a different size, rotation, and color.
            Color randomValues = new Color((byte)MathUtil.GetRandomInt(255),
                                           (byte)MathUtil.GetRandomInt(255),
                                           (byte)MathUtil.GetRandomInt(255),
                                           (byte)MathUtil.GetRandomInt(255));

            // Fill in the particle vertex structure.
            for (int i = 0; i < 4; i++)
            {
                mParticlesBuffer[mFirstFreeParticle * 4 + i].Position = position;
                mParticlesBuffer[mFirstFreeParticle * 4 + i].Velocity = velocity;
                mParticlesBuffer[mFirstFreeParticle * 4 + i].Random = randomValues;
                mParticlesBuffer[mFirstFreeParticle * 4 + i].Time = mCurrentTime;
            }

            mFirstFreeParticle = nextFreeParticle;
        }
        #endregion
    }
}
