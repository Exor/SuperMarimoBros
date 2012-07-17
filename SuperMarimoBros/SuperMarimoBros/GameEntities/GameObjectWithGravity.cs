using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    class GameObjectWithGravity : GameObject
    {
        internal Vector2 velocity;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 200f;
        

        public GameObjectWithGravity(Texture2D texture, Rectangle frame, Vector2 position)
            : base(texture, frame, position)
        {
            
        }

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
            //CollisionDetection();
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
            if (isFalling)
                velocity.Y = velocity.Y + gravity * elapsedGameTime;
        }

        public override void OnSideCollision(GameObject touchedObject, int x)
        {
            position.X = x;
            velocity.X = -velocity.X;
            base.OnSideCollision(touchedObject);
        }

        public override void OnHeadbutt(GameObject touchedObject, int y)
        {
            position.Y = y;
            velocity.Y = 0;
            base.OnHeadbutt(touchedObject);
        }

        public override void OnStomp(GameObject touchedObject, int y)
        {
            position.Y = y;
            velocity.Y = 0;
            base.OnStomp(touchedObject);
        }
    }
}
