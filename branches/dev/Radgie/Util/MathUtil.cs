using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Util
{
    public class MathUtil
    {
        #region Properties
        /// <summary>
        /// Generador de numeros aletorios.
        /// </summary>
        private static Random mRandomGenerator = new Random();
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el angulo en radianes entre dos vectores.
        /// </summary>
        /// <param name="source">Vector1.</param>
        /// <param name="dest">Vector2.</param>
        /// <param name="destsRight">Vector derecha del vector2.</param>
        /// <returns>angulo en radianes.</returns>
        public static float GetSignedAngleBetweenTwoVectors(ref Vector3 source, ref Vector3 dest, ref Vector3 destsRight)
        {
            // We make sure all of our vectors are unit length 
            source.Normalize();
            dest.Normalize();
            destsRight.Normalize();

            float forwardDot = Vector3.Dot(source, dest);
            float rightDot = Vector3.Dot(source, destsRight);

            // Make sure we stay in range no matter what, so Acos doesn't fail later 
            forwardDot = MathHelper.Clamp(forwardDot, -1.0f, 1.0f);

            double angleBetween = Math.Acos((float)forwardDot);

            if (rightDot < 0.0f)
                angleBetween *= -1.0f;

            return (float)angleBetween;
        }

        /// <summary>
        /// Obtiene un double aleatorio.
        /// </summary>
        /// <returns>Valor aleatorio de tipo double.</returns>
        public static double GetRandomDouble()
        {
            lock (mRandomGenerator)
            {
                return mRandomGenerator.NextDouble();
            }
        }

        /// <summary>
        /// Obtiene un int aleatorio.
        /// </summary>
        /// <returns>Valor aleatorio de tipo int.</returns>
        public static int GetRandomInt()
        {
            lock (mRandomGenerator)
            {
                return mRandomGenerator.Next();
            }
        }

        /// <summary>
        /// Obtiene un int aleatorio inferior o igual a max.
        /// </summary>
        /// <param name="max">Limite superior del numero aleatorio.</param>
        /// <returns>Valor aleatorio de tipo int.</returns>
        public static int GetRandomInt(int max)
        {
            lock (mRandomGenerator)
            {
                return mRandomGenerator.Next(max);
            }
        }

        /// <summary>
        /// Obtiene un valor aleatorio comprendido entre min y max.
        /// </summary>
        /// <param name="min">Limite inferior.</param>
        /// <param name="max">Limite superior.</param>
        /// <returns>Valor aleatorio de tipo int.</returns>
        public static int GetRandomInt(int min, int max)
        {
            lock (mRandomGenerator)
            {
                return mRandomGenerator.Next(min, max);
            }
        }

        /// <summary>
        /// Rellena un array de bytes con valores aleatorios.
        /// </summary>
        /// <param name="array">Array a rellenar.</param>
        public static void GetRandomByte(byte[] array)
        {
            lock (mRandomGenerator)
            {
                mRandomGenerator.NextBytes(array);
            }
        }
        #endregion
    }
}
