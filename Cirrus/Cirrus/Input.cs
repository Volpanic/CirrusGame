using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus
{
    public class Input
    {
        KeyboardState previousState;

        public Input()
        {
            previousState = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            previousState = Keyboard.GetState();
        }

        public bool GetKeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }

        public bool GetKeyReleased(Keys key)
        {
            return (Keyboard.GetState().IsKeyUp(key) && previousState.IsKeyDown(key));
        }

        public bool GetKeyPressed(Keys key)
        {
            return (Keyboard.GetState().IsKeyDown(key) && previousState.IsKeyUp(key));
        }
    }
}
