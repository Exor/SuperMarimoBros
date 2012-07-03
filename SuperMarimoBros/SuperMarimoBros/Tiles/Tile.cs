using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros
{
    class Tile
    {
        Texture2D Texture;
        Vector2 Position;
        internal Rectangle Frame;
        internal Boolean isSolid;

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

        public Rectangle BoundingRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Frame.Width, Frame.Height);
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public virtual void OnHeadbutt()
        {

        }

        public virtual void OnStomp()
        {

        }

        public virtual void OnSideCollision()
        {

        }
    }
}
