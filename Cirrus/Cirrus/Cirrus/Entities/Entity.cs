using Cirrus.Cirrus.Collision;
using Cirrus.Cirrus.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cirrus.Cirrus.Entities
{
    public abstract class Entity : Component
    {
        public LevelScene levelScene;
        public AABB collisionBox;
        public Vector2 Velocity;

        protected AABB AutoMakeCollisionBox()
        {
            Vector2 size = new Vector2(Sprite.Width,Sprite.Height);
            return (new AABB(Position - (size/2),size));
        }

        public virtual AABB GetCollisionBox()
        {
            return AutoMakeCollisionBox();
        }
    }
}
