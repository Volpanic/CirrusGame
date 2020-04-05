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

        public CollisionTileSet(TileSet _tileSet) : base(new Point(_tileSet.TileGridArray.GetLength(0), _tileSet.TileGridArray.GetLength(1)))
        {
            TileSetTexture = Sprites.GetSprite("spr_collisionTileSet");
            TileGridArray = _tileSet.TileGridArray;
            Depth = _tileSet.Depth;
        }

        
    }
}
