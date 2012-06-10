using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Radgie.Graphics.Entity;
using Radgie.Core;
using Radgie.Graphics;
using Microsoft.Xna.Framework;

namespace AsteroidsStorm.GameComponents.GUI
{
    /// <summary>
    /// Widget para mostrar un texto en el GUI.
    /// </summary>
    class Label: AWidget
    {
        /// <summary>
        /// Texto del widget.
        /// </summary>
        public string Text
        {
            get
            {
                return mTextObject.Value;
            }
            set
            {
                mTextObject.Value = value;

            }
        }

        /// <summary>
        /// Fuente con la que se dibujara el texto.
        /// </summary>
        public SpriteFont Font
        {
            get
            {
                return mTextObject.Font;
            }
            set
            {
                mTextObject.Font = value;
            }
        }

        /// <summary>
        /// Color empleado al dibujar el texto.
        /// </summary>
        public Color Color
        {
            get
            {
                return mTextObject.Color;
            }
            set
            {
                mTextObject.Color = value;
            }
        }

        /// <summary>
        /// Escala del texto.
        /// </summary>
        public float Scale
        {
            get
            {
                return mTextObject.Scale;
            }
            set
            {
                mTextObject.Scale = value;
            }
        }

        private Text mTextObject;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.AWidget.CreateGameComponent"/>
        /// </summary>
        protected override void CreateGameComponent()
        {
            mGameComponent = new Radgie.Core.GameComponent(Id);
            mTextObject = new Text();
            mTextObject.Material = RadgieGame.Instance.ResourceManager.Load<Material>("Radgie/Graphics/Materials/defaultFont").Clone();
            mGameComponent.AddGameObject(mTextObject);
            Color = Color.White;
            Scale = 1.0f;
        }

        /// <summary>
        /// Crea una etiqueta para mostrar texto en el GUI.
        /// </summary>
        /// <param name="id">Identificador del widget.</param>
        /// <param name="width">Anchura de la etiqueta en pixeles.</param>
        /// <param name="height">Altura de la etiqueta en pixeles.</param>
        public Label(string id, int width, int height)
            : base(id, width, height)
        {
        }
    }
}
