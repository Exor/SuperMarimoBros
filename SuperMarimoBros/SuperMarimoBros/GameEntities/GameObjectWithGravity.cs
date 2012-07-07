using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros
{
    class GameObjectWithGravity : GameObject
    {
        internal Vector2 velocity;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 200f;

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CalculateHorizontalVelocity(elapsedGameTime);
            CalculateVerticalVelocity(elapsedGameTime);
            CalculatePosition(elapsedGameTime);
        }

        internal virtual void CalculatePosition(float elapsedGameTime)
        {
            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
        }

        internal virtual void CalculateHorizontalVelocity(float elapsedGameTime)
        {

        }

        internal virtual void CalculateVerticalVelocity(float elapsedGameTime)
        {
            velocity.Y = velocity.Y + gravity * elapsedGameTime;
        }     
    }
}
