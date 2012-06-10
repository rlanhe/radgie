using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace RadgieContentPipelineExtensions.Graphics.ModelFile
{
    [ContentSerializerRuntimeType("Radgie.Graphics.Mesh, Radgie")]
    public class ModelFileContent
    {
        public List<GeometryContent> geometryParts = new List<GeometryContent>();
        public List<MaterialContent> materials = new List<MaterialContent>();
        //public ExternalReference<XmlFile.XmlFileContent> defaultMaterial;
        public string defaultMaterialId;

        public void AddMeshPart(GeometryContent geometryContent, MaterialContent materialContent)
        {
            geometryParts.Add(geometryContent);
            materials.Add(materialContent);
        }
    }
}
