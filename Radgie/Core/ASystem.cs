using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
#if WIN32
using log4net;
#endif
using System.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace Radgie.Core
{
    /// <summary>
    /// Definicion abstracta de un system.
	/// Un objeto system engloba toda la funcionalidad relativa a una tarea o contexto.
    /// RadgieGame actualiza el system segun el frame rate que se haya establecido para el. 
	/// Cada system debe derivar de esta clase, implementando el metodo UpdateAction con la logica del system.
    /// </summary>
    public abstract class ASystem: AUpdateableAtRate, ISystem
    {
        #region Properties

        /// <summary>
        /// Logger de la clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(ASystem));
        #endif

        #endregion

        #region Constants

        /// <summary>
        /// Propiedad del fichero de configuracion que indica el rate al que se va a actualizar el sistema (Veces por segundo).
        /// </summary>
        public const string KEY_RATE = "rate";
        /// <summary>
        /// Seccion del fichero de configuracion para los parametros especificos del sistema.
        /// </summary>
        public const string KEY_PARAMETERS = "parameters";
        /// <summary>
        /// Etiqueta del fichero de configuracion para indicar un parametro.
        /// </summary>
        public const string KEY_PARAMETER = "parameter";
        /// <summary>
        /// Etiqueta del fichero de configuracion para indicar el nombre de un parametro.
        /// </summary>
        public const string KEY_NAME = "name";
        /// <summary>
        /// Etiqueta del fichero de configuracion para indicar el valor de un parametro.
        /// </summary>
        public const string KEY_VALUE = "value";
        /// <summary>
        /// Etiqueta del fichero de configuracion para indicar la implementacion de una clase.
        /// </summary>
        public const string KEY_IMPLEMENTATION = "implementation";
        /// <summary>
        /// Etiqueta del fichero de configuracion para indicar un tipo de un objeto.
        /// </summary>
        public const string KEY_TYPE = "type";

        #endregion

        #region Constructors

        /// <summary>
        /// Abstraccion de un sistema de Radgie.
        /// </summary>
        /// <param name="sc">Configuracion del sistema</param>
        public ASystem(XElement sc)
        {
            LoadParameters(sc);
        }

        #endregion

		#region Methods

		/// <summary>
		/// Carga los parametros del sistema.
		/// Puede ser sobrecargado por sus clases hijas para cargar sus propios parametros.
		/// </summary>
		/// <param name="name">Nombre del parametro</param>
		/// <param name="value">Valor</param>
		/// <returns>True si cargo el valor del parametro, False en caso contrario.</returns>
		protected virtual bool LoadParameters(string name, string value)
        {
            bool result = false;
            switch (name)
            {
                case KEY_RATE:
                    UpdateRate = float.Parse(value);
                    result = true;
                    break;
            }
            return result;
        }

		/// <summary>
		/// Carga la configuracion del sistema.
		/// </summary>
		/// <param name="sc">Seccion de configuracion del sistema del fichero de configuracion de Radgie.</param>
		/// <exception cref="ArgumentException">Si la seccion de configuracion del sistema no mantiene la estructura:
		/// 
		///		<parameters>
		///			<parameter name="id" value="value"/>
		///		</parameters>
		/// 
		/// </exception>
        private void LoadParameters(XElement sc)
        {
            var parameters = from elements in sc.Descendants()
                             where elements.Name == KEY_PARAMETER
                             select elements;

            foreach(var param in parameters)
            {
                string name = param.Attribute(KEY_NAME).Value;
                string value = param.Attribute(KEY_VALUE).Value;
                LoadParameters(name, value);
            }
        }

		/// <summary>
		/// Crea un sistema a partir de la especificacion del fichero de configuracion.
		/// </summary>
		/// <param name="sc">Seccion del fichero de configuracion donde se especifica el sistema.</param>
		/// <exception cref="ArgumentNullException">Type e Implementation debe ser definidos.</exception>
        public static void CreateSystem(XElement sc)
        {
            string type = null;
            string implementation = null;

            type = sc.Attribute(KEY_TYPE).Value;
            implementation = sc.Attribute(KEY_IMPLEMENTATION).Value;

            if((type == null) || (implementation == null))
            {
                throw new ArgumentNullException("Type and Implementation of system must be defined");
            }

			// Crea el nuevo sistema segun la configuracion y lo annade a la lista de sistemas que maneja RadgieGame
            Type iType = Type.GetType(implementation);
            ConstructorInfo ci = iType.GetConstructor(new Type[] { typeof(XElement) });
            ASystem system = (ASystem)ci.Invoke(new Object[] { sc });
            RadgieGame.Instance.AddSystem(Type.GetType(type), system);
		}

		#endregion
	}
}
