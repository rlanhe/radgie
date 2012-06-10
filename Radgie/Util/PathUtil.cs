using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radgie.Util
{
    /// <summary>
    /// Clase de utilidad para resolver problemas con paths de ficheros.
    /// </summary>
    public class PathUtil
    {
        #region Constants
        /// <summary>
        /// Separador de directorios.
        /// </summary>
        public const string DIR = "/";
        /// <summary>
        /// Separador de directorios en windows.
        /// </summary>
        public const string WIN_DIR = "\\";
        /// <summary>
        /// Simbolo para subir un directorio.
        /// </summary>
        public const string UP_DIR = "../";
        #endregion

        #region Methods
        /// <summary>
        /// Obtiene el directorio a partir del path completo del fichero.
        /// </summary>
        /// <param name="filepath">path del fichero.</param>
        /// <returns>Directorio donde reside el fichero.</returns>
        public static string GetDirFromFilePath(string filepath)
        {
            string path = TranslatePath(filepath);
            string[] splits = path.Split(DIR.ToCharArray()[0]);
            return path.Replace(splits.Last(), string.Empty);
        }

        /// <summary>
        /// Traduce un path de windows a un path de un elemento de Content.
        /// </summary>
        /// <param name="filepath">Path original.</param>
        /// <returns>Path traducido.</returns>
        public static string TranslatePath(string filepath)
        {
            return filepath.Replace(WIN_DIR, DIR);
        }

        /// <summary>
        /// Obtiene el numero de directorios que hay que subir.
        /// </summary>
        /// <param name="path">Path original.</param>
        /// <returns>Numero de directorios que hay que subir.</returns>
        private static int NumberOfUpDirs(string path)
        {
            string translatedPath = TranslatePath(path);
            int pos = 0;
            int count = 0;

            while (pos != -1)
            {
                pos = translatedPath.IndexOf(UP_DIR, pos);
                if (pos != -1)
                {
                    pos += UP_DIR.Length - 1;
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Elimina la cadena que indica que hay que subir un directorio.
        /// </summary>
        /// <param name="path">Path original.</param>
        /// <returns>Cadena sin simbolos de subir un directorio.</returns>
        private static string CleanPathOfUpDirs(string path)
        {
            return path.Replace(UP_DIR, string.Empty);
        }

        /// <summary>
        /// Sube tantos directorios como se le indique.
        /// </summary>
        /// <param name="path">Path original.</param>
        /// <param name="numberOfGoUps">Numero de directorios que hay que subir.</param>
        /// <returns>Path expandido sin los simbolos de subir directorio.</returns>
        private static string GoUp(string path, int numberOfGoUps)
        {
            if (numberOfGoUps != 0)
            {
                string[] split = path.Split(DIR.ToCharArray()[0]);
                if (split.Count() <= numberOfGoUps)
                {
                    return string.Empty;
                }

                string result = string.Empty;
                int max = split.Count() - numberOfGoUps - 1;
                for (int i = 0; i < max; i++)
                {
                    result += split[i] + DIR;
                }
                return result;
            }
            return path;
        }

        /// <summary>
        /// Combina el paht del asset que esta cargando con un path relativo del propio asset para encontrar un recurso dentro de Content.
        /// </summary>
        /// <param name="currentAssetPath">Path del fichero actual.</param>
        /// <param name="relativeNewAssetPath">Path relativo del fichero que se quiere cargar.</param>
        /// <returns>Path completo del fichero que se quiere cargar.</returns>
        public static string CombinePaths(string currentAssetPath, string relativeNewAssetPath)
        {
            string currentDir = GetDirFromFilePath(currentAssetPath);
            string newDir = GoUp(currentDir, NumberOfUpDirs(relativeNewAssetPath));
            return newDir + CleanPathOfUpDirs(TranslatePath(relativeNewAssetPath));
        }
        #endregion
    }
}
