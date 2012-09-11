using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros;

namespace SuperMarimoBros.Blocks
{
    class BrickParticle : GameObjectWithGravity
    {
        Sprite brickParticle;

        public BrickParticle(Vector2 initialVelocity, Vector2 initialPosition)
            : base(initialPosition)
        {
            brickParticle = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(0, 102, 8, 8));
            velocity = initialVelocity;
        }

        private void CalculateHorizontalVelocity(float elapsedGameTime)
        {
            if (velocity.X > 0)
            {
                velocity.X = velocity.X - friction * elapsedGameTime;
                if (velocity.X < 0)
                    velocity.X = 0;
                effects = SpriteEffects.FlipHorizontally;
            }
            else if (velocity.X < 0)
            {
                velocity.X = velocity.X + friction * elapsedGameTime;
                if (velocity.X > 0)
                    velocity.X = 0;
                effects = SpriteEffects.None;
            }
        }

        public override void Update(GameTime gameTime)
        {
            CalculateHorizontalVelocity((float)gameTime.ElapsedGameTime.TotalSeconds);
            if (position.Y > 256)
                shouldRemove = true;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            brickParticle.Draw(spriteBatch, position, effects);
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, brickParticle.Frame.Width, brickParticle.Frame.Height);
        }
    }
}
