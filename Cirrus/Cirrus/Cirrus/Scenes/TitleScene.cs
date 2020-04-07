using Cirrus.Cirrus.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Scenes
{
    class TitleScene : Scene
    {
        public int MenuPos = 0;
        public string[] MenuItems = new string[] { "Start Game", "Load Game", "Options", "Exit" };
        public float menuPosScale = 0.5f;
        public float SinTimer = 0;

        public TitleScene(GameRunner _game) : base(_game)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(166,224,245,255));
        }

        public override void DrawGui(SpriteBatch spriteBatch, GameTime gameTime)
        {

            menuPosScale = MathHelper.Lerp(menuPosScale,1.5f,0.08f);

            for(int i = 0; i < MenuItems.Count(); i++)
            {
                if(MenuPos != i)
                {
                    Vector2 pos = new Vector2(Screen.GameWidth/2,Screen.GameHeight/2);
                    pos.Y += (16 * i) + MathMore.SinWave(-4,4,4,(float)i/4.0f,SinTimer);
                    Drawing.DrawTextExt(spriteBatch,baseGame.BasicFont,MenuItems[i],pos,TextAlign.Center,TextAlign.Center,Vector2.One,Color.White);
                }
                else
                {
                    Vector2 pos = new Vector2(Screen.GameWidth / 2, Screen.GameHeight / 2);
                    pos.Y += (16 * i) + MathMore.SinWave(-4, 4, 4, (float)i / 4.0f, SinTimer);
                    Vector2 Scale = (Vector2.One * MathMore.SinWave(0.9f,1.1f,4, (float)i / 4.0f,SinTimer)) * menuPosScale;
                    Drawing.DrawTextExt(spriteBatch, baseGame.BasicFont, MenuItems[i], pos, TextAlign.Center, TextAlign.Center, Scale, Color.White);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            SinTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * 60;

            if(baseGame.input.GetKeyPressed(Keys.Up))
            {
                MenuPos--;
                menuPosScale = 0.5f;
            }

            if(baseGame.input.GetKeyPressed(Keys.Down))
            {
                MenuPos++;
                menuPosScale = 0.5f;
            }

            MenuPos = MathMore.WrapValue(MenuPos,0,MenuItems.Count());
        }
    }
}
