using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace SuperMarimoBros.Screens
{
    class TransitionScreen : GameScreen
    {
        string level;
        int lives;
        bool otherScreensAreGone;
        GameScreen screenToLoad;

        private TransitionScreen(GameScreen screenToLoad)
        {
            TransitionOnTime = TimeSpan.Zero;
            TransitionOffTime = TimeSpan.Zero;
            level = Player.World + "-" + Player.Level;
            lives = Player.Lives;
            this.screenToLoad = screenToLoad;
        }

        public static void Load(ScreenManager screenManager, GameScreen screenToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            TransitionScreen loadingScreen = new TransitionScreen(screenToLoad);

            screenManager.AddScreen(loadingScreen, null);
        }

        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.

            SpriteBatch sb = ScreenManager.SpriteBatch;

            sb.Begin();
            sb.DrawString(ScreenManager.Font, "Level " + level, new Vector2(100, 50), Color.White);
            sb.DrawString(ScreenManager.Font, "Lives x" + lives.ToString(), new Vector2(100, 100), Color.White);
            sb.End();


            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                Thread.Sleep(1000);

                ScreenManager.AddScreen(screenToLoad, ControllingPlayer);
                
                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }


    }
}
