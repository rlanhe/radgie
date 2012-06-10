using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;

namespace Radgie.Scene
{
	/// <summary>
	/// Interfaz de un nodo de escena.
    /// Artificio para evitar realizar cast de manera constante.
	/// </summary>
	/// <typeparam name="T">Tipo del nodo de escena</typeparam>
	public interface IComplexSceneNode<T>: ISceneNode
	{
	}
}
