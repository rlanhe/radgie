using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace AsteroidsStorm.GameComponents.GUI
{
    /// <summary>
    /// Interfaz de elementos de interfaz 2D
    /// </summary>
    public interface IWidget : IComparable<IWidget>
    {
        #region IWidget Properties
        /// <summary>
        /// Widget padre.
        /// </summary>
        IContainer Parent { get; set; }
        /// <summary>
        /// Posicion en el eje X relativa a la esquina inferior izquierda de su componente padre.
        /// </summary>
        int X { get; set; }
        /// <summary>
        /// Posicion en el eje Y relativa a la esquina inferior izquierda de su componente padre.
        /// </summary>
        int Y { get; set; }
        /// <summary>
        /// Ancho del componente.
        /// Puede expresarse como un entero o como un % respecto al ancho del componente padre.
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Altura del componente.
        /// Puede expresarse como un entero o como un % respecto a la altura del componente padre.
        /// </summary>
        int Height { get; set; }
        /// <summary>
        /// Representacion grafica del componente.
        /// </summary>
        IGameComponent GameComponent { get; }
        /// <summary>
        /// Draw order usado para dibujar el widget.
        /// </summary>
        int DrawOrder { get; set; }
        /// <summary>
        /// Orden de tabulacion.
        /// Se utiliza para determinar el orden de navegacion entre varios componentes con un mismo componente padre.
        /// </summary>
        byte TabOrder { get; set; }
        /// <summary>
        /// Widget visible.
        /// </summary>
        bool Visible { get; set; }
        /// <summary>
        /// Delegado que se ejecuta al tomar el foco.
        /// </summary>
        WidgetActionDelegate OnActivateDelegate { get; set; }
        /// <summary>
        /// Delegado que se ejecuta al perder el foco.
        /// </summary>
        WidgetActionDelegate OnDeactivateDelegate { get; set; }
        /// <summary>
        /// Delegado que se ejecuta al ejecutar la accion sobre el control.
        /// </summary>
        WidgetActionDelegate FireDelegate { get; set; }

        #endregion

        #region IWidget Methods
        /// <summary>
        /// Codigo que se ejecutara al recivir el foco de la aplicacion.
        /// </summary>
        void OnActivate();
        /// <summary>
        /// Codigo que se ejecutara al perder el foco de la aplicacion.
        /// </summary>
        void OnDeactive();
        /// <summary>
        /// Codigo que se ejecutara al seleccionara el componente.
        /// </summary>
        void FireAction();
        #endregion
    }
}
