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
using Editor.Other;

namespace Editor.EditorMenus
{
    public class LevelEditor : EditorMenu
    {
        //All
        public Point MapSize;
        public string MapName;
        public Color MapClearCol;

        //GameView stuff
        public LevelScene levelScene = new LevelScene(null);
        public RenderTarget2D gameRenderTarget; // Game View
        public RenderTarget2D levelRenderTarget; //Place View
        private IntPtr gameTexture;
        private IntPtr levelTexture;
        private int GameZoom = 1;

        //TileView
        public IntPtr tilePallateTexture;
        public Point tilePallateTextureSize;
        public List<TilesetWorldLayer> TileLayers;

        public LevelEditor(ImGuiRenderer _imgr, EditorGame _gme, Point _mapSize, string _mapName, Color _mapClearCol) : base(_imgr, _gme)
        {
            //Map constraints
            MapSize = _mapSize;
            MapName = _mapName;
            MapClearCol = _mapClearCol;

            //(So drawing tiles doesnt draw the window)
            ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;

            //Configure layers
            TileLayers = new List<TilesetWorldLayer>();
            TileLayers.Add(new TilesetWorldLayer(new CollisionTileSet(MapSize),"Collision",true,false));
            LayerListPosition = 0;

            //Create rendertargets
            levelRenderTarget = new RenderTarget2D(_gme.GraphicsDevice, 16 * MapSize.X, 16 * MapSize.Y);
            gameRenderTarget = new RenderTarget2D(_gme.GraphicsDevice, Screen.GameWidth, Screen.GameHeight);
            //Set Tile Pallate textures
            tilePallateTexture = _imGuiRenderer.BindTexture(TileLayers[0].tileSet.TileSetTexture);
            tilePallateTextureSize = new Point(TileLayers[0].tileSet.TileSetTexture.Width, TileLayers[0].tileSet.TileSetTexture.Height);
        }

        

        public override void ImGuiLayout()
        {

            TilePallateWindow();
            //Tile Layers window
            TileLayerWindow();
            GameWindow();
        }

        bool NewLayerWindow = false;
        int LayerListPosition = 0;
        string newTileName = "Temp";

        public Num.Vector2 tilePallateMousePos;
        public int tileSetZoom = 2;
        public Point SelectedTile;
        public void TilePallateWindow()
        {
            //Tile Pallate Window
            {
                ImGui.Begin("Tiles");
                //Draw Tile set
                ImGui.Image(tilePallateTexture,new Num.Vector2(tilePallateTextureSize.X,tilePallateTextureSize.Y)* tileSetZoom);

                //Get top left of window and draw rect around texture
                Num.Vector2 imageTopLeft = ImGui.GetItemRectMin();
                Rectangle tilePallateRect = new Rectangle((int)(imageTopLeft.X), (int)(imageTopLeft.Y), (int)tilePallateTextureSize.X * tileSetZoom, (int)tilePallateTextureSize.Y * tileSetZoom);
                Prim.DrawRectangle(tilePallateRect, Color.Black);

                Prim.DrawGrid(tilePallateRect, (16 * tileSetZoom), (16 * tileSetZoom),Color.Black);

                //Allow zoom on tileset
                ImGui.InputInt("Zoom",ref tileSetZoom,1);
                tileSetZoom = MathHelper.Clamp(tileSetZoom,1,3);

                //MousePosition in tile pallate
                tilePallateMousePos = (ImGui.GetMousePos() - imageTopLeft);
                tilePallateMousePos /= (16*tileSetZoom);
                tilePallateMousePos.X = (int)Math.Floor(tilePallateMousePos.X);
                tilePallateMousePos.Y = (int)Math.Floor(tilePallateMousePos.Y);
                tilePallateMousePos *= (16 * tileSetZoom);

                Num.Vector2 flatMousePos = ImGui.GetMousePos();
                if (MathMore.PointInRectangle(new Vector2(flatMousePos.X, flatMousePos.Y), tilePallateRect))
                {
                    Rectangle tileCursorRect = new Rectangle((int)(tilePallateMousePos.X + imageTopLeft.X), (int)(tilePallateMousePos.Y + imageTopLeft.Y), 16 * tileSetZoom, 16 * tileSetZoom);
                    Prim.DrawRectangle(tileCursorRect, Color.Maroon);

                    //Click Tile
                    if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        Num.Vector2 tilePos = (tilePallateMousePos / (16 * tileSetZoom));
                        SelectedTile = new Point((int)tilePos.X, (int)tilePos.Y);
                    }
                }

                ImGui.Separator();
                ImGui.Text("Pen Type");
                ImGui.Button("Pencil");
                ImGui.Button("Rectangle");
                ImGui.Button("Fill");
                ImGui.Text($"Tile Pos: {SelectedTile.ToString()}");

                ImGui.End();
            }
        }

