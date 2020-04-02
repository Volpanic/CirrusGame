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

namespace Editor.EditorMenus
{
    public class StartMenu : EditorMenu
    {
        public Point MinScreenSize;

        public StartMenu(ImGuiRenderer _imgr, EditorGame _gme) : base(_imgr, _gme)
        {
            MinScreenSize = new Point((int)Math.Ceiling((double)Screen.GameWidth / 16),(int)Math.Ceiling((double)Screen.GameHeight / 16));
        }

        public override void ImGuiLayout()
        {

            ImGui.Begin("Start Menu");

            ImGui.Text("Level Editor");

            if(ImGui.Button("New Map"))
            {
                newMapWindowActive = true;
                mapName = "Blank";
                mapWidth = MinScreenSize.X;
                mapHeight = MinScreenSize.Y;
            }
            ImGui.Button("Load Map");

            ImGui.Separator();
            ImGui.Text("Tiles");

            ImGui.Button("New Tileset");
            ImGui.Button("Edit Tileset");
            ImGui.Button("AutoTile Maker");

            ImGui.End();

            if(newMapWindowActive)
            {
                
                NewMap();
            }
        }

        bool newMapWindowActive = false;
        int mapWidth = 16;
        int mapHeight = 9;
        string mapName = "Blank";
        Num.Vector3 clearColour = new Num.Vector3(24.0f/255,12.0f/255,48.0f/255);

        public void NewMap()
        {
            ImGui.Begin("New Map");

            ImGui.InputText("Map Name(With spaces)",ref mapName, 32);
            ImGui.InputInt("Map Width",ref mapWidth);
            ImGui.InputInt("Map Height", ref mapHeight);
            ImGui.Text($"Game Size is {Screen.GameWidth} x {Screen.GameHeight}, One Screen of tile is {MinScreenSize.X} x {MinScreenSize.Y}. ({MinScreenSize.X*16}x{MinScreenSize.Y * 16})");
            ImGui.ColorEdit3("Clear Colour",ref clearColour);
            ImGui.NewLine();

            if(ImGui.Button("Create Map"))
            {
                LevelEditor newMap = new LevelEditor(_imGuiRenderer,_game,new Point(mapWidth,mapHeight),mapName,new Color(clearColour.X, clearColour.Y, clearColour.Z,255));
                _game.CurrentRunningMenu = newMap;
            }

            ImGui.End();
        }
    }
}
