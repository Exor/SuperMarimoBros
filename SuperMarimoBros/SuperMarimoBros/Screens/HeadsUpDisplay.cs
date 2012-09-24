using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Screens
{
    class HeadsUpDisplay
    {
        float remainingTime;
        SpriteFont font;

        public HeadsUpDisplay(float initialTime, SpriteFont font)
        {
            this.font = font;
            remainingTime = initialTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Score", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, Player.Score.ToString(), new Vector2(0, 10), Color.White);
            spriteBatch.DrawString(font, "Coins", new Vector2(50, 0), Color.White);
            spriteBatch.DrawString(font, Player.Coins.ToString(), new Vector2(50, 10), Color.White);
            spriteBatch.DrawString(font, "World", new Vector2(100, 0), Color.White);
            spriteBatch.DrawString(font, Player.World + "-" + Player.Level, new Vector2(100, 10), Color.White);
            spriteBatch.DrawString(font, "Time", new Vector2(150, 0), Color.White);
            spriteBatch.DrawString(font, remainingTime.ToString(), new Vector2(150, 10), Color.White);
        }

        public void Update(float remainingTime)
        {
            this.remainingTime = remainingTime;
        }

    }
}
