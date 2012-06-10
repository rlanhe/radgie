using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Scene
{
	/// <summary>
	/// Interfaz del sistema de escenas.
	/// </summary>
    public interface ISceneSystem: ISystem
    {
        #region Properties
        /// <summary>
        /// Estadisticas del sistema
        /// </summary>
        SceneSystemStatistics Statistics { get; }
        #endregion

        #region Methods
        /// <summary>
		/// Annade una escena al sistema para que la actualice automaticamente en cada frame.
		/// Para annadirla al sistema debera tener un identificador unico.
		/// </summary>
		/// <param name="scene">Nueva escena.</param>
		/// <returns>True si la annadio correctamente, False en caso contrario.</returns>
		bool AddScene(IScene scene);
		/// <summary>
		/// Obtiene una escena a partir de su nombre.
		/// </summary>
		/// <param name="id">Identificador de la escena.</param>
		/// <returns>La escena buscada si existe, null en caso contrario.</returns>
		IScene GetScene(string id);
		/// <summary>
		/// Quita la escena del sistema.
		/// La escena ya no sera actualizada por el sistema.
		/// </summary>
		/// <param name="scene">Escena que se quiere quitar del sistema.</param>
		/// <returns>True si se quito la escena con exito, False en caso contrario.</returns>
		bool RemoveScene(IScene scene);
		#endregion
	}
}
