using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;

namespace SuperMarimoBros
{
    class Tile
    {
        internal Texture2D Texture;
        internal Vector2 Position;
        internal Rectangle Frame;
        internal Boolean isSolid;
        internal SoundManager soundManager;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm)
        {
            Texture = texture;
            Position = position;
            Frame = frame;
            isSolid = solid;
            soundManager = sm;
        }

        public virtual void Draw(SpriteBatch sb)
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

        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void OnHeadbutt()
        {
            if (isSolid)
                soundManager.Play(SoundManager.Sound.blockhit);
        }

        public virtual void OnStomp()
        {

        }

        public virtual void OnSideCollision()
        {

        }
    }
}
