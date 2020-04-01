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
        public Game _game;

        public EditorMenu(ImGuiRenderer _imgr, Game _gme)
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

        public static void DrawPrimRectangle(Rectangle rect)
        {
            Num.Vector2 bottemRight = new Num.Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            // -- //
            ImGui.GetWindowDrawList().AddLine(new Num.Vector2(rect.X, rect.Y), new Num.Vector2(bottemRight.X, rect.Y), Color.White.PackedValue);
            ImGui.GetWindowDrawList().AddLine(new Num.Vector2(rect.X, bottemRight.Y), new Num.Vector2(bottemRight.X, bottemRight.Y), Color.White.PackedValue);

            // || //
            ImGui.GetWindowDrawList().AddLine(new Num.Vector2(rect.X, rect.Y), new Num.Vector2(rect.X, bottemRight.Y), Color.White.PackedValue);
            ImGui.GetWindowDrawList().AddLine(new Num.Vector2(bottemRight.X, rect.Y), new Num.Vector2(bottemRight.X, bottemRight.Y), Color.White.PackedValue);
        }

    }
}
