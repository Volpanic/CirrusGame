using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Backgrounds
{
    public class Background
    {
        public Texture2D BackgroundSprite;
        public Vector2 Position;
        private Vector2 basePos; // Un Parralaxed
        public Vector2 ScrollSpeed;
        public bool xTile;
        public bool yTile;
        public Vector2 ParralxMulit;
       
        public Background(Texture2D _sprite,Vector2 _pos, Vector2 _speed,bool _xt, bool _yt, Vector2 _parramulti)
        {
            BackgroundSprite = _sprite;
            Position = _pos;
            basePos = _pos;
            ScrollSpeed = _speed;
            xTile = _xt;
            yTile = _yt;
            ParralxMulit = _parramulti;
        }

        public Background(Texture2D _sprite, Vector2 _pos, Vector2 _speed, bool _xt, bool _yt)
        {
            BackgroundSprite = _sprite;
            Position = _pos;
            basePos = _pos;
            ScrollSpeed = _speed;
            xTile = _xt;
            yTile = _yt;
            ParralxMulit = Vector2.One;
        }

        public Background(Texture2D _sprite, Vector2 _pos, Vector2 _speed)
        {
            BackgroundSprite = _sprite;
            Position = _pos;
            basePos = _pos;
            ScrollSpeed = _speed;
            xTile = false;
            yTile = false;
            ParralxMulit = Vector2.One;
        }

        public void Update(GameTime gameTime, Vector2 ParralaxPosition)
        {
            basePos += ScrollSpeed * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60);

            Position = basePos;
        }

        public void Draw(SpriteBatch spriteBatch,float DrawDepth,Vector2 CameraPos)
        {
            if (!xTile && !yTile)
            {
                spriteBatch.Draw(BackgroundSprite,Position, null, new Rectangle(0, 0, BackgroundSprite.Width, BackgroundSprite.Height), Vector2.Zero,0.0f,Vector2.One,Color.White,SpriteEffects.None,DrawDepth);
            }
            else
            {
                Vector2 newpos;
                newpos = new Vector2(Position.X % BackgroundSprite.Width, Position.Y % BackgroundSprite.Height);
                spriteBatch.Draw(BackgroundSprite, newpos - new Vector2(BackgroundSprite.Width * 2, BackgroundSprite.Height * 2), null, new Rectangle(0, 0, BackgroundSprite.Width*4, BackgroundSprite.Height*4), Vector2.Zero, 0.0f, Vector2.One, Color.White, SpriteEffects.None, DrawDepth);
            }
        }
    }
}
