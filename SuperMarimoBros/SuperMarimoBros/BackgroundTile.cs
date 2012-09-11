using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    public class BackgroundTile
    {
        private Rectangle Frame;
        public Vector2 position;
        private Texture2D texture;
        public bool shouldRemove;

        public BackgroundTile(Rectangle frame, Vector2 position)
        {
            this.position = position;
            texture = Textures.GetTexture(Textures.Texture.smbTiles);
            Frame = frame;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Frame, Color.White);
        }
    }
}
