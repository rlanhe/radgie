using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.State;
using AsteroidsStorm.GameComponents.FreeCamera.States;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Camera;

namespace AsteroidsStorm.GameComponents.FreeCamera
{
    /// <summary>
    /// Camara que se mueve libremente por el espacio de juego.
    /// </summary>
    public class FreeCamera: Radgie.Core.GameComponent
    {
        /// <summary>
        /// Propiedad para el acceso al objeto ICamera3D que encapsula.
        /// </summary>
        public ICamera3D Camera
        {
            get
            {
                return mCamera;
            }
        }
        private ICamera3D mCamera;

        /// <summary>
        /// Crea una nueva FreeCamera que hace uso de input como controlador de movimientos.
        /// </summary>
        /// <param name="id">Identificador de la camara.</param>
        /// <param name="input">Controlador para manejar la camara.</param>
        public FreeCamera(string id, IFreeCameraInputController input)
            : base(id)
        {
            SetInContext("Input", input);

            ChaseCamera.ChaseCamera camera = new ChaseCamera.ChaseCamera("", this, Vector3.Backward, Vector3.Zero);
            SetInContext("Camera", camera);
            mCamera = camera;

            AddGameObject(new Behaviour(new FreeCameraController()));
            AddGameComponent(camera);
        }
    }
}
