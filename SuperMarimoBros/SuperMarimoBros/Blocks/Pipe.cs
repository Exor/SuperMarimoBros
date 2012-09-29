using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Blocks
{
    class Pipe : GameObject
    {
        int height;

        Sprite entrance;
        Sprite shaft;

        public Pipe(Vector2 position, int height)
            :base(position)
        {
            entrance = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(255, 0, 32, 16));
            shaft = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(255, 17, 32, 16));
            this.height = height;
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X + 3, (int)position.Y, 26, 16 * height);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            entrance.Draw(spriteBatch, position, effects);
            for (int x = 1; x < height; x++)
            {
                shaft.Draw(spriteBatch, new Vector2(position.X, position.Y + x * 16), effects);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public bool IsOnPipe(Marimo mario)
        {
            if (mario.BoundingRectangle().Bottom == this.BoundingRectangle().Top
                && mario.BoundingRectangle().Left > this.BoundingRectangle().Left 
                && mario.BoundingRectangle().Right < this.BoundingRectangle().Right)
            {
                return true;
            }
            return false;
        }
    }
}
