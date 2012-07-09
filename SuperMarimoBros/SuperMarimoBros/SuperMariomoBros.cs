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
using SuperMarimoBros;
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
        Animations animations;
        Textures textures;
        Input input;
        TileManager tileManager;
        Sounds soundManager;

        Marimo marimo;

        World world;

        SpriteFont font;
        String left;
        String right;

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
            input = new Input(this);
            this.Components.Add(input);
            soundManager = new Sounds();
            tileManager = new TileManager();
            animations = new Animations();
            textures = new Textures();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            soundManager.LoadSounds(Content);
            textures.LoadTextures(Content);

            marimo = new Marimo(new Vector2(32, 192), input);
            world = new World(tileManager, marimo);

            world.AddGameObject(marimo);

            Sounds.Play(Sounds.Music.overworld);

            font = Content.Load<SpriteFont>("myFont");

            StreamReader streamReader = new StreamReader("Levels/levelOneOne.txt");
            string level = streamReader.ReadToEnd();
            streamReader.Close();
            tileManager.CreateTiles(level, world);
            
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
            if (input.IsButtonPressed(Keys.Escape))
                this.Exit();
            if (input.WasButtonPressed(Keys.D1))
                Sounds.Play(Sounds.Music.overworldfast);


            //tileManager.Update(gameTime);

            world.Update(gameTime);

            animations.Update(gameTime);
            
            CollisionDetection();

            base.Update(gameTime);
        }

        private void CollisionDetection()
        {
            Point bottomLeft = new Point(marimo.BoundingRectangle().Left, marimo.BoundingRectangle().Bottom);
            Point bottomLeftPlusOne = new Point(marimo.BoundingRectangle().Left, marimo.BoundingRectangle().Bottom + 1);
            Point bottomRight = new Point(marimo.BoundingRectangle().Right, marimo.BoundingRectangle().Bottom);
            Point bottomRightPlusOne = new Point(marimo.BoundingRectangle().Right, marimo.BoundingRectangle().Bottom + 1);
            Point topLeft = new Point(marimo.BoundingRectangle().Left, marimo.BoundingRectangle().Top);
            Point topRight = new Point(marimo.BoundingRectangle().Right, marimo.BoundingRectangle().Top);

            if (tileManager.SolidTileExistsAt(bottomLeft))
                CollidesWithTile(tileManager.ReturnTileAt(bottomLeft));
            if (tileManager.SolidTileExistsAt(bottomRight))
                CollidesWithTile(tileManager.ReturnTileAt(bottomRight));
            if (tileManager.SolidTileExistsAt(topLeft))
                CollidesWithTile(tileManager.ReturnTileAt(topLeft));
            if (tileManager.SolidTileExistsAt(topRight))
                CollidesWithTile(tileManager.ReturnTileAt(topRight));

            if (!tileManager.SolidTileExistsAt(bottomLeftPlusOne) && !tileManager.SolidTileExistsAt(bottomRightPlusOne))
                marimo.ShouldFall();

            left = tileManager.SolidTileExistsAt(bottomLeftPlusOne).ToString();
            right = tileManager.SolidTileExistsAt(bottomRightPlusOne).ToString();

        }

        private void CollidesWithTile(Tile t)
        {
            Rectangle mario = marimo.BoundingRectangle();
            Rectangle tile = t.BoundingRectangle();
            Rectangle collision = Rectangle.Intersect(mario, tile);
            if (collision.Width < collision.Height)
            {
                if (mario.X > tile.X)
                {
                    marimo.OnSideCollision(collision.X + collision.Width);
                    t.OnSideCollision();
                }

                if (mario.X < tile.X)
                {
                    marimo.OnSideCollision(collision.X - mario.Width + collision.Width);
                    t.OnSideCollision();
                }
                
            }
            else if (collision.Width > collision.Height)
            {
                if (mario.Y > tile.Y)
                {
                    marimo.OnHeadbutt(collision.Y + collision.Height);
                    t.OnHeadbutt(marimo.IsBig);
                }
                if (mario.Y < tile.Y)
                {
                    marimo.OnStomp(collision.Y - mario.Height);
                    t.OnStomp();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();            

            world.Draw(spriteBatch);

            animations.Draw(spriteBatch);

            spriteBatch.DrawString(font, "left: " + left, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "right: " + right, new Vector2(0, 10), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
