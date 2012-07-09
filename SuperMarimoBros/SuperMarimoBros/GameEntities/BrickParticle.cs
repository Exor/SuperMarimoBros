﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    class BrickParticle : GameObjectWithGravity
    {

        public BrickParticle(Texture2D texture, Vector2 initialVelocity, Vector2 initialPosition)
            : base(texture, new Rectangle(0, 102, 8, 8), initialPosition)
        {
            velocity = initialVelocity;
            if (velocity.X > 1)
                frame = new Rectangle(8, 102, 8, 8);
        }

        internal override void CalculateHorizontalVelocity(float elapsedGameTime)
        {
            if (velocity.X > 0)
            {
                velocity.X = velocity.X - friction * elapsedGameTime;
                if (velocity.X < 0)
                    velocity.X = 0;
            }
            else if (velocity.X < 0)
            {
                velocity.X = velocity.X + friction * elapsedGameTime;
                if (velocity.X > 0)
                    velocity.X = 0;
            }
            

            base.CalculateHorizontalVelocity(elapsedGameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (position.Y > 256)
                Remove();
            base.Update(gameTime);
        }
    }
}
