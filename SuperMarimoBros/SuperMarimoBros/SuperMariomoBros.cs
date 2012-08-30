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
using SuperMarimoBros.Enemies;

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
        static string debugMessage = "";

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

            Sounds.Play(Sounds.Music.overworld);

            font = Content.Load<SpriteFont>("myFont");

            StreamReader streamReader = new StreamReader("Levels/levelOneOne.txt");
            string level = streamReader.ReadToEnd();
            streamReader.Close();
            tileManager.CreateTiles(level, world);

            World.AddGameObject(new Goomba(new Vector2(100, 0)));
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

            world.Update(gameTime);

            animations.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();            

            world.Draw(spriteBatch);

            animations.Draw(spriteBatch);

            spriteBatch.DrawString(font, debugMessage, new Vector2(0, 0), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void AddDebugMessage(string message)
        {
            debugMessage = message;
        }
    }
}
