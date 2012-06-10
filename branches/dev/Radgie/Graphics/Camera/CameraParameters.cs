using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Radgie.Graphics.Camera
{
    /// <summary>
    /// Parametros de una camara.
    /// </summary>
    public struct CameraParameters
    {
        #region Delegates
        /// <summary>
        /// Delegado para actualizar matrices de la camara.
        /// </summary>
        /// <param name="cameraParameters">Parametros de la camara.</param>
        /// <returns>Matriz actualizada.</returns>
        public delegate Matrix UpdateMatrix(CameraParameters cameraParameters);
        #endregion

        #region Properties
        /// <summary>
        /// Viewport de la camara.
        /// Seccion del rendertarget donde dibujara la escena.
        /// </summary>
        public Viewport Viewport
        {
            get
            {
                return mViewport;
            }
            set
            {
                mViewport = value;
                mUpdateProjection = true;
            }
        }
        private Viewport mViewport;
        
        /// <summary>
        /// Distancia al plano Near de la camara.
        /// </summary>
        public float NearPlaneDistance
        {
            get
            {
                return mNearPlaneDistance;
            }
            set
            {
                // TODO: check value
                if (value != mNearPlaneDistance)
                {
                    mNearPlaneDistance = value;
                    mUpdateProjection = true;
                }
            }
        }
        private float mNearPlaneDistance;

        /// <summary>
        /// Distancia al plano Far de la camara.
        /// </summary>
        public float FarPlaneDistance
        {
            get
            {
                return mFarPlaneDistance;
            }
            set
            {
                // TODO: check value
                if (value != mFarPlaneDistance)
                {
                    mFarPlaneDistance = value;
                    mUpdateProjection = true;
                }
            }
        }
        private float mFarPlaneDistance;

        /// <summary>
        /// Campo de vision de la camara.
        /// Expresado en radianes.
        /// </summary>
        public float FieldOfView
        {
            get
            {
                return mFieldOfView;
            }
            set
            {
                // TODO: check value
                if (value != mFieldOfView)
                {
                    mFieldOfView = value;
                    mUpdateProjection = true;
                }
            }
        }
        private float mFieldOfView;

        /// <summary>
        /// Color de fondo.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return mBackgroundColor;
            }
            set
            {
                // TODO: check value
                if (value != mBackgroundColor)
                {
                    mBackgroundColor = value;
                }
            }
        }
        private Color mBackgroundColor;

        /// <summary>
        /// Matriz de vista.
        /// </summary>
        public Matrix View
        {
            get
            {
                return mView;
            }
        }
        private Matrix mView;

        /// <summary>
        /// Matriz de proyeccion.
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return mProjection;
            }
        }
        private Matrix mProjection;

        /// <summary>
        /// Indica si se debe actualizar la matriz de vista.
        /// </summary>
        public bool UpdateView
        {
            get
            {
                return mUpdateView;
            }
        }
        private bool mUpdateView;

        /// <summary>
        /// Indica si se debe actualizar la matriz de proyeccion.
        /// </summary>
        public bool UpdateProjection
        {
            get
            {
                return mUpdateProjection;
            }
        }
        private bool mUpdateProjection;

        /// <summary>
        /// Posicion de la camara en el espacio.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
            set
            {
                if (mPosition != value)
                {
                    mPosition = value;
                    mUpdateView = true;
                }
            }
        }
        private Vector3 mPosition;

        /// <summary>
        /// Posicion a donde apunta la camara.
        /// </summary>
        public Vector3 Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                if (mTarget != value)
                {
                    mTarget = value;
                    mUpdateView = true;
                }
            }
        }
        private Vector3 mTarget;

        /// <summary>
        /// Vector Up de la camara.
        /// </summary>
        public Vector3 Up
        {
            get
            {
                return mUp;
            }
            set
            {
                if (mUp != value)
                {
                    mUp = value;
                    mUpdateView = true;
                }
            }
        }
        private Vector3 mUp;

        #endregion

        #region Methods
        /// <summary>
        /// Actualiza la matriz de vista de la camara.
        /// </summary>
        /// <param name="updateMatrix">Delegado que recalcula la matriz de vista.</param>
        public void UpdateViewMatrix(UpdateMatrix updateMatrix)
        {
            mView = updateMatrix(this);
            mUpdateView = false;
        }

        /// <summary>
        /// Actualiza la matriz de proyeccion de la camara.
        /// </summary>
        /// <param name="updateMatrix">Delegado que recalcula la matriz de proyeccion de la camara.</param>
        public void UpdateProjectionMatrix(UpdateMatrix updateMatrix)
        {
            mProjection = updateMatrix(this);
            mUpdateProjection = false;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa la configuracion de la camara.
        /// </summary>
        /// <param name="viewport">Viewport</param>
        /// <param name="nearPlane">Distancia al plano near.</param>
        /// <param name="farPlane">Distancia al plano far.</param>
        /// <param name="fieldOfView">Campo de vision de la camara.</param>
        /// <param name="backgroundColor">Color de fondo.</param>
        /// <param name="up">Vector up de la camara.</param>
        public CameraParameters(Viewport viewport, float nearPlane, float farPlane, float fieldOfView, Color backgroundColor, Vector3 up)
        {
            mViewport = viewport;
            mNearPlaneDistance = nearPlane;
            mFarPlaneDistance = farPlane;
            mFieldOfView = fieldOfView;
            mBackgroundColor = backgroundColor;
            mUp = up;
            mView = Matrix.Identity;
            mProjection = Matrix.Identity;
            mPosition = Vector3.Backward;
            mTarget = Vector3.Zero;
            mUpdateView = mUpdateProjection = true;
        }
        #endregion
    }
}
