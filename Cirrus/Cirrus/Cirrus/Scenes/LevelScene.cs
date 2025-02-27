﻿using Cirrus.Cirrus.Entities;
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
        Player player;

        //Tiles
        public List<TilesetWorldLayer> TileSetList = new List<TilesetWorldLayer>();
        public TileSet CollisionSet { get { return GetCollisionSet(); } }

        public LevelScene(GameRunner _game, List<TilesetWorldLayer> _tileSetList = null) : base(_game)
        {
            TileSetList = _tileSetList;

            player = new Player(this);
            player.Position.X += 16;
        }

        private TileSet GetCollisionSet()
        {
            if(TileSetList != null && TileSetList.Count > 0)
            {
                return TileSetList[0].tileSet;
            }
            else
            {
                TileSetList = new List<TilesetWorldLayer>();
                CollisionTileSet cts = new CollisionTileSet(new Point(16,9));

                TileSetList.Add(new TilesetWorldLayer(cts,"Collision"));
                return TileSetList[0].tileSet;
            }
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            BackgroundsUpdate(gameTime, Vector2.Zero);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            ////////////////////////////////////////////////////////////////////////
            player.Draw(spriteBatch,gameTime);
            BackgroundsDraw(spriteBatch,GetWorldSize(),Vector2.Zero);

            //Tiles
            foreach(TilesetWorldLayer tswl in TileSetList)
            {
                if (tswl.Visible)
                {
                    tswl.tileSet.DrawTileSet(spriteBatch);
                }
                
            }
        }

        public Point GetWorldSize()
        {
            if(TileSetList.Count <= 0)
            {
                return new Point(Screen.GameHeight, Screen.GameWidth);
            }
            else
            {
                return new Point(TileSetList[0].tileSet.TileGridArray.GetLength(0)*16, TileSetList[0].tileSet.TileGridArray.GetLength(0) * 16);
            }
        }

        public override void DrawGui(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }
    }
}
