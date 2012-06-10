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
    [ContentTypeWriter]
    class XmlFileWriter : ContentTypeWriter<tInput>
    {
        #region Properties

        private string mType;

        #endregion

        #region Methods

        #region ContentTypeWriter members

        /// <summary>
		/// Escribe el contenido del fichero para pasarlo a un fichero binario.
		/// </summary>
		/// <param name="output">Flujo de salida.</param>
		/// <param name="input">Contenido del fichero en memoria.</param>
        protected override void Write(ContentWriter output, tInput input)
        {
            mType = input.Type;
            input.Write(output);
        }

		/// <summary>
		/// Devuelve el tipo del objeto que devolvera al cargar el contenido del fichero binario.
		/// </summary>
		/// <param name="targetPlatform">Plataforma de destino.</param>
		/// <returns>Nombre del tipo.</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return XmlFileConfigurator.Get(mType).RuntimeType;
        }

		/// <summary>
		/// Devuelve el nombre del tipo que se usara al leer el fichero binario.
		/// </summary>
		/// <param name="targetPlatform">Plataforma de destino.</param>
		/// <returns>Nombre del tipo.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            string type = XmlFileConfigurator.Get(mType).RuntimeReader;
            return type;
		}

		#endregion

		#endregion
	}
}
