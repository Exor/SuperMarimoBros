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
        SoundManager soundManager;

        Marimo marimo;
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
            input = new InputHandler(this);
            this.Components.Add(input);
            soundManager = new SoundManager();
            tileManager = new TileManager();
            animations = new AnimationHandler();
            marimo = new Marimo();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/blockbreak"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/blockhit"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/boom"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/bowserfall"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/bridgebreak"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/bulletbill"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/coin"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/fire"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/fireball"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/jump"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/jumpbig"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/mushroomappear"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/mushroomeat"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/oneup"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/pause"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/pipe"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/scorering"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/shot"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/shrink"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/stomp"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/swim"));
            soundManager.AddSound(Content.Load<SoundEffect>("SoundFX/vine"));

            soundManager.AddMusic(Content.Load<SoundEffect>("Music/castle-fast"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/castle"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/castleend"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/death"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/gameover"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/intermission"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/levelend"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/lowtime"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/overworld-fast"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/overworld"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/princessmusic"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/starmusic-fast"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/starmusic"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/underground-fast"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/underground"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/underwater-fast"));
            soundManager.AddMusic(Content.Load<SoundEffect>("Music/underwater"));

            soundManager.Play(SoundManager.Music.overworld);

            marimo.Load(Content.Load<Texture2D>("Graphics/mariospritesheet"), new Vector2(32,192), animations, input, soundManager);
            marioGraphics = Content.Load<Texture2D>("Graphics/smbtiles");
            

            StreamReader streamReader = new StreamReader("Levels/levelOneOne.txt");
            string level = streamReader.ReadToEnd();
            streamReader.Close();
            tileManager.CreateTiles(level, marioGraphics, soundManager);
            
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
                soundManager.Play(SoundManager.Music.overworldfast);


            tileManager.Update(gameTime);
            marimo.Update(gameTime);
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
                    t.OnHeadbutt();
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

            tileManager.Draw(spriteBatch);
            animations.Draw(spriteBatch);
            marimo.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
