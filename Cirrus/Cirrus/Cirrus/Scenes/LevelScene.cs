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
        //Tiles
        CollisionTileSet ctl = new CollisionTileSet();
        public List<TilesetWorldLayer> TileSetList = new List<TilesetWorldLayer>();

        public LevelScene(Game _game) : base(_game)
        {

            //Tiles
            TileSetList.Add(new TilesetWorldLayer(ctl,"Collision"));
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Tiles
            foreach(TilesetWorldLayer tswl in TileSetList)
            {
                if(tswl.Visible)
                {
                    tswl.tileSet.DrawTileSet(spriteBatch);
                }
            }

        }

        public override void DrawGui(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }
    }
}
