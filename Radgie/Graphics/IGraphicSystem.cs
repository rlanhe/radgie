using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Util.Collection.ReferencePool;
using Microsoft.Xna.Framework;

namespace Radgie.Graphics
{
    /// <summary>
    /// Interfaz del sistema de graficos.
    /// </summary>
    public interface IGraphicSystem: ISystem
    {
        #region Properties
        /// <summary>
        /// Objeto encargado controlar que y como se dibujan los objetos por pantalla.
        /// </summary>
        IRenderable RenderProcess { get; set; }
        /// <summary>
        /// Interfaz del dispositivo grafico.
        /// </summary>
        GraphicsDevice Device { get; }
        /// <summary>
        /// Graphics Device Manager.
        /// </summary>
        GraphicsDeviceManager DeviceManager { get; }
        /// <summary>
        /// Estadisticas del sistema.
        /// </summary>
        GraphicsSystemStatistics Statistics { get; }
        /// <summary>
        /// Objetos graficos que actualiza el sistema.
        /// </summary>
        IReferencePool<IGraphicEntity> GraphicEntityReferences { get; }
        /// <summary>
        /// Renderer encargado de dibujar los objetos.
        /// </summary>
        IRenderer Renderer { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Dibuja un frame.
        /// </summary>
        void Draw();
        #endregion
    }
}
