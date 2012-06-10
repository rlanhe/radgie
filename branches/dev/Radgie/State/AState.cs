using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Util.Collection.Context;
using Radgie.Core;

namespace Radgie.State
{
	/// <summary>
	/// Abstraccion de un estado de una maquina de estados de Radgie.
	/// </summary>
    public abstract class AState: IState
	{
		#region Properties
		/// <summary>
		/// Maquina de estados padre de este estado.
		/// Si es null, se trata de una maquina de estados que no esta contenida en otra.
		/// </summary>
		protected readonly IStateMachine mStateMachine = null;

        /// <summary>
        /// Indica si ya se ejecuto el metodo OnEnty la primera vez.
        /// </summary>
        private bool mInitialize;

        /// <summary>
        /// Propietario del estado.
        /// </summary>
        public IStateEntity Owner
        {
            get
            {
                if((mOwner == null) && (mStateMachine != null))
                {
                    return mStateMachine.Owner;
                }
                return mOwner;
            }
            set
            {
                if (mStateMachine != null)
                {
                    mStateMachine.Owner = value;
                }
                else
                {
                    mOwner = value;
                }
            }
        }
        private IStateEntity mOwner;

        /// <summary>
        /// Indica si ya se llamo al metodo OnEntry.
        /// Previene que se llame al metodo Update sin que se haya llamado al metodo OnEntry.
        /// </summary>
        private bool mOnEntryExecuted = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Crea un nuevo estado.
		/// </summary>
		/// <param name="stateMachine">Maquina de estados a la que pertenece.</param>
		public AState(IStateMachine stateMachine)
		{
			mStateMachine = stateMachine;
		}
		#endregion

		#region Methods

        /// <summary>
		/// Envia un evento a su maquina de estados para que lo trate.
		/// </summary>
		/// <param name="ev">Evento</param>
		protected void SendEvent(Event ev)
		{
			if (mStateMachine != null)
			{
				mStateMachine.CastEvent(ev);
			}
		}

		/// <summary>
		/// Metodo a ejecutar por cada actualizacion del estado.
		/// </summary>
		/// <param name="time">Tiempo transcurrido desde la ultima actualizacion</param>
        public virtual void Update(GameTime time)
        {
            if (!mOnEntryExecuted)
            {
                OnEntry();
            }
        }

		#region IState Members
		/// <summary>
		/// Ver <see cref="Radgie.State.IState.OnEntry"/>
		/// </summary>
        public virtual void OnEntry()
        {
            mOnEntryExecuted = true;

            if (!mInitialize)
            {
                mInitialize = true;
                OnInitialize();
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.IState.OnExit"/>
        /// </summary>
        public virtual void OnExit()
        {
            mOnEntryExecuted = false;
        }
        #endregion

        #region IHasContext Methods
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext"/>
        /// </summary>
        public Variable GetFromContext(string id)
        {
            Variable result = null;

            if (this is IStateMachine)
            {
                IStateMachine stateMachine = (IStateMachine)this;
                result = stateMachine.Context.Get(id);
            }
            if (result == null)
            {
                if (mStateMachine != null)
                {
                    result = mStateMachine.GetFromContext(id);
                }
                else if (Owner != null)
                {
                    result = Owner.Component.GetFromContext(id);
                }
                else
                {
                    result = ((IStateSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IStateSystem))).Context.Get(id);
                }
            }
            
            return result;
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext<T>"/>
        /// </summary>
        public T GetFromContext<T>(string id)
        {
            T result = default(T);

            if (this is IStateMachine)
            {
                IStateMachine stateMachine = (IStateMachine)this;
                result = stateMachine.Context.Get<T>(id);
            }
            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                if (mStateMachine != null)
                {
                    result = mStateMachine.GetFromContext<T>(id);
                }
                else if (Owner != null)
                {
                    result = Owner.Component.GetFromContext<T>(id);
                }
                else
                {
                    result = ((IStateSystem)Radgie.Core.RadgieGame.Instance.GetSystem(typeof(IStateSystem))).Context.Get<T>(id);
                }
            }
            
            return result;
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext"/>
        /// </summary>
        public void SetInContext(string id, Variable value)
        {
            if (this is IStateMachine)
            {
                IStateMachine stateMachine = (IStateMachine)this;
                stateMachine.Context.Set(id, value);
            }
            else
            {
                if (mStateMachine != null)
                {
                    mStateMachine.Context.Set(id, value);
                }
                else if (Owner != null)
                {
                    Owner.Component.SetInContext(id, value);
                }
            }
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext<T>"/>
        /// </summary>
        public void SetInContext<T>(string id, T value)
        {
            if (this is IStateMachine)
            {
                IStateMachine stateMachine = (IStateMachine)this;
                stateMachine.Context.Set<T>(id, value);
            }
            else
            {
                if (mStateMachine != null)
                {
                    mStateMachine.Context.Set<T>(id, value);
                }
                else if (Owner != null)
                {
                    Owner.Component.SetInContext<T>(id, value);
                }
            }
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.RemoveFromContext"/>
        /// </summary>
        public bool RemoveFromContext(string id)
        {
            if (this is IStateMachine)
            {
                IStateMachine stateMachine = (IStateMachine)this;
                return stateMachine.Context.Remove(id);
            }
            else if (Owner != null)
            {
                return Owner.Component.RemoveFromContext(id);
            }
            return false;
        }
		#endregion

        /// <summary>
        /// Inicializa el estado.
        /// Se ejecuta la primera vez que se llama a el metodo OnEntry.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }
		#endregion
    }
}
