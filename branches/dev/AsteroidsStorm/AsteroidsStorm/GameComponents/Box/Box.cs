using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Entity;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Radgie.State;

namespace AsteroidsStorm.GameComponents.Box
{
    /// <summary>
    /// Caja contenedora que delimita la zona de juego
    /// </summary>
    public class Box: Radgie.Core.GameComponent
    {
        /// <summary>
        /// Posicion de la nave.
        /// </summary>
        private Vector3 mTargetPosition = Vector3.Zero;

        /// <summary>
        /// Coordenada maxima en el eje Y.
        /// </summary>
        public float Top
        {
            get
            {
                return mHeigth;
            }
        }

        /// <summary>
        /// Coordenada minima en el eje Y.
        /// </summary>
        public float Bottom
        {
            get
            {
                return -mHeigth;
            }
        }

        /// <summary>
        /// Coordenada minima en el eje X.
        /// </summary>
        public float Left
        {
            get
            {
                return -mWidth;
            }
        }

        /// <summary>
        /// Coordenada maxima en el eje X.
        /// </summary>
        public float Right
        {
            get
            {
                return mWidth;
            }
        }

        /// <summary>
        /// Anchura de la caja.
        /// </summary>
        private float mWidth;
        /// <summary>
        /// Altura de la caja.
        /// </summary>
        private float mHeigth;

        /// <summary>
        /// Crea una nueva caja.
        /// </summary>
        /// <param name="width">Ancho de la caja.</param>
        /// <param name="heigth">Alto de la caja.</param>
        /// <param name="boxTexture">Textura de la caja.</param>
        public Box(float width, float heigth, string boxTexture)
            : base("Box")
        {
            mWidth = width;
            mHeigth = heigth;

            ResourceManager rManager = RadgieGame.Instance.ResourceManager;
            Texture texture = rManager.Load<Texture>(boxTexture, false);
            Material material = rManager.Load<Material>("GameComponents/Box/Graphics/Materials/box");
            material[Semantic.Texture0].SetValue(texture);
            material["TargetPosition"].UpdateFreq = UpdateFreq.PerFrame;
            material["TargetPosition"].UpdateCallback = this.UpdatePositionCallback;

            GraphicSystem gSystem = (GraphicSystem)RadgieGame.Instance.GetSystem(typeof(GraphicSystem));
            StaticGeometry geometry = new StaticGeometry();
            float hWidth = width / 2.0f;
            float hHeight = heigth / 2.0f;
            geometry.SetData(new VertexPositionTexture[]{ new VertexPositionTexture(new Vector3(-hWidth, -hHeight, hWidth), new Vector2(0.0f, 1.0f)),
                                                          new VertexPositionTexture(new Vector3(-hWidth, -hHeight, -hWidth), new Vector2(0.0f, 0.0f)),
                                                          new VertexPositionTexture(new Vector3(hWidth, -hHeight, hWidth), new Vector2(1.0f, 1.0f)),
                                                          new VertexPositionTexture(new Vector3(hWidth, -hHeight, -hWidth), new Vector2(1.0f, 0.0f)),
                                                          new VertexPositionTexture(new Vector3(-hWidth, hHeight, hWidth), new Vector2(1.0f, 1.0f)),
                                                          new VertexPositionTexture(new Vector3(-hWidth, hHeight, -hWidth), new Vector2(1.0f, 0.0f)),
                                                          new VertexPositionTexture(new Vector3(hWidth, hHeight, hWidth), new Vector2(0.0f, 1.0f)),
                                                          new VertexPositionTexture(new Vector3(hWidth, hHeight, -hWidth), new Vector2(0.0f, 0.0f))},
                                                          new int[] { 0, 1, 2, 2, 1, 3, 3, 7, 2, 2, 7, 6, 5, 4, 7, 7, 4, 6, 0, 4, 1, 1, 4, 5 },
                                                          PrimitiveType.TriangleList);
            Geometry geometryEntity = new Geometry(geometry);
            
            geometryEntity.Material = material;
            AddGameObject(geometryEntity);
        }

        /// <summary>
        /// Actualiza la posicion del target.
        /// </summary>
        /// <param name="translation">Posicion del target.</param>
        public void UpdatePosition(Vector3 translation)
        {
            mTargetPosition = translation;
            Transformation.Translation = new Vector3(0.0f, 0.0f, translation.Z);
        }

        /// <summary>
        /// Callback utilizado para actualizar el parametro del shadder que indica la posicion del target.
        /// </summary>
        /// <param name="renderer">Renderer.</param>
        /// <param name="parameter">Posicion del target.</param>
        private void UpdatePositionCallback(IRenderer renderer, MaterialParameter parameter)
        {
            parameter.SetValue(mTargetPosition + new Vector3(0.0f, 0.0f, -1.7f));
        }
    }
}
