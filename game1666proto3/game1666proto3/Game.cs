/***
 * game1666proto3: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto3
{
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### CONSTANTS ####################
		#region

		private const float GRID_SIZE = 5f;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		private BasicEffect m_basicEffect;
		private City m_city;
		private readonly GraphicsDeviceManager m_graphics;
		private IViewEntity m_viewer;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and window, and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphics = new GraphicsDeviceManager(this);
			if(!GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphics.GraphicsProfile = GraphicsProfile.Reach;
			}
			m_graphics.PreferredBackBufferWidth = 640;
			m_graphics.PreferredBackBufferHeight = 480;

			this.IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Set up the view matrix.
			m_basicEffect.View = Matrix.CreateLookAt(new Vector3(5, -20, 20), new Vector3(5, 20, 0), new Vector3(0, 0, 1));

			// Set up the world matrix.
			m_basicEffect.World = Matrix.Identity;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Draw the city.
			m_viewer.Draw();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Set up the basic effect.
			m_basicEffect = new BasicEffect(GraphicsDevice);
			
			// Set up the projection matrix.
			m_basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)m_graphics.PreferredBackBufferWidth / m_graphics.PreferredBackBufferHeight, 0.1f, 1000.0f);

			// Set up the city.
			var terrainHeightmap = new float[,]
			{
				{1,2,2,1},
				{1,1,1,1},
				{1,1,1,1},
				{2,1,1,2},
				{3,2,2,3},
				{4,2,2,4},
				{4,2,2,4},
				{4,2,2,4}
			};
			m_city = new City(new TerrainMesh(terrainHeightmap, GRID_SIZE, GRID_SIZE));

			// Set up the viewer.
			m_viewer = new CityViewer(m_city, GraphicsDevice.Viewport);

			// Fill in the RenderingDetails static class as a global point of access for the graphics device, basic effect and content.
			RenderingDetails.BasicEffect = m_basicEffect;
			RenderingDetails.Content = Content;
			RenderingDetails.GraphicsDevice = GraphicsDevice;

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Pre-load content.
			Content.Load<Texture2D>("landscape");
		}

		/// <summary>
		/// Called once per game to allow content to be unloaded.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if(Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			MouseEventManager.Update();

			base.Update(gameTime);
		}

		#endregion
	}
}
