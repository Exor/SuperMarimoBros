using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperMarimoBros.Screens
{
    class GameplayScreen : GameScreen
    {
        ContentManager content;

        Textures textures;
        LevelBuilder levelBuilder;
        Sounds soundManager;

        Marimo marimo;
        World world;
        HeadsUpDisplay hud;

        SpriteFont font;
        static string debugMessage = "";

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            soundManager = new Sounds();
            levelBuilder = new LevelBuilder();
            textures = new Textures();

            soundManager.LoadSounds(content);
            textures.LoadTextures(content);

            hud = new HeadsUpDisplay(400, ScreenManager.Font);
            marimo = new Marimo(new Vector2(32, 192), ScreenManager.GetInput());
            world = new World(levelBuilder, marimo);

            Sounds.Play(Sounds.Music.overworld);

            font = content.Load<SpriteFont>("myFont");

            LevelBuilder.LoadLevelFile();

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            world.Update(gameTime);
            hud.Update(400);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            world.Draw(spriteBatch);
            hud.Draw(spriteBatch);
            //spriteBatch.DrawString(font, debugMessage, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void AddDebugMessage(string message)
        {
            debugMessage = message;
        }

        public override void HandleInput(Input input)
        {
            if (input.WasButtonPressed(Keys.Escape))
            {
                this.ExitScreen();
            }
            base.HandleInput(input);
        }
    }
}
