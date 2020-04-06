using Cirrus.Cirrus.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Cirrus.Cirrus
{
    public abstract class Component
    {
        public Texture2D Sprite;

        public Vector2 Position     = Vector2.Zero;
        public Vector2 Origin       = Vector2.Zero;
        public Vector2 Scale        = Vector2.One;

        public float Rotation       = 0.0f;
        public float Depth          = 0.0f;

        public int ImageIndex       = 0;
        public int SpriteWidth      = 1;
        public int SpriteHeight     = 1;
        public int AnimationSpeed   = 0;
        private int AnimationTimer  = 0;

        public bool Active = true;

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void DrawGui()
        {

        }

        public void DrawSelf(SpriteBatch spriteBatch) //AutoDrawing
        {
            if (Sprite == null) return;

            Rectangle spriteRect = new Rectangle(0,0, SpriteWidth, SpriteHeight);

            bool flipX = (Math.Sign(Scale.X) == -1);
            bool flipY = (Math.Sign(Scale.Y) == -1);
            Vector2 fscale = new Vector2(Math.Abs(Scale.X),Math.Abs(Scale.Y));

            if (Sprite.Width != SpriteWidth)
            {
                AnimationTimer++;
                //Animation
                if(AnimationSpeed != 0 && AnimationTimer >= AnimationSpeed)
                {
                    AnimationTimer = AnimationTimer % AnimationSpeed;

                    ImageIndex++;
                    if(ImageIndex*SpriteWidth >= Sprite.Width)
                    {
                        ImageIndex = 0;
                    }
                }


                spriteRect.X = (SpriteWidth * ImageIndex);
                spriteBatch.Draw(Sprite,Position,spriteRect,Color.White,Rotation, Origin, fscale, (flipX? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipY ? SpriteEffects.FlipVertically : SpriteEffects.None), Depth);
            }
            else
            {
                spriteBatch.Draw(Sprite, Position, spriteRect, Color.White, Rotation, Origin, fscale, (flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (flipY ? SpriteEffects.FlipVertically : SpriteEffects.None), Depth);
            }
        }

    }
}
