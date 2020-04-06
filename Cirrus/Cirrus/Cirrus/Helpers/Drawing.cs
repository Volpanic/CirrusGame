using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Helpers
{
    public static class Drawing
    {
        public static void DrawRectangle(SpriteBatch spriteBatch,Rectangle rect, Color col)
        {
            Texture2D tex = new Texture2D(spriteBatch.GraphicsDevice,rect.Width,rect.Height);

            Color[] drawn = new Color[rect.Width * rect.Height];

            for(int pixel = 0; pixel < drawn.Count(); pixel++)
            {
                drawn[pixel] = col;
            }

            tex.SetData(drawn);

            spriteBatch.Draw(tex,new Vector2(rect.X,rect.Y),col);
        }
    }
}
