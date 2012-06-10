using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsStorm.GameComponents.GUI
{
    /// <summary>
    /// Clase para gestionar la disposicion de los elementos del gui en pantalla.
    /// </summary>
    class LayaoutUtil
    {
        /// <summary>
        /// Situa el widget en funcion de su ancho y del ancho del frame al que pertenece.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="percent">% de la anchura del widget padre.</param>
        public static void SetPositionPercentInX(IWidget widget, float percent)
        {
            IWidget parent = widget.Parent;
            if (parent != null)
            {
                widget.X = (int)(parent.Width * percent / 100.0f + widget.Width/2.0f);
                widget.GameComponent.Scene.Update(null);
            }
        }

        public static float GetPositionPercentInX(IWidget widget)
        {
            float res = 0.0f;
            IWidget parent = widget.Parent;
            if(parent != null)
            {
                res = (0.5f*widget.X - 0.25f*widget.Width)/(parent.Width);
            }
            return res;
        }

        /// <summary>
        /// Situal el widget en funcion de su altura y de la altura del frame al que pertenece.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="percent">% de la altura del widget padre.</param>
        public static void SetPositionPercentInY(IWidget widget, float percent)
        {
            IWidget parent = widget.Parent;
            if (parent != null)
            {
                widget.Y = (int)(parent.Height * percent / 100.0f + widget.Height / 2.0f);
                widget.GameComponent.Scene.Update(null);
            }
        }

        public static float GetPositionPercentInY(IWidget widget)
        {
            float res = 0.0f;
            IWidget parent = widget.Parent;
            if (parent != null)
            {
                res = (0.5f * widget.Y - 0.25f * widget.Height) / (parent.Height);
            }
            return res;
        }

        /// <summary>
        /// Obtiene la posición del widet en el eje X relativa al widget padre.
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <returns>Posicion en el eje X relativa a la esquina superior izquierda del widget padre.</returns>
        public static int GetPositionInX(IWidget widget)
        {
            return widget.X - widget.Width / 2;
        }

        /// <summary>
        /// Situa el widget.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="value">Posicion en el eje X.</param>
        public static void SetPositionInX(IWidget widget, int value)
        {
            widget.X = widget.Width / 2 + value;
            widget.GameComponent.Scene.Update(null);
        }

        /// <summary>
        /// Obtiene la posición del widet en el eje Y relativa al widget padre.
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <returns>Posicion en el eje Y relativa a la esquina superior izquierda del widget padre.</returns>
        public static int GetPositionInY(IWidget widget)
        {
            return widget.Y - widget.Height / 2;
        }

        /// <summary>
        /// Situal el widget.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="value">Posicion en el eje Y.</param>
        public static void SetPositionInY(IWidget widget, int value)
        {
            widget.Y = widget.Height / 2 + value;
            widget.GameComponent.Scene.Update(null);
        }

        /// <summary>
        /// Centra el widget en funcion de su tamanno y el del widget que lo contiene.
        /// </summary>
        /// <param name="widget">Widget.</param>
        public static void Center(IWidget widget)
        {
            CenterInX(widget);
            CenterInY(widget);
        }

        /// <summary>
        /// Centra el widget en el eje horizontal en funcion de su tamanno y el del frame padre.
        /// </summary>
        /// <param name="widget">Widget.</param>
        public static void CenterInX(IWidget widget)
        {
            PlaceInX(widget, 50.0f);
        }

        /// <summary>
        /// Centra el widget en el eje vertical en funcion de su tamanno y el del frame padre.
        /// </summary>
        /// <param name="widget">Widget.</param>
        public static void CenterInY(IWidget widget)
        {
            PlaceInY(widget, 50.0f);
        }

        /// <summary>
        /// Posiciona el widget en el eje horizontal un % de la anchura del frame que lo contiene.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="percent">% de la anchura del padre donde se quiere situar el widget.</param>
        public static void PlaceInX(IWidget widget, float percent)
        {
            IWidget parent = widget.Parent;
            if (parent != null)
            {
                int diff = parent.Width - widget.Width;
                widget.X = (int)(diff * percent / 100.0f) + (int)(widget.Width*0.5f);
                widget.GameComponent.Scene.Update(null);
            }
        }

        /// <summary>
        /// Posiciona el widget en el eje vertical en funcion de su tamanno y el del frame padre.
        /// </summary>
        /// <param name="widget">Widget.</param>
        /// <param name="percent">% de la altura del padre donde se quiere situar el widget.</param>
        public static void PlaceInY(IWidget widget, float percent)
        {
            IWidget parent = widget.Parent;
            if (parent != null)
            {
                int diff = parent.Height - widget.Height;
                widget.Y = (int)(diff * percent / 100.0f) + (int)(widget.Height * 0.5f);
                widget.GameComponent.Scene.Update(null);
            }
        }

        /// <summary>
        /// Posiciona los widget hijos de un frame a lo ancho del widget manteniendo equidistancias entre si.
        /// </summary>
        /// <param name="container">Frame cuyo widget se van a posicionar.</param>
        public static void SpreadWidgetsInX(IContainer container)
        {
            IEnumerator<IWidget> childs = container.Childs;
            int totalWidth = 0;
            int count = 0;
            while (childs.MoveNext())
            {
                IWidget current = childs.Current;
                totalWidth += current.Width;
                count++;
            }

            int diff = container.Width - totalWidth;
            int distance = diff / (count+1);

            int incrementalPos = 0;
            childs.Reset();
            while (childs.MoveNext())
            {
                IWidget current = childs.Current;
                incrementalPos += distance;
                current.X = incrementalPos;
                incrementalPos += current.Width;
            }

            container.GameComponent.Scene.Update(null);
        }

        /// <summary>
        /// Posiciona los widgets del container a lo largo del eje vertical manteniendo espacios equidistantes entre ellos.
        /// </summary>
        /// <param name="container">Container de los widgets que se van a posicionar.</param>
        public static void SpreadWidgetsInY(IContainer container)
        {
            IEnumerator<IWidget> childs = container.Childs;
            int totalHeight = 0;
            int count = 0;

            while (childs.MoveNext())
            {
                IWidget current = childs.Current;
                totalHeight += current.Height;
                count++;
            }

            int diff = container.Height - totalHeight;
            int distance = diff / (count + 1);

            int incrementalPos = 0;
            childs.Reset();
            while (childs.MoveNext())
            {
                IWidget current = childs.Current;
                incrementalPos += distance;
                current.Y = incrementalPos;
                incrementalPos += current.Height;
            }

            container.GameComponent.Scene.Update(null);
        }
    }
}
