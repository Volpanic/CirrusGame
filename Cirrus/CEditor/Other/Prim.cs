using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


using Num = System.Numerics;
using ImGuiNET;
using CEditor;
using Cirrus;
using Cirrus.Cirrus.Tiles;
using Cirrus.Cirrus.Helpers;
using Cirrus.Cirrus.Scenes;
namespace Editor.Other
{
    public static class Prim
    {
        public static void DrawRectangle(Rectangle rect, Color color)
        {
            Num.Vector2 bottemRight = new Num.Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            ImGui.GetWindowDrawList().AddRect(new Num.Vector2(rect.X, rect.Y), new Num.Vector2(bottemRight.X, bottemRight.Y), color.PackedValue);

        }

        public static void DrawRectangleFilled(Rectangle rect, Color color)
        {
            Num.Vector2 bottemRight = new Num.Vector2(rect.X + rect.Width, rect.Y + rect.Height);

            ImGui.GetWindowDrawList().AddRectFilled(new Num.Vector2(rect.X, rect.Y), new Num.Vector2(bottemRight.X, bottemRight.Y), color.PackedValue);

        }

        public static void DrawGrid(Rectangle bounds,int cWidth, int cHeight, Color color)
        {
            for(int xx =  0; xx < cWidth; xx++)
            {
                if(bounds.X + (xx * cWidth) <= bounds.X + bounds.Width)
                {
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(bounds.X + (xx * cWidth),bounds.Y), new Num.Vector2(bounds.X + (xx * cWidth), bounds.Y + bounds.Height),color.PackedValue,2);
                }
            }

            for (int yy = 0; yy < cHeight; yy++)
            {
                if (bounds.Y + (yy * cHeight) <= bounds.Y + bounds.Height)
                {
                    ImGui.GetWindowDrawList().AddLine(new Num.Vector2(bounds.X, bounds.Y + (cHeight*yy)), new Num.Vector2(bounds.X + bounds.Width, bounds.Y + (cHeight * yy)), color.PackedValue,2);
                }
            }

        }
    }
}
