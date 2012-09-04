using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    class Koopa : Enemy
    {
        float walkingSpeed = 20f;
        float timeBetweenAnimation = 0.3f;
        float timeToDie = 1f;
        float elapsedGameTime;

        enum CurrentState
        {
            hopping,
            flying,
            walking,
            hitByFireball,
            shell
        };

        public Koopa(Vector2 initialPosition)
            : base(initialPosition)
        {

        }

        public override Rectangle BoundingRectangle()
        {
            throw new NotImplementedException();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
        

    }
}
