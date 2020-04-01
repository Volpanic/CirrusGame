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

            //Level View window
            {
                ImGui.Begin("Level");
                
                ImGui.End();
            }

            
        }

        bool NewLayerWindow = false;
        int LayerListPosition = 0;
        string newTileName = "Temp";

        public Num.Vector2 tilePallateMousePos;
        public void TilePallateWindow()
        {
            //Tile Pallate Window
            {
                ImGui.Begin("Tiles");

                tilePallateMousePos = ImGui.GetMousePos() / 16;
                tilePallateMousePos.X = (int)Math.Floor(tilePallateMousePos.X);
                tilePallateMousePos.Y = (int)Math.Floor(tilePallateMousePos.Y);
                tilePallateMousePos *= 16;

                ImGui.Image(tilePallateTexture,new Num.Vector2(tilePallateTextureSize.X,tilePallateTextureSize.Y));
                ImGui.GetWindowDrawList();

                DrawPrimRectangle(new Rectangle((int)(tilePallateMousePos.X), (int)(tilePallateMousePos.Y), 16, 16));

                ImGui.Separator();
                ImGui.Text("Pen Type");
                ImGui.Button("Pencil");
                ImGui.Button("Rectangle");
                ImGui.Button("Fill");

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
