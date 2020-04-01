using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Cirrus
{
    public class Screen
    {
        //Statics
        public const int GameWidth = 320;
        public const int GameHeight = 180;
        public static int GameZoom = 4;
        //Statics

        public GraphicsDeviceManager graphicsDeviceM;

        public Screen(GraphicsDeviceManager _gd)
        {
            graphicsDeviceM = _gd;

            graphicsDeviceM.PreferredBackBufferWidth = GameWidth * GameZoom;
            graphicsDeviceM.PreferredBackBufferHeight = GameHeight * GameZoom;
            graphicsDeviceM.ApplyChanges();
        }

        public static Vector2 GetMousePos()
        {
            Point pt = Mouse.GetState().Position;
            return new Vector2(pt.X,pt.Y) / GameZoom;
        }
    }
}
