using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core
{
    /// <summary>
    /// Representa una transformacion geometrica sobre un objeto.
    /// Combina las operaciones 2D y 3D mas habituales que se realizan sobre los objetos del juego. Para las transformaciones 2D se considera que Z es 0.
    /// </summary>
    public class Transformation
    {
        #region Properties

        #region 3D
        /// <summary>
        /// Matriz de transformacion.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                Update();
                return mMatrix;
            }
            set
            {
                if (mMatrix != value)
                {
                    Init(ref value);
                }
            }
        }
        private Matrix mMatrix;

        /// <summary>
        /// Transformacion sobre el centro del objeto.
        /// Normalmente es Vector3.Zero. Se utiliza para desviar el centro del objeto a la hora de indicar su posicion y rotacion.
        /// </summary>
        public Vector3 Center
        {
            get
            {
                return mCenter;
            }
            set
            {
                if (mCenter != value)
                {
                    mCenter = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Vector3 mCenter;

        /// <summary>
        /// Indica la posicion del objeto.
        /// </summary>
        public Vector3 Translation
        {
            get
            {
                return mTranslation;
            }
            set
            {
                if (mTranslation != value)
                {
                    mTranslation = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Vector3 mTranslation;

        /// <summary>
        /// Rotacion del objeto.
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return mRotation;
            }
            set
            {
                if (mRotation != value)
                {
                    mRotation = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Quaternion mRotation;

        /// <summary>
        /// Punto a partir del cual se aplica la escala.
        /// </summary>
        public Vector3 ScaleCenter
        {
            get
            {
                return mScaleCenter;
            }
            set
            {
                if (mScaleCenter != value)
                {
                    mScaleCenter = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Vector3 mScaleCenter;

        /// <summary>
        /// Rotacion con la que se aplica la escala.
        /// </summary>
        public Quaternion ScaleRotation
        {
            get
            {
                return mScaleRotation;
            }
            set
            {
                if (mScaleRotation != value)
                {
                    mScaleRotation = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Quaternion mScaleRotation;

        /// <summary>
        /// Escala que se aplica al objeto.
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                return mScale;
            }
            set
            {
                if (mScale != value)
                {
                    mScale = value;
                    mUpdateTransformation = true;
                }
            }
        }
        private Vector3 mScale;

        /// <summary>
        /// Vector Forward tras aplicar la transformacion.
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                Update();
                return Vector3.Transform(Vector3.Forward, mRotation);
            }
        }

        /// <summary>
        /// Vector Up tras aplicar la transformacion.
        /// </summary>
        public Vector3 Up
        {
            get
            {
                Update();
                return Vector3.Transform(Vector3.Up, mRotation);
            }
        }

        /// <summary>
        /// Vector Right tras aplicar la transformacion.
        /// </summary>
        public Vector3 Right
        {
            get
            {
                Update();
                return Vector3.Transform(Vector3.Right, mRotation);
            }
        }

        #endregion

        #region 2D

        /// <summary>
        /// Centro a partir del cual se especifica la posicion y rotacion del objeto.
        /// </summary>
        public Vector2 Center2D
        {
            get
            {
                Vector3 tmp = Center;
                return new Vector2(tmp.X, tmp.Y);
            }
            set
            {
                Vector3 tmp = new Vector3(value.X, value.Y, 0.0f);
                Center = tmp;
            }
        }

        /// <summary>
        /// Desplazamiento 2D del objeto.
        /// </summary>
        public Vector2 Translation2D
        {
            get
            {
                Vector3 tmp = Translation;
                return new Vector2(tmp.X, tmp.Y);
            }
            set
            {
                Vector3 tmp = new Vector3(value.X, value.Y, 0.0f);
                Translation = tmp;
            }
        }

        /// <summary>
        /// Rotacion 2D del objeto.
        /// </summary>
        public float Rotation2D
        {
            get
            {
                return Rotation.W;
            }
            set
            {
                Quaternion tmp = Rotation;
                tmp.W = value;
                Rotation = tmp;
            }
        }

        /// <summary>
        /// Punto a partir del cual se escala el objeto.
        /// </summary>
        public Vector2 ScaleCenter2D
        {
            get
            {
                Vector3 tmp = ScaleCenter;
                return new Vector2(tmp.X, tmp.Y);
            }
            set
            {
                Vector3 tmp = new Vector3(value.X, value.Y, 0.0f);
                ScaleCenter = tmp;
            }
        }

        /// <summary>
        /// Rotacion con la que se aplica la escala.
        /// </summary>
        public float ScaleRotation2D
        {
            get
            {
                return ScaleRotation.W;
            }
            set
            {
                Quaternion tmp = ScaleRotation;
                tmp.W = value;
                ScaleRotation = tmp;
            }
        }

        /// <summary>
        /// Escala sobre el objeto.
        /// </summary>
        public Vector2 Scale2D
        {
            get
            {
                Vector3 tmp = Scale;
                return new Vector2(tmp.X, tmp.Y);
            }
            set
            {
                Vector3 tmp = new Vector3(value.X, value.Y, 0.0f);
                Scale = tmp;
            }
        }

        #endregion

        public bool mUpdateTransformation;

        #region Constants

        /// <summary>
        /// Matriz identidad.
        /// </summary>
        private static Matrix DEFAULT_MATRIX = Matrix.Identity;

        /// <summary>
        /// Translacion por defecto.
        /// </summary>
        private static Vector3 DEFAULT_TRANSLATION = Vector3.Zero;
        /// <summary>
        /// Rotacion por defecto.
        /// </summary>
        private static Quaternion DEFAULT_ROTATION = Quaternion.Identity;
        /// <summary>
        /// Escala por defecto.
        /// </summary>
        private static Vector3 DEFAULT_SCALE = Vector3.One;

        /// <summary>
        /// Traslacion por defecto 2D.
        /// </summary>
        private static Vector2 DEFAULT_TRANSLATION_2D = Vector2.Zero;
        /// <summary>
        /// Rotacion por defecto 2D.
        /// </summary>
        private static float DEFAULT_ROTATION_2D = 0.0f;
        /// <summary>
        /// Escala por defecto 2D.
        /// </summary>
        private static Vector2 DEFAULT_SCALE_2D = Vector2.One;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Transformacion con valores por defecto.
        /// </summary>
        public Transformation()
        {
            Init();
        }

        /// <summary>
        /// Crea una transformacion a partir de otra.
        /// </summary>
        /// <param name="transformation">Transformacion base.</param>
        public Transformation(Transformation transformation)
        {
            Init(transformation);
        }

        /// <summary>
        /// Crea una transformacion a partir de una matriz de transformacion.
        /// </summary>
        /// <param name="transformation">Matrix de transformacion.</param>
        public Transformation(Matrix transformation)
        {
            Init(transformation);
        }

        #region 3D

        public Transformation(Vector3 translation)
        {
            Init(ref translation);
        }

        public Transformation(Quaternion rotation)
        {
            Init(ref rotation);
        }

        public Transformation(Vector3 translation, Quaternion rotation)
        {
            Init(ref translation, ref rotation);
        }

        public Transformation(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            Init(ref translation, ref rotation, ref scale);
        }

        public Transformation(ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale)
        {
            Init(ref translation, ref rotation, ref scale);
        }

        public Transformation(Vector3 translation, Quaternion rotation, Vector3 scale, Vector3 center, Vector3 scaleCenter, Quaternion scaleRotation)
        {
            Init(ref translation, ref rotation, ref scale, ref center, ref scaleCenter, ref scaleRotation);
        }

        public Transformation(ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale, ref Vector3 center, ref Vector3 scaleCenter, ref Quaternion scaleRotation)
        {
            Init(ref translation, ref rotation, ref scale, ref center, ref scaleCenter, ref scaleRotation);
        }
        #endregion

        #region 2D
        public Transformation(Vector2 translation2D)
        {
            Init(ref translation2D);
        }

        public Transformation(ref Vector2 translation2D)
        {
            Init(ref translation2D);
        }

        public Transformation(float rotation2D)
        {
            Init(rotation2D);
        }
        
        public Transformation(Vector2 translation2D, float rotation2D)
        {
            Init(ref translation2D, rotation2D);
        }

        public Transformation(ref Vector2 translation2D, float rotation2D)
        {
            Init(ref translation2D, rotation2D);
        }

        public Transformation(Vector2 translation2D, float rotation2D, Vector2 scale2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D);
        }

        public Transformation(ref Vector2 translation2D, float rotation2D, ref Vector2 scale2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D);
        }

        public Transformation(Vector2 translation2D, float rotation2D, Vector2 scale2D, Vector2 center2D, Vector2 scaleCenter2D, float scaleRotation2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref center2D, ref scaleCenter2D, scaleRotation2D);
        }

        public Transformation(ref Vector2 translation2D, float rotation2D, ref Vector2 scale2D, ref Vector2 center2D, ref Vector2 scaleCenter2D, float scaleRotation2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref center2D, ref scaleCenter2D, scaleRotation2D);
        }
        #endregion

        #endregion

        #region Methods

        public void Init()
        {
            Init(ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(Transformation transformation)
        {
            Set(transformation);
        }

        public void Init(Matrix transformation)
        {
            Init(ref transformation);
        }

        public void Init(ref Matrix transformation)
        {
            mMatrix = transformation;
            mMatrix.Decompose(out mScale, out mRotation, out mTranslation);
            mCenter = DEFAULT_TRANSLATION;
            mScaleCenter = DEFAULT_TRANSLATION;
            mScaleRotation = DEFAULT_ROTATION;
            mUpdateTransformation = false;
        }

        #region 3D

        public void Init(Vector3 translation)
        {
            Init(ref translation, ref DEFAULT_ROTATION, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(ref Vector3 translation)
        {
            Init(ref translation, ref DEFAULT_ROTATION, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(Quaternion rotation)
        {
            Init(ref DEFAULT_TRANSLATION, ref rotation, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(ref Quaternion rotation)
        {
            Init(ref DEFAULT_TRANSLATION, ref rotation, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(Vector3 translation, Quaternion rotation)
        {
            Init(ref translation, ref rotation, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(ref Vector3 translation, ref Quaternion rotation)
        {
            Init(ref translation, ref rotation, ref DEFAULT_SCALE, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            Init(ref translation, ref rotation, ref scale, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale)
        {
            Init(ref translation, ref rotation, ref scale, ref DEFAULT_TRANSLATION, ref DEFAULT_TRANSLATION, ref DEFAULT_ROTATION);
        }

        public void Init(Vector3 translation, Quaternion rotation, Vector3 scale, Vector3 center, Vector3 scaleCenter, Quaternion scaleRotation)
        {
            Init(ref translation, ref rotation, ref scale, ref center, ref scaleCenter, ref scaleRotation);
        }

        public void Init(ref Vector3 translation, ref Quaternion rotation, ref Vector3 scale, ref Vector3 center, ref Vector3 scaleCenter, ref Quaternion scaleRotation)
        {
            mCenter = center;
            mTranslation = translation;
            mRotation = rotation;
            mScaleCenter = scaleCenter;
            mScaleRotation = scaleRotation;
            mScale = scale;
            mUpdateTransformation = true;
            Update();
        }

        #endregion

        #region 2D
        public void Init(Vector2 translation2D)
        {
            Init(ref translation2D, DEFAULT_ROTATION_2D, ref DEFAULT_SCALE_2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(ref Vector2 translation2D)
        {
            Init(ref translation2D, DEFAULT_ROTATION_2D, ref DEFAULT_SCALE_2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(float rotation2D)
        {
            Init(ref DEFAULT_TRANSLATION_2D, rotation2D, ref DEFAULT_SCALE_2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }
        
        public void Init(Vector2 translation2D, float rotation2D)
        {
            Init(ref translation2D, rotation2D, ref DEFAULT_SCALE_2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(ref Vector2 translation2D, float rotation2D)
        {
            Init(ref translation2D, rotation2D, ref DEFAULT_SCALE_2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(Vector2 translation2D, float rotation2D, Vector2 scale2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(ref Vector2 translation2D, float rotation2D, ref Vector2 scale2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref DEFAULT_TRANSLATION_2D, ref DEFAULT_TRANSLATION_2D, DEFAULT_ROTATION_2D);
        }

        public void Init(Vector2 translation2D, float rotation2D, Vector2 scale2D, Vector2 center2D, Vector2 scaleCenter2D, float scaleRotation2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref center2D, ref scaleCenter2D, scaleRotation2D);
        }

        public void Init(ref Vector2 translation2D, float rotation2D, ref Vector2 scale2D, ref Vector2 center2D, ref Vector2 scaleCenter2D, float scaleRotation2D)
        {
            Init(ref translation2D, rotation2D, ref scale2D, ref center2D, ref scaleCenter2D, scaleRotation2D);
        }
        #endregion

        /// <summary>
        /// Descompone la matriz de transformacion en las componentes de escala, rotacion y traslacion.
        /// Si se usa este metodo, se pierden center, scaleCenter y scaleRotation.
        /// </summary>
        private void Descompose()
        {
            mMatrix.Decompose(out mScale, out mRotation, out mTranslation);
            mCenter = DEFAULT_TRANSLATION;
            mScaleCenter = DEFAULT_TRANSLATION;
            mScaleRotation = DEFAULT_ROTATION;
            mUpdateTransformation = false;
        }

        #region Operations

        /// <summary>
        /// Multiplica una transformacion por otra.
        /// </summary>
        /// <param name="op">Transformacion que va a aplicar sobre la primera.</param>
        /// <returns>Transformacion resultante.</returns>
        public Transformation Multiply(Transformation op)
        {
            Transformation result = new Transformation();
            this.Multiply(op, result);
            return result;
        }

        /// <summary>
        /// Multiplica una transformacion por otra.
        /// </summary>
        /// <param name="op">Transformacion que va a aplicar sobre la primera.</param>
        /// <param name="result">Transformacion resultante.</param>
        public void Multiply(Transformation op, Transformation result)
        {
            this.Update();
            op.Update();
            Matrix resultM = new Matrix();
            Matrix.Multiply(ref mMatrix, ref op.mMatrix, out resultM);
            result.Init(ref resultM);
        }

        /// <summary>
        /// Copia una transformacion sobre otra.
        /// </summary>
        /// <param name="original">Transformacion original.</param>
        public void Set(Transformation original)
        {
            Init(ref original.mTranslation, ref original.mRotation, ref original.mScale, ref original.mCenter, ref original.mScaleCenter, ref original.mScaleRotation);
            mMatrix = original.mMatrix;
            mUpdateTransformation = original.mUpdateTransformation;
        }

        /// <summary>
        /// Indica si la transformacion es identica a la pasada por parametro.
        /// </summary>
        /// <param name="op">Transformacion con la que comparar.</param>
        /// <returns>True si son iguales, False en caso contrario.</returns>
        public bool Equals(Transformation op)
        {
            this.Update();
            op.Update();
            return Matrix == op.Matrix;
        }

        #endregion

        // TODO: Test
        /// <summary>
        /// Actualiza la matriz de transformacion a partir del resto de atributos de la transformacion.
        /// </summary>
        public void Update()
        {
            if (mUpdateTransformation)
            {
                bool hasScale = false;
                Matrix preTransformMatrix = Matrix.Identity;
                bool preTransform = false;
                Matrix scale = Matrix.CreateScale(mScale);
                Matrix result = Matrix.Identity;

                if (mScale != Vector3.One)
                {
                    hasScale = true;
                    scale = Matrix.CreateScale(mScale);
                    
                    if (mScaleCenter != Vector3.Zero)
                    {
                        preTransformMatrix *= Matrix.CreateTranslation(mScaleCenter);
                        preTransform = true;
                    }
                    if (mScaleRotation != Quaternion.Identity)
                    {
                        preTransformMatrix *= Matrix.CreateFromQuaternion(mScaleRotation);
                        preTransform = true;
                    }
                    if (preTransform)
                    {
                        scale = preTransformMatrix * scale;
                    }
                }

                bool hasRotation = false;
                Matrix rotation = Matrix.CreateFromQuaternion(mRotation);

                if (mRotation != Quaternion.Identity)
                {
                    hasRotation = true;
                    if (mCenter != Vector3.Zero)
                    {
                        preTransformMatrix = Matrix.CreateTranslation(mCenter);
                        rotation = preTransformMatrix * rotation;
                    }
                }

                Matrix translation = Matrix.CreateTranslation(mCenter + mTranslation);

                if (hasScale && hasRotation)
                {
                    result = scale * rotation * translation;
                }
                else if (hasRotation)
                {
                    result = rotation * translation;
                }
                else if (hasScale)
                {
                    result = scale * translation;
                }
                else
                {
                    result = translation;
                }

                mMatrix = result;
                mUpdateTransformation = false;
            }
        }

        #region Operators

        /// <summary>
        /// Operador *.
        /// </summary>
        /// <param name="op1">Transformation op1.</param>
        /// <param name="op2">Transformation op2.</param>
        /// <returns>Multiplicacion de matrices.</returns>
        public static Transformation operator* (Transformation op1, Transformation op2)
        {
            return op1.Multiply(op2);
        }

        /// <summary>
        /// Operador ==.
        /// </summary>
        /// <param name="op1">Transformation op1.</param>
        /// <param name="op2">Transformation op2.</param>
        /// <returns>True si son iguales, false en caso contrario.</returns>
        public static bool operator== (Transformation op1, Transformation op2)
        {
            return op1.Equals(op2);
        }

        /// <summary>
        /// Operador ==.
        /// </summary>
        /// <param name="op1">Transformation op1.</param>
        /// <param name="op2">Transformation op2.</param>
        /// <returns>True si son distintas, false en caso contrario.</returns>
        public static bool operator!= (Transformation op1, Transformation op2)
        {
            return !op1.Equals(op2);
        }

        #endregion

        /// <summary>
        /// Indica si la transformacion es identica a la pasada por parametro.
        /// </summary>
        /// <param name="obj">Transformacion con la que comparar.</param>
        /// <returns>True si son iguales, False en caso contrario.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals((Transformation)obj);
        }

        /// <summary>
        /// HashCode del objeto
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
