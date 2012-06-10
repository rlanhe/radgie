using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Util.Collection.Context;

namespace Radgie.State
{
    /// <summary>
    /// Entidad del sistema de estados para asociar maquinas de estados a GameComponentes.
    /// </summary>
    public class Behaviour: AStateEntity
    {
        #region Properties
        /// <summary>
        /// Referencia al estado o maquina de estados que encapsula.
        /// </summary>
        public IState State
        {
            get
            {
                return mState;
            }
        }
        private IState mState;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea un nuevo behaviour a partir de un estado o maquina de estados.
        /// </summary>
        /// <param name="state">Estado o maquina de estados.</param>
        public Behaviour(IState state)
        {
            mState = state;
            mState.Owner = this;
        }
        #endregion

        #region Methods
        #region AStateEntity Methods
        /// <summary>
        /// Ver <see cref="Radgie.State.AStateEntity.CreateSpecificInstance"/>
        /// </summary>
        protected override Core.IInstance CreateSpecificInstance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ver <see cref="Radgie.State.AStateEntity.Update"/>
        /// </summary>
        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            if (IsActive())
            {
                mState.Update(time);
            }
        }
        #endregion
        #endregion
    }
}
