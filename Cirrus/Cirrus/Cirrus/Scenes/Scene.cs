using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Scenes
{
    public abstract class Scene
    {
        public GameRunner baseGame;

        public Scene(GameRunner _game)
        {
            baseGame = _game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void DrawGui(SpriteBatch spriteBatch, GameTime gameTime);

    }
}
