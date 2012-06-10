using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util.Collection.Context;

namespace Radgie.State
{
	/// <summary>
	/// Maquina de estados abstracta.
	/// Controla las transiciones entre un grupo de estados. A su vez, una maquina de estados
    /// es un estado por lo que puede formar parte de otra maquina de estados.
	/// </summary>
	/// <typeparam name="T">Tipo comun de los estados que contiene.</typeparam>
    public abstract class AStateMachine<T>: AState, IStateMachine where T: IState
	{
		#region Properties
		/// <summary>
		/// Estado actual de la maquina de estados.
		/// </summary>
        protected T CurrentState
        {
            get
            {
                return mCurrentState;
            }
        }
		private T mCurrentState;

		/// <summary>
		/// Estado inicial de la maquina de estados.
		/// Comienza la ejecucion de la maquina de estados por este estado.
		/// </summary>
		protected T InitialState
		{
			get
			{
				return mInitialState;
			}
			set
			{
				if (mInitialState != null)
				{
					throw new Exception("Invalid operation exception");
				}
				mInitialState = value;
			}
		}
		private T mInitialState;

		/// <summary>
		/// Tabla de transiciones entre los estados de la maquina de estados.
		/// </summary>
		private Dictionary<T, Dictionary<Event, T>> mTransitions = new Dictionary<T, Dictionary<Event, T>>();

		/// <summary>
		/// Evento que debe evaluarse despues de actualizar el estado actual.
		/// </summary>
		private Event mNextEvent = null;

        /// <summary>
        /// Ver <see cref="Radgie.Core.HasContext.Context"/>
        /// </summary>
        public IContext Context
        {
            get
            {
                return mContext;
            }
        }    
        private IContext mContext;

        #region IStateMachine 
        /// <summary>
        /// Ver <see cref="Radgie.State.IStateMachine.LastEvent"/>
        /// </summary>
        public Event LastEvent
        {
            get
            {
                return mLastEvent;
            }
        }
        protected Event mLastEvent;
        #endregion

        #endregion

        #region Constructors
        /// <summary>
		/// Crea una nueva maquina de estados.
		/// </summary>
		/// <param name="eventSink">Maquina de estados padre si la nueva maquina de estados va a depender de ella.</param>
		public AStateMachine(IStateMachine eventSink)
			: base(eventSink)
		{
            mContext = new Context(this);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Crea una nueva transicion dentre dos estados de la maquina.
		/// </summary>
		/// <param name="source">Estado inicial.</param>
		/// <param name="ev">Evento.</param>
		/// <param name="target">Estado final.</param>
		protected void AddTransition(T source, Event ev, T target)
		{
			Dictionary<Event, T> row;

			if (!mTransitions.TryGetValue(source, out row))
			{
				row = new Dictionary<Event, T>();
				mTransitions.Add(source, row);
			}
			row.Add(ev, target);
		}

		/// <summary>
		/// Comprueba el evento mNextEvent y lo ejecuta si es correcto.
		/// </summary>
		/// <exception cref="KeyNotFoundException">Si no existe la entrada buscada</exception>
		private void CheckEvent()
		{
			if (mNextEvent != null)
			{
                mLastEvent = mNextEvent;
                try
                {
                    SetState(mTransitions[mCurrentState][mNextEvent]);
                }
                catch
                {
                }
				mNextEvent = null;
			}
		}

		/// <summary>
		/// Establece el nuevo estado de la maquina de estados.
		/// </summary>
		/// <param name="state">Nuevo estado.</param>
		private void SetState(T state)
		{
			if (mCurrentState != null)
			{
				mCurrentState.OnExit();
			}
			mCurrentState = state;
			if (mCurrentState != null)
			{
				mCurrentState.OnEntry();
			}
			else
			{
				OnFinish();
			}
		}

        /// <summary>
		/// Metodo que se ejecuta al finalizar la ejecucion de la maquina.
		/// </summary>
        protected virtual void OnFinish()
        {
            mLastEvent = null;
        }

        #region IStateMachine Methods
        /// <summary>
        /// Ver <see cref="Radgie.State.IStateMachine.CastEvent"/>
        /// </summary>
        public virtual void CastEvent(Event ev)
        {
            mNextEvent = ev;
        }
        #endregion

		#region IState Members
        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnEntry"/>
        /// </summary>
		public override void OnEntry()
		{
            base.OnEntry();

			if (mCurrentState == null)
			{
				if (mInitialState == null)
				{
					throw new Exception("Se debe definir un estado inicial");
				}
				SetState(mInitialState);
			}
		}

		#endregion

		#region AState Members
        /// <summary>
        /// Ver <see cref="Radgie.State.AState.Update"/>
        /// </summary>
		public override void Update(Microsoft.Xna.Framework.GameTime time)
		{
            base.Update(time);

			if (mCurrentState == null)
			{
				OnEntry();
			}
			mCurrentState.Update(time);
			CheckEvent();
		}
		#endregion
		#endregion
    }
}
