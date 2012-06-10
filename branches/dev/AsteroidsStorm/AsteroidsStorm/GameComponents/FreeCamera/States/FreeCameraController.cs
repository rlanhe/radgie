using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.State;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Camera;

namespace AsteroidsStorm.GameComponents.FreeCamera.States
{
    /// <summary>
    /// Controlador de camara que permite moverla libremente por la escena del juego.
    /// </summary>
    class FreeCameraController: AState
    {
        private static Quaternion DEFAULT_ROTATION = Quaternion.Identity;
        private const float ROTATION_MULTIPLIER = 0.05f;
        private const float TRANSLATION_MULTIPLIER = 0.7f;

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public FreeCameraController(): base(null)
        {
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            base.Update(time);

            float secondsElapsed = (float)time.ElapsedGameTime.TotalSeconds;
            float rotationMultiplier = ROTATION_MULTIPLIER * secondsElapsed;
            float translationMultiplier = TRANSLATION_MULTIPLIER * secondsElapsed;

            Radgie.Core.IGameComponent owner = Owner.Component;
            ICamera3D camera = GetFromContext<ICamera3D>("Camera");
            IFreeCameraInputController input = GetFromContext<IFreeCameraInputController>("Input");

            int xDelta = (input.Rotation.X - input.Rotation.PreviousX);
            int yDelta = (input.Rotation.Y - input.Rotation.PreviousY);
            if (input.RotationActivated.Pressed)
            {
                owner.Transformation.Rotation *= (Quaternion.CreateFromAxisAngle(Vector3.Right, yDelta * rotationMultiplier) * Quaternion.CreateFromAxisAngle(Vector3.Up, xDelta * rotationMultiplier));
            }

            Vector3 moveVector = Vector3.Zero;
            if (input.Forward.Pressed)
            {
                moveVector += Vector3.Forward;
            }
            else if (input.Backward.Pressed)
            {
                moveVector -= Vector3.Forward;
            }

            if (input.Left.Pressed)
            {
                moveVector -= Vector3.Right;
            }
            else if (input.Right.Pressed)
            {
                moveVector += Vector3.Right;
            }
            moveVector *= translationMultiplier;

            moveVector = Vector3.Transform(moveVector, owner.Transformation.Rotation);
            owner.Transformation.Translation += moveVector;
        }
    }
}
