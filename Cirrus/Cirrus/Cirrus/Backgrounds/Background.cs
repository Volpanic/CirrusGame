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
            ParralxMulit = Vector2.Zero;
        }

        public Background(Texture2D _sprite, Vector2 _pos, Vector2 _speed)
        {
            BackgroundSprite = _sprite;
            Position = _pos;
            basePos = _pos;
            ScrollSpeed = _speed;
            xTile = false;
            yTile = false;
            ParralxMulit = Vector2.Zero;
        }

        public void Update(GameTime gameTime, Vector2 ParralaxPosition)
        {
            basePos += ScrollSpeed * ((float)gameTime.ElapsedGameTime.TotalSeconds * 60);

            Position = basePos;
        }

        public void Draw(SpriteBatch spriteBatch,float DrawDepth, Point WorldSize,Vector2 CameraPos)
        {
            if (BackgroundSprite == null) return;
            if (!xTile && !yTile)
            {
                spriteBatch.Draw(BackgroundSprite,Position + (CameraPos * ParralxMulit), null, new Rectangle(0, 0, BackgroundSprite.Width, BackgroundSprite.Height), Vector2.Zero,0.0f,Vector2.One,Color.White,SpriteEffects.None,DrawDepth);
            }
            else
            {
                Vector2 newpos = Position + (CameraPos * ParralxMulit);
                spriteBatch.Draw(BackgroundSprite, Vector2.Zero, null, new Rectangle((int)newpos.X, (int)newpos.Y, WorldSize.X,WorldSize.Y), Vector2.Zero, 0.0f, Vector2.One, Color.White, SpriteEffects.None, DrawDepth);
            }
        }

        public static void UpdateBackgroundArray(ref Background[] backArray, GameTime gameTime, Vector2 CameraPos)
        {
            foreach (Background bk in backArray)
            {
                bk.Update(gameTime, CameraPos);
            }
        }

        public static void DrawBackgroundArray(ref Background[] backArray,SpriteBatch spriteBatch, Point WorldSize, Vector2 CameraPos)
        {
            float depth = 0;
            foreach (Background bk in backArray)
            {
                bk.Draw(spriteBatch, depth, WorldSize, CameraPos);
                depth += 0.01f;
            }
        }

        //List
        public static void UpdateBackgroundArray(ref List<Background> backArray, GameTime gameTime, Vector2 CameraPos)
        {
            foreach (Background bk in backArray)
            {
                bk.Update(gameTime, CameraPos);
            }
        }

        public static void DrawBackgroundArray(ref List<Background> backArray, SpriteBatch spriteBatch, Point WorldSize, Vector2 CameraPos)
        {
            float depth = 1;
            foreach (Background bk in backArray)
            {
                bk.Draw(spriteBatch, depth, WorldSize, CameraPos);
                depth -= 0.01f;
            }
        }
    }
}
