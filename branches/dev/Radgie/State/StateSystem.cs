using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Radgie.Core;
#if WIN32
using log4net;
#endif
using System.Reflection;
using System.Xml;
using Radgie.Util.Collection.ReferencePool;
using Radgie.Util.Collection.Context;
using System.Xml.Linq;

namespace Radgie.State
{
    /// <summary>
    /// Implementacion core del sistema de estados.
    /// Controla la logica del juego. Permite al usuario personalizar el juego mediante la programacion de estados y las transicciones entre estos.
    /// </summary>
    public class StateSystem: ASystem, IStateSystem
    {
        #region Properties
        /// <summary>
        /// Logger de la clase.
        /// </summary>
        #if WIN32
        private static readonly ILog log = LogManager.GetLogger(typeof(StateSystem));
#endif

        /// <summary>
		/// Estado principal.
		/// </summary>
        private IStateMachine mGameState = null;

        #region IStateSystem Properties
        /// <summary>
        /// Ver <see cref="Radgie.State.IStateSystem.StateEntityReferences"/>
        /// </summary>
        public IReferencePool<IStateEntity> StateEntityReferences
        {
            get
            {
                return mStateEntityReferences;
            }
        }
        private IReferencePool<IStateEntity> mStateEntityReferences;

        /// <summary>
        /// Ver <see cref="Radgie.State.IStateSystem.Statistics"/>
        /// </summary>
        public StateSystemStatistics Statistics
        {
            get
            {
                return mStatistics;
            }
        }
        private StateSystemStatistics mStatistics;

        /// <summary>
        /// Ver <see cref="Radgie.State.IStateSystem.Context"/>
        /// </summary>
        public IContext Context
        {
            get
            {
                return mStateSystemContext;
            }
        }
        private Context mStateSystemContext;
        #endregion

        /// <summary>
        /// ultima vez en la que se actualizo el sistema.
        /// </summary>
        private GameTime mUpdateTime;

        /// <summary>
        /// Tipo de la maquina de estados principal.
        /// </summary>
        private string mGameStateMachine;

        /// <summary>
        /// Callback para actualizar los elementos del pool.
        /// </summary>
        private PoolAction<IStateEntity> mUpdatePoolActionCallback;
        #endregion

        #region Consts
        // Constantes para la lectura de la configuracion del sistema.
        private const string GAME_STATE = "GameState";
		#endregion

		#region Constructors
		/// <summary>
        /// Configura el sistema de estados a partir de la configuracion cargada desde fichero.
        /// </summary>
        /// <param name="sc">Configuracion del sistema.</param>
        public StateSystem(XElement sc): base(sc)
        {
            if ((mGameStateMachine == null) || (mGameStateMachine == ""))
            {
                throw new NullReferenceException("It's necesary to indicate the GameState for this system");
            }

            mStateEntityReferences = new ReferencePool<IStateEntity>(false);
            mStatistics = new StateSystemStatistics();
            mStateSystemContext = new Context(this);

            mUpdatePoolActionCallback = UpdatePoolAction;
        }

        #endregion

        #region Methods

		#region ASystem
        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.UpdateAction"/>
        /// </summary>
		protected override void UpdateAction(GameTime time)
		{
            if (mGameState == null)
            {
                // Crea la maquina de estados principal.
                Type implementation = Type.GetType(mGameStateMachine);
                ConstructorInfo ci = implementation.GetConstructor(Type.EmptyTypes);
                mGameState = (IStateMachine)ci.Invoke(null);
            }

            mStatistics.Reset();
            mStatistics.StartUpdateTimer();
            mUpdateTime = time;
			
            if (mGameState != null)
			{
				mGameState.Update(time);
			}

            mStateEntityReferences.FireActionOverPoolItems(mUpdatePoolActionCallback);
            mStatistics.StopUpdateTimer();
		}

        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.UpdatePoolAction"/>
        /// </summary>
        private void UpdatePoolAction(IStateEntity sEntity)
        {
            mStatistics.NumberOfStateObjectsInPool++;
            if ((sEntity.Component != null) && (sEntity.Component.IsActive()))
            {
                mStatistics.NumberOfStateObjectsToUpdateInPool++;
                sEntity.Update(mUpdateTime);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.ASystem.LoadParameters"/>
        /// </summary>
		protected override bool LoadParameters(string name, string value)
		{
			bool result = base.LoadParameters(name, value);

			if (!result)
			{
				if (GAME_STATE == name)
				{
                    mGameStateMachine = value;
					result = true;
				}
			}
			return result;
		}
        #endregion

        #region IHasContext Methods

        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext"/>
        /// </summary>
        public Variable GetFromContext(string id)
        {
            return Context.Get(id);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.GetFromContext<T>"/>
        /// </summary>
        public T GetFromContext<T>(string id)
        {
            return Context.Get<T>(id);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext"/>
        /// </summary>
        public void SetInContext(string id, Variable value)
        {
            Context.Set(id, value);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.SetInContext<T>"/>
        /// </summary>
        public void SetInContext<T>(string id, T value)
        {
            Context.Set<T>(id, value);
        }
        /// <summary>
        /// Ver <see cref="Radgie.Core.IHasContext.RemoveFromContext"/>
        /// </summary>
        public bool RemoveFromContext(string id)
        {
            return Context.Remove(id);
        }
        #endregion
        
        #endregion
    }
}
