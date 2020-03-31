using Cirrus.Cirrus.Helpers;
using Cirrus.Cirrus.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Scenes
{
    public class LevelScene : Scene
    {
        CollisionTileSet ctl = new CollisionTileSet();

        public LevelScene(Game _game) : base(_game)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ctl.DrawTileSet(spriteBatch);
        }

        public override void DrawGui(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            ctl.TileSetTexture = Sprites.GetSprite("spr_collisionTileSet");
        }
    }
}
