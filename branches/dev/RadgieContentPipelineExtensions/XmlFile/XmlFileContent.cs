using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using System.Xml;
using System.IO;

namespace RadgieContentPipelineExtensions.XmlFile
{
    public class XmlFileContent
    {
        public string Type { get; set; }
        public string Text { get; set; }

        public XmlFileContent(string textData, string type)
        {
            Text = textData;
            Type = type;
        }

        public void Write(ContentWriter writer)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            // Elimina los nodos de los comentarios
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(new XmlTextReader(new StringReader(Text)), settings);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            writer.Write(XmlFileConfigurator.Get(Type).RuntimeType);
            writer.Write(xmlDoc.InnerXml);
        }
    }
}
