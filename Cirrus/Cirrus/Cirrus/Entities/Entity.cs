using Cirrus.Cirrus.Collision;
using Cirrus.Cirrus.Scenes;
using Cirrus.Cirrus.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Cirrus.Cirrus.Entities
{
    public abstract class Entity : Component
    {
        public LevelScene levelScene;
        public AABB collisionBox { get {return GetCollisionBox(); } }
        public Vector2 Velocity;

        public Entity(LevelScene _levelScene)
        {
            levelScene = _levelScene;
        }

        protected AABB AutoMakeCollisionBox()
        {
            Vector2 size = new Vector2(Sprite.Width,Sprite.Height);
            return (new AABB(Position - (size/2),size));
        }

        public virtual AABB GetCollisionBox() // virtual so hitboxes can be overidden to be a diffent shape
        {
            return AutoMakeCollisionBox();
        }

        public void Collision(float deltaTime)
        {
            AABB ColRect = collisionBox;

            Vector2 actualVel = (Velocity) * (deltaTime);
            
            if(levelScene.CollisionSet.RectTileCollision(collisionBox,new Vector2(actualVel.X,0)))
            {
                while(!levelScene.CollisionSet.RectTileCollision(collisionBox, new Vector2(Math.Sign(actualVel.X), 0)))
                {
                    Position.X += Math.Sign(actualVel.X);
                }
                Velocity.X = 0;
                actualVel.X = 0;
            }
            Position.X += actualVel.X;

            if (levelScene.CollisionSet.RectTileCollision(collisionBox,new Vector2(0,actualVel.Y)))
            {
                while (!levelScene.CollisionSet.RectTileCollision(collisionBox, new Vector2(0, Math.Sign(actualVel.Y))))
                {
                    Position.Y += Math.Sign(actualVel.Y);
                }
                actualVel.Y = 0;
                Velocity.Y = 0;
            }
            Position.Y += actualVel.Y;
        }

        public bool OnGround()
        {
            TileSet coll = levelScene.CollisionSet;
            return (coll.RectTileCollision(collisionBox, new Vector2(0,1)));
        }
    }
}
