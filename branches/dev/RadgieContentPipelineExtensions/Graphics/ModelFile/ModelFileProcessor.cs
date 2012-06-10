using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using RadgieContentPipelineExtensions.Common;
using System.Collections.ObjectModel;

namespace RadgieContentPipelineExtensions.Graphics.ModelFile
{
    [ContentProcessor(DisplayName = "Custom Model Processor - Radgie")]
    public class ModelFileProcessor: ContentProcessor<NodeContent, ModelFileContent>
    {
        private ContentProcessorContext mContext;
        private ModelFileContent mResultModel;

        [Browsable(true)]
        public bool GenerateTangentFrames
        {
            get
            {
                return mGenerateTangentFrames;
            }
            set
            {
                mGenerateTangentFrames = value;
            }
        }
        private bool mGenerateTangentFrames;

        [Browsable(true)]
        public string DefaultMaterial
        {
            get
            {
                return mDefaultMaterial;
            }
            set
            {
                mDefaultMaterial = value;
            }
        }
        private string mDefaultMaterial;

        // A single material may be reused on more than one piece of geometry.
        // This dictionary keeps track of materials we have already converted,
        // to make sure we only bother processing each of them once.
        Dictionary<MaterialContent, MaterialContent> processedMaterials =
                            new Dictionary<MaterialContent, MaterialContent>();

        public override ModelFileContent Process(NodeContent input, ContentProcessorContext context)
        {
            mContext = context;
            mResultModel = new ModelFileContent();

            if (!String.IsNullOrWhiteSpace(mDefaultMaterial))
            {
                string defaultMaterial = null;
                if (!mDefaultMaterial.StartsWith(PathUtil.DIR))
                {
                    defaultMaterial = PathUtil.RelativeToGlobalPath(PathUtil.GetContentPath(context.OutputFilename, context), mDefaultMaterial);
                }
                else
                {
                    defaultMaterial = mDefaultMaterial.Substring(1, mDefaultMaterial.Length-1);
                }
                mResultModel.defaultMaterialId = defaultMaterial;
            }
            else
            {
                throw new ArgumentNullException("DefaultMaterial File Not Specified!");
            }

            ProcessNode(input);

            return mResultModel;
        }

        private void ProcessNode(NodeContent node)
        {
            MeshHelper.TransformScene(node, node.Transform);

            node.Transform = Matrix.Identity;

            // Is this node in fact a mesh?
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                // Reorder vertex and index data so triangles will render in
                // an order that makes efficient use of the GPU vertex cache.
                MeshHelper.OptimizeForCache(mesh);

                if(GenerateTangentFrames)
                {
                    bool flag1 = !GeometryContainsChannel(mesh, VertexChannelNames.Tangent(0));
                    bool flag2 = !GeometryContainsChannel(mesh, VertexChannelNames.Binormal(0));
                    if (flag1 || flag2)
                    {
                        string tangentChannelName = flag1 ? VertexChannelNames.Tangent(0) : (string)null;
                        string binormalChannelName = flag2 ? VertexChannelNames.Binormal(0) : (string)null;
                        MeshHelper.CalculateTangentFrames(mesh, VertexChannelNames.TextureCoordinate(0), tangentChannelName, binormalChannelName);
                    }
                }

                // Process all the geometry in the mesh.
                foreach (Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent geometry in mesh.Geometry)
                {
                    GeometryContent geometryContent = ProcessGeometry(geometry);
                    MaterialContent materialContent = ProcessMaterial(geometry.Material);

                    mResultModel.AddMeshPart(geometryContent, materialContent);
                }
            }

            // Recurse over any child nodes.
            foreach (NodeContent child in node.Children)
            {
                ProcessNode(child);
            }
        }

        private bool GeometryContainsChannel(MeshContent mesh, string channel)
        {
            if (string.IsNullOrEmpty(channel))
            {
                throw new ArgumentNullException("channel");
            }
            
            foreach (Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent geometryContent in mesh.Geometry)
            {
                if (geometryContent.Vertices.Channels.Contains(channel))
                {
                    return true;
                }
            }
            return false;
        }

        GeometryContent ProcessGeometry(Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent geometry)
        {
            return mContext.Convert<Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent, GeometryContent>(geometry, "GeometryProcessor");

        }

        MaterialContent ProcessMaterial(MaterialContent material)
        {
            // Have we already processed this material?
            if (!processedMaterials.ContainsKey(material))
            {
                // If not, process it now.
                processedMaterials[material] = mContext.Convert<MaterialContent, MaterialContent>(material, "MaterialProcessor");
            }

            return processedMaterials[material];
        }
    }
}
