using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsStorm.GameComponents.GUI
{
    /// <summary>
    /// Interfaz de los widgets que pueden contener otros widgets.
    /// </summary>
    public interface IContainer: IWidget
    {

        /// <summary>
        /// Annade un componente hijo.
        /// Si el componente que se quiere annadir esta annadido a otro componente, lo quita de este.
        /// </summary>
        /// <param name="widget">Componente hijo.</param>
        /// <returns>True si lo annade, False en caso contrario.</returns>
        bool AddWidget(IWidget widget);
        /// <summary>
        /// Quita un componente hijo.
        /// </summary>
        /// <param name="widget">Componente hijo a quitar.</param>
        /// <returns>True si lo elimina, False en caso contrario.</returns>
        bool RemoveWidget(IWidget widget);
        /// <returns>Devuelve el siguiente widget al actual segun su TabOrder</returns>
        IWidget NextWidget();
        /// <returns>Devuelve el widget anterior al actual segun su TabOrder</returns>
        IWidget PreviousWidget();
        /// <returns>Devuelve el widget actual.</returns>
        IWidget CurrentWidget();
        /// <summary>
        /// Resetea el estado del widget.
        /// </summary>
        void Reset();
        /// <summary>
        /// Lista de widgets hijos.
        /// </summary>
        IEnumerator<IWidget> Childs { get; }
    }
}
