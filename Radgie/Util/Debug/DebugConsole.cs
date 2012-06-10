using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Graphics.Entity;
using Microsoft.Xna.Framework;
using Radgie.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Radgie.Util.Debug
{
    /// <summary>
    /// Consola de debug para mostrar informacion en pantalla.
    /// </summary>
    public class DebugConsole: Radgie.Core.GameComponent
    {
        private Text FPS;
        private Text FPS_Label;

        /// <summary>
        /// Inicializa la consola de debug.
        /// </summary>
        public DebugConsole()
            : base("debug_console")
        {
            Material material = RadgieGame.Instance.ResourceManager.Load<Material>("Radgie/Graphics/Materials/defaultFont");
            SpriteFont font = RadgieGame.Instance.ResourceManager.Load<SpriteFont>("Radgie/Graphics/Fonts/default_big", false);
            Color color = Color.GreenYellow;

            FPS_Label = new Text();
            FPS_Label.Color = color;
            FPS_Label.Material = material;
            FPS_Label.Font = font;
            FPS_Label.Value = "FPS:";
            FPS_Label.Scale = 1.0f;
            Radgie.Core.GameComponent gc1 = new Radgie.Core.GameComponent("gc_label");
            gc1.AddGameObject(FPS_Label);
            
            FPS = new Text();
            FPS.Color = color;
            FPS.Material = material;
            FPS.Font = font;
            FPS.Value = "60";
            FPS.Scale = 1.0f;
            Radgie.Core.GameComponent gc2 = new Radgie.Core.GameComponent("gc_value");
            gc2.AddGameObject(FPS);
            
            AddGameComponent(gc1);
            AddGameComponent(gc2);
            gc1.Transformation.Translation2D = new Vector2(-90, 50);
            gc2.Transformation.Translation2D = new Vector2(-200, 50);
        }

        /// <summary>
        /// Ver <see cref="Radgie.Core.IUpdateable.Update"/>
        /// </summary>
        public override void Update(GameTime time)
        {
            base.Update(time);

            FPS.Value = RadgieGame.Instance.Statistics.FPS.ToString();
        }
    }
}
