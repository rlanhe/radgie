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

namespace AsteroidsStorm.GameComponents.Skybox
{
    /// <summary>
    /// Caja que muestra texturas del espacio y encierra la accion del juego.
    /// </summary>
    public class Skybox: Radgie.Core.GameComponent
    {
        /// <summary>
        /// Crea el skybox de un tamanno determinado, especificando las texturas de cada una de sus caras.
        /// </summary>
        /// <param name="size">Tamanno del lado de la caja del skybox.</param>
        /// <param name="top">Textura para la cara superio del cubo.</param>
        /// <param name="back">Textura para la cara posterior del cubo.</param>
        /// <param name="front">Textura de la cara frontal del cubo.</param>
        /// <param name="bottom">Textura de la cara inferior del cubo.</param>
        /// <param name="left">Textura de la cara izquierda del cubo.</param>
        /// <param name="right">Textura de la cara derecha del cubo.</param>
        public Skybox(float size, string top, string back, string front, string bottom, string left, string right)
            : base("Skybox")
        {
            ResourceManager rManager = RadgieGame.Instance.ResourceManager;

            Radgie.Core.GameComponent gc = new Radgie.Core.GameComponent("top");
            Sprite temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(top, false));
            gc.Transformation.Translation += Vector3.Up * (size / 2.0f);
            gc.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2);
            gc.AddGameObject(temp);
            AddGameComponent(gc);

            gc = new Radgie.Core.GameComponent("back");
            temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(back, false));
            gc.Transformation.Translation += Vector3.Backward * (size / 2.0f);
            gc.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi);
            gc.AddGameObject(temp);
            AddGameComponent(gc);

            gc = new Radgie.Core.GameComponent("front");
            temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(front, false));
            gc.Transformation.Translation += Vector3.Forward * (size / 2.0f);
            gc.AddGameObject(temp);
            AddGameComponent(gc);

            gc = new Radgie.Core.GameComponent("bottom");
            temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(bottom, false));
            gc.Transformation.Translation += Vector3.Down * (size / 2.0f);
            gc.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.Pi + MathHelper.PiOver2);
            gc.AddGameObject(temp);
            AddGameComponent(gc);

            gc = new Radgie.Core.GameComponent("left");
            temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(left, false));
            gc.Transformation.Translation += Vector3.Left * (size / 2.0f);
            gc.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.PiOver2);
            gc.AddGameObject(temp);
            AddGameComponent(gc);

            gc = new Radgie.Core.GameComponent("right");
            temp = new Sprite(size, size);
            temp.Material = rManager.Load<Material>("Radgie/Graphics/Materials/default").Clone();
            temp.Material[Semantic.Texture0].SetValue(rManager.Load<Texture>(right, false));
            gc.Transformation.Translation += Vector3.Right * (size / 2.0f);
            gc.Transformation.Rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi + MathHelper.PiOver2);
            gc.AddGameObject(temp);
            AddGameComponent(gc);
        }

        /// <summary>
        /// Actualiza la posicion del skybox dentro de la escena.
        /// </summary>
        /// <param name="translation">Nueva posicion del centro del skybox.</param>
        public void UpdatePosition(Vector3 translation)
        {
            Transformation.Translation = translation;
        }
    }
}
