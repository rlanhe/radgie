using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Sound
{
    /// <summary>
    /// Entidad del sistema de sonido.
    /// </summary>
    public abstract class ASoundEntity: AEntity, ISoundEntity
    {
        #region Constructors
        /// <summary>
        /// Crea una entidad de sonido annadiendola al sistema de sonido para que la actualice periodicamente.
        /// </summary>
        public ASoundEntity()
        {
            // Annade la entidad a la lista de entidades que el sisteam debe actualizar.
            ((ISoundSystem)RadgieGame.Instance.GetSystem(typeof(ISoundSystem))).SoundEntityReferences.Add(this);
        }
        #endregion
    }
}
