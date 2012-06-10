using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Intefaz para los objetos que pueden ser observadores de GameComponents.
    /// Estos objetos seran notificados cuando se produzca un movimiento del objeto o cambie su posicion en la escena.
    /// </summary>
    public interface IGameComponentObserver
    {
        #region Methods
        /// <summary>
        /// Metodo usado por los GameComponents del nodo para notificar cambios en su estado.
        /// </summary>
        /// <param name="gc">Gamecomponent del nodo que ha modificado su estado.</param>
        void NotifyUpdateFromComponent(IGameComponent gc);
        /// <summary>
        /// Metodo usado por los GameComponents del nodo para notificar una cambio de GameComponent padre.
        /// </summary>
        /// <param name="gc">Gamecomponent que ha cambiado de component padre.</param>
        void NotifyParentChangeFromComponent(IGameComponent gc);
        #endregion
    }
}
