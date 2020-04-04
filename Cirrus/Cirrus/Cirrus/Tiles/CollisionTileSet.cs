using Cirrus.Cirrus.Collision;
using Cirrus.Cirrus.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Tiles
{
    public class CollisionTileSet : TileSet
    {
        public CollisionTileSet(Point GridSize) : base(GridSize)
        {
            TileSetTexture = Sprites.GetSprite("spr_collisionTileSet");
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
            float MinSpaceY = Math.Max(min.X, 0);

            float MaxSpaceX = Math.Min(max.X, TileGridArray.GetLength(0) - 1);
            float MaxSpaceY = Math.Min(max.X, TileGridArray.GetLength(1) - 1);

            //Loop thorugh tile pos
            for (int xx = (int)MinSpaceX; xx <= MaxSpaceX; xx++)
            {
                for (int yy = (int)MinSpaceY; yy <= MaxSpaceY; yy++)
                {
                    if (TileGridArray[xx, yy] == new Point(0,1))
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
