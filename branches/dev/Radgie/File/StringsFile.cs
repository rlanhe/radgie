using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace Radgie.File
{
    /// <summary>
    /// Contenido de un fichero de Strings.
    /// </summary>
    public class StringsFile: XmlFile
    {
        #region Properties

        /// <summary>
        /// Obtiene el valor asociado a una clave.
        /// </summary>
        /// <param name="key">Clave en el fichero.</param>
        /// <returns>Valor asociado.</returns>
        public string this[string key]
        {
            get
            {
                return mDictionary[key];
            }
        }
        private IDictionary<string, string> mDictionary;

        #endregion

        #region Constructors

        /// <summary>
        /// Fichero xml extraido de un fichero xnb.
        /// </summary>
        /// <param name="content">Contenido del fichero xml.</param>
        public StringsFile(string content): base(content)
        {
            if (content != null)
            {
                XDocument doc = XDocument.Load(new StringReader(content));
                var stringElements = from strings in doc.Root.Elements("String")
                                     select strings;
                mDictionary = new Dictionary<string, string>();

                foreach(var stringElement in stringElements)
                {
                    string key = stringElement.Attribute("key").Value;
                    string value = stringElement.Attribute("value").Value;
                    mDictionary[key] = value;
                }
            }
        }

        #endregion
    }
}
