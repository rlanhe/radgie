using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radgie.Core;
using Radgie.Scene.Managers.Simple;
using AsteroidsStorm.GameComponents;
using Radgie.Scene.Managers.Octree;
using AsteroidsStorm.GameComponents.AsteroidsField;

namespace AsteroidsStorm.States.Game
{
    /// <summary>
    /// Clase que almacena el estado de la partida.
    /// </summary>
    class GameData
    {
        /// <summary>
        /// Escena principal del juego.
        /// </summary>
        public IScene GameScene
        {
            get
            {
                return mGameScene;
            }
        }
        private IScene mGameScene;

        /// <summary>
        /// Estado del jugador.
        /// </summary>
        public PlayerState PlayerState
        {
            get
            {
                return mPlayerState;
            }
        }
        private PlayerState mPlayerState;
        /// <summary>
        /// Sector de asteroides.
        /// </summary>
        public AsteroidsField AsteroidsField
        {
            get
            {
                return mAsteroidsField;
            }
            set
            {
                mAsteroidsField = value;
            }
        }
        private AsteroidsField mAsteroidsField;

        public bool GameOver { get; set; }

        /// <summary>
        /// Inicializa el estado de la partida.
        /// </summary>
        public GameData()
        {
            //mGameScene = new OctreeScene("GameScene", 10, 4);
            mGameScene = new SimpleScene("GameScene");
            mPlayerState = new PlayerState();
            GameOver = false;
        }
    }
}
