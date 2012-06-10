using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Core
{
    /// <summary>
    /// Interfaz para entidades de Radgie.
    /// Una entidad es cualquier recurso (grafico, sonido, comportamiento) que es candidato a ser compartido/referenciado por varias instancias.
    /// </summary>
    public interface IEntity: IGameObject
    {
        #region Methods
        /// <summary>
        /// Crea una instancia sobre la entidad.
        /// </summary>
        /// <returns>Nueva instancia</returns>
        IInstance CreateInstance();

        /// <summary>
        /// Elimina una de las instancias sobre la entidad.
        /// </summary>
        /// <param name="instance">Instancia a borrar.</param>
        void RemoveInstance(IInstance instance);
        #endregion
    }
}
