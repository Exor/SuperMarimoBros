using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    abstract class MovingGameObject : GameObject
    {

        internal Vector2 velocity;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 200f;

        public MovingGameObject(Vector2 position)
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

        public override void OnStomp(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                isOnSolidTile = true;
            }
            base.OnStomp(touchedObject);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                velocity.Y = 0;
                position.Y = touchedObject.position.Y + touchedObject.BoundingRectangle().Height ;
            }
            base.OnHeadbutt(touchedObject);
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                velocity.X = 0;
                if (position.X > touchedObject.position.X)//collision from the right
                {
                    position.X = touchedObject.position.X + touchedObject.BoundingRectangle().Width;
                }
                else//collision from the left
                {
                    position.X = touchedObject.position.X - this.BoundingRectangle().Width;
                }
                
            }

            base.OnSideCollision(touchedObject);
        }
    }
}
