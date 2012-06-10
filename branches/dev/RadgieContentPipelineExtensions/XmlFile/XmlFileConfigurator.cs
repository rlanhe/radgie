using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadgieContentPipelineExtensions.XmlFile
{
    public static class XmlFileConfigurator
    {
        public struct XmlFileConfiguratorEntry
        {
            public string Extension;
            public string RuntimeType;
            public string RuntimeReader;
        }

        private static IDictionary<string, XmlFileConfiguratorEntry> mTypes = new Dictionary<string, XmlFileConfiguratorEntry>();

        public static void Add(XmlFileConfiguratorEntry entry)
        {
            mTypes.Add(entry.Extension, entry);
        }

        public static XmlFileConfiguratorEntry Get(string key)
        {
            return mTypes[key];
        }
    }
}
