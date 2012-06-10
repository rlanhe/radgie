using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace RadgieContentPipelineExtensions.Common
{
    class PathUtil
    {
        public const string WINDOWS_DIR = "\\";
        private const string OBJ = "\\obj\\";
        private const string PARENT_DIR = "../";
        public const string DIR = "/";
        private const char DOT = '.';

        public static string GetContentPath(string filename, ContentImporterContext context)
        {
            string intermediateDir = context.IntermediateDirectory;
            string outputDir = context.OutputDirectory;
            int pos = intermediateDir.IndexOf(OBJ);
            string contentDirPath = outputDir.Substring(0, pos);
            string contentPath = filename.Replace(contentDirPath + WINDOWS_DIR, string.Empty);
            //contentPath = contentPath.Replace(GetFileExtension(filename), string.Empty);
            //contentPath = contentPath.Replace(WINDOWS_DIR, DIR);
            contentPath = contentPath.Replace(WINDOWS_DIR, DIR);
            string[] splits = contentPath.Split(DIR.ToCharArray()[0]);
            contentPath = contentPath.Replace(splits.Last(), string.Empty);
            if (contentPath.Length > 0)
            {
                contentPath = contentPath.Substring(0, contentPath.Length - 1);
            }
            return contentPath;
        }

        public static string GetContentPath(string filename, ContentProcessorContext context)
        {
            string outputDir = context.OutputDirectory;
            string contentPath = filename.Replace(outputDir, string.Empty);
            contentPath = contentPath.Replace(WINDOWS_DIR, DIR);
            string[] splits = contentPath.Split(DIR.ToCharArray()[0]);
            contentPath = contentPath.Replace(splits.Last(), string.Empty);
            if (contentPath.Length > 0)
            {
                contentPath = contentPath.Substring(0, contentPath.Length - 1);
            }
            return contentPath;
        }

        public static string GetFileExtension(string filename)
        {
            string[] splits = filename.Split(DOT);
            return DOT + splits.Last();
        }

        private static int RemoveParentDirsSymbols(ref string contentId)
        {
            int pos = 0;
            int count = 0;

            while (pos != -1)
            {
                pos = contentId.IndexOf(PARENT_DIR, pos);
                if (pos != -1)
                {
                    pos += PARENT_DIR.Length-1;
                    count++;
                }
            }

            contentId = contentId.Replace(PARENT_DIR, string.Empty);

            return count;
        }

        private static string RemoveParentDirs(string path, int numberOfParentDirs)
        {
            if (numberOfParentDirs != 0)
            {
                string[] split = path.Split(DIR.ToCharArray()[0]);
                if (split.Count() <= numberOfParentDirs)
                {
                    return string.Empty;
                }

                string result = string.Empty;
                int max = split.Count() - numberOfParentDirs;
                for (int i = 0; i < max; i++)
                {
                    result += split[i] + DIR;
                }
                return result;
            }
            return path + DIR;
        }

        public static string RelativeToGlobalPath(string contentPath, string localPath)
        {
            string value = localPath;
            int parentDirs = PathUtil.RemoveParentDirsSymbols(ref value);
            return PathUtil.RemoveParentDirs(contentPath, parentDirs) + value;
        }
    }
}
