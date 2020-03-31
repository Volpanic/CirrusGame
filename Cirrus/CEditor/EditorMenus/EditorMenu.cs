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
        ImGuiRenderer _imGuiRenderer;
        public GameWindow _window;

        public EditorMenu(ImGuiRenderer _imgr, GameWindow _wind)
        {
            _imGuiRenderer = _imgr;
            _window = _wind;
        }

        public abstract void ImGuiLayout();

    }
}
