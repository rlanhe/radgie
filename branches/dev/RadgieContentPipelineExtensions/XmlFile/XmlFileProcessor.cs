using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using System.Xml;

namespace RadgieContentPipelineExtensions.XmlFile
{
    /// <summary>
    /// Transforma el objeto en memoria en un fichero binario.
    /// </summary>
    [ContentProcessor(DisplayName = "Xml File Processor")]
    public class XmlFileProcessor : ContentProcessor<XmlFileContent, XmlFileContent>
	{
		#region Methods

		#region ContentProcessor members

		/// <summary>
		/// Procesa el contenido leido del fichero y lo transforma en un XmlDocument.
		/// </summary>
		/// <param name="input">Contenido del fichero de entrada</param>
		/// <param name="context">Contexto del procesador</param>
		/// <returns></returns>
        public override XmlFileContent Process(XmlFileContent input, ContentProcessorContext context)
        {
            return input;
		}

		#endregion

		#endregion
	}
}
