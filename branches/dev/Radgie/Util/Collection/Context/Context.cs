using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Util.Collection.Context
{
    /// <summary>
    /// Contexto.
    /// </summary>
    public class Context: IContext
    {
        #region Properties
        /// <summary>
        /// Coleccion de objetos del contexto.
        /// </summary>
        private IDictionary<string, Variable> mDictionary;

        #region IContext Properties
        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Owner"/>
        /// </summary>
        public IHasContext Owner
        {
            get
            {
                return mOwner;
            }
        }
        private IHasContext mOwner;
        #endregion
        #endregion

        #region Constructors.
        /// <summary>
        /// Crea un nuevo contexto para el objeto owner.
        /// </summary>
        /// <param name="owner">Propietario del contexto.</param>
        public Context(IHasContext owner)
        {
            mOwner = owner;
        }
        #endregion

        #region Methods

        #region IContext Methods
        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Get"/>
        /// </summary>
        public Variable Get(string id)
        {
            if (mDictionary == null)
            {
                return null;
            }
            Variable tmp;
            lock (this)
            {
                mDictionary.TryGetValue(id, out tmp);
            }
            return tmp;
        }

        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Get"/>
        /// </summary>
        public T Get<T>(string id)
        {
            if (mDictionary == null)
            {
                return default(T);
            }
            Variable var = Get(id);
            if (var != null)
            {
                return var.Get<T>();
            }
            return default(T);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Set"/>
        /// </summary>
        public void Set(string id, Variable value)
        {
            lock (this)
            {
                if (mDictionary == null)
                {
                    Initialize();
                }
                mDictionary[id] = value;
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Set"/>
        /// </summary>
        public void Set<T>(string id, T value)
        {
            lock (this)
            {
                if (mDictionary == null)
                {
                    Initialize();
                }

                Variable var = Get(id);
                if (var == null)
                {
                    var = new Variable(id);
                    mDictionary[id] = var;
                }
                var.Set(value);
            }
        }

        /// <summary>
        /// Ver <see cref="Radgie.Util.Collection.Context.IContext.Remove"/>
        /// </summary>
        public bool Remove(string id)
        {
            if (mDictionary != null)
            {
                lock (this)
                {
                    return mDictionary.Remove(id);
                }
            }
            return false;
        }
        #endregion

        /// <summary>
        /// Inicializa el contexto.
        /// </summary>
        private void Initialize()
        {
            mDictionary = new Dictionary<string, Variable>();
        }
        #endregion
    }
}
