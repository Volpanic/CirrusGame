using Cirrus.Cirrus;
using Cirrus.Cirrus.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cirrus
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Screen screen;

        private Scene runningScene;
        public Scene CurrentScene;

        public Texture2D Dummy;
        public TestCom anim;

        //RenderTargets
        RenderTarget2D ApplicationSurface;
        RenderTarget2D GuiSurface;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            screen = new Screen(graphics);
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Init Surfaces
            ApplicationSurface = new RenderTarget2D(GraphicsDevice, Screen.GameWidth, Screen.GameHeight);
            GuiSurface = new RenderTarget2D(GraphicsDevice, Screen.GameWidth, Screen.GameHeight);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Dummy = Content.Load<Texture2D>("Sprites/spr_testAnim");

            
        }

        protected override void UnloadContent()
        {
            Dummy.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if(anim == null)
            {
                anim = new TestCom(Dummy);
            }

            //Regular Draw
            GraphicsDevice.SetRenderTarget(ApplicationSurface);
            spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.AlphaBlend,SamplerState.PointClamp);

            //Clear
            GraphicsDevice.Clear(Color.Black);

            anim.Draw(spriteBatch,gameTime);

            spriteBatch.End();

            //Regular Draw
            GraphicsDevice.SetRenderTarget(GuiSurface);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

            //Clear
            GraphicsDevice.Clear(Color.Transparent);

            //runningScene.DrawGui(spriteBatch, gameTime);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            spriteBatch.Draw(ApplicationSurface, new Rectangle(0, 0, Screen.GameWidth * Screen.GameZoom, Screen.GameHeight * Screen.GameZoom), new Rectangle(0, 0, Screen.GameWidth, Screen.GameHeight), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.Draw(GuiSurface, new Rectangle(0, 0, Screen.GameWidth * Screen.GameZoom, Screen.GameHeight * Screen.GameZoom), new Rectangle(0, 0, Screen.GameWidth, Screen.GameHeight), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
