using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Radgie.Core.BoundingVolumes
{
    /// <summary>
    /// Interfaz de los volumenes de colision.
    /// </summary>
    public interface IBoundingVolume
    {
        #region Properties
        /// <summary>
        /// Un volumen de colision pueden tener un volumen hijo de colision.
        /// Cuando existe una colision con un objeto IBoundingVolume, si tiene un volumen de colision hijo se comprueba de nuevo la colision con este.
        /// </summary>
        IBoundingVolume ChildVolume { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Actualiza la posicion del volumen de colision en la escena.
        /// Este metodo es llamado automaticamente por el motor, no es necesario que sea invocado por el usuario.
        /// </summary>
        /// <param name="worldMatrix">Matriz de transformacion de mundo.</param>
        void Update(ref Matrix worldMatrix);

        /// <summary>
        /// Comprueba si intersecciona con otro volumen de colision.
        /// </summary>
        /// <param name="bv">Volumen de colision con el que comprueba si colisiona.</param>
        /// <returns>null si no colisiona, cualquier otro valor en caso de interseccion. En el caso de rayos de colision, indica la distancia entre el origen de este y el punto donde se produce la interseccion.</returns>
        float? Intersects(IBoundingVolume bv);

        /// <summary>
        /// Comprueba si contiene a otro volumen de colision.
        /// </summary>
        /// <param name="bv">Volumen de colision con el que comprueba si lo contiene.</param>
        /// <returns>Enumeracion indicando si lo contiene o no.</returns>
        ContainmentType Contains(IBoundingVolume bv);

        /// <summary>
        /// Clona el volumen de colision.
        /// </summary>
        /// <returns>Copia identica del volumen de colision.</returns>
        IBoundingVolume Clone();
        #endregion
    }
}
