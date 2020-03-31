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
    public class LevelEditor : EditorMenu
    {
        public LevelEditor(ImGuiRenderer _imgr,GameWindow _wind) : base(_imgr, _wind)
        {
        }

        bool NewLayerWindow = false;

        public int LayerListPosition = 0;

        public override void ImGuiLayout()
        {
            {
                ImGui.Begin("Tiles");
                //Put Tile set stuff here later
                ImGui.Separator();
                ImGui.Text("Pen Type");
                ImGui.Button("Pencil");
                ImGui.Button("Rectangle");
                ImGui.Button("Fill");

                ImGui.End();
            }

            {
                ImGui.Begin("Layers");
                bool vis = false;
                //Put Tile set stuff here later
                if (ImGui.Button("Add New Layer")) NewLayerWindow = true;
                ImGui.ListBox("",ref LayerListPosition, new string[] {"Collision" ,"PlaceHolder"},2);
                
                ImGui.SameLine(); ImGui.Checkbox("Visable",ref vis);

                ImGui.End();
            }

            if(NewLayerWindow)
            {
                ImGui.Begin("New Tile Layer");

                string name = "Temp";
                //Put Tile set stuff here later
                ImGui.InputText("Layer Name", ref name, 32);
                ImGui.Button("Add Layer");

                ImGui.End();
            }
        }
    }
}
