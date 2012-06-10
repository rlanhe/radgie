using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Microsoft.Xna.Framework;
using Radgie.Input.Device;
using Radgie.Input.Control;
using Radgie.Input.Device.Mouse;
using Radgie.Input.Action;
using System.Xml;
using System.Reflection;
using System.Xml.Linq;

namespace Radgie.Input
{
    /// <summary>
    /// Sistema para la gestion de dispositivos de entrada.
    /// </summary>
    public class InputSystem: ASystem, IInputSystem
    {
        #region Properties
        
        /// <summary>
        /// Lista de dispositivos conectados que maneja el subsistema.
        /// </summary>
		private List<IDevice> mDevices = new List<IDevice>();

        #region IInputSystem Properties
        /// <summary>
        /// Ver <see cref="Ragie.Input.IInputSystem.Statistics"/>
        /// </summary>
        public InputSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private InputSystemStatistics mStatistics;
        #endregion

        #endregion

        #region Constants
        // Constantes del fichero de configuracion del sistema de input.
        public const string KEY_DEVICES = "devices";
        public const string KEY_DEVICE = "device";

        #endregion

        #region Constructors

        /// <summary>
        /// Crea e inicializa el sistema de entrada.
        /// </summary>
        /// <param name="sc">Seccion del fichero xml de configuracion relacionada con el sistema de entrada.</param>
        public InputSystem(XElement sc)
            : base(sc)
        {
            ConfigInputSystem(sc);
            mStatistics = new InputSystemStatistics();
        }

        #endregion

        #region Methods

        #region AUpdateableAtRate members

        /// <summary>
        /// Ver <see cref="Radgie.Core.AUpdateableAtRate.UpdateAction"/>
        /// </summary>
        protected override void UpdateAction(GameTime time)
        {
            mStatistics.Reset();
            mStatistics.StartUpdateTimer();
            foreach (IDevice device in mDevices)
            {
                mStatistics.NumberOfDevices++;
                device.Update(time);
            }

            mStatistics.StopUpdateTimer();
        }

        #endregion

        /// <summary>
        /// Configura el subsistema de acuerdo con la configuracion definida en el fichero de configuracion.
        /// </summary>
        /// <param name="sc">Seccion del fichero de configuracion</param>
        private void ConfigInputSystem(XElement sc)
        {
            var devicesList = from devices in sc.Descendants(KEY_DEVICES).Elements(KEY_DEVICE)
                              select devices;

            foreach (var device in devicesList)
            {
                LoadDevice(device.Attribute(KEY_NAME).Value, device.Attribute(KEY_IMPLEMENTATION).Value);
            }
        }

        /// <summary>
        /// Inicializa un nuevo dispositivo.
        /// </summary>
        /// <param name="name">Nombre del dispositivo.</param>
        /// <param name="implementation">Clase que implementa el dispositivo.</param>
        private void LoadDevice(string name, string implementation)
        {
            Type iType = Type.GetType(implementation);
            ConstructorInfo ci = iType.GetConstructor(new Type[]{typeof(PlayerIndex)});
            IDevice[] device = new IDevice[4];
            
            // Crea una instancia del dispositivo por cada jugador.
            for (int i = 0; i < (int)PlayerIndex.Four; i++)
            {
                mDevices.Add((IDevice)ci.Invoke(new object[]{(PlayerIndex)i}));
            }
        }

        #endregion
    }
}
