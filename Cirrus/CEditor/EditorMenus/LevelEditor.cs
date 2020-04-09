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
using Cirrus.Cirrus.Backgrounds;

namespace Editor.EditorMenus
{
    public class LevelEditor : EditorMenu
    {
        //All
        public Point MapSize;
        public string MapName;
        public Color MapClearCol;

        public bool RunningGame = false;

        //GameView stuff
        public GameRunner gameRunner;
        public RenderTarget2D gameRenderTarget; // Game View
        public RenderTarget2D levelRenderTarget; //Place View
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
            ImGui.GetIO().Framerate = 60;

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
            //MainTab
            if (ImGui.BeginMainMenuBar())
            {
                if(ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Save")) _game.Exit();
                    ImGui.MenuItem("Load");
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("View"))
                {
                    ImGui.Checkbox("Editor Tile Grid",ref viewTileGrid);
                    ImGui.EndMenu();
                }

                if (RunningGame)
                {
                    if (ImGui.MenuItem("Stop Run"))
                    { 
                        RunningGame = false;
                        gameRunner = null;
                    }
                }
                else if (ImGui.MenuItem("Run"))
                {
                    gameRunner = new GameRunner(_game._graphics,_game.spriteBatch,_game.Content);
                    LevelScene ls = new LevelScene(gameRunner, TileLayers);
                    ls.Backgrounds = BackgroundList.ToArray();
                    gameRunner.CurrentScene = ls;

                    RunningGame = true;
                }

                ImGui.EndMainMenuBar();
            }

            if (!RunningGame)
            {
                TilePallateWindow();
                //Tile Layers window
                if (ImGui.BeginTabBar("Layers"))
                {
                    TileLayerWindow();
                    BackgroundTab();
                    ImGui.EndTabBar();
                }


                GameWindow();
            }
            else
            {
                ImGui.Begin("GameWindow",ref RunningGame,ImGuiWindowFlags.AlwaysAutoResize);

                ImGui.Image(levelTexture,new Num.Vector2(Screen.GameWidth,Screen.GameHeight)*3);
               
                ImGui.End();
            }
        }

        bool NewLayerWindow = false;
        int LayerListPosition = 0;
        string newTileName = "Temp";

        #region //Tile Pallate Window
        public Num.Vector2 tilePallateMousePos;
        public int tileSetZoom = 2;
        public Point[,] SelectedTileBrush = new Point[1, 1];
        public Point selectedTopLeft;
        public Point selectedTopRight;
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

                int xdif = selectedTopRight.X - selectedTopLeft.X;
                int ydif = selectedTopRight.Y - selectedTopLeft.Y;
                xdif = Math.Max(1, xdif);
                ydif = Math.Max(1, ydif);

                Rectangle tileSelectedRect = new Rectangle((int)((selectedTopLeft.X * 16 * tileSetZoom) + imageTopLeft.X), (int)((selectedTopLeft.Y * 16 * tileSetZoom) + imageTopLeft.Y), (16 * tileSetZoom) * xdif, (16 * tileSetZoom) * ydif);
                Prim.DrawRectangle(tileSelectedRect, Color.Lime);

