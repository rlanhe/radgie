using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace RadgieContentPipelineExtensions.Graphics.ModelFile
{
    [ContentProcessor(DisplayName = "GeometryProcessor - Radgie")]
    class GeometryProcessor : ContentProcessor<Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent, GeometryContent>
    {
        public override GeometryContent Process(Microsoft.Xna.Framework.Content.Pipeline.Graphics.GeometryContent input, ContentProcessorContext context)
        {
            GeometryContent geometry = new GeometryContent();

            geometry.TriangleCount = input.Indices.Count / 3;
            geometry.VertexCount = input.Vertices.VertexCount;

            geometry.VertexBufferContent = input.Vertices.CreateVertexBuffer();
            geometry.IndexCollection = input.Indices;

            return geometry;
        }
    }
}
