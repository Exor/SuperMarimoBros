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
        Point SpriteLocation;
        Point SpriteSize;

        public Sprite(Texture2D spriteSheet, Point spriteLocation, Point spriteSize)
        {
            SpriteSheet = spriteSheet;
            SpriteLocation = spriteLocation;
            SpriteSize = spriteSize;
        }

        public void Draw(SpriteBatch sb, Vector2 position, SpriteEffects effects)
        {
            sb.Draw(SpriteSheet,
                position,
                new Rectangle(SpriteLocation.X, SpriteLocation.Y, SpriteSize.X, SpriteSize.Y),
                Color.White,
                0f, //Rotation
                Vector2.Zero,
                1f,
                effects,
                0);
        }
        
    }
}
