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
    /// Widget para mostrar una imagen.
    /// </summary>
    class Image: AWidget
    {
        /// <summary>
        /// Textura del widget.
        /// </summary>
        public Texture Texture
        {
            get
            {
                return mSprite.Material[Semantic.Texture0].EffectParameter.GetValueTexture2D();
            }
            set
            {
                mSprite.Material[Semantic.Texture0].SetValue(value);
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.Width"/>
        /// </summary>
        public override int Width
        {
            set
            {
                mWidth = value;
                if (mSprite != null)
                {
                    Quad previousQuad = mSprite.Quad;
                    mSprite.Quad = new Quad(mWidth, previousQuad.Height);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.Height"/>
        /// </summary>
        public override int Height
        {
            set
            {
                mHeight = value;
                if (mSprite != null)
                {
                    Quad previousQuad = mSprite.Quad;
                    mSprite.Quad = new Quad(previousQuad.Width, mHeight);
                }
            }
        }

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.IWidget.DrawOrder"/>
        /// </summary>
        public override int DrawOrder
        {
            get
            {
                if (mSprite != null)
                {
                    return mSprite.Material.DrawOrder;
                }
                return 0;
            }
            set
            {
                if (mSprite != null)
                {
                    mSprite.Material.DrawOrder = value;
                }
            }
        }

        /// <summary>
        /// Sprite que se usara para dibujar la textura de la imagen por pantalla.
        /// </summary>
        private Sprite mSprite;

        /// <summary>
        /// Ver <see cref="AsteroidsStorm.GameComponents.GUI.AWidget.CreateGameComponent"/>
        /// </summary>
        protected override void CreateGameComponent()
        {
            mGameComponent = new Radgie.Core.GameComponent(Id);
            mSprite = new Sprite(mWidth, mHeight);
            ResourceManager rManager = RadgieGame.Instance.ResourceManager;
            mSprite.Material = rManager.Load<Material>("Radgie/Graphics/Materials/alphaBlend").Clone();
            mGameComponent.AddGameObject(mSprite);
        }

        /// <summary>
        /// Crea un widget para mostrar una imagen.
        /// </summary>
        /// <param name="id">Identificador del widget.</param>
        /// <param name="width">Ancho de la imagen en pixeles.</param>
        /// <param name="height">Altura de la imagen en pixeles.</param>
        public Image(string id, int width, int height)
            : base(id, width, height)
        {
        }
    }
}
