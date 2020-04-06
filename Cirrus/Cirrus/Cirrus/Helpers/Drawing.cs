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

        public static void DrawTextExt(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, TextAlign hori, TextAlign vert, Vector2 Scale, Color col)
        {
            Vector2 size = font.MeasureString(text);
            Vector2 offset = new Vector2();

            switch(hori)
            {
                case TextAlign.Left: break;
                case TextAlign.Center: offset.X = size.X/2; break;
                case TextAlign.Right: offset.X = size.X; break;
            }

            switch (vert)
            {
                case TextAlign.Left: break;
                case TextAlign.Center: offset.Y = size.Y / 2; break;
                case TextAlign.Right: offset.Y = size.Y; break;
            }

            offset *= Scale;

            spriteBatch.DrawString(font, text, position - offset, col, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
        }
    }

    public enum TextAlign
    {
        Left = 0,
        Center = 1,
        Right = 2
    }
}
