using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Radgie.Input.Device.Mouse
{
	/// <summary>
	/// Control para determinar la posicion del cursor del Mouse.
    /// La posicion 0,0 corresponde con la esquina superior izquierda de la pantalla.
    /// Las coordenadas pueden ser negativas si el raton se encuentra mas a la izquierda o 
    /// por encima de la pantalla.
	/// </summary>
	public class MousePosition : ADeviceControl<IMouse>, IPositionControl
	{
		#region Delegates and events
        /// <summary>
        /// Definicion del delegado para la actualizacion del estado del control.
        /// </summary>
        /// <param name="state">Estado del dispositivo.</param>
        /// <returns>Coordenadas 2D con la posicion del raton.</returns>
		public delegate Vector2 MousePositionDelegate(MouseState state);
		#endregion

		#region Properties
		#region IPositionControl members
        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.X"/>
        /// </summary>
		public int X
		{
			get
			{
				return (int)mGetter(mParent.State).X;
			}
		}

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.Y"/>
        /// </summary>
		public int Y
		{
			get
			{
				return (int)mGetter(mParent.State).Y;
			}
		}

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.PreviousX"/>
        /// </summary>
		public int PreviousX
		{
			get
			{
				return (int)mGetter(mParent.PreviousState).X;
			}
		}

        /// <summary>
        /// Ver <see cref="Radgie.Input.Control.IPositionControl.PreviousY"/>
        /// </summary>
		public int PreviousY
		{
			get
			{
				return (int)mGetter(mParent.PreviousState).Y;
			}
		}
		#endregion

        /// <summary>
        /// Delegado usado para actualizar el estado del control.
        /// </summary>
		protected MousePositionDelegate mGetter = null;
		#endregion

		#region Constructors
		/// <summary>
		/// Crea e inicializa un control.
		/// </summary>
		/// <param name="getter">Delegado que obtiene el estado del control a partir del estado del dispositivo.</param>
		public MousePosition(IMouse parent, MousePositionDelegate getter)
			: base(parent)
		{
			mGetter = getter;
		}
		#endregion
	}
}
