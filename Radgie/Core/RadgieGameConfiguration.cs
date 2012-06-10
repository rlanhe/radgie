using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.File;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;
using System.Globalization;
using System.Threading;

namespace Radgie.Core
{
    /// <summary>
    /// Clase que almacena la configuracion de arranque del motor.
    /// </summary>
    public class RadgieGameConfiguration
    {
        #region Properties

        /// <summary>
        /// Contenido del fichero de configuracion.
        /// </summary>
        private XDocument mConfiguration = null;

        #endregion

        #region Constants

        /// <summary>
        /// Etiqueta que indica el comienzo del fichero de configuracion.
        /// </summary>
        public const string KEY_RADGIE = "radgie";
        /// <summary>
        /// Etiqueta que indica el comienzo de una seccion de parametros.
        /// </summary>
        public const string KEY_PARAMETERS = "parameters";
        /// <summary>
        /// Etiqueta que indica el comienzo de un parametro.
        /// </summary>
        public const string KEY_PARAMETER = "parameter";
        /// <summary>
        /// Etiqueta que indica el comienzo de la seccion de sistemas.
        /// </summary>
        public const string KEY_SYSTEMS = "systems";
        /// <summary>
        /// Etiqueta que indica el comienzo de un sistema.
        /// </summary>
        public const string KEY_SYSTEM = "system";
        /// <summary>
        /// Id. de un parámetro.
        /// </summary>
        public const string KEY_NAME = "name";
        /// <summary>
        /// Valor de un parámetro.
        /// </summary>
        public const string KEY_VALUE = "value";
        /// <summary>
        /// Propiedad para configurar el local de la aplicación.
        /// </summary>
        public const string KEY_LOCALE = "locale";

        #endregion

        #region Constructors

        /// <summary>
        /// Carga la configuracion a partir de un fichero xml.
        /// </summary>
        /// <param name="config">Fichero de configuracion</param>
        public RadgieGameConfiguration(XmlFile config)
        {
            mConfiguration = config.GetXmlElement();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Procesa la seccion de parametros.
        /// </summary>
        /// <param name="parameters">Seccion de parametros en el fichero de configuracion.</param>
        private void ProcessParameters(IEnumerable<XElement> parameters)
        {
            foreach (var parameter in parameters)
            {
                string name = parameter.Attribute(KEY_NAME).Value;
                string value = parameter.Attribute(KEY_VALUE).Value;

                if (KEY_LOCALE.Equals(name))
                {
                    CultureInfo culture = CultureInfo.GetCultureInfoByIetfLanguageTag(value);
                    if(culture != null)
                    {
                        Thread.CurrentThread.CurrentCulture = culture;
                    }
                }
            }
        }

        /// <summary>
        /// Procesa la seccion de sistemas.
        /// </summary>
        /// <param name="systems">Seccion de sistemas en el fichero de configuracion.</param>
        private void ProcessSystems(IEnumerable<XElement> systems)
        {
            foreach (var system in systems)
            {
                ASystem.CreateSystem(system);
            }
        }

        /// <summary>
        /// Configura el motor a partir de la configuracion almacenada en esta clase.
        /// </summary>
        public void Configure()
        {
            var radgieParameters = from parameters in (from parameters in mConfiguration.Root.Elements(KEY_PARAMETERS)
                                                       select parameters).Elements(KEY_PARAMETER)
                                   select parameters;
            ProcessParameters(radgieParameters);

            var radgieSystems = from parameters in (from parameters in mConfiguration.Root.Elements(KEY_SYSTEMS)
                                                    select parameters).Elements(KEY_SYSTEM)
                                select parameters;
            ProcessSystems(radgieSystems);
        }

        #endregion
    }
}