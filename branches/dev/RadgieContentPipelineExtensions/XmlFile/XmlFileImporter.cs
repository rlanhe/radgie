using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using tInput = RadgieContentPipelineExtensions.XmlFile.XmlFileContent;
using System.Xml;
using System.IO;
using RadgieContentPipelineExtensions.Common;

namespace RadgieContentPipelineExtensions.XmlFile
{
    /// <summary>
    /// Lee el fichero del disco y lo deja en memoria.
    /// </summary>
    [ContentImporter(".xml", ".strings", ".m", ".bs", ".dss", ".rs", ".ss", ".pss", DisplayName = "XmlFileImporter - Radgie")]
    public class XmlFileImporter : ContentImporter<tInput>
    {
        static XmlFileImporter()
        {
            XmlFileConfigurator.XmlFileConfiguratorEntry xml;
            xml.Extension = ".xml";
            xml.RuntimeType = "Radgie.File.XmlFile, Radgie";
            xml.RuntimeReader = "Radgie.File.XmlFileReader, Radgie";
            XmlFileConfigurator.Add(xml);

            XmlFileConfigurator.XmlFileConfiguratorEntry m;
            m.Extension = ".m";
            m.RuntimeType = "Radgie.Graphics.Material, Radgie";
            m.RuntimeReader = "Radgie.File.MaterialFileReader, Radgie";
            XmlFileConfigurator.Add(m);

            XmlFileConfigurator.XmlFileConfiguratorEntry strings;
            strings.Extension = ".strings";
            strings.RuntimeType = "Radgie.File.StringsFile, Radgie";
            strings.RuntimeReader = "Radgie.File.XmlFileReader, Radgie";
            XmlFileConfigurator.Add(strings);

            XmlFileConfigurator.XmlFileConfiguratorEntry blenderState;
            blenderState.Extension = ".bs";
            blenderState.RuntimeType = "Microsoft.Xna.Framework.Graphics.BlendState, Microsoft.Xna.Framework.Graphics";
            blenderState.RuntimeReader = "Radgie.File.BlendStateFileReader, Radgie";
            XmlFileConfigurator.Add(blenderState);

            XmlFileConfigurator.XmlFileConfiguratorEntry depthState;
            depthState.Extension = ".dss";
            depthState.RuntimeType = "Microsoft.Xna.Framework.Graphics.DepthStencilState, Microsoft.Xna.Framework.Graphics";
            depthState.RuntimeReader = "Radgie.File.DepthStencilStateFileReader, Radgie";
            XmlFileConfigurator.Add(depthState);

            XmlFileConfigurator.XmlFileConfiguratorEntry rasterizerState;
            rasterizerState.Extension = ".rs";
            rasterizerState.RuntimeType = "Microsoft.Xna.Framework.Graphics.RasterizerState, Microsoft.Xna.Framework.Graphics";
            rasterizerState.RuntimeReader = "Radgie.File.RasterizerStateFileReader, Radgie";
            XmlFileConfigurator.Add(rasterizerState);

            XmlFileConfigurator.XmlFileConfiguratorEntry samplerState;
            samplerState.Extension = ".ss";
            samplerState.RuntimeType = "Microsoft.Xna.Framework.Graphics.SamplerState, Microsoft.Xna.Framework.Graphics";
            samplerState.RuntimeReader = "Radgie.File.SamplerStateFileReader, Radgie";
            XmlFileConfigurator.Add(samplerState);

            XmlFileConfigurator.XmlFileConfiguratorEntry particleSystemSettings;
            particleSystemSettings.Extension = ".pss";
            particleSystemSettings.RuntimeType = "Radgie.Graphics.ParticleSystemSettings, Radgie";
            particleSystemSettings.RuntimeReader = "Radgie.File.ParticleSystemSettingsFileReader, Radgie";
            XmlFileConfigurator.Add(particleSystemSettings);
        }

        #region Methods

        #region ContentImporter members
        /// <summary>
		/// Este metodo es llamado durante la generacion de los ficheros xnb.
		/// Carga el contenido del fichero y lo deja en memoria.
		/// </summary>
		/// <param name="filename">Path hasta el fichero.</param>
		/// <param name="context">Contexto del importer.</param>
		/// <returns></returns>
        public override tInput Import(string filename, ContentImporterContext context)
        {
            return new tInput(System.IO.File.ReadAllText(filename), PathUtil.GetFileExtension(filename));
		}
		#endregion

		#endregion
	}
}
