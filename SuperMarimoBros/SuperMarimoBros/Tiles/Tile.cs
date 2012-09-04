using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    public class Tile
    {
        private Rectangle Frame;
        private Boolean isSolid;
        public Vector2 position;
        private Texture2D texture;

        public Tile(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid)
        {
            this.texture = texture;
            Frame = frame;
            isSolid = solid;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Frame, Color.White);
        }

        public Boolean IsSolid()
        {
            return isSolid;
        }

        public void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo" && isSolid)
                Sounds.Play(Sounds.SoundFx.blockhit);
        }
    }
}
