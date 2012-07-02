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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace XnaLibrary
{
    public class FPS : Microsoft.Xna.Framework.GameComponent
    {
        double frames;
        double elapsedTime;
        Game game;

        public FPS(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            frames++;
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime > 1)
            {
                game.Window.Title = "FPS: " + frames.ToString();
                elapsedTime -= 1;
                frames = 0;
            }
            base.Update(gameTime);
        }
    }
}