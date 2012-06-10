using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
#if WIN32
using log4net;
#endif
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Radgie.Graphics;

namespace Radgie.Core
{
    /// <summary>
    /// Simplifica la carga de recursos de disco.
    /// Abstrae al usuario de las tareas de localizacion de recursos.
    /// </summary>
    public class ResourceManager
    {
        #region Constants
        /// <summary>
        /// Marca en la ruta que se sustitye por el local empleado por el jugador.
        /// </summary>
        public const string LOCALE_KEY = "$locale$";
        #endregion

        #region Properties
        /// <summary>
        /// Directorio raiz del Content.
        /// </summary>
        public string ContentDirPath
        {
            get
            {
                return mContentDirPath;
            }
        }
        private string mContentDirPath;

        /// <summary>
        /// LanguageId utilizado por el jugador.
        /// </summary>
        public string LanguageId
        {
            get
            {
                return CultureInfo.CurrentCulture.Name;
            }
        }

        /// <summary>
        /// LanguageId utilizado cuando no se encuentra un recurso en el local del usuario.
        /// </summary>
        public string DefaultLanguageId
        {
            get
            {
                return mDefaultLanguageId;
            }
        }
        private string mDefaultLanguageId;

        private IDictionary<int, ContentManager> mContentManager;
        #endregion

        #region Constructors
        /// <summary>
        /// Construye un objeto resource manager.
        /// </summary>
        /// <param name="defaultLanguageId">Id del lenguaje por defecto cuando no se encuentra un recurso en el idioma del usuario.</param>
        public ResourceManager(string contentPath, string defaultLanguageId)
        {
            mContentDirPath = contentPath;
            mContentManager = new Dictionary<int, ContentManager>();
            mDefaultLanguageId = defaultLanguageId;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Carga un recurso.
        /// Op. thread safe.
        /// </summary>
        /// <typeparam name="T">Tipo del recurso.</typeparam>
        /// <param name="id">Id del recurso.</param>
        /// <returns>Recurso cargado desde el disco.</returns>
        /// <exception cref="Exception">Si el recurso no se encuentra en disco.</exception>
        public T Load<T>(string id)
        {
            return Load<T>(id, id.Contains(LOCALE_KEY), true);
        }

        /// <summary>
        /// Carga un recurso.
        /// Si se marca como no threadSafe, trata de cargar el recurso sin esperar por tener el dispositivo grafico (Usar solo para recursos no graficos).
        /// </summary>
        /// <typeparam name="T">Tipo del recurso.</typeparam>
        /// <param name="id">Id del recurso.</param>
        /// <param name="forceThreadSafe">True, fuerza a que la carga quede bloqueada hasta que el dispositivo grafico este disponible, False en caso contrario.</param>
        /// <returns>Recurso cargado desde el disco.</returns>
        /// <exception cref="Exception">Si el recurso no se encuentra en disco.</exception>
        public T Load<T>(string id, bool forceThreadSafe)
        {
            return Load<T>(id, id.Contains(LOCALE_KEY), forceThreadSafe);
        }

        /// <summary>
        /// Carga un recurso de disco.
        /// </summary>
        /// <typeparam name="T">Tipo del recurso.</typeparam>
        /// <param name="id">Id del recurso.</param>
        /// <param name="internationalized">Indica si se quiere cargar un recurso internacionalizado.</param>
        /// <param name="forceThreadSafe">True, fuerza a que la carga quede bloqueada hasta que el dispositivo grafico este disponible, False en caso contrario.</param>
        /// <returns></returns>
        private T Load<T>(string id, bool internationalized, bool forceThreadSafe)
        {
            T resource = default(T);

            IGraphicSystem graphicSystem = (IGraphicSystem)RadgieGame.Instance.GetSystem(typeof(IGraphicSystem));

            // Sólo se pueden cargar contenidos desde un único hilo a la vez.
            lock (this)
            {
                try
                {
                    if ((graphicSystem != null) && (forceThreadSafe))
                    {
                        // Bloquea el acceso al dispositivo grafico para evitar conflictos entre hilos.
                        Monitor.Enter(graphicSystem.Device);
                    }
                    // Trata de buscar el recurso en el local del jugador.
                    if (internationalized)
                    {
                        string path = id.Replace(LOCALE_KEY, LanguageId);
                        try
                        {
                            resource = GetContentManager().Load<T>(path);
                        }
                        catch
                        {
                        }

                        // Trata de buscar el recurso en el local por defecto.
                        if (resource == null)
                        {
                            path = id.Replace(LOCALE_KEY, DefaultLanguageId);
                            resource = GetContentManager().Load<T>(path);
                        }
                    }
                    else
                    {
                        // Si no esta internacionalizado carga directamente el recurso.
                        resource = GetContentManager().Load<T>(id);
                    }
                }
                catch
                {
                }
                finally
                {
                    if ((graphicSystem != null) && (forceThreadSafe))
                    {
                        // Desbloquea el dispositivo grafico.
                        Monitor.Exit(graphicSystem.Device);
                    }
                }
            }

            return resource;
        }

        /// <summary>
        /// Obtiene el ContentManager asociado al hilo de ejecucion actual.
        /// Si el hilo no tiene un ContentManager asociado crea uno nuevo.
        /// </summary>
        /// <returns>ContentManager del hilo de ejecucion.</returns>
        private ContentManager GetContentManager()
        {
            ContentManager cManager;
            mContentManager.TryGetValue(Thread.CurrentThread.ManagedThreadId, out cManager);
            
            if (cManager == null)
            {
                cManager = new ContentManager(RadgieGame.Instance.Services, mContentDirPath);
                mContentManager.Add(Thread.CurrentThread.ManagedThreadId, cManager);
            }

            return cManager;
        }
        #endregion
    }
}
