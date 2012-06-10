using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    /// <summary>
    /// Pasada del proceso de renderizado.
    /// </summary>
    public abstract class ARenderPass: IRenderable
    {
        #region Properties
        /// <summary>
        /// Target en el que dibujara esta pasada.
        /// Si es null, se dibuja diractamente en Backbuffer.
        /// </summary>
        public RenderTarget2D Target
        {
            get
            {
                return mTarget;
            }
            set
            {
                mTarget = value;
            }
        }
        protected RenderTarget2D mTarget;

        /// <summary>
        /// Indica si debe descartar los objetos que no estan dentro del frustum de la camara.
        /// </summary>
        public bool FrustumCulling
        {
            get
            {
                return mFrustumCulling;
            }
            set
            {
                mFrustumCulling = value;
            }
        }
        protected bool mFrustumCulling;

        /// <summary>
        /// Indica si debe limpiar el renderTarget antes de dibujar sober el.
        /// </summary>
        public bool CleanRT
        {
            get
            {
                return mCleanRT;
            }
            set
            {
                mCleanRT = value;
            }
        }
        protected bool mCleanRT;

        #region IRenderable Members
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderable.Camera"/>
        /// </summary>
        public ICamera Camera
        {
            get
            {
                return mCamera;
            }
            set
            {
                mCamera = value;
            }
        }
        protected ICamera mCamera;

        private static ICamera mDefaultCamera = new Camera2D("defaultCamera");
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Crea una nueva pasada de renderizado.
        /// </summary>
        /// <param name="target">Destino donde se dejara el resultado de ejecutar esta pasada. Si es null, se dibujara directamente en Backbuffer.</param>
        /// <param name="camera">Camara para dibujar la escena.</param>
        /// <param name="frustumCulling">Indica si debe descartar los objetos que no estan dentro del frustum de la camara.</param>
        /// <param name="cleanRT">Indica si debe limpiar el target antes de dibujar en el.</param>
        /// <exception cref="Exception">Si no hay un sistema grafico inicializado.</exception>
        public ARenderPass(RenderTarget2D target, ICamera camera, bool frustumCulling, bool cleanRT)
        {
            mCamera = camera;
            mTarget = target;
            mFrustumCulling = frustumCulling;
            mCleanRT = cleanRT;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Acciones que debe realizar antes de dibujar sobre el target.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        public virtual void PreRenderAction(IRenderer renderer)
        {
        }

        /// <summary>
        /// Renderizado en si.
        /// Personalizable en clases hijas.
        /// </summary>
        public abstract void RenderAction(IRenderer renderer);

        /// <summary>
        /// Acciones que debe realizar despues de dibujar sobre el target.
        /// </summary>
        /// <param name="renderer"></param>
        public virtual void PosRenderAction(IRenderer renderer)
        {
        }

        #region IRenderable Members
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.IRenderable.Render"/>
        /// </summary>
        public void Render(IRenderer renderer)
        {
            PreRenderAction(renderer);

            if (mTarget != null)
            {
                renderer.Device.SetRenderTarget(mTarget);
            }

            if (mCamera != null)
            {
                renderer.CurrentCamera = mCamera;
            }
            else
            {
                renderer.CurrentCamera = mDefaultCamera;
            }

            if (mCleanRT)
            {
                renderer.Clean();
            }

            RenderAction(renderer);

            if (mTarget != null)
            {
                renderer.Device.SetRenderTarget(null);
            }

            PosRenderAction(renderer);
        }
        #endregion
        #endregion
    }
}
