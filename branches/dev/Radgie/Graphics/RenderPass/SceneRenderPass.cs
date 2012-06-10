using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Core;
using Radgie.Graphics.Camera;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Entity;

namespace Radgie.Graphics.RenderPass
{
    /// <summary>
    /// RenderPass para dibujar una escena.
    /// </summary>
    public class SceneRenderPass : ARenderPass
    {
        #region Properties
        /// <summary>
        /// Escena a dibujar.
        /// </summary>
        public IScene Scene
        {
            get
            {
                return mScene;
            }
            set
            {
                if (value != null)
                {
                    mScene = value;
                }
            }
        }
        protected IScene mScene;

        private List<IDrawable> mGraphicObjects;
        private List<ILight> mLights;
        private List<IEntityInstancesRenderer> mInstancesRenderersList;
        #endregion

        #region Constructors
        /// <summary>
        /// Pasada para dibujar una escena.
        /// </summary>
        /// <param name="scene">Escena a dibujar.</param>
        /// <param name="camera">Camara a usar durante el dibujado de la escena.</param>
        /// <param name="frustumCulling">True si debe descartar los objetos fuera del frustum de la camara, False en caso contrario.</param>
        /// <param name="cleanRT">True si debe limpiar el target antes de dibujar la escena, False en caso contrario.</param>
        public SceneRenderPass(IScene scene, ICamera camera, bool frustumCulling, bool cleanRT)
            : this(scene, null, camera, frustumCulling, cleanRT)
        {
        }

        /// <summary>
        /// Pasada para dibujar una escena.
        /// </summary>
        /// <param name="scene">Escena a dibujar.</param>
        /// <param name="target">Target donde dejar el resultado de dibujar la escena. Null si se quiere dejar en el backbuffer.</param>
        /// <param name="camera">Camara a usar durante el dibujado de la escena.</param>
        /// <param name="frustumCulling">True si debe descartar los objetos fuera del frustum de la camara, False en caso contrario.</param>
        /// <param name="cleanRT">True si debe limpiar el target antes de dibujar la escena, False en caso contrario.</param>
        public SceneRenderPass(IScene scene, RenderTarget2D target, ICamera camera, bool frustumCulling, bool cleanRT)
            : base(target, camera, frustumCulling, cleanRT)
        {
            mScene = scene;
            mGraphicObjects = new List<IDrawable>();
            mLights = new List<ILight>();
            mInstancesRenderersList = new List<IEntityInstancesRenderer>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Ver <see cref="Radgie.Graphics.ARenderPass.RenderAction"/>
        /// </summary>
        public override void RenderAction(IRenderer renderer)
        {
            mLights.Clear();
            mGraphicObjects.Clear();
            mInstancesRenderersList.Clear();

            if (mFrustumCulling)
            {
                Radgie.Core.BoundingVolumes.BoundingFrustum frustum = new Radgie.Core.BoundingVolumes.BoundingFrustum(mCamera.View * mCamera.Projection);
                mScene.GetGameObjects<ILight>(frustum, false, mLights);
                mScene.GetGameObjects<IDrawable>(frustum, false, mGraphicObjects);
            }
            else
            {
                mScene.GetGameObjects<ILight>(false, mLights);
                mScene.GetGameObjects<IDrawable>(false, mGraphicObjects);
            }

            for (int i = mGraphicObjects.Count - 1; i >= 0; i--)
            {
                IDrawable obj = mGraphicObjects[i];
                if (obj is IGraphicInstance)
                {
                    IGraphicInstance gInstance = (IGraphicInstance)obj;
                    mGraphicObjects.RemoveAt(i);

                    IEntityInstancesRenderer instanceRenderer = ((IGraphicEntity)gInstance.Entity).InstancesRenderer;
                    instanceRenderer.AddInstance(gInstance);
                    if (!mInstancesRenderersList.Contains(instanceRenderer))
                    {
                        mInstancesRenderersList.Add(instanceRenderer);
                    }
                }
            }

            if (mInstancesRenderersList != null)
            {
                foreach (IEntityInstancesRenderer instanceRenderer in mInstancesRenderersList)
                {
                    mGraphicObjects.Add(instanceRenderer);
                }
            }
            
            renderer.Lights = mLights;
            mGraphicObjects.Sort();
            foreach (IDrawable gObject in mGraphicObjects)
            {
                gObject.Draw(renderer);
            }
        }
        #endregion
    }
}
