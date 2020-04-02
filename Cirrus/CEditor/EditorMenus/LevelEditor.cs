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

        //GameView stuff
        public LevelScene levelScene = new LevelScene(null);
        public RenderTarget2D gameRenderTarget; // Game View
        public RenderTarget2D levelRenderTarget; //Place View
        private IntPtr gameTexture;
        private float GameZoom = 1;

        //TileView
        public IntPtr tilePallateTexture;
        public Point tilePallateTextureSize;
        public List<TilesetWorldLayer> TileLayers;

        public LevelEditor(ImGuiRenderer _imgr,Game _gme) : base(_imgr, _gme)
        {
            gameRenderTarget = new RenderTarget2D(_gme.GraphicsDevice,Screen.GameWidth,Screen.GameHeight);
            levelRenderTarget = new RenderTarget2D(_gme.GraphicsDevice, Screen.GameWidth*4, Screen.GameHeight*4);

            TileLayers = levelScene.TileSetList;

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

                ImGui.PushItemWidth(-1);
                ImGui.ListBoxHeader(" ", TileLayers.Count, 8);

                //Draw List box
                for (int i = 0; i < TileLayers.Count; i++)
                {
                    TilesetWorldLayer tswl = TileLayers[i];

                    //Lock Layer
                    ImGuiSelectableFlags sel = ImGuiSelectableFlags.AllowDoubleClick;
                    if (tswl.Locked) sel = ImGuiSelectableFlags.Disabled;

                    if(ImGui.Selectable(tswl.Name, i == LayerListPosition, sel, new Num.Vector2(128, 24)))
                    {
                        LayerListPosition = i;
                    }

                    ImGui.SameLine(); ImGui.Checkbox("Vis ", ref tswl.Visible);
                    ImGui.SameLine(); ImGui.Checkbox("Lock", ref tswl.Locked);
                    ImGui.NewLine();
                    TileLayers[i] = tswl;
                }

                ImGui.ListBoxFooter();

                ImGui.End();
            }

            if (NewLayerWindow)
            {
                ImGui.Begin("New Tile Layer");


                //Put Tile set stuff here later
                ImGui.InputText("Layer Name", ref newTileName, 24);
                if (ImGui.Button("Add Layer"))
                {
                    TileLayers.Add(new TilesetWorldLayer(new CollisionTileSet(), newTileName));
                    newTileName = "Temp";
                    NewLayerWindow = false;
                }


                ImGui.End();
            }
        }



        public void GameWindow()
        {
            {
                ImGui.Begin("Game Window");
                //
                
                ImGui.Image(gameTexture,new Num.Vector2(Screen.GameWidth * GameZoom, Screen.GameHeight * GameZoom));
                ImGui.SliderFloat("GameZoom",ref GameZoom,1,4);
                //
                ImGui.End();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            _game.GraphicsDevice.SetRenderTarget(gameRenderTarget);
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack,BlendState.AlphaBlend,SamplerState.PointClamp);

            levelScene.Draw(spriteBatch,gameTime);

            spriteBatch.End();

            _game.GraphicsDevice.SetRenderTarget(null);

            gameTexture = _imGuiRenderer.BindTexture(gameRenderTarget);
        }
    }
}
