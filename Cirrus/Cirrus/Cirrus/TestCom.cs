using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus
{
    public class TestCom : Component
    {

        public TestCom(Texture2D _sprite)
        {
            Sprite = _sprite;
            SpriteWidth = 16;
            SpriteHeight = 16;

            AnimationSpeed = 2;
        }
        


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawSelf(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
