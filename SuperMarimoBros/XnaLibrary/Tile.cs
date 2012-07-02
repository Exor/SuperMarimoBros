using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XnaLibrary
{
    public class Tile
    {
        Texture2D Texture;
        Vector2 Position;
        Rectangle Frame;
        Boolean isSolid;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid)
        {
            Texture = texture;
            Position = position;
            Frame = frame;
            isSolid = solid;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, Frame, Color.White);
        }

        public Boolean IsSolid()
        {
            return isSolid;
        }
    }
}
