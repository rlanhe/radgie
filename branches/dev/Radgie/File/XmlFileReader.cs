using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Reflection;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using System.IO;
using System.Globalization;
using System.Xml.Linq;

namespace Radgie.File
{
    /// <summary>
    /// Extrae de un fichero binario el contenido del fichero xml.
    /// </summary>
    public class XmlFileReader: ContentTypeReader<XmlFile>
    {
        #region Properties
        /// <summary>
        /// Color serializer para el parseo de ficheros xml.
        /// </summary>
        private static XmlSerializer mColorSerializer = new XmlSerializer(typeof(Color));
        /// <summary>
        /// Configuracion del local para identificar '.' como separador de decimales.
        /// </summary>
        public static CultureInfo mCI = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        /// <summary>
        /// Vector3 serializer para el parseo de ficheros xml.
        /// </summary>
        private static XmlSerializer mVector3Serializer = new XmlSerializer(typeof(Vector3));
        /// <summary>
        /// Quaternion serializer para el parseo de ficheros xml.
        /// </summary>
        private static XmlSerializer mQuaternionSerializer = new XmlSerializer(typeof(Quaternion));
        #endregion

        #region Constructors
        /// <summary>
        /// Inicializa la clase.
        /// </summary>
        static XmlFileReader()
        {
            // Configura el punto como separador de decimales independientemente del idioma del usuario que lo ejecute.
            mCI.NumberFormat.NumberDecimalSeparator = ".";
        }
        #endregion

        #region Methods
        #region ContentTypeReader members

        /// <summary>
        /// ContentTypeReader para objetos XmlFile.
        /// </summary>
        /// <param name="input">Reader del fichero xnb.</param>
        /// <param name="existingInstance"></param>
        /// <returns>Contenido del fichero xml.</returns>
        protected override XmlFile Read(ContentReader input, XmlFile existingInstance)
        {
            string type = input.ReadString();
            
            Type iType = Type.GetType(type);
            ConstructorInfo ci = iType.GetConstructor(new Type[] { typeof(string), typeof(string) });
            XmlFile xmlFile = (XmlFile)ci.Invoke(new Object[] { input.AssetName, input.ReadString() });

            return xmlFile;
        }

        #endregion

        #region Metodos de utilidad para trabajar con ficheros xml.
        
        /// <summary>
        /// Obtiene un objeto Color a partir de un nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>Objeto Color esperado.</returns>
        public static Color GetColor(XElement value)
        {
            string xml = "Color".Equals(value.Name.LocalName) ? value.ToString() : value.Element("Color").ToString();
            return (Color)mColorSerializer.Deserialize(new StringReader(xml));
        }

        /// <summary>
        /// Obtiene un entero a partir de un nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>Obtiene un valor entero.</returns>
        public static int GetInt(XNode value)
        {
            if (value is XElement)
            {
                XElement element = (XElement)value;
                return int.Parse(element.Value);
            }
            else
            {
                return int.Parse(value.ToString());
            }
        }

        /// <summary>
        /// Obtiene un float a partir de un nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>Valor float.</returns>
        public static float GetFloat(XNode value)
        {
            if (value is XElement)
            {
                XElement element = (XElement)value;
                return float.Parse(element.Value, mCI);
            }
            else
            {
                return float.Parse(value.ToString(), mCI);
            }
        }

        /// <summary>
        /// Obtiene un vector3 a partir de un nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>Vector 3.</returns>
        public static Vector3 GetVector3(XElement value)
        {
            XElement node = null;
            if ("Vector3".Equals(value.Name))
            {
                node = value;
            }
            else
            {
                node = value.Element("Vector3");
            }
            return (Vector3)mVector3Serializer.Deserialize(new StringReader(node.ToString()));
        }

        /// <summary>
        /// Obtiene un TimeSpan a partir de un nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan GetTimeSpan(XElement value)
        {
            return TimeSpan.Parse(value.Value);
        }

        /// <summary>
        /// Obtiene un Quaternion a partir del nodo xml.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <returns>Quaternion.</returns>
        public static Quaternion GetQuaternion(XElement value)
        {
            XElement node = null;
            if ("Quaternion".Equals(value.Name))
            {
                node = value;
            }
            else
            {
                node = value.Element("Quaternion");
            }
            return (Quaternion)mQuaternionSerializer.Deserialize(new StringReader(node.ToString()));
        }
        #endregion
        #endregion
    }
}
