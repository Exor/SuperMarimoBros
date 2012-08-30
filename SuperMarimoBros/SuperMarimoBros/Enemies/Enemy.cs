using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    class Enemy : GameObjectWithGravity
    {
        public Enemy(Texture2D texture, Rectangle frame, Vector2 position)
            : base(texture, frame, position)
        {

        }
    }
}
