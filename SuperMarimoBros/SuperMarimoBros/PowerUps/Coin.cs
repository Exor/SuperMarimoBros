using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.PowerUps
{
    class Coin : GameObject
    {
        Animation coin;

        public Coin(Vector2 initialPosition)
            : base(initialPosition)
        {
            coin = new Animation(Textures.GetTexture(Textures.Texture.coin), new Rectangle(0, 0, 16, 16), 3, 0.2f, 0);
            coin.IsLooping = true;
            coin.Play();
        }

        public override void Update(GameTime gameTime)
        {
            coin.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            coin.Draw(spriteBatch, position, effects);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType() == typeof(Marimo))
            {
                Player.AddCoin();
                shouldRemove = true;
            }
            base.OnTouch(touchedObject);
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X + 3, (int)position.Y + 2, 10, 14);
        }
    }
}
