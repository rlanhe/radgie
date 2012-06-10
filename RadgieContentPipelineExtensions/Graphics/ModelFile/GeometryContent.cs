using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace RadgieContentPipelineExtensions.Graphics.ModelFile
{
    [ContentSerializerRuntimeType("Radgie.Graphics.Mesh+Geometry, Radgie")]
    public class GeometryContent
    {
        public int TriangleCount;
        public int VertexCount;

        public VertexBufferContent VertexBufferContent;
        public IndexCollection IndexCollection;
    }
}
