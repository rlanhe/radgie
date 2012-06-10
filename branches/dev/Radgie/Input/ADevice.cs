using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml;
using Radgie.Core;

namespace Radgie.Input
{
    /// <summary>
    /// Abstraccion de un dispositivo de entrada conectable con Radgie.
    /// Cualquier dispositivo nuevo debe derivar de esta clase.
    /// </summary>
    /// <typeparam name="T">Interfaz que implementa el dispositivo</typeparam>
    public abstract class ADevice<T>: IDevice where T:IDevice
    {
        #region Properties

        /// <summary>
        /// Array de todos los dispositivos del mismo tipo.
        /// Se crea un dispositivo por cada jugador.
        /// </summary>
        private static T[] mDevices = new T[4];

        /// <summary>
        /// Lista de controles que agrupa el dispositivo.
        /// Uso interno.
        /// </summary>
        protected List<ADeviceControl<T>> mControls = new List<ADeviceControl<T>>();

        #region IDevice members
        
        /// <summary>
        /// Ver <see cref="Radgie.Input.IDevice.Index"/>
        /// </summary>
        public PlayerIndex Index
        {
            get
            {
                return mIndex;
            }
        }
        protected PlayerIndex mIndex;

        /// <summary>
        /// Ver <see cref="Radgie.Input.IDevice.TimeElapsed"/>
        /// </summary>
        public TimeSpan TimeElapsed
        {
            get
            {
                return mTimeElapsed;
            }
        }
        protected TimeSpan mTimeElapsed;
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Crea un nuevo dispositivo relacionado con el jugador pIndex.
        /// </summary>
        /// <param name="pIndex">Id del jugador</param>
        public ADevice(PlayerIndex pIndex)
        {
            mIndex = pIndex;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Obtiene el dispositivo relacionado con el jugador.
        /// </summary>
        /// <param name="index">Id del jugador.</param>
        /// <returns>Dispositivo</returns>
        public static T Get(PlayerIndex index)
        {
            return mDevices[(int)index];
        }

        /// <summary>
        /// Annade el dispositivo a la lista de dispositivos de la clase.
        /// </summary>
        /// <param name="index">Jugador relacionado con el dispositivo.</param>
        /// <param name="device">Id del jugador.</param>
        protected static void AddDevice(PlayerIndex index, T device)
        {
            mDevices[(int)index] = device;
        }

        #region IDevice members

        /// <summary>
        /// Ver <see cref="Radgie.Input.IDevice.Update"/>
        /// </summary>
        public virtual void Update(GameTime time)
        {
            mTimeElapsed = time.ElapsedGameTime;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Input.IDevice.HasChanged"/>
        /// </summary>
        public abstract bool HasChanged();
        
        #endregion

        #endregion
    }
}
