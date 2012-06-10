using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Util
{
    /// <summary>
    /// Clase de utilidad para el manejo de volumenes de colision.
    /// </summary>
    public static class BoundingUtil
    {
        #region Properties
        #region Supported Types
        /// <summary>
        /// Esfera de colision.
        /// </summary>
        public static readonly Type SPHERE = typeof(BoundingSphere);
        /// <summary>
        /// Caja de colision.
        /// </summary>
        public static readonly Type BOX = typeof(BoundingBox);
        /// <summary>
        /// ViewFrustum de colision.
        /// </summary>
        public static readonly Type FRUSTUM = typeof(BoundingFrustum);
        /// <summary>
        /// Plano de colision.
        /// </summary>
        public static readonly Type PLANE = typeof(Plane);
        /// <summary>
        /// Rayo de colision.
        /// </summary>
        public static readonly Type RAY = typeof(Ray);
        /// <summary>
        /// Volumen de colision formado por varios volumenes hijos.
        /// </summary>
        public static readonly Type COMPOSITE = typeof(CompositeBoundingVolume);
        /// <summary>
        /// Punto en el espacio.
        /// </summary>
        public static readonly Type POINT = typeof(Point);
        #endregion
        #endregion

        #region Methods
        #region Intersects
        /// <summary>
        /// Determina si dos bounding volumes se intersecan
        /// </summary>
        /// <param name="bv1">Volumen 1</param>
        /// <param name="bv2">Volumen 2</param>
        /// <returns>Devuelve null si no hay colision, cualquier otro valor en caso contrario.</returns>
        public static float? Intersects(IBoundingVolume bv1, IBoundingVolume bv2)
        {
            Type bv1Type = bv1.GetType();
            Type bv2Type = bv2.GetType();

            // Sphere - Sphere
            if ((bv1Type == SPHERE) && (bv2Type == SPHERE))
            {
                return Intersects((BoundingSphere)bv1, (BoundingSphere)bv2);
            }
            // Sphere - Box
            else if ((bv1Type == SPHERE) && (bv2Type == BOX))
            {
                return Intersects((BoundingSphere)bv1, (BoundingBox)bv2);
            }
            else if ((bv1Type == BOX) && (bv2Type == SPHERE))
            {
                return Intersects((BoundingSphere)bv2, (BoundingBox)bv1);
            }
            // Sphere - Frustum
            else if ((bv1Type == SPHERE) && (bv2Type == FRUSTUM))
            {
                return Intersects((BoundingSphere)bv1, (BoundingFrustum)bv2);
            }
            else if ((bv1Type == FRUSTUM) && (bv2Type == SPHERE))
            {
                return Intersects((BoundingSphere)bv2, (BoundingFrustum)bv1);
            }
            // Sphere - Plane
            else if ((bv1Type == SPHERE) && (bv2Type == PLANE))
            {
                return Intersects((BoundingSphere)bv1, (Plane)bv2);
            }
            else if ((bv1Type == PLANE) && (bv2Type == SPHERE))
            {
                return Intersects((BoundingSphere)bv2, (Plane)bv1);
            }
            // Sphere - Ray
            else if ((bv1Type == SPHERE) && (bv2Type == RAY))
            {
                return Intersects((BoundingSphere)bv1, (Ray)bv2);
            }
            else if ((bv1Type == RAY) && (bv2Type == SPHERE))
            {
                return Intersects((BoundingSphere)bv2, (Ray)bv1);
            }
            // Box - Box
            else if ((bv1Type == BOX) && (bv2Type == BOX))
            {
                return Intersects((BoundingBox)bv1, (BoundingBox)bv2);
            }
            // Box - Frustum
            else if ((bv1Type == BOX) && (bv2Type == FRUSTUM))
            {
                return Intersects((BoundingBox)bv1, (BoundingFrustum)bv2);
            }
            else if ((bv1Type == FRUSTUM) && (bv2Type == BOX))
            {
                return Intersects((BoundingBox)bv2, (BoundingFrustum)bv1);
            }
            // Box - Plane
            else if ((bv1Type == BOX) && (bv2Type == PLANE))
            {
                return Intersects((BoundingBox)bv1, (Plane)bv2);
            }
            else if ((bv1Type == PLANE) && (bv2Type == BOX))
            {
                return Intersects((BoundingBox)bv2, (Plane)bv1);
            }
            // Box - Ray
            else if ((bv1Type == BOX) && (bv2Type == RAY))
            {
                return Intersects((BoundingBox)bv1, (Ray)bv2);
            }
            else if ((bv1Type == RAY) && (bv2Type == BOX))
            {
                return Intersects((BoundingBox)bv2, (Ray)bv1);
            }
            // Frustum - Frustum
            else if ((bv1Type == FRUSTUM) && (bv2Type == FRUSTUM))
            {
                return Intersects((BoundingFrustum)bv1, (BoundingFrustum)bv2);
            }
            // Frustum - Plane
            else if ((bv1Type == FRUSTUM) && (bv2Type == PLANE))
            {
                return Intersects((BoundingFrustum)bv1, (Plane)bv2);
            }
            else if ((bv1Type == PLANE) && (bv2Type == FRUSTUM))
            {
                return Intersects((BoundingFrustum)bv2, (Plane)bv1);
            }
            // Frustum - Ray
            else if ((bv1Type == FRUSTUM) && (bv2Type == RAY))
            {
                return Intersects((BoundingFrustum)bv1, (Ray)bv2);
            }
            else if ((bv1Type == RAY) && (bv2Type == FRUSTUM))
            {
                return Intersects((BoundingFrustum)bv2, (Ray)bv1);
            }
            // Composite - Composite
            else if ((bv1Type == COMPOSITE) && (bv2Type == COMPOSITE))
            {
                return Intersects((CompositeBoundingVolume)bv2, (CompositeBoundingVolume)bv1);
            }
            // OtherVolume - Composite
            else if ((bv2Type == COMPOSITE))
            {
                return Intersects(bv1, (CompositeBoundingVolume)bv2);
            }
            else if ((bv1Type == COMPOSITE))
            {
                return Intersects(bv2, (CompositeBoundingVolume)bv1);
            }

            return null;
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre dos esferas.
        /// </summary>
        /// <param name="bv1">Esfera1.</param>
        /// <param name="bv2">Esfera2.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingSphere bv1, BoundingSphere bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre una esfera y un cubo.
        /// </summary>
        /// <param name="bv1">Esfera.</param>
        /// <param name="bv2">Cubo.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingSphere bv1, BoundingBox bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre una esfera y un frustum.
        /// </summary>
        /// <param name="bv1">Esfera.</param>
        /// <param name="bv2">Frustum.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingSphere bv1, BoundingFrustum bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre una esfera y un plano.
        /// </summary>
        /// <param name="bv1">Esfera.</param>
        /// <param name="bv2">Plano.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingSphere bv1, Plane bv2)
        {
            return PlaneIntersection2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre una esfera y un rayo.
        /// </summary>
        /// <param name="bv1">Esfera.</param>
        /// <param name="bv2">Rayo.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingSphere bv1, Ray bv2)
        {
            return bv1.BoundingVolume.Intersects(bv2.BoundingVolume);
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre dos cubos.
        /// </summary>
        /// <param name="bv1">Cubo 1.</param>
        /// <param name="bv2">Cubo 2.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingBox bv1, BoundingBox bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre un cubo y un frustum.
        /// </summary>
        /// <param name="bv1">Cubo.</param>
        /// <param name="bv2">Frustum.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingBox bv1, BoundingFrustum bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre un cubo y un plano.
        /// </summary>
        /// <param name="bv1">Cubo.</param>
        /// <param name="bv2">Plano.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingBox bv1, Plane bv2)
        {
            return PlaneIntersection2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre un cubo y un rayo.
        /// </summary>
        /// <param name="bv1">Cubo.</param>
        /// <param name="bv2">Rayo.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingBox bv1, Ray bv2)
        {
            return bv1.BoundingVolume.Intersects(bv2.BoundingVolume);
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre dos frustum.
        /// </summary>
        /// <param name="bv1">Frustum 1.</param>
        /// <param name="bv2">Frustum 2.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingFrustum bv1, BoundingFrustum bv2)
        {
            return Bool2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre un frustum y un plano.
        /// </summary>
        /// <param name="bv1">Frustum.</param>
        /// <param name="bv2">Plano.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingFrustum bv1, Plane bv2)
        {
            return PlaneIntersection2Float(bv1.BoundingVolume.Intersects(bv2.BoundingVolume));
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre un frustum y un rayo.
        /// </summary>
        /// <param name="bv1">Frustum.</param>
        /// <param name="bv2">Rayo.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(BoundingFrustum bv1, Ray bv2)
        {
            return bv1.BoundingVolume.Intersects(bv2.BoundingVolume);
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre cualquier volumen de colision y un volumen de tipo Composite
        /// </summary>
        /// <param name="bv1">Volumen de colision 1.</param>
        /// <param name="bv2">CompositeBoundingVolume.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(IBoundingVolume bv1, CompositeBoundingVolume bv2)
        {
            float? result;
            
            foreach(IBoundingVolume bv in bv2.BoundingVolumes)
            {
                result = Intersects(bv1, bv);
                if(result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Metodo para comprobar la interseccion entre dos volumenes de colision de tipo CompositeBoundingVolume.
        /// </summary>
        /// <param name="bv1">CompositeBoundingVolume 1.</param>
        /// <param name="bv2">CompositeBoundingVolume 2.</param>
        /// <returns>null si no colisionan, cualquier otro valor en caso contrario.</returns>
        private static float? Intersects(CompositeBoundingVolume bv1, CompositeBoundingVolume bv2)
        {
            float? result;

            foreach (IBoundingVolume bv1_1 in bv1.BoundingVolumes)
            {
                foreach (IBoundingVolume bv2_1 in bv2.BoundingVolumes)
                {
                    result = Intersects(bv1_1, bv2_1);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Convierte un valor bool en float.
        /// </summary>
        /// <param name="value">Valor booleano.</param>
        /// <returns>1.0f si value es true, null si es false.</returns>
        private static float? Bool2Float(bool value)
        {
            return value ? (float?)1.0f : null;
        }

        /// <summary>
        /// Convierte un valor PlaneIntersectionType en float.
        /// </summary>
        /// <param name="type">Valor PlaneIntersectionType.</param>
        /// <returns>1.0f si value es Intersecting, null en caso contrario.</returns>
        private static float? PlaneIntersection2Float(Microsoft.Xna.Framework.PlaneIntersectionType type)
        {
            return type == Microsoft.Xna.Framework.PlaneIntersectionType.Intersecting ? (float?)1.0f : null;
        }

        #endregion

        #region Contains

        /// <summary>
        /// Determina si un volumen contiene a otro.
        /// </summary>
        /// <param name="bv1">Volumen que contiene a otro.</param>
        /// <param name="bv2">Volumen que es contenido.</param>
        /// <returns>Resultado de evaluar si un volumen contiene a otro.</returns>
        public static Microsoft.Xna.Framework.ContainmentType Contains(IBoundingVolume bv1, IBoundingVolume bv2)
        {
            if((bv1 != null) && (bv2 != null))
            {
                Type bv1Type = bv1.GetType();
                Type bv2Type = bv2.GetType();

                // Sphere - Sphere
                if ((bv1Type == SPHERE) && (bv2Type == SPHERE))
                {
                    return Contains((BoundingSphere)bv1, (BoundingSphere)bv2);
                }
                // Sphere - Box
                else if ((bv1Type == SPHERE) && (bv2Type == BOX))
                {
                    return Contains((BoundingSphere)bv1, (BoundingBox)bv2);
                }
                else if ((bv1Type == BOX) && (bv2Type == SPHERE))
                {
                    return Contains((BoundingBox)bv1, (BoundingSphere)bv2);
                }
                // Sphere - Frustum
                else if ((bv1Type == SPHERE) && (bv2Type == FRUSTUM))
                {
                    return Contains((BoundingSphere)bv1, (BoundingFrustum)bv2);
                }
                else if ((bv1Type == FRUSTUM) && (bv2Type == SPHERE))
                {
                    return Contains((BoundingFrustum)bv1, (BoundingSphere)bv2);
                }
                // Sphere - Point
                else if ((bv1Type == SPHERE) && (bv2Type == POINT))
                {
                    return Contains((BoundingSphere)bv1, (Point)bv2);
                }
                // Box - Box
                if ((bv1Type == BOX) && (bv2Type == BOX))
                {
                    return Contains((BoundingBox)bv1, (BoundingBox)bv2);
                }
                // Box - Frustum
                else if ((bv1Type == BOX) && (bv2Type == FRUSTUM))
                {
                    return Contains((BoundingBox)bv1, (BoundingFrustum)bv2);
                }
                else if ((bv1Type == FRUSTUM) && (bv2Type == BOX))
                {
                    return Contains((BoundingFrustum)bv1, (BoundingBox)bv2);
                }
                // Box - Point
                else if ((bv1Type == BOX) && (bv2Type == POINT))
                {
                    return Contains((BoundingBox)bv1, (Point)bv2);
                }
                // Frustum - Frustum
                if ((bv1Type == FRUSTUM) && (bv2Type == FRUSTUM))
                {
                    return Contains((BoundingFrustum)bv1, (BoundingFrustum)bv2);
                }
                // Frustum - Point
                else if ((bv1Type == FRUSTUM) && (bv2Type == POINT))
                {
                    return Contains((BoundingFrustum)bv1, (Point)bv2);
                }
                // Composite - Composite
                else if ((bv1Type == COMPOSITE) && (bv2Type == COMPOSITE))
                {
                    return Contains((CompositeBoundingVolume)bv2, (CompositeBoundingVolume)bv1);
                }
                // OtherVolume - Composite
                else if ((bv2Type == COMPOSITE))
                {
                    return Contains(bv1, (CompositeBoundingVolume)bv2);
                }
                else if ((bv1Type == COMPOSITE))
                {
                    return Contains((CompositeBoundingVolume)bv1, bv2);
                }
            }

            return Microsoft.Xna.Framework.ContainmentType.Disjoint;
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingSphere bv1, BoundingSphere bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingSphere bv1, BoundingBox bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingBox bv1, BoundingSphere bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingSphere bv1, BoundingFrustum bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingFrustum bv1, BoundingSphere bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingSphere bv1, Point bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingBox bv1, BoundingBox bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingBox bv1, BoundingFrustum bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingFrustum bv1, BoundingBox bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingBox bv1, Point bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingFrustum bv1, BoundingFrustum bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(BoundingFrustum bv1, Point bv2)
        {
            return bv1.BoundingVolume.Contains(bv2.BoundingVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(CompositeBoundingVolume bv1, CompositeBoundingVolume bv2)
        {
            return bv1.CompositeVolume.Contains(bv2.CompositeVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(IBoundingVolume bv1, CompositeBoundingVolume bv2)
        {
            return bv1.Contains(bv2.CompositeVolume);
        }

        /// <summary>
        /// Evalua si bv1 contiene a bv2.
        /// </summary>
        /// <param name="bv1">Volumen de colision que contiene a otro.</param>
        /// <param name="bv2">Volumen de colision que es contenido.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        private static Microsoft.Xna.Framework.ContainmentType Contains(CompositeBoundingVolume bv1, IBoundingVolume bv2)
        {
            return bv1.CompositeVolume.Contains(bv2);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// A partir de una lista de puntos obtiene la esfera que los contiene a todos.
        /// </summary>
        /// <param name="source">Lista de puntos.</param>
        /// <returns>Esfera resultado.</returns>
        public static BoundingSphere GetSphere(List<Point> source)
        {
            List<Microsoft.Xna.Framework.Vector3> points = new List<Microsoft.Xna.Framework.Vector3>();

            foreach (Point point in source)
            {
                points.Add(point.BoundingVolume);
            }
            return new BoundingSphere(Microsoft.Xna.Framework.BoundingSphere.CreateFromPoints(points));
        }

        /// <summary>
        /// A partir de un frustum genera la esfera que lo contiene.
        /// </summary>
        /// <param name="source">Frustum.</param>
        /// <returns>Esfera resultado.</returns>
        public static BoundingSphere GetSphere(BoundingFrustum source)
        {
           return new BoundingSphere(Microsoft.Xna.Framework.BoundingSphere.CreateFromFrustum(source.BoundingVolume));
        }

        /// <summary>
        /// A partir de un cubo genera la esfera que lo contiene.
        /// </summary>
        /// <param name="source">Cubo.</param>
        /// <returns>Esfera resultado.</returns>
        public static BoundingSphere GetSphere(BoundingBox source)
        {
            return new BoundingSphere(Microsoft.Xna.Framework.BoundingSphere.CreateFromBoundingBox(source.BoundingVolume));
        }

        /// <summary>
        /// A partir de una lista de puntos genera el cubo que los contiene.
        /// </summary>
        /// <param name="source">Lista de puntos.</param>
        /// <returns>Esfera resultado.</returns>
        public static BoundingBox GetBox(List<Point> source)
        {
            List<Microsoft.Xna.Framework.Vector3> points = new List<Microsoft.Xna.Framework.Vector3>();

            foreach (Point point in source)
            {
                points.Add(point.BoundingVolume);
            }
            return new BoundingBox(Microsoft.Xna.Framework.BoundingBox.CreateFromPoints(points));
        }

        /// <summary>
        /// A partir de una esfera genera el cubo que la contiene.
        /// </summary>
        /// <param name="source">Esfera.</param>
        /// <returns>Esfera resultado.</returns>
        public static BoundingBox GetBox(BoundingSphere source)
        {
            return new BoundingBox(Microsoft.Xna.Framework.BoundingBox.CreateFromSphere(source.BoundingVolume));
        }

        #endregion

        #region Merges

        /// <summary>
        /// A partir de volumenes de colision, genera uno que contiene a los dos.
        /// </summary>
        /// <param name="source1">Volumen de colision 1.</param>
        /// <param name="source2">Volumen de colision 2.</param>
        /// <returns>Volumen resultado.</returns>
        public static IBoundingVolume Merge(IBoundingVolume source1, IBoundingVolume source2)
        {
            Type typeS1 = source1.GetType();
            Type typeS2 = source2.GetType();

            if ((typeS1 == SPHERE) && (typeS2 == SPHERE))
            {
                return Merge((BoundingSphere) source1, (BoundingSphere) source2);
            }
            else if ((typeS1 == BOX) && (typeS2 == BOX))
            {
                return Merge((BoundingBox) source1, (BoundingBox) source2);
            }

            return null;
        }

        /// <summary>
        /// A partir de dos esferas, genera una que contiene a las dos.
        /// </summary>
        /// <param name="source1">Volumen de colision 1.</param>
        /// <param name="source2">Volumen de colision 2.</param>
        /// <returns>Volumen Resultado.</returns>
        public static BoundingSphere Merge(BoundingSphere source1, BoundingSphere source2)
        {
            return new BoundingSphere(Microsoft.Xna.Framework.BoundingSphere.CreateMerged(source1.BoundingVolume, source2.BoundingVolume));
        }

        /// <summary>
        /// A partir de dos cubos, genera uno que contiene a los dos.
        /// </summary>
        /// <param name="source1">Volumen de colision 1.</param>
        /// <param name="source2">Volumen de colision 2.</param>
        /// <returns>Volumen Resultado.</returns>
        public static BoundingBox Merge(BoundingBox source1, BoundingBox source2)
        {
            return new BoundingBox(Microsoft.Xna.Framework.BoundingBox.CreateMerged(source1.BoundingVolume, source2.BoundingVolume));
        }

        #endregion
        #endregion
    }
}
