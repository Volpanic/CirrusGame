using Cirrus.Cirrus.Collision;
using Cirrus.Cirrus.Helpers;
using Cirrus.Cirrus.Scenes;
using Cirrus.Cirrus.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Cirrus.Cirrus.Entities
{
    public class Player : Entity
    {
        public float MoveSpeed = 2.0f;
        public float JumpSpeed = 4.0f;
        public float GroundFricc = 0.33f;
        public float AirFricc = 0.11f;
        public float Grav = 0.2f;

        public int State = 0;

        public Player(LevelScene _levelScene) : base(_levelScene)
        {
            Sprite = Sprites.GetSprite("spr_player_idle");
            SpriteWidth = Sprite.Width;
            SpriteHeight = Sprite.Height;
            Origin = new Vector2(SpriteWidth/2,SpriteHeight/2);
            Depth = .75f;
        }

        public override void Update(GameTime gameTime)
        {

            PlayerMovement(gameTime);
            switch (State)
            {
                case 0:
                {
                    
                    break;
                }
            }

            if(Velocity.X != 0)
            {
                Scale.X = Math.Sign(Velocity.X); ;
            }
        }

        public void PlayerMovement(GameTime gameTime)
        {

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            KeyboardState kb = Keyboard.GetState();

            bool keyRight = kb.IsKeyDown(Keys.Right);
            bool keyLeft = kb.IsKeyDown(Keys.Left);
            bool keyJump = kb.IsKeyDown(Keys.Z);
            bool keyActive = kb.IsKeyDown(Keys.X);

            float fricc = GroundFricc*deltaTime;
            if (!OnGround()) fricc = AirFricc*deltaTime;

            if(keyRight)
            {
                Velocity.X = MathMore.Approach(Velocity.X,MoveSpeed,fricc);
            }

            if (keyLeft)
            {
                Velocity.X = MathMore.Approach(Velocity.X, -MoveSpeed, fricc);
            }

            if ((keyRight && keyLeft) || (!keyRight && !keyLeft))
            {
                Velocity.X = MathMore.Approach(Velocity.X, 0, fricc);
            }

            //Grav
            Velocity.Y = MathMore.Approach(Velocity.Y,10,Grav*deltaTime);

            if(OnGround())
            {
                if(keyJump)
                {
                    Velocity.Y = -JumpSpeed;
                }
            }

            Collision(deltaTime);
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawSelf(spriteBatch);
            //Drawing.DrawRectangle(spriteBatch, (Rectangle)collisionBox, Color.Red);
        }
    }
}
