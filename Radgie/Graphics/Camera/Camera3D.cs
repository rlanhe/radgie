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
    /// Camara 3D.
    /// </summary>
    public class Camera3D: Radgie.Core.GameComponent, ICamera3D
    {
        #region Properties
        /// <summary>
        /// Parametros de camara.
        /// </summary>
        protected CameraParameters mCameraParameters;

        #region ICamera3D Properties
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.Target"/>
        /// </summary>
        public virtual Vector3 Target
        {
            get
            {
                return mCameraParameters.Target;
            }
            set
            {
                mCameraParameters.Target = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.Up"/>
        /// </summary>
        public virtual Vector3 Up
        {
            get
            {
                return mCameraParameters.Up;
            }
            set
            {
                mCameraParameters.Up = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.NearPlaneDistance"/>
        /// </summary>
        public virtual float NearPlaneDistance
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
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.FarPlaneDistance"/>
        /// </summary>
        public virtual float FarPlaneDistance
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
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.FieldOfView"/>
        /// </summary>
        public virtual float FieldOfView
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
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.BackgroundColor"/>
        /// </summary>
        public virtual Color BackgroundColor
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
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.View"/>
        /// </summary>
        public virtual Matrix View
        {
            get
            {
                return mCameraParameters.View;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.Projection"/>
        /// </summary>
        public virtual Matrix Projection
        {
            get
            {
                return mCameraParameters.Projection;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Graphics.Camera.ICamera3D.Viewport"/>
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

        /// <summary>
        /// Callback para actualizar la matriz de vista.
        /// </summary>
        private CameraParameters.UpdateMatrix mUpdateViewCallback;
        /// <summary>
        /// Callback para actualizar la matriz de proyeccion.
        /// </summary>
        private CameraParameters.UpdateMatrix mUpdateProjectionCallback;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva camara 3D.
        /// </summary>
        /// <param name="id">Id de la camara.</param>
        public Camera3D(string id): base(id)
        {
            mUpdateViewCallback = UpdateView;
            mUpdateProjectionCallback = UpdateProjection;
            IGraphicSystem gSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));
            mCameraParameters = new CameraParameters(new Viewport(gSystem.Device.PresentationParameters.Bounds), 0.1f, 1000.0f, MathHelper.PiOver4, Color.Transparent, Vector3.Up);
            mCameraParameters.Target = -Vector3.UnitZ;
            Update(null);
            mBoundingVolume = new Radgie.Core.BoundingVolumes.BoundingSphere(Transformation.Translation, 1.0f);
        }
        #endregion

        #region Methods
        #region GameComponents Methods
        /// <summary>
        /// Actualiza la posicion de la camara.
        /// </summary>
        /// <param name="time">Tiempo transcurrido desde la ultima actualizacion.</param>
        public override void Update(GameTime time)
        {
            if (IsActive())
            {
                base.Update(time);

                mCameraParameters.Position = World.Translation;

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
        /// Actualiza la matriz de vista de la camara.
        /// </summary>
        /// <param name="cameraParameters">Parametros de la camara.</param>
        /// <returns>Matriz de vista de la camara.</returns>
        public static Matrix UpdateView(CameraParameters cameraParameters)
        {
            return Matrix.CreateLookAt(cameraParameters.Position, cameraParameters.Target, cameraParameters.Up);
        }

        /// <summary>
        /// Actualiza la matriz de proyeccion de la camara.
        /// </summary>
        /// <param name="cameraParameters">Parametros de la camara.</param>
        /// <returns>Matriz de proyeccion de la camara.</returns>
        public static Matrix UpdateProjection(CameraParameters cameraParameters)
        {
            Matrix mProjection;
            Matrix.CreatePerspectiveFieldOfView(cameraParameters.FieldOfView, cameraParameters.Viewport.AspectRatio, cameraParameters.NearPlaneDistance, cameraParameters.FarPlaneDistance, out mProjection);
            return mProjection;
        }
        #endregion
    }
}
