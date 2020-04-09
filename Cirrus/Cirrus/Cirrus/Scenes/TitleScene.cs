using Cirrus.Cirrus.Backgrounds;
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
        public Texture2D Logo;

        public TitleScene(GameRunner _game) : base(_game)
        {
            Background blue = new Background(Sprites.GetSprite("spr_blue_back"), new Vector2(0, 180), new Vector2(0, 0), true, false);
            Background cloud = new Background(Sprites.GetSprite("bk_clouds"),new Vector2(0,0),new Vector2(2,0),true,false);
            Backgrounds = new Background[] {blue,cloud};

            Logo = Sprites.GetSprite("spr_cirrus_logo");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.GraphicsDevice.Clear(new Color(166, 224, 245, 255));
            BackgroundsDraw(spriteBatch,new Point(320,180),Vector2.One);
        }

        public override void DrawGui(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //Draw Logo
            spriteBatch.Draw(Logo,new Vector2(Screen.GameWidth/2,Screen.GameHeight/4) - new Vector2(Logo.Width/2,Logo.Height/2),Color.White);
            menuPosScale = MathHelper.Lerp(menuPosScale,1.5f,0.08f);

            for(int i = 0; i < MenuItems.Count(); i++)
            {
                if(MenuPos != i)
                {
                    Vector2 pos = new Vector2(Screen.GameWidth/2,Screen.GameHeight/2);
                    pos.Y += (16 * i) + MathMore.SinWave(-4,4,4,(float)i/4.0f,SinTimer);
                    Drawing.DrawTextExtShadow(spriteBatch,baseGame.BasicFont,MenuItems[i],pos,TextAlign.Center,TextAlign.Center,Vector2.One,Color.White, new Color(24, 20, 37, 128));
                }
                else
                {
                    Vector2 pos = new Vector2(Screen.GameWidth / 2, Screen.GameHeight / 2);
                    pos.Y += (16 * i) + MathMore.SinWave(-4, 4, 4, (float)i / 4.0f, SinTimer);
                    Vector2 Scale = (Vector2.One * MathMore.SinWave(0.9f,1.1f,4, (float)i / 4.0f,SinTimer)) * menuPosScale;
                    Drawing.DrawTextExtShadow(spriteBatch, baseGame.BasicFont, MenuItems[i], pos, TextAlign.Center, TextAlign.Center, Scale, Color.White, new Color(24, 20, 37, 128));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            BackgroundsUpdate(gameTime, Vector2.One);
            SinTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * 60;
            
            if (baseGame.input.GetKeyPressed(Keys.Up))
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
