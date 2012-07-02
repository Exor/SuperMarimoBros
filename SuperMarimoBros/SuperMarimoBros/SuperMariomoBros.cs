using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaLibrary;
using System.IO;

namespace SuperMarimoBros
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SuperMariomoBros : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        FPS fps;
        AnimationHandler animations;
        InputHandler input;
        TileManager tileManager;

        Marimo marimo;
        Song levelOneOneMusic;
        Texture2D marioGraphics;

        public SuperMariomoBros()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 256;
            graphics.PreferredBackBufferHeight = 240;

#if DEBUG
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
#endif

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            fps = new FPS(this);
            this.Components.Add(fps);
            tileManager = new TileManager(this);
            this.Components.Add(tileManager);
            animations = new AnimationHandler(this);
            this.Components.Add(animations);
            input = new InputHandler(this);
            this.Components.Add(input);
            marimo = new Marimo();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            marimo.Load(Content.Load<Texture2D>("Graphics/mariospritesheet"), new Vector2(32,192), animations, input);
            marioGraphics = Content.Load<Texture2D>("Graphics/smbtiles");
            levelOneOneMusic = Content.Load<Song>("BackgroundMusic/OneOne");
            MediaPlayer.Play(levelOneOneMusic);

            StreamReader streamReader = new StreamReader("Levels/levelOneOne.txt");
            string level = streamReader.ReadToEnd();
            streamReader.Close();
            tileManager.CreateTiles(level, marioGraphics);
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            marimo.Update(gameTime);
            CollisionDetection();

            base.Update(gameTime);
        }

        private void CollisionDetection()
        {
            Point bottomLeft = new Point(marimo.BoundingRectangle().Left, marimo.BoundingRectangle().Bottom);
            Point bottomRight = new Point(marimo.BoundingRectangle().Right, marimo.BoundingRectangle().Bottom);
            Point topLeft = new Point(marimo.BoundingRectangle().Left, marimo.BoundingRectangle().Top);
            Point topRight = new Point(marimo.BoundingRectangle().Right, marimo.BoundingRectangle().Top);

            if (tileManager.SolidTileExistsAt(bottomLeft))
                marimo.CollidesWithTile(tileManager.ReturnTileAt(bottomLeft));
            if (tileManager.SolidTileExistsAt(bottomRight))
                marimo.CollidesWithTile(tileManager.ReturnTileAt(bottomRight));
            if (tileManager.SolidTileExistsAt(topLeft))
                marimo.CollidesWithTile(tileManager.ReturnTileAt(topLeft));
            if (tileManager.SolidTileExistsAt(topRight))
                marimo.CollidesWithTile(tileManager.ReturnTileAt(topRight));

        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            marimo.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
