using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;

namespace SuperMarimoBros
{
    class Tile : GameObject
    {
        internal Rectangle Frame;
        internal Boolean isSolid;
        internal SoundManager soundManager;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm)
        {
            this.texture = texture;
            this.position = position;
            Frame = frame;
            isSolid = solid;
            soundManager = sm;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Frame, Color.White);
        }

        public Boolean IsSolid()
        {
            return isSolid;
        }

        public Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, Frame.Width, Frame.Height);
        }

        public override void OnHeadbutt()
        {
            if (isSolid)
                soundManager.Play(SoundManager.Sound.blockhit);
        }
    }
}
