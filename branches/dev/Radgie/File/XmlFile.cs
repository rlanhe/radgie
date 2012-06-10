using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Radgie.Util;
using System.Xml.Linq;

namespace Radgie.File
{
    /// <summary>
    /// Contenido de un fichero Xml.
    /// </summary>
    public class XmlFile
    {
        #region Constants
        // Constantes del fichero xml.
        private const string RELATIVE = "Relative";

        #endregion

        #region Properties

        /// <summary>
        /// Contenido del fichero xml.
        /// </summary>
        public string Content
        {
            get
            {
                return mContent;
            }
        }
        private string mContent = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Fichero xml extraido de un fichero xnb.
        /// </summary>
        /// <param name="content">Contenido del fichero xml.</param>
        public XmlFile(string content)
        {
            mContent = content;
        }

        /// <summary>
        /// Fichero xml extraido de un fichero xnb.
        /// </summary>
        /// <param name="asset">Asset que identifica al fichero xml.</param>
        /// <param name="content">Contenido del fichero xml.</param>
        public XmlFile(string asset, string content)
        {
            mContent = ExpandXmlContent(asset, content);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Procesa las etiquetas especiales del fichero, sustituyendolas por el valor esperado.
        /// </summary>
        /// <param name="asset">Id del fichero.</param>
        /// <param name="content">Contenido del fichero.</param>
        /// <returns></returns>
        public static string ExpandXmlContent(string asset, string content)
        {
            //string contentPath = PathUtil.GetContentPath(filename, context);

            XDocument doc = XDocument.Load(new StringReader(content));
            var relatives = (from xmlNode in doc.Descendants(RELATIVE)
                            select xmlNode).ToArray();

            foreach (var relative in relatives)
            {
                string value = relative.Value.Trim();
                value = PathUtil.CombinePaths(asset, value);
                relative.ReplaceWith(new XText(value));
            }

            return doc.ToString();
        }

        /// <summary>
        /// Obtiene el nodo raiz del fichero xml.
        /// </summary>
        /// <returns>Nodo raiz.</returns>
        public XDocument GetXmlElement()
        {
            return mContent == null ? null : XDocument.Load(new StringReader(mContent));
        }

        #endregion
    }
}
