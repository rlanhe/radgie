using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;

namespace Radgie.Graphics
{
    /// <summary>
    /// Delegado que se invocara por el renderer a la hora de dibujar el objeto.
    /// Cada entidad proporcionara el delegado que sabe como dibujarla.
    /// </summary>
    /// <param name="renderer">Renderer</param>
    public delegate void DrawDelegate(IRenderer renderer);

    /// <summary>
    /// Determina el modo en que se dibuja.
    /// </summary>
    public enum RenderMode
    {
        Instancing,
        NoInstancing
    }

    /// <summary>
    /// Interfaz del renderer.
    /// </summary>
    public interface IRenderer
    {
        #region Properties
        /// <summary>
        /// Matriz de mundo usada para dibujar objetos.
        /// </summary>
        Matrix World { get; set;  }
        /// <summary>
        /// Lista de matrices de mundo para dibujar varias instancias de un mismo objeto de golpe.
        /// </summary>
        DynamicVertexBuffer InstancesData { get; set; }
        /// <summary>
        /// Matriz de vista usada para dibujar objetos.
        /// </summary>
        Matrix View { get; }
        /// <summary>
        /// Matriz de proyecion usada para dibujar objetos.
        /// </summary>
        Matrix Projection { get; }
        /// <summary>
        /// Matriz combinada de mundo-vista-proyecion.
        /// </summary>
        Matrix WorldViewProjection { get; }

        /// <summary>
        /// Dispositivo grafico.
        /// </summary>
        GraphicsDevice Device { get; }
        /// <summary>
        /// Objeto usado para dibujar sprites.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Material usado para dibujar objetos.
        /// </summary>
        Material Material { get; set; }

        /// <summary>
        /// Camara usada para dibujar objetos.
        /// </summary>
        ICamera CurrentCamera { get; set; }
        /// <summary>
        /// Viewport de la camara usada para dibujar objetos.
        /// </summary>
        Viewport Viewport { get; }

        /// <summary>
        /// Color de la luz de ambiente.
        /// </summary>
        Color AmbientLightColor { get; set; }
        /// <summary>
        /// Numero maximo de lueces a utilizar.
        /// </summary>
        int MaxLightsNumber { get; set; }
        /// <summary>
        /// Lista de luces que hay en la escena.
        /// </summary>
        List<ILight> Lights { get; set; }
        /// <summary>
        /// Estadisticas del sistema grafico.
        /// </summary>
        GraphicsSystemStatistics Statistics { get; }
        /// <summary>
        /// Referencia al sistema grafico.
        /// </summary>
        IGraphicSystem GraphicSystem { get; }

        /// <summary>
        /// Indica el modo de dibujado empleado.
        /// </summary>
        RenderMode SelectedRenderMode { get; }

        #endregion

        #region Methods
        /// <summary>
        /// Metodo empleado por las entidades graficas para solicitar que las dibujen.
        /// </summary>
        /// <param name="drawDelegate">Delegado que sabe como dibujar la entidad.</param>
        void Render(DrawDelegate drawDelegate);
        /// <summary>
        /// Limpia el target.
        /// </summary>
        void Clean();
        #endregion
    }
}
