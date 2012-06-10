using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadgieContentPipelineExtensions.XmlFile
{
    public static class XmlFile
    {
        static XmlFile()
        {
            XmlFileConfigurator.XmlFileConfiguratorEntry entry;
            entry.Extension = ".xml";
            entry.RuntimeType = "Radgie.File.XmlFile, Radgie";
            entry.RuntimeReader = "Radgie.File.XmlFileReader, Radgie";
            XmlFileConfigurator.Add(entry);
        }
    }
}
