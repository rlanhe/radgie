using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using tInput = RadgieContentPipelineExtensions.XmlFile.XmlFileContent;

namespace RadgieContentPipelineExtensions.XmlFile.Strings
{
    /// <summary>
    /// Lee el fichero del disco y lo deja en memoria.
    /// </summary>
    [ContentImporter(".strings", DisplayName = "StringsFileImporter")]
    public class StringsFileImporter : ContentImporter<tInput>
    {
        #region Methods

        #region ContentImporter members
        /// <summary>
        /// Este método es llamado durante la generación de los ficheros xnb.
        /// Carga el contenido del fichero y lo deja en memoria.
        /// </summary>
        /// <param name="filename">Path hasta el fichero.</param>
        /// <param name="context">Contexto del importer.</param>
        /// <returns></returns>
        public override tInput Import(string filename, ContentImporterContext context)
        {
            return new tInput(System.IO.File.ReadAllText(filename));
        }
        #endregion

        #endregion
    }
}
