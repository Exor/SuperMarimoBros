using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros
{
    class Camera
    {
        GameObject focus;
        Vector2 position;

        public Camera(GameObject focus)
        {
            this.focus = focus;
        }

        public void Update(GameTime gameTime)
        {
            if (focus.position.X >= 100)
                position.X = focus.position.X - 100;
            else if (focus.position.X <= 10)
                position.X = focus.position.X - 10;
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public static float X { get; set; }
    }
}
