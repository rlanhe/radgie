using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core.BoundingVolumes;

namespace Radgie.Graphics.Camera
{
    /// <summary>
    /// Camara2D.
    /// </summary>
    public class Camera2D : Radgie.Core.GameComponent, ICamera2D
    {
        #region Properties
        /// <summary>
        /// Posicion a la que apunta la camara.
        /// </summary>
        protected Vector3 mTarget;

        /// <summary>
        /// Configuracion de la camara.
        /// </summary>
        protected CameraParameters mCameraParameters;

        #region ICamera2D Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.Up"/>
        /// </summary>
        public Vector2 Up
        {
            get
            {
                mUp.X = mCameraParameters.Up.X;
                mUp.Y = mCameraParameters.Up.Y;
                return mUp;
            }
            set
            {
                if(value != mUp)
                {
                    mUp = value;
                    mCameraParameters.Up = new Vector3(mUp.X, mUp.Y, 0.0f);
                }
            }
        }
        protected Vector2 mUp;

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.NearPlaneDistance"/>
        /// </summary>
        public float NearPlaneDistance
        {
            get
            {
                return mCameraParameters.NearPlaneDistance;
            }
            set
            {
                mCameraParameters.NearPlaneDistance = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.FarPlaneDistance"/>
        /// </summary>
        public float FarPlaneDistance
        {
            get
            {
                return mCameraParameters.FarPlaneDistance;
            }
            set
            {
                mCameraParameters.FarPlaneDistance = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.FieldOfView"/>
        /// </summary>
        public float FieldOfView
        {
            get
            {
                return mCameraParameters.FieldOfView;
            }
            set
            {
                mCameraParameters.FieldOfView = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.BackgroundColor"/>
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return mCameraParameters.BackgroundColor;
            }
            set
            {
                mCameraParameters.BackgroundColor = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.View"/>
        /// </summary>
        public Matrix View
        {
            get
            {
                return mCameraParameters.View;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.Projection"/>
        /// </summary>
        public Matrix Projection
        {
            get
            {
                return mCameraParameters.Projection;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera2D.Viewport"/>
        /// </summary>
        public virtual Viewport Viewport
        {
            get
            {
                return mCameraParameters.Viewport;
            }
            set
            {
                mCameraParameters.Viewport = value;
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// Callback para actualizar la matriz de vista.
        /// </summary>
        private CameraParameters.UpdateMatrix mUpdateViewCallback;
        /// <summary>
        /// Camara para actualizar la matriz de proyeccion.
        /// </summary>
        private CameraParameters.UpdateMatrix mUpdateProjectionCallback;

        #region Constructors
        /// <summary>
        /// Crea una camara 2d.
        /// </summary>
        /// <param name="id">Id de la camara.</param>
        public Camera2D(string id): base(id)
        {
            mUpdateViewCallback = UpdateView;
            mUpdateProjectionCallback = UpdateProjection;
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            mCameraParameters = new CameraParameters(new Viewport(gSystem.Device.PresentationParameters.Bounds), 0.1f, 1000.0f, MathHelper.PiOver4, Color.Transparent, Vector3.Up);
            Update(null);
        }
        #endregion

        #region Methods
        #region GameComponent Methods
        /// <summary>
        /// Actualiza la posicion de la camara.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la ultima actualizacion.</param>
        public override void Update(GameTime time)
        {
            if (Active)
            {
                //base.Update(time);

                mCameraParameters.Position = Vector3.Backward;

                if (mCameraParameters.UpdateView)
                {
                    mCameraParameters.UpdateViewMatrix(mUpdateViewCallback);
                }

                if (mCameraParameters.UpdateProjection)
                {
                    mCameraParameters.UpdateProjectionMatrix(mUpdateProjectionCallback);
                }
            }
        }
        #endregion

        /// <summary>
        /// Actualiza la matriz de vista.
        /// </summary>
        /// <param name="cameraParameters">Parametros de la camara.</param>
        /// <returns>Matriz de vista.</returns>
        public static Matrix UpdateView(CameraParameters cameraParameters)
        {
            cameraParameters.Target = cameraParameters.Position - Vector3.UnitZ;
            return Matrix.CreateLookAt(cameraParameters.Position, cameraParameters.Target, cameraParameters.Up);
        }

        /// <summary>
        /// Actualiza la matriz de proyeccion.
        /// </summary>
        /// <param name="cameraParameters">Parametros de la camara.</param>
        /// <returns>Matriz de proyeccion.</returns>
        public static Matrix UpdateProjection(CameraParameters cameraParameters)
        {
            Matrix mProjection;
            Matrix.CreateOrthographicOffCenter(0, cameraParameters.Viewport.Width, 0, cameraParameters.Viewport.Height, cameraParameters.NearPlaneDistance, cameraParameters.FarPlaneDistance, out mProjection);
            return mProjection;
        }
        #endregion
    }
}
