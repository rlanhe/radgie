using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using tInput = RadgieContentPipelineExtensions.XmlFile.XmlFileContent;

namespace RadgieContentPipelineExtensions.XmlFile
{
    /// <summary>
    /// Escribe el objeto en memoria en un fichero binario.
    /// </summary>
    abstract class AXmlFileWriter : ContentTypeWriter<tInput>
    {
        #region Methods

        #region ContentTypeWriter members

        /// <summary>
        /// Escribe el contenido del fichero para pasarlo a un fichero binario.
        /// </summary>
        /// <param name="output">Flujo de salida.</param>
        /// <param name="input">Contenido del fichero en memoria.</param>
        protected override void Write(ContentWriter output, tInput input)
        {
            input.Write(output);
        }

        #endregion

        #endregion
    }
}
