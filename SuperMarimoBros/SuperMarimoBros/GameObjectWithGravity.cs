using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    abstract class GameObjectWithGravity : GameObject
    {

        internal Vector2 velocity;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 200f;

        public GameObjectWithGravity(Vector2 position)
            : base(position)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CalculateVerticalVelocity(elapsedGameTime);
            CalculatePosition(elapsedGameTime);
        }

        private void CalculatePosition(float elapsedGameTime)
        {
            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
        }

        private void CalculateVerticalVelocity(float elapsedGameTime)
        {
            if (!isOnSolidTile)
                velocity.Y = velocity.Y + gravity * elapsedGameTime;
        }
    }
}
