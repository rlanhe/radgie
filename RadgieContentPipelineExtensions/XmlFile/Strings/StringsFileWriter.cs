using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;
using tInput = RadgieContentPipelineExtensions.XmlFile.XmlFileContent;

namespace RadgieContentPipelineExtensions.XmlFile.Strings
{
    /// <summary>
    /// Escribe el objeto en memoria en un fichero binario.
    /// </summary>
    [ContentTypeWriter]
    class StringsFileWriter : AXmlFileWriter
    {
        #region Methods

        #region ContentTypeWriter members

        /// <summary>
        /// Devuelve el tipo del objeto que devolverá al cargar el contenido del fichero binario.
        /// </summary>
        /// <param name="targetPlatform">Plataforma de destino.</param>
        /// <returns>Nombre del tipo.</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Radgie.File.XmlFile, Radgie";
        }

        /// <summary>
        /// Devuelve el nombre del tipo que se usará al leer el fichero binario.
        /// </summary>
        /// <param name="targetPlatform">Plataforma de destino.</param>
        /// <returns>Nombre del tipo.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Radgie.File.XmlFileReader, Radgie";
        }

        #endregion

        #endregion
    }
}
