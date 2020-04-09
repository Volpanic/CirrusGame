﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Num = System.Numerics;
using ImGuiNET;
using Editor.EditorMenus;
using Cirrus.Cirrus.Helpers;
using Cirrus;

namespace CEditor
{
    public class EditorGame : Game
    {
        public GraphicsDeviceManager _graphics;
        public SpriteBatch spriteBatch;
        private ImGuiRenderer _imGuiRenderer;

        private Texture2D _xnaTexture;
        private IntPtr _imGuiTexture;

        public EditorMenu CurrentRunningMenu;

        public SamplerState PointWrap = new SamplerState();

        public EditorGame()
        {
            Window.AllowUserResizing = true;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferMultiSampling = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Load Content
            string basePath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            basePath = Path.GetFullPath(Path.Combine(basePath,@"..\"));
            Content.RootDirectory = Path.Combine(basePath, @"Cirrus\bin\DesktopGL\AnyCPU\Debug\Content");

            Sprites.Init(Content);

            //
            _imGuiRenderer = new ImGuiRenderer(this);
            _imGuiRenderer.RebuildFontAtlas();

            //Create Custom Sampler
            PointWrap.Filter = TextureFilter.Point;
            PointWrap.AddressU = TextureAddressMode.Wrap;
            PointWrap.AddressV = TextureAddressMode.Wrap;

            CurrentRunningMenu = new StartMenu(_imGuiRenderer,this);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Texture loading example

			// First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
			_xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
			{
				var red = (pixel % 300) / 2;
				return new Color(red, 1, 1);
			});

			// Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
			_imGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            Sprites.Unload(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentRunningMenu.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(clear_color.X, clear_color.Y, clear_color.Z));

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            CurrentRunningMenu.Draw(spriteBatch,gameTime);
            // Draw our UI
            CurrentRunningMenu.ImGuiLayout();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            base.Draw(gameTime);
        }

        private Num.Vector3 clear_color = new Num.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        private byte[] _textBuffer = new byte[100];

        protected virtual void ImGuiLayout()
        {
            ImGui.BeginMainMenuBar();

            ImGui.EndMainMenuBar();
            
            {
                ImGui.Begin("Base Editor");

                ImGui.Button("Level Editor");
                ImGui.Button("AutoTile Editor");
                ImGui.Button("Event  Editor");
                ImGui.Separator();

                ImGui.End();
            }


            //// 1. Show a simple window
            //// Tip: if we don't call ImGui.Begin()/ImGui.End() the widgets appears in a window automatically called "Debug"
            //{
            //    ImGui.Text("Hello, world!");
            //    ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty, 1f);
            //    ImGui.ColorEdit3("clear color", ref clear_color);
            //    if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
            //    if (ImGui.Button("Another Window")) show_another_window = !show_another_window;
            //    ImGui.Text(string.Format("Application average {0:F3} ms/frame ({1:F1} FPS)", 1000f / ImGui.GetIO().Framerate, ImGui.GetIO().Framerate));

            //    ImGui.InputText("Text input", _textBuffer, 100);

            //    ImGui.Text("Texture sample");
            //    ImGui.Image(_imGuiTexture, new Num.Vector2(300, 150), Num.Vector2.Zero, Num.Vector2.One, Num.Vector4.One, Num.Vector4.One); // Here, the previously loaded texture is used
            //}

            //// 2. Show another simple window, this time using an explicit Begin/End pair
            //if (show_another_window)
            //{
            //    ImGui.SetNextWindowSize(new Num.Vector2(200, 100), ImGuiCond.FirstUseEver);
            //    ImGui.Begin("Another Window", ref show_another_window);
            //    ImGui.Text("Hello, world!");
            //    ImGui.SliderFloat("float", ref f, 0.0f, 1.0f, string.Empty, 1f);
            //    ImGui.Text("Hello");
            //    ImGui.End();
            //}

            //// 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
            //if (show_test_window)
            //{
            //    ImGui.SetNextWindowPos(new Num.Vector2(650, 20), ImGuiCond.FirstUseEver);
            //    ImGui.ShowDemoWindow(ref show_test_window);
            //}
        }

		public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
		{
			//initialize a texture
			var texture = new Texture2D(device, width, height);

			//the array holds the color for each pixel in the texture
			Color[] data = new Color[width * height];
			for(var pixel = 0; pixel < data.Length; pixel++)
			{
				//the function applies the color according to the specified pixel
				data[pixel] = paint( pixel );
			}

			//set the color
			texture.SetData( data );

			return texture;
		}
	}
}