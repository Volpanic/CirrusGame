using Cirrus.Cirrus.Backgrounds;
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
        public Background[] Backgrounds;

        public Scene(GameRunner _game)
        {
            baseGame = _game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
        public abstract void DrawGui(SpriteBatch spriteBatch, GameTime gameTime);

        public void BackgroundsDraw(SpriteBatch spriteBatch, Point WorldSize, Vector2 CameraPos)
        {
            Background.DrawBackgroundArray(ref Backgrounds, spriteBatch, WorldSize, CameraPos);
        }

        public void BackgroundsUpdate(GameTime gameTime,Vector2 CameraPos)
        {
            Background.UpdateBackgroundArray(ref Backgrounds, gameTime, CameraPos);
        }
    }
}
