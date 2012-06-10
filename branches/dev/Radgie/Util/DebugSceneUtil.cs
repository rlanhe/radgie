using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Entity;

namespace Radgie.Util
{
    /// <summary>
    /// Clase de utilidad para mostrar/ocultar los volumenes de colision de los objetos.
    /// </summary>
    public static class DebugSceneUtil
    {
        #region Methods
        /// <summary>
        /// Crea la geometria de los volumenes de colision de la escena.
        /// </summary>
        /// <param name="scene">Escena para la que se van a crear las geometrias de colision.</param>
        public static void ActivateDebug(IScene scene)
        {
            List<IDebugObject> dobjects = scene.GetGameObjects<IDebugObject>(true);
            if (dobjects.Count == 0)
            {
                List<IGameComponent> list = scene.GetGameComponents(true);
                foreach (IGameComponent gc in list)
                {
                    IterateGameComponents(gc);
                }
            }
        }

        /// <summary>
        /// Recorre recursivamente los GameComponentes creando las geometrias de los volumenes de colision.
        /// </summary>
        /// <param name="gc">GameComponent padre.</param>
        private static void IterateGameComponents(IGameComponent gc)
        {
            if (gc.BoundingVolume != null)
            {
                IDebugObject dObject = new BoundingVolume(gc.BoundingVolume);
                gc.AddGameObject(dObject);
            }

            IEnumerator<IGameComponent> components = gc.GameComponents;
            if (components != null)
            {
                while (components.MoveNext())
                {
                    IterateGameComponents(components.Current);
                }
            }
        }

        /// <summary>
        /// Elimina las geometrias de debug de los volumenes de colision de la escena si existen.
        /// </summary>
        /// <param name="scene">Escena para la que se quieren eliminar los volumenes de colision.</param>
        public static void DeactivateDebug(IScene scene)
        {
            List<IDebugObject> dobjects = scene.GetGameObjects<IDebugObject>(true);

            foreach (IDebugObject obj in dobjects)
            {
                obj.Component.RemoveGameObject(obj);
            }
        }
        #endregion
    }
}
