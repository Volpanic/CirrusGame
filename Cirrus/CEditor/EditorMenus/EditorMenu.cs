using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Num = System.Numerics;
using ImGuiNET;
using CEditor;

namespace Editor.EditorMenus
{
    public abstract class EditorMenu
    {
        public ImGuiRenderer _imGuiRenderer;
        public EditorGame _game;

        public EditorMenu(ImGuiRenderer _imgr, EditorGame _gme)
        {
            _imGuiRenderer = _imgr;
            _game = _gme;
        }

        public abstract void ImGuiLayout();

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
    }
}
