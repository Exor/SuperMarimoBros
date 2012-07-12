using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    class Tile : GameObject
    {
        internal Rectangle Frame;
        internal Boolean isSolid;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid)
            : base(texture, frame, position)
        {
            Frame = frame;
            isSolid = solid;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Frame, Color.White);
        }

        public Boolean IsSolid()
        {
            return isSolid;
        }

        public override void  OnHeadbutt(GameObject headbutter, GameObject headbuttee)
        {
            if (headbutter.GetType().Name == "Marimo" && isSolid)
                Sounds.Play(Sounds.SoundFx.blockhit);
        }
    }
}
