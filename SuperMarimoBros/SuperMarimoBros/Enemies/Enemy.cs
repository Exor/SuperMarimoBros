using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    abstract class Enemy : GameObjectWithGravity
    {
        public Enemy(Vector2 position)
            : base(position)
        {

        }
    }
}
