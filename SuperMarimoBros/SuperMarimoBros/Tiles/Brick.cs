using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Tiles
{
    class Brick : Tile
    {
        List<Tile> particles;
        
        public Brick(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid) 
            : base(texture, frame, position, solid)
        {

        }

        public override void OnHeadbutt()
        {
            particles = new List<Tile>();

            isSolid = false;
            Frame = new Rectangle(0, 0, 16, 16);
            
            //change the texture to blank
            //create 4 particles that fly out
            base.OnHeadbutt();
        }
    }
}
