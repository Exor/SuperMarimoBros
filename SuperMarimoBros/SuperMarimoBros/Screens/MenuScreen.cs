using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Screens
{
    class MenuScreen : GameScreen
    {

        public MenuScreen()
        {
            TransitionOffTime = TimeSpan.Zero;
            TransitionOnTime = TimeSpan.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Super Marimo Bros!", new Vector2(200, 0), Color.White);
            ScreenManager.SpriteBatch.DrawString(ScreenManager.Font, "Press Enter to start!", new Vector2(200, 100), Color.White);
            ScreenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public override void HandleInput(Input input)
        {
            if (input.WasButtonPressed(Keys.Escape))
            {
                ScreenManager.Game.Exit();
            }
            if (input.WasButtonPressed(Keys.Enter))
            {
                //ScreenManager.AddScreen(new GameplayScreen(), null);
                TransitionScreen.Load(ScreenManager, new GameplayScreen());
            }
            base.HandleInput(input);
        }

    }
}
