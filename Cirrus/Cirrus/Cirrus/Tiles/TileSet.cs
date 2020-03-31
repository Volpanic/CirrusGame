using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Tiles
{
    public abstract class TileSet
    {
        public Texture2D TileSetTexture;
        public int TileWidth = 16;
        public int TileHeight = 16;
        public float Depth = 0;

        public bool Visable = true;

        public Point[,] TileGridArray;

        /// <summary>
        /// Set the tile pos (x,y in consideration to width and height, i.e the 3rd tile on the first row wound be (2,0))
        /// </summary>
        /// <param name="GridPos"></param> The positon on the level grid place a tile
        /// <param name="TilePos"></param> The tile (Within the texture) to place at point
        public void SetGridPosition(Point GridPos,Point TilePos)
        {
            //Within range X check
            if(GridPos.X < 0 || GridPos.X > TileGridArray.GetLength(0))
            {
                return;
            }

            //Within range Y check
            if (GridPos.Y < 0 || GridPos.Y > TileGridArray.GetLength(1))
            {
                return;
            }
    
            TileGridArray[GridPos.X,GridPos.Y] = TilePos;
        }

        // () in grid pos
        public Rectangle GetTileRect(Point TileWorldLocation)
        {    
            Point sheetPos = TileGridArray[TileWorldLocation.X,TileWorldLocation.Y];
            sheetPos.X *= TileWidth;
            sheetPos.Y *= TileHeight;

            return new Rectangle(sheetPos.X, sheetPos.Y, TileWidth, TileHeight);

        }

        public Point[,] TileGridFillEmpty(int width, int height)
        {
            Point[,] temp = new Point[width,height];

            for (int xx = 0; xx < width; xx++)
            {
                for (int yy = 0; yy < height; yy++)
                {
                    temp[xx, yy] = Point.Zero;
                }
            }

            return temp;
        }

        public void DrawTileSet(SpriteBatch spriteBatch)
        {
            //Don't draw if we can't ya know?
            if (TileSetTexture == null) return;
            if (TileGridArray == null) return;

            for (int xx = 0; xx < TileGridArray.GetLength(0); xx++)
            {
                for (int yy = 0; yy < TileGridArray.GetLength(1); yy++)
                {
                    Rectangle sourceRect = GetTileRect(new Point(xx,yy));
                    Console.WriteLine(sourceRect.ToString());

                    spriteBatch.Draw(TileSetTexture,new Vector2(xx * TileWidth, yy * TileHeight),sourceRect,Color.White,0.0f,Vector2.Zero,1.0f,SpriteEffects.None, Depth);

                }
            }
        }
    }
}
