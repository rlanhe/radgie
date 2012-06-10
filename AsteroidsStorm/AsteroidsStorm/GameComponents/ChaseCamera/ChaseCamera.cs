using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics.Camera;
using Radgie.Core;
using Microsoft.Xna.Framework;

namespace AsteroidsStorm.GameComponents.ChaseCamera
{
    /// <summary>
    /// Camara para seguir la accion del juego que persigue a la nave del jugador.
    /// </summary>
    public class ChaseCamera: Camera3D
    {
        /// <summary>
        /// Target de la camara.
        /// </summary>
        public Radgie.Core.IGameComponent TargetComponent
        {
            get
            {
                if ((mTarget != null) && (mTarget.IsAlive))
                {
                    return (Radgie.Core.GameComponent)mTarget.Target;
                }
                return null;
            }
            set
            {
                mTarget = new WeakReference(value);
            }
        }
        private WeakReference mTarget;

        /// <summary>
        /// Offset del target.
        /// </summary>
        public virtual Vector3 TargetOffset { get; set; }
        /// <summary>
        /// Offset de la posicion de la camara respecto al target.
        /// </summary>
        public virtual Vector3 PositionOffset { get; set; }

        /// <summary>
        /// Posicion minima hasta la que se puede mover la camara.
        /// </summary>
        public virtual Vector3 MinPos { get; set; }
        /// <summary>
        /// Posicion maxima hasta la que se puede mover la camara.
        /// </summary>
        public virtual Vector3 MaxPos { get; set; }
        /// <summary>
        /// Si la camara es Fixed, persigue constantemente el objetivo, si no solo mientras su posicion sea mayor que Min y menor que Max.
        /// </summary>
        public virtual bool Fixed { get; set; }

        /// <summary>
        /// Constructor de ChaseCamara.
        /// </summary>
        /// <param name="id">Identificador de la camara.</param>
        /// <param name="target">Target de la camara.</param>
        /// <param name="positionOffset">Offset de la posicion de la camara respecto al target.</param>
        /// <param name="targetOffset">Offset del target de la camara.</param>
        public ChaseCamera(string id, Radgie.Core.IGameComponent target, Vector3 positionOffset, Vector3 targetOffset)
            : base(id)
        {
            TargetComponent = target;
            PositionOffset = positionOffset;
            TargetOffset = targetOffset;
            mCameraParameters.FieldOfView = 0.76f;
            Fixed = false;
        }

        /// <summary>
        /// Constructor de ChaseCamara.
        /// </summary>
        /// <param name="id">Identificador de la camara.</param>
        /// <param name="target">Target de la camara.</param>
        /// <param name="positionOffset">Offset de la posicion de la camara respecto al target.</param>
        /// <param name="targetOffset">Offset del target de la camara.</param>
        /// <param name="minPos">Posicion minima hasta la que puede moverse la camara.</param>
        /// <param name="maxPos">Posicion maxima hasta la que se puede mover la camara.</param>
        public ChaseCamera(string id, Radgie.Core.IGameComponent target, Vector3 positionOffset, Vector3 targetOffset, Vector3 minPos, Vector3 maxPos)
            : this(id, target, positionOffset, targetOffset)
        {
            MaxPos = maxPos;
            MinPos = minPos;
            Fixed = true;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            if (IsActive() && (mTarget != null) && (mTarget.IsAlive) && (mTarget.Target != null))
            {
                Radgie.Core.GameComponent target = (Radgie.Core.GameComponent)mTarget.Target;
                Vector3 targetTranslation = target.Transformation.Matrix.Translation;

                if (Fixed)
                {
                    if (targetTranslation.Y < MinPos.Y)
                    {
                        targetTranslation.Y = MinPos.Y;
                    }
                    if (targetTranslation.X < MinPos.X)
                    {
                        targetTranslation.X = MinPos.X;
                    }
                    if (targetTranslation.Y > MaxPos.Y)
                    {
                        targetTranslation.Y = MaxPos.Y;
                    }
                    if (targetTranslation.X > MaxPos.X)
                    {
                        targetTranslation.X = MaxPos.X;
                    }
                }

                Transformation.Translation = targetTranslation + PositionOffset;
                mCameraParameters.Target = target.Transformation.Matrix.Translation + TargetOffset;
                base.Update(time);
            }
        }
    }
}
