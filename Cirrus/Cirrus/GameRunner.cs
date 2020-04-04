using Cirrus.Cirrus.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus
{
    public class GameRunner
    {
        //RenderTargets
        RenderTarget2D ApplicationSurface;
        RenderTarget2D GuiSurface;

        //Scenes
        private Scene runningScene;
        public Scene CurrentScene;

        public Screen screen;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice GraphicsDevice { get{return graphics.GraphicsDevice; } }

        public GameRunner(GraphicsDeviceManager _graphics,SpriteBatch _spriteBatch,ContentManager _content)
        {
            graphics = _graphics;
            spriteBatch = _spriteBatch;

            screen = new Screen(graphics);

            ApplicationSurface = new RenderTarget2D(GraphicsDevice, Screen.GameWidth, Screen.GameHeight);
            GuiSurface = new RenderTarget2D(GraphicsDevice, Screen.GameWidth, Screen.GameHeight);

            runningScene = new LevelScene(this);
        }

        public void Unload()
        {
            ApplicationSurface.Dispose();
            GuiSurface.Dispose();
        }

        public void Update(GameTime gameTime)
        {
            runningScene.Update(gameTime);
        }

        public void Draw(GameTime gameTime, RenderTarget2D finalRenderTarget = null)
        {
            GraphicsDevice.Clear(Color.Black);

            //Regular Draw
            GraphicsDevice.SetRenderTarget(ApplicationSurface);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            //Clear
            GraphicsDevice.Clear(Color.DarkGray);

            runningScene.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            //Regular Draw
            GraphicsDevice.SetRenderTarget(GuiSurface);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            //Clear
            GraphicsDevice.Clear(Color.Transparent);

            //runningScene.DrawGui(spriteBatch, gameTime);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(finalRenderTarget);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            spriteBatch.Draw(ApplicationSurface, new Rectangle(0, 0, Screen.GameWidth * Screen.GameZoom, Screen.GameHeight * Screen.GameZoom), new Rectangle(0, 0, Screen.GameWidth, Screen.GameHeight), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(GuiSurface, new Rectangle(0, 0, Screen.GameWidth * Screen.GameZoom, Screen.GameHeight * Screen.GameZoom), new Rectangle(0, 0, Screen.GameWidth, Screen.GameHeight), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
