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
using System.Xml.Linq;

namespace Radgie.File
{
    /// <summary>
    /// Objeto especializado en la lectura de ficheros con Materiales (.m)
    /// </summary>
    public class MaterialFileReader : ContentTypeReader<Material>
    {
        #region Constants
        // Etiquetas del fichero xml.
        private const string RENDER_STATE = "RenderState";
        private const string EFFECT = "Effect";
        private const string EFFECT_PARAMETERS = "EffectParameters";
        private const string EFFECT_PARAMETER = "EffectParameter";
        private const string SEMANTIC = "semantic";
        private const string NAME = "name";
        private const string COLOR = "Color";
        private const string TEXTURE = "Texture";
        private const string BLEND_STATE = "BlendState";
        private const string RASTERIZER_STATE = "RasterizerState";
        private const string SAMPLER_STATE = "SamplerState";
        private const string DEPTH_STENCIL_STATE = "DepthStencilState";
        private const string BLEND_FACTOR = "BlendFactor";
        private const string MULTI_SAMPLE_MASK = "MultiSampleMask";
        private const string REFERENCE_STENCIL = "ReferenceStencil";
        private const string DRAW_ORDER = "DrawOrder";
        #endregion

        #region Methods
        #region ContentTypeReader members
        /// <summary>
        /// ContentTypeReader para objetos RenderState.
        /// </summary>
        /// <param name="input">Reader del fichero xnb.</param>
        /// <param name="existingInstance"></param>
        /// <returns>RenderState</returns>
        protected override Material Read(ContentReader input, Material existingInstance)
        {
            input.ReadString();
            string xmlContent = XmlFile.ExpandXmlContent(input.AssetName, input.ReadString());
            return CreateMaterial(input.AssetName, XDocument.Load(new StringReader(xmlContent)));
        }

        /// <summary>
        /// Crea un nuevo material a partir de los parametros del fichero.
        /// </summary>
        /// <param name="document">Documento xml.</param>
        /// <returns>Nuevo Material.</returns>
        private Material CreateMaterial(string id, XDocument document)
        {
            Material material = new Material(id);
            
            XElement renderStates = document.Root.Element(RENDER_STATE);
            if (renderStates != null)
            {
                ParseRenderState(renderStates, material);
            }

            XElement effect = document.Root.Element(EFFECT);
            if (effect != null)
            {
                material.Effect = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Effect>(effect.Value).Clone();
            }

            XElement effectParameters = document.Root.Element(EFFECT_PARAMETERS);
            if (effectParameters != null)
            {
                ParseParameters(effectParameters, material);
            }

            XElement drawOrder = document.Root.Element(DRAW_ORDER);
            if (drawOrder != null)
            {
                ParseDrawOrder(drawOrder, material);
            }

            return material;
        }

        /// <summary>
        /// Parsea la seccion DrawOrder.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <param name="material">Nuevo material.</param>
        private void ParseDrawOrder(XElement value, Material material)
        {
            material.DrawOrder = int.Parse(value.Value);
        }

        /// <summary>
        /// Parsea la seccion Parameters.
        /// </summary>
        /// <param name="value">Nodo xml.</param>
        /// <param name="material">Nuevo material.</param>
        private void ParseParameters(XElement parameters, Material material)
        {
            var parametersList = from p in parameters.Elements(EFFECT_PARAMETER)
                                 select p;
            foreach (var parameter in parametersList)
            {
                ParseParameter(parameter, material);
            }
        }

        /// <summary>
        /// Obtiene un parametro a partir de su nombre o semantic.
        /// </summary>
        /// <param name="material">Nuevo material.</param>
        /// <param name="name">Nombre del parametro.</param>
        /// <param name="semantic">Semantic del parametro.</param>
        /// <returns>Null si no lo encuentra, o el MaterialParameter en caso contrario.</returns>
        private MaterialParameter GetParameter(Material material, string semantic, string name)
        {
            if (semantic != null)
            {
                return material[(Semantic)Enum.Parse(typeof(Semantic), semantic, false)];
            }
            else if (name != null)
            {
                return material[name];
            }
            return null;
        }

        /// <summary>
        /// Obtiene el valor de un atributo de un nodo xml.
        /// </summary>
        /// <param name="parameter">Nodo xml.</param>
        /// <param name="aName">Nombre del atributo.</param>
        /// <returns>Null si no lo encuentra, si no un string con el valor.</returns>
        private string GetAttributeValue(XElement parameter, string aName)
        {
            XAttribute attribute = parameter.Attribute(aName);
            return attribute != null ? attribute.Value : null;
        }

        /// <summary>
        /// Parsea un parametro.
        /// </summary>
        /// <param name="parameter">Nodo xml.</param>
        /// <param name="material">Nuevo material.</param>
        private void ParseParameter(XElement parameter, Material material)
        {
            string name = GetAttributeValue(parameter, NAME);
            string semantic = GetAttributeValue(parameter, SEMANTIC);
            XNode value = parameter.FirstNode;
            MaterialParameter mParameter = GetParameter(material, semantic, name);

            if (mParameter != null)
            {
                if (value is XElement)
                {
                    XElement elementNode = (XElement)value;
                    string type = elementNode.Name.ToString();

                    switch (type)
                    {
                        case COLOR:
                            mParameter.SetValue(XmlFileReader.GetColor(elementNode).ToVector4());
                            break;
                        case TEXTURE:
                            mParameter.SetValue(Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Texture>(elementNode.Value, false));
                            break;
                    }
                }
                else
                {
                    // Infiere el tipo del objeto, si no esta especificado.
                    bool boolValue;
                    bool result = bool.TryParse(value.ToString(), out boolValue);

                    if (result)
                    {
                        mParameter.SetValue(boolValue);
                    }
                    else
                    {
                        int intValue;
                        result = int.TryParse(value.ToString(), out intValue);
                        if (result)
                        {
                            mParameter.SetValue(XmlFileReader.GetInt(value));
                        }
                        else
                        {
                            float floatValue;
                            result = float.TryParse(value.ToString(), out floatValue);
                            if (result)
                            {
                                mParameter.SetValue(XmlFileReader.GetFloat(value));
                            }
                            else
                            {
                                throw new Exception("Unknow type");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Parsea la seccion RenderStates.
        /// </summary>
        /// <param name="renderState">Nodo xml.</param>
        /// <param name="material">Nuevo Material.</param>
        private void ParseRenderState(XElement renderState, Material material)
        {
            RenderState rs = new RenderState();

            // TODO: Asociar hojas de validacion a los xml para que puedan validar que tiene todas las entradas
            foreach (var node in renderState.Elements())
            {
                switch (node.Name.LocalName)
                {
                    case BLEND_STATE:
                        rs.BlendState = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<BlendState>(node.Value, false);
                        break;
                    case DEPTH_STENCIL_STATE:
                        rs.DepthStencilState = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<DepthStencilState>(node.Value, false);
                        break;
                    case RASTERIZER_STATE:
                        rs.RasterizerState = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<RasterizerState>(node.Value, false);
                        break;
                    case SAMPLER_STATE:
                        rs.SamplerState = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<SamplerState>(node.Value, false);
                        break;
                    case BLEND_FACTOR:
                        rs.BlendFactor = XmlFileReader.GetColor(node);
                        break;
                    case MULTI_SAMPLE_MASK:
                        rs.MultiSampleMask = XmlFileReader.GetInt(node);
                        break;
                    case REFERENCE_STENCIL:
                        rs.ReferenceStencil = XmlFileReader.GetInt(node);
                        break;
                }
            }
            material.RenderState = rs;
        }

        #endregion
        #endregion
    }
}