        public void TileLayerWindow()
        {
            {
                ImGui.Begin("Layers");
                if (ImGui.Button("Add New Layer")) NewLayerWindow = true;

                //Select layer to edit
                if (ImGui.BeginCombo("Selected Layer", TileLayers[LayerListPosition].Name))
                {
                    for (int i = 0; i < TileLayers.Count; i++)
                    {
                        bool selected = (LayerListPosition == i);
                        if (ImGui.Selectable(TileLayers[i].Name, selected))
                        {
                            LayerListPosition = i;
                        }

                        if (selected)
                        {
                            ImGui.SetItemDefaultFocus();
                        }
                    }
                    ImGui.EndCombo();
                }

                ImGui.Separator();
                ImGui.Text("Layers:");

                ImGui.PushItemWidth(-1);

                //Draw List box
                for (int i = 0; i < TileLayers.Count; i++)
                {
                    TilesetWorldLayer tswl = TileLayers[i];

                    if(ImGui.TreeNode(tswl.Name))
                    {
                        ImGui.InputText("Name", ref tswl.Name,32,i == 0? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
                        ImGui.Checkbox("Visible",ref tswl.Visible);
                        ImGui.Checkbox("Locked", ref tswl.Locked);
                        ImGui.TreePop();
                    }
                    

                    TileLayers[i] = tswl;
                }

                ImGui.End();
            }

            if (NewLayerWindow)
            {
                ImGui.Begin("New Tile Layer");


                //Put Tile set stuff here later
                ImGui.InputText("Layer Name", ref newTileName, 24);
                if (ImGui.Button("Add Layer"))
                {
                    TileLayers.Add(new TilesetWorldLayer(new CollisionTileSet(MapSize), newTileName));
                    newTileName = "Temp";
                    NewLayerWindow = false;
                }


                ImGui.End();
            }
        }

        public Num.Vector2 editorMousePos;
        public void GameWindow()
        {
            {
                ImGui.Begin("Level Window");
                //
                
                ImGui.Image(levelTexture, new Num.Vector2((MapSize.X * 16) * GameZoom,(MapSize.Y * 16) * GameZoom));
                //Get Top left of image
                Num.Vector2 editorTopLeft = ImGui.GetItemRectMin();
                Rectangle editorRect = new Rectangle((int)(editorTopLeft.X), (int)(editorTopLeft.Y),(int)(MapSize.X * 16) * GameZoom, (int)((MapSize.Y * 16) * GameZoom));
                Prim.DrawRectangle(editorRect, Color.Black);
                Prim.DrawGrid(editorRect, (16 * GameZoom), (16 * GameZoom), new Color(0,0,0,128));
                Prim.DrawGrid(editorRect, (Screen.GameWidth * GameZoom), (Screen.GameHeight * GameZoom), Color.Yellow);

                //MousePosition in tile pallate
                editorMousePos = (ImGui.GetMousePos() - editorTopLeft);
                editorMousePos /= (16 * GameZoom);
                editorMousePos.X = (int)Math.Floor(editorMousePos.X);
                editorMousePos.Y = (int)Math.Floor(editorMousePos.Y);
                editorMousePos *= (16 * GameZoom);

                Num.Vector2 flatMousePos = ImGui.GetMousePos();
                if (MathMore.PointInRectangle(new Vector2(flatMousePos.X, flatMousePos.Y), editorRect))
                {
                    Rectangle editorCursorRect = new Rectangle((int)(editorMousePos.X + editorRect.X), (int)(editorMousePos.Y + editorRect.Y), 16 * GameZoom, 16 * GameZoom);
                    Prim.DrawRectangle(editorCursorRect, Color.Maroon);

                    //Click Tile
                    if (ImGui.IsMouseDown(ImGuiMouseButton.Left))
                    {
                        Num.Vector2 gridPos = (editorMousePos / (16 * GameZoom));
                        TileLayers[LayerListPosition].tileSet.SetGridPosition(new Point((int)gridPos.X, (int)gridPos.Y), SelectedTile);
                    }

                    if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
                    {
                        Num.Vector2 gridPos = (editorMousePos / (16 * GameZoom));
                        TileLayers[LayerListPosition].tileSet.SetGridPosition(new Point((int)gridPos.X, (int)gridPos.Y), Point.Zero);
                    }
                }

                ImGui.SliderInt("GameZoom",ref GameZoom,1,4);
                //
                ImGui.End();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            _game.GraphicsDevice.SetRenderTarget(levelRenderTarget);
            _game.GraphicsDevice.Clear(MapClearCol);

            spriteBatch.Begin(SpriteSortMode.FrontToBack,BlendState.AlphaBlend,SamplerState.PointClamp);

            foreach (TilesetWorldLayer tswl in TileLayers)
            {
                if (tswl.Visible)
                {
                    tswl.tileSet.DrawTileSet(spriteBatch);
                }
            }

            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(null);

            levelTexture = _imGuiRenderer.BindTexture(levelRenderTarget);
        }
    }
}
