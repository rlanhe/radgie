using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace Radgie.Scene
{
	/// <summary>
	/// Sistema de escenas.
	/// </summary>
	public class SceneSystem: ASystem, ISceneSystem
	{
		#region Properties
		/// <summary>
		/// Escenas que maneja el sistema.
		/// </summary>
		private IDictionary<string, IScene> mScenes;

        #region ISceneSystem Properties
        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneSystem.Statistics"/>
        /// </summary>
        public SceneSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private SceneSystemStatistics mStatistics;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
		/// Inicializa el sistema de escenas.
		/// </summary>
		/// <param name="sc">Seccion de configuracion del sistema en el fichero de configuracion de Radgie</param>
		public SceneSystem(XElement sc): base(sc)
		{
			ConfigSceneSystem(sc);
			mScenes = new ConcurrentDictionary<string, IScene>();
            mStatistics = new SceneSystemStatistics();
		}
		#endregion

		#region Methods
		#region ASystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.UpdateAction"/>
        /// </summary>
		protected override void UpdateAction(GameTime time)
		{
            mStatistics.Reset();
            mStatistics.StartUpdateTimer();
			foreach (KeyValuePair<string, IScene> tuple in mScenes)
			{
                mStatistics.NumberOfScenes++;
				IScene scene = tuple.Value;
				if (scene.Active)
				{
                    mStatistics.NumberOfScenesToUpdate++;
                    RadgieGame.Instance.JobScheduler.AddJob(scene.UpdateJob);
				}
			}

            foreach (KeyValuePair<string, IScene> tuple in mScenes)
            {
                IScene scene = tuple.Value;
                if (scene.Active)
                {
                    scene.UpdateJob.Wait();
                }
            }
            mStatistics.StopUpdateTimer();
		}
		#endregion

		#region ISceneSystem Members
        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneSystem.AddScene"/>
        /// </summary>
		public bool AddScene(IScene scene)
		{
            if (scene == null)
            {
                throw new ArgumentNullException("Scene is null");
            }

			if (mScenes.ContainsKey(scene.Id))
			{
				return false;
			}
			mScenes.Add(scene.Id, scene);
            // Activa la escena al annadirla al system
            scene.Active = true;
			return true;
		}

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneSystem.GetScene"/>
        /// </summary>
		public IScene GetScene(string id)
		{
			IScene scene = null;
			mScenes.TryGetValue(id, out scene);
			return scene;
		}

        /// <summary>
        /// Ver <see cref="Radgie.Scene.ISceneSystem.RemoveScene"/>
        /// </summary>
		public bool RemoveScene(IScene scene)
		{
            if (scene == null)
            {
                throw new ArgumentNullException("Scene is null");
            }

			bool result = mScenes.Remove(new KeyValuePair<string, IScene>(scene.Id, scene));

            // Desactiva la escena al quitarla del ScenSystem
            if (result)
            {
                scene.Active = false;
            }

            return result;
		}
		#endregion

		/// <summary>
		/// Configura el sistema de escenas.
		/// </summary>
		/// <param name="sc">Seccion de configuracion del sistema en el fichero de configuracion de Radgie</param>
        private void ConfigSceneSystem(XElement sc)
		{
			// TODO: Cargar configuracion del sistema
		}
		#endregion
	}
}
