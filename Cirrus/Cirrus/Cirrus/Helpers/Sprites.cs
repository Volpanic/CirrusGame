using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cirrus.Cirrus.Helpers
{
    public static class Sprites
    {
        private static SortedDictionary<string, Texture2D> SpriteList = new SortedDictionary<string, Texture2D>();

        public static void Init(ContentManager _content)
        {
            string[] f = Directory.GetFiles(Path.Combine(_content.RootDirectory,"Sprites"));

            foreach(string name in f)
            {
                if (Path.GetExtension(name) == ".xnb")
                {
                    Texture2D tex = _content.Load<Texture2D>(Path.Combine("Sprites", Path.GetFileNameWithoutExtension(name)));
                    SpriteList.Add(Path.GetFileNameWithoutExtension(name),tex);
                    Console.WriteLine(Path.GetFileNameWithoutExtension(name));
                }
            }
        }

        public static void Unload(ContentManager _content)
        {
            foreach(KeyValuePair<string,Texture2D> tex in SpriteList)
            {
                tex.Value.Dispose();
            }
        }

        public static Texture2D GetSprite(string SpriteName)
        {
            if (SpriteList.ContainsKey(SpriteName))
            {
                return SpriteList[SpriteName];
            }

            return null;
        }
    }
}
