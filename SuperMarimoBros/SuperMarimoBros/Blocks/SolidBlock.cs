using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros.Blocks
{
    class SolidBlock : GameObject
    {
        private Rectangle Frame;
        private Texture2D texture;

        public SolidBlock(Rectangle frame, Vector2 position)
            : base(position)
        {
            texture = Textures.GetTexture(Textures.Texture.smbTiles);
            Frame = frame;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Frame, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, Frame.Width, Frame.Height);
        }
    }
}
