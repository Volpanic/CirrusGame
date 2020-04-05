using Cirrus.Cirrus.Collision;
using Cirrus.Cirrus.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Tiles
{
    public struct TilesetWorldLayer
    {
        public TileSet tileSet;
        public bool Visible;
        public bool Locked;
        public string Name;

        public TilesetWorldLayer(TileSet _tle, string _name)
        {
            Visible = true;
            Locked = false;
            tileSet = _tle;
            Name = _name;
        }

        public TilesetWorldLayer(TileSet _tle, string _name, bool _vis)
        {
            Visible = _vis;
            Locked = false;
            tileSet = _tle;
            Name = _name;
        }

        public TilesetWorldLayer(TileSet _tle, string _name, bool _vis,bool _lock)
        {
            Visible = _vis;
            Locked = _lock;
            tileSet = _tle;
            Name = _name;
        }
    }

    public class TileSet
    {
        public Texture2D TileSetTexture;
        public int TileWidth = 16;
        public int TileHeight = 16;
        public float Depth = 0;

        public Point[,] TileGridArray;

        public TileSet(Point GridSize)
        {
            TileGridArray = TileGridFillEmpty(GridSize.X,GridSize.Y);
        }

        /// <summary>
        /// Set the tile pos (x,y in consideration to width and height, i.e the 3rd tile on the first row wound be (2,0))
        /// </summary>
        /// <param name="GridPos"></param> The positon on the level grid place a tile
        /// <param name="TilePos"></param> The tile (Within the texture) to place at point
        public void SetGridPosition(Point GridPos,Point TilePos)
        {
            //Within range X check
            if(GridPos.X >= 0 && GridPos.X < TileGridArray.GetLength(0))
            {
                if (GridPos.Y >= 0 && GridPos.Y < TileGridArray.GetLength(1))
                {
                    TileGridArray[GridPos.X, GridPos.Y] = TilePos;
                }
            }

            return;
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

                    spriteBatch.Draw(TileSetTexture,new Vector2(xx * TileWidth, yy * TileHeight),sourceRect,Color.White,0.0f,Vector2.Zero,1.0f,SpriteEffects.None, Depth);

                }
            }
        }

        public bool RectTileCollision(AABB rect, Vector2 offset)
        {
            int CellWidth = 16;
            int CellHeight = 16;

            rect.X += offset.X;
            rect.Y += offset.Y;

            // Get rounded tile positions
            float minx = (float)Math.Floor(rect.X / CellWidth) - 1;
            float miny = (float)Math.Floor(rect.Y / CellHeight) - 1;

            float maxx = (float)Math.Ceiling((rect.X + rect.width) / CellWidth) + 1;
            float maxy = (float)Math.Ceiling((rect.Y + rect.height) / CellHeight) + 1;

            Vector2 min = new Vector2(minx, miny);
            Vector2 max = new Vector2(maxx, maxy);

            //Set range of tile view
            float MinSpaceX = Math.Max(min.X, 0);
            float MinSpaceY = Math.Max(min.Y, 0);

            float MaxSpaceX = Math.Min(max.X, TileGridArray.GetLength(0) - 1);
            float MaxSpaceY = Math.Min(max.Y, TileGridArray.GetLength(1) - 1);

            //Loop thorugh tile pos
            for (int xx = (int)MinSpaceX; xx <= MaxSpaceX; xx++)
            {
                for (int yy = (int)MinSpaceY; yy <= MaxSpaceY; yy++)
                {
                    if (TileGridArray[xx, yy] != new Point(0, 0))
                    {
                        //Creates a rect representing the tile
                        AABB TileRect = new AABB(xx * CellWidth, yy * CellHeight, CellWidth, CellHeight);

                        if (rect.CollidingWith(TileRect))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
