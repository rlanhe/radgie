using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Input.Control;

namespace Radgie.Input.Action
{
    /// <summary>
    /// Accion a la que se asocian controles.
    /// </summary>
    /// <typeparam name="T">Tipo del control del accion.</typeparam>
    public abstract class AAction<T> where T: IControl
    {
        #region Properties
        /// <summary>
        /// Bindings asociados a esta accion.
        /// </summary>
        protected List<T> mBindings = new List<T>();
        #endregion

        #region Constructors
        /// <summary>
        /// Crea e inicializa la accion.
        /// </summary>
        public AAction()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Asocia un control con la accion.
        /// </summary>
        /// <param name="binding">Control asociado a esta accion.</param>
        public void AddBinding(T binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException("binding is null");
            }

            mBindings.Add(binding);
        }
        #endregion
    }
}
