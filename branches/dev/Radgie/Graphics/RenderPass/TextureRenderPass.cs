using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Camera;

namespace Radgie.Graphics.RenderPass
{
    /// <summary>
    /// Pasada para dibujar texturas sobre el render target.
    /// </summary>
    public class TextureRenderPass: MultipleTexturesRenderPass
    {
        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="source">Textura de entrada.</param>
        /// <param name="camera">Camara a usar durante el dibujado.</param>
        /// <param name="cleanRT">Indica si debe limpiar el target donde dibujar.</param>
        public TextureRenderPass(Texture2D source, ICamera camera, bool cleanRT)
            :this(source, camera, null, cleanRT)
        {
        }

        /// <summary>
        /// Construye una pasada de render.
        /// </summary>
        /// <param name="source">Textura de entrada.</param>
        /// <param name="camera">Camara a usar durante el dibujado.</param>
        /// <param name="rt">Target en el que dibujar el resultado.</param>
        /// <param name="cleanRT">Indica si debe limpiar el target donde dibujar.</param>
        public TextureRenderPass(Texture2D source, ICamera camera, RenderTarget2D rt, bool cleanRT)
            :base(new List<Texture2D>{source}, camera, rt, cleanRT)
        {
        }
    }
}
