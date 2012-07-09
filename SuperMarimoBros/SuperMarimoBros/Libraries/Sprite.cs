using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros
{
    class Sprite
    {
        Texture2D SpriteSheet;
        Rectangle Frame;

        public Sprite(Texture2D spriteSheet, Rectangle frame)
        {
            SpriteSheet = spriteSheet;
            Frame = frame;
        }

        public void Draw(SpriteBatch sb, Vector2 position, SpriteEffects effects)
        {
            sb.Draw(SpriteSheet,
                position,
                Frame,
                Color.White,
                0f, //Rotation
                Vector2.Zero,
                1f,
                effects,
                0);
        }
        
    }
}
