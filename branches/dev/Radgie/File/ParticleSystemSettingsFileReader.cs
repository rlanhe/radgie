using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Radgie.Core;
using System.Xml.Linq;

namespace Radgie.File
{
    /// <summary>
    /// Objeto especializado en la carga de ficheros de definicion de sistemas de particulas (.ps).
    /// </summary>
    public class ParticleSystemSettingsFileReader : ContentTypeReader<ParticleSystemSettings>
    {

        #region Constants
        // Etiquetas del fichero xml.
        private const string MAX_PARTICLES = "MaxParticles";
        private const string DURATION = "Duration";
        private const string DURATION_RANDOMNESS = "DurationRandomness";
        private const string EMITTER_VELOCITY_SENSITIVITY = "EmitterVelocitySensitivity";
        private const string MIN_HORIZONTAL_VELOCITY = "MinHorizontalVelocity";
        private const string MAX_HORIZONTAL_VELOCITY = "MaxHorizontalVelocity";
        private const string MIN_VERTICAL_VELOCITY = "MinVerticalVelocity";
        private const string MAX_VERTICAL_VELOCITY = "MaxVerticalVelocity";
        private const string GRAVITY = "Gravity";
        private const string END_VELOCITY = "EndVelocity";
        private const string MIN_COLOR = "MinColor";
        private const string MAX_COLOR = "MaxColor";
        private const string MIN_ROTATE_SPEED = "MinRotateSpeed";
        private const string MAX_ROTATE_SPEED = "MaxRotateSpeed";
        private const string MIN_START_SIZE = "MinStartSize";
        private const string MAX_START_SIZE = "MaxStartSize";
        private const string MIN_END_SIZE = "MinEndSize";
        private const string MAX_END_SIZE = "MaxEndSize";
        private const string MATERIAL = "Material";
        #endregion

        #region Methods

        #region ContentTypeReader members

        /// <summary>
        /// ContentTypeReader para objetos RenderState.
        /// </summary>
        /// <param name="input">Reader del fichero xnb.</param>
        /// <param name="existingInstance"></param>
        /// <returns>RenderState</returns>
        protected override ParticleSystemSettings Read(ContentReader input, ParticleSystemSettings existingInstance)
        {
            input.ReadString();
            XDocument doc = XDocument.Load(new StringReader(XmlFile.ExpandXmlContent(input.AssetName, input.ReadString())));
            return CreateParticleSystemSettings(doc);
        }
        #endregion

        /// <summary>
        /// Crea un objeto de configuraicon de un sistema de particulas a partir de un fichero xml.
        /// </summary>
        /// <param name="document">Documento xml.</param>
        /// <returns>Configuracion del sistema de particulas.</returns>
        private ParticleSystemSettings CreateParticleSystemSettings(XDocument document)
        {
            ParticleSystemSettings settings = new ParticleSystemSettings();
            
            foreach (XElement node in document.Root.Nodes())
            {
                switch (node.Name.LocalName)
                {
                    case MAX_PARTICLES:
                        settings.MaxParticles = XmlFileReader.GetInt(node);
                        break;
                    case DURATION:
                        settings.Duration = XmlFileReader.GetTimeSpan(node);
                        break;
                    case DURATION_RANDOMNESS:
                        settings.DurationRandomness = XmlFileReader.GetFloat(node);
                        break;
                    case EMITTER_VELOCITY_SENSITIVITY:
                        settings.EmitterVelocitySensitivity = XmlFileReader.GetFloat(node);
                        break;
                    case MIN_HORIZONTAL_VELOCITY:
                        settings.MinHorizontalVelocity = XmlFileReader.GetFloat(node);
                        break;
                    case MAX_HORIZONTAL_VELOCITY:
                        settings.MaxHorizontalVelocity = XmlFileReader.GetFloat(node);
                        break;
                    case MIN_VERTICAL_VELOCITY:
                        settings.MinVerticalVelocity = XmlFileReader.GetFloat(node);
                        break;
                    case MAX_VERTICAL_VELOCITY:
                        settings.MaxVerticalVelocity = XmlFileReader.GetFloat(node);
                        break;
                    case GRAVITY:
                        settings.Gravity = XmlFileReader.GetVector3(node);
                        break;
                    case END_VELOCITY:
                        settings.EndVelocity = XmlFileReader.GetFloat(node);
                        break;
                    case MIN_COLOR:
                        settings.MinColor = XmlFileReader.GetColor(node);
                        break;
                    case MAX_COLOR:
                        settings.MaxColor = XmlFileReader.GetColor(node);
                        break;
                    case MIN_ROTATE_SPEED:
                        settings.MinRotateSpeed = XmlFileReader.GetFloat(node);
                        break;
                    case MAX_ROTATE_SPEED:
                        settings.MaxRotateSpeed = XmlFileReader.GetFloat(node);
                        break;
                    case MIN_START_SIZE:
                        settings.MinStartSize = XmlFileReader.GetFloat(node);
                        break;
                    case MAX_START_SIZE:
                        settings.MaxStartSize = XmlFileReader.GetFloat(node);
                        break;
                    case MIN_END_SIZE:
                        settings.MinEndSize = XmlFileReader.GetFloat(node);
                        break;
                    case MAX_END_SIZE:
                        settings.MaxEndSize = XmlFileReader.GetFloat(node);
                        break;
                    case MATERIAL:
                        settings.Material = RadgieGame.Instance.ResourceManager.Load<Material>(node.Value, false);
                        break;
                }
            }
            
            return settings;
        }

        #endregion
    }
}
