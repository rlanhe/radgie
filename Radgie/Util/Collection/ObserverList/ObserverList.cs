using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util.Collection.ObserverList
{
    /// <summary>
    /// Lista de objetos que deben ser notificados ante el suceso de un evento.
    /// Permite annadir observers a un objeto y este decide cuando notificar cambios en su estado.
    /// </summary>
    /// <typeparam name="T">Tipo del parametro pasado en la notificacion.</typeparam>
    public class ObserverList<T>
    {
        #region Delegates
        /// <summary>
        /// Delegado para notificar a los objetos.
        /// </summary>
        /// <typeparam name="T">Tipo del parametro pasado en la notificacion.</typeparam>
        /// <param name="param">Valor pasado en la notificacion.</param>
        public delegate void NotifyObserverDelegate(T param);
        #endregion

        #region Properties
        /// <summary>
        /// Estructura de datos para mantener la lista de observers y los metodos que han de ejecutarse durante su notificacion.
        /// </summary>
        private IDictionary<object, NotifyObserverDelegate> mObservers;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa el objeto.
        /// </summary>
        public ObserverList()
        {
            mObservers = new Dictionary<object, NotifyObserverDelegate>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Annade un nuevo observer.
        /// </summary>
        /// <param name="observer">Observer a annadir.</param>
        /// <param name="delegateMethod">Metodo que se ejecutara para notificarle cambios.</param>
        public void AddObserver(object observer, NotifyObserverDelegate delegateMethod)
        {
            NotifyObserverDelegate tmp;
            mObservers.TryGetValue(observer, out tmp);

            if(tmp == null)
            {
                mObservers.Add(observer, delegateMethod);
            }
        }

        /// <summary>
        /// Elimina un observer.
        /// </summary>
        /// <param name="observer">Observer a eliminar.</param>
        public void RemoveObserver(object observer)
        {
            mObservers.Remove(observer);
        }

        /// <summary>
        /// Notifica la actualizacion a los observers.
        /// </summary>
        /// <param name="param">Valor notificado a los observers.</param>
        public void NotifyObservers(T param)
        {
            foreach (KeyValuePair<object, NotifyObserverDelegate> tuple in mObservers)
            {
                tuple.Value(param);
            }
        }
        #endregion
    }
}