                //Select Rectangle of tiles
                Num.Vector2 flatMousePos = ImGui.GetMousePos();
                if (MathMore.PointInRectangle(new Vector2(flatMousePos.X, flatMousePos.Y), tilePallateRect))
                {
                    Num.Vector2 tilePos = (tilePallateMousePos / (16 * tileSetZoom));

                    Rectangle tileCursorRect = new Rectangle((int)((selectedTopLeft.X*16*tileSetZoom) + imageTopLeft.X), (int)((selectedTopLeft.Y*16 * tileSetZoom) + imageTopLeft.Y), (16 * tileSetZoom) * xdif, (16 * tileSetZoom) * ydif);
                    Prim.DrawRectangle(tileCursorRect, Color.Maroon);

                    if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selectedTopLeft = new Point((int)tilePos.X, (int)tilePos.Y);
                    }
                    else if(ImGui.IsMouseDown(ImGuiMouseButton.Left))
                    {
                        selectedTopRight = new Point((int)tilePos.X+1, (int)tilePos.Y+1);
                    }
                    else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                    {
                        
                        SelectedTileBrush = new Point[xdif,ydif];

                        for (int xx = 0; xx < xdif; xx++)
                        {
                            for (int yy = 0; yy < ydif; yy++)
                            {
                                SelectedTileBrush[xx, yy] = new Point(xx + selectedTopLeft.X,yy + selectedTopLeft.Y);
                            }
                        }

                    }

                }
                //ImGui.Text($"Tile Pos: {SelectedTile.ToString()}");

                ImGui.End();
            }
        }
        #endregion

        #region //TileLayerWindow
        int tileLayerSelectedSprite = 0;
        public void TileLayerWindow()
        {
            if(ImGui.BeginTabItem("Layers"))
            {
                
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
                            tilePallateTexture = _imGuiRenderer.BindTexture(TileLayers[i].tileSet.TileSetTexture);
                            tilePallateTextureSize = new Point(TileLayers[i].tileSet.TileSetTexture.Width, TileLayers[i].tileSet.TileSetTexture.Height);
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

                    if(ImGui.TreeNodeEx(tswl.Name))
                    {
                        ImGui.InputText("Name", ref tswl.Name,32,i == 0? ImGuiInputTextFlags.ReadOnly : ImGuiInputTextFlags.None);
                        ImGui.Checkbox("Visible",ref tswl.Visible);
                        ImGui.Checkbox("Locked", ref tswl.Locked);
                        ImGui.TreePop();
                    }

                    TileLayers[i] = tswl;
                }

                ImGui.EndTabItem();
            }

            if (NewLayerWindow)
            {
                //ImGui.SetNextWindowFocus();
                ImGui.Begin("New Tile Layer");

                //Put Tile set stuff here later
                ImGui.InputText("Layer Name", ref newTileName, 24,ImGuiInputTextFlags.CharsNoBlank);

                if (ImGui.BeginCombo("Sprite", Sprites.SpriteList.Keys[tileLayerSelectedSprite]))
                {
                    for (int i = 0; i < Sprites.SpriteList.Count; i++)
                    {
                        bool selected = (tileLayerSelectedSprite == i);
                        if (ImGui.Selectable(Sprites.SpriteList.Keys[i], selected))
                        {
                            tileLayerSelectedSprite = i;
                        }

                        if (selected)
                        {
                            ImGui.SetItemDefaultFocus();
                        }
                    }
                    ImGui.EndCombo();
                }

                if (ImGui.Button("Add Layer"))
                {
                    TileSet tmp = new TileSet(MapSize);
                    tmp.TileSetTexture = Sprites.SpriteList.Values[tileLayerSelectedSprite];

                    TileLayers.Add(new TilesetWorldLayer(tmp, newTileName));
                    newTileName = "Temp";
                    NewLayerWindow = false;
                }


                ImGui.End();
            }
        }
        #endregion

        public List<Background> BackgroundList = new List<Background>();
        public bool PlayBackgroundUpdate = false;
        public bool MouseIsParralaxPos = false;
        public void BackgroundTab()
        {
            if (ImGui.BeginTabItem("Backgrounds"))
            {
                
                if(ImGui.Button("Add new Background")) BackgroundList.Add(new Background(Sprites.GetSprite("spr_cursor"),Vector2.Zero,Vector2.Zero));
                
                ImGui.SameLine();
                //Update
                if(PlayBackgroundUpdate)
                {
                    if(ImGui.Button("Stop Background"))
                    {
                        PlayBackgroundUpdate = false;
                        foreach (Background bk in BackgroundList)
                        {
                            bk.Position = Vector2.Zero;
                        }
                    }
                }
                else
                {
                    if (ImGui.Button("Play Background"))
                    {
                        PlayBackgroundUpdate = true;
                    }
                }
                ImGui.SameLine();
                ImGui.Checkbox("Mouse Parralax", ref MouseIsParralaxPos);

                ImGui.Separator();

                for (int i = 0; i < BackgroundList.Count; i++)
                {
                    if(ImGui.TreeNode(BackgroundList[i].BackgroundSprite.Name))
                    {
                        Background bk = BackgroundList[i];

                        //Draw Sprite List
                        if(ImGui.BeginCombo("Background Sprite",bk.BackgroundSprite.Name))
                        {
                            for (int b = 0; b < Sprites.SpriteList.Count; b++)
                            {
                                if (ImGui.Selectable(Sprites.SpriteList.Keys[b]))
                                {
                                    bk.BackgroundSprite = Sprites.SpriteList.Values[b];
                                }
                            }
                            ImGui.EndCombo();
                        }
                        ImGui.Checkbox("xTile",ref bk.xTile);
                        ImGui.Checkbox("yTile", ref bk.yTile);

                        //Position Speed
                        Num.Vector2 pos = new Num.Vector2(bk.Position.X, bk.Position.Y);
                        ImGui.SliderFloat2("Position", ref pos, 0, 0);
                        bk.Position = new Vector2(pos.X, pos.Y);

                        //Scroll Speed
                        Num.Vector2 spd = new Num.Vector2(bk.ScrollSpeed.X, bk.ScrollSpeed.Y);
                        ImGui.SliderFloat2("Speed",ref spd, -5,5);
                        bk.ScrollSpeed = new Vector2(spd.X, spd.Y);

                        //Parralax Speed
                        Num.Vector2 parra = new Num.Vector2(bk.ParralxMulit.X, bk.ParralxMulit.Y);
                        ImGui.SliderFloat2("Parralax", ref parra, -2, 2);
                        bk.ParralxMulit = new Vector2(parra.X, parra.Y);



                        BackgroundList[i] = bk;

                        ImGui.TreePop();
                    }
                }

                ImGui.EndTabItem();
            }
        }

        public bool viewTileGrid = true;
        public Num.Vector2 editorMousePos;
        public int PlaceMode = 0; // 0 = pen | 1 = rectangle | 2 = fill?
        public Point rectPlaceTopLeft = Point.Zero;
        public Point rectPlaceBotRight = Point.Zero;
        public void GameWindow()
        {
            {
                ImGui.Begin("Level Window");
                //
                ImGui.BeginChild("Level batch",Num.Vector2.Zero,true,ImGuiWindowFlags.AlwaysHorizontalScrollbar);

                //TileTools
                ImGui.BeginGroup();

                if (ImGui.Button("Pencil")) PlaceMode = 0;
                ImGui.SameLine();
                if (ImGui.Button("Rectangle")) PlaceMode = 1;
                ImGui.SameLine();
                if (ImGui.Button("Fill (Unimplemented)")) PlaceMode = 2;
                ImGui.SliderInt("GameZoom", ref GameZoom, 1, 4);

                ImGui.EndGroup();

                    ImGui.Image(levelTexture, new Num.Vector2((MapSize.X * 16) * GameZoom,(MapSize.Y * 16) * GameZoom));
                    //Get Top left of image
                    Num.Vector2 editorTopLeft = ImGui.GetItemRectMin();
                    Rectangle editorRect = new Rectangle((int)(editorTopLeft.X), (int)(editorTopLeft.Y),(int)(MapSize.X * 16) * GameZoom, (int)((MapSize.Y * 16) * GameZoom));
                    Prim.DrawRectangle(editorRect, Color.Black);
                    if (viewTileGrid)
                    {
                        Prim.DrawGrid(editorRect, (16 * GameZoom), (16 * GameZoom), new Color(0, 0, 0, 128));
                    }
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
                        switch (PlaceMode)
                        {
                            case 0: // Pen
                            {
                                #region // PenTool
                                Rectangle editorCursorRect = new Rectangle((int)(editorMousePos.X + editorRect.X), (int)(editorMousePos.Y + editorRect.Y), (SelectedTileBrush.GetLength(0) * 16) * GameZoom, (SelectedTileBrush.GetLength(1) * 16) * GameZoom);
                                Prim.DrawRectangle(editorCursorRect, Color.Maroon);

                                //Click Tile
                                if (ImGui.IsMouseDown(ImGuiMouseButton.Left))
                                {
                                    Num.Vector2 gridPos = (editorMousePos / (16 * GameZoom));

                                    //Draw rect
                                    for (int xx = (int)gridPos.X; xx < (int)gridPos.X + SelectedTileBrush.GetLength(0); xx++)
                                    {
                                        for (int yy = (int)gridPos.Y; yy < (int)gridPos.Y + SelectedTileBrush.GetLength(1); yy++)
                                        {
                                            int bX = (int)(xx - (int)gridPos.X);
                                            int bY = (int)(yy - (int)gridPos.Y);

                                            Point tileP = SelectedTileBrush[bX, bY];

                                            TileLayers[LayerListPosition].tileSet.SetGridPosition(new Point(xx, yy), tileP);
                                        }
                                    }

                                }

                                if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
                                {
                                    Num.Vector2 gridPos = (editorMousePos / (16 * GameZoom));
                                    //EraseRect
                                    for (int xx = (int)gridPos.X; xx < (int)gridPos.X + SelectedTileBrush.GetLength(0); xx++)
                                    {
                                        for (int yy = (int)gridPos.Y; yy < (int)gridPos.Y + SelectedTileBrush.GetLength(1); yy++)
                                        {

                                            TileLayers[LayerListPosition].tileSet.SetGridPosition(new Point(xx, yy), Point.Zero);
                                        }
                                    }
                                }
                                #endregion // EndPenTool
                                break;
                            }

                            case 1:
                            {
                                #region // RectangleTool
                                Num.Vector2 gridPos = (editorMousePos / (16 * GameZoom));

                                if (ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                                {
                                    rectPlaceTopLeft = new Point((int)gridPos.X, (int)gridPos.Y);
                                    rectPlaceBotRight = new Point((int)gridPos.X + 1, (int)gridPos.Y + 1);
                                }
                                else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
                                {
                                    rectPlaceBotRight = new Point((int)gridPos.X + 1, (int)gridPos.Y + 1);

                                    //order
                                    Point tl = new Point(Math.Min(rectPlaceBotRight.X,rectPlaceTopLeft.X), Math.Min(rectPlaceBotRight.Y, rectPlaceTopLeft.Y));
                                    Point rl = new Point(Math.Max(rectPlaceBotRight.X, rectPlaceTopLeft.X), Math.Max(rectPlaceBotRight.Y, rectPlaceTopLeft.Y));

                                    rectPlaceTopLeft = tl;
                                    rectPlaceBotRight = rl;

                                    //Place
                                    int xdif = rectPlaceBotRight.X - rectPlaceTopLeft.X;
                                    int ydif = rectPlaceBotRight.Y - rectPlaceTopLeft.Y;
                                    xdif = Math.Max(1, xdif);
                                    ydif = Math.Max(1, ydif);

                                    for (int xx = rectPlaceTopLeft.X; xx < rectPlaceTopLeft.X + xdif; xx++)
                                    {
                                        for (int yy = rectPlaceTopLeft.Y; yy < rectPlaceTopLeft.Y + ydif; yy++)
                                        {
                                            int bX = (int)(xx - (int)rectPlaceTopLeft.X) % SelectedTileBrush.GetLength(0);
                                            int bY = (int)(yy - (int)rectPlaceTopLeft.Y) % SelectedTileBrush.GetLength(1);
                                        
                                        

                                            Point tileP = SelectedTileBrush[bX,bY];

                                            TileLayers[LayerListPosition].tileSet.SetGridPosition(new Point(xx, yy), tileP);
                                        }
                                    }


                                }
                                else if (ImGui.IsMouseDown(ImGuiMouseButton.Left))
                                {
                                    rectPlaceBotRight = new Point((int)gridPos.X+1, (int)gridPos.Y+1);

                                    int mgic = 16 * GameZoom;
                                    Point tl = new Point((int)editorTopLeft.X + (rectPlaceTopLeft.X * mgic), (int)editorTopLeft.Y + (rectPlaceTopLeft.Y * mgic));
                                    Rectangle rectRect = new Rectangle(tl, new Point((rectPlaceBotRight.X - rectPlaceTopLeft.X)* mgic, (rectPlaceBotRight.Y - rectPlaceTopLeft.Y)* mgic));

                                    Prim.DrawRectangle(rectRect,Color.Maroon);
                                }
                                else
                                {
                                    Rectangle editorCursorRect = new Rectangle((int)(editorMousePos.X + editorRect.X), (int)(editorMousePos.Y + editorRect.Y), (SelectedTileBrush.GetLength(0) * 16) * GameZoom, (SelectedTileBrush.GetLength(1) * 16) * GameZoom);
                                    Prim.DrawRectangle(editorCursorRect, Color.Maroon);
                                }

                                #endregion
                                break;
                            }
                        }
                    }

                ImGui.EndChild();

                //
                ImGui.End();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!RunningGame)
            {
                _game.GraphicsDevice.SetRenderTarget(levelRenderTarget);
                _game.GraphicsDevice.Clear(MapClearCol);

                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, _game.PointWrap);
 
                Background.DrawBackgroundArray(ref BackgroundList,spriteBatch,new Point(MapSize.X*16,MapSize.Y*16), MouseIsParralaxPos ? new Vector2(editorMousePos.X, editorMousePos.Y) : Vector2.Zero);

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
            else // Game Running
            {
                gameRunner.Draw(gameTime,ref gameRenderTarget);

                levelTexture = _imGuiRenderer.BindTexture(gameRenderTarget);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(RunningGame) // GameRunning
            {
                gameRunner.Update(gameTime);
            }
            else
            {
                if(PlayBackgroundUpdate)
                {
                    Background.UpdateBackgroundArray(ref BackgroundList,gameTime,MouseIsParralaxPos? new Vector2(editorMousePos.X, editorMousePos.Y) : Vector2.Zero);
                }
            }
        }
    }
}
