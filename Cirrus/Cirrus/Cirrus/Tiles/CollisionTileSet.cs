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
        public CollisionTileSet()
        {
            TileSetTexture = Sprites.GetSprite("spr_collisionTileSet");

            TileGridArray = TileGridFillEmpty(16,16);

            SetGridPosition(new Point(1, 1), new Point(2, 0));
            SetGridPosition(new Point(1, 2), new Point(1, 0));
            SetGridPosition(new Point(2, 1), new Point(3, 0));

            for (int xx = 0; xx < TileGridArray.GetLength(0); xx++)
            {
                Console.WriteLine();
                for (int yy = 0; yy < TileGridArray.GetLength(1); yy++)
                {
                    Console.Write($" - {TileGridArray[xx,yy].X},{TileGridArray[xx, yy].Y}-");
                }
            }

            Console.Write(TileGridArray.ToString());
        }
    }
}
