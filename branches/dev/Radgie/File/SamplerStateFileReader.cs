using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using tOutput = Microsoft.Xna.Framework.Graphics.SamplerState;

namespace Radgie.File
{
    /// <summary>
    /// Objeto para la lectura de ficheros .ss.
    /// </summary>
    public class SamplerStateFileReader : ContentTypeReader<tOutput>
    {
        #region Methods
        #region ContentTypeReader members

        /// <summary>
        /// ContentTypeReader para objetos RenderState.
        /// </summary>
        /// <param name="input">Reader del fichero xnb.</param>
        /// <param name="existingInstance"></param>
        /// <returns>RenderState</returns>
        protected override tOutput Read(ContentReader input, tOutput existingInstance)
        {
            input.ReadString();
            XmlSerializer serializer = new XmlSerializer(typeof(tOutput));
            return (tOutput)serializer.Deserialize(new StringReader(XmlFile.ExpandXmlContent(input.AssetName, input.ReadString())));
        }

        #endregion
        #endregion
    }
}
