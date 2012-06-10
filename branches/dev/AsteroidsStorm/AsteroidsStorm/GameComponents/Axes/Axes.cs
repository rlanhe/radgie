using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.Graphics.Entity;

namespace AsteroidsStorm.GameComponents.Axes
{
    /// <summary>
    /// Ejes para utilizar en el desarrollo.
    /// </summary>
    public class Axes : Radgie.Core.GameComponent
    {
        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Axes()
            : base(string.Empty)
        {
            VertexPositionColor[] vertices = {new VertexPositionColor(Vector3.Zero, Color.Red),
                                              new VertexPositionColor(Vector3.Right, Color.Red),
                                              new VertexPositionColor(Vector3.Zero, Color.Yellow),
                                              new VertexPositionColor(Vector3.Up, Color.Yellow),
                                              new VertexPositionColor(Vector3.Zero, Color.Green),
                                              new VertexPositionColor(Vector3.Forward, Color.Green)};
            int[] indices = { 0, 1,
                              2, 3,
                              4, 5 };
            StaticGeometry g = new StaticGeometry();
            g.SetData(vertices, indices, PrimitiveType.LineList);

            Geometry mGeometry = new Geometry(g);
            mGeometry.Material = Radgie.Core.RadgieGame.Instance.ResourceManager.Load<Material>(@"Radgie/Graphics/Materials/debug");
            AddGameObject(mGeometry);
        }
    }
}
