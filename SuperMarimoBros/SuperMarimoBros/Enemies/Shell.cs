using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    class Shell : Enemy
    {
        Sprite shell;
        float speed = 170f;

        public Shell(Vector2 initialPosition, bool isKoopa)
            : base(initialPosition)
        {
            if (isKoopa)
                shell = new Sprite(Textures.GetTexture(Textures.Texture.koopa), new Rectangle(32, 10 + (24 * World.WorldType), 16, 16));
            else
                shell = new Sprite(Textures.GetTexture(Textures.Texture.beetle), new Rectangle(32, 7 + (24 * World.WorldType), 16, 16));
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasHitByFireball)
                return Rectangle.Empty;
            else
                return new Rectangle((int)position.X, (int)position.Y, shell.Frame.Width, shell.Frame.Height);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            shell.Draw(spriteBatch, position, effects);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                if (velocity.X == 0)
                {
                    Kick(touchedObject);
                }
                else
                {
                    velocity.X = 0;
                }
            }
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                Kick(touchedObject);
            }
            else
                base.OnSideCollision(touchedObject);
        }

        private void Kick(GameObject touchedObject)
        {
            if (touchedObject.position.X < position.X)
                velocity.X = speed;
            else
                velocity.X = -speed;
        }

        public static bool ShellIsMoving(Shell shell)
        {
            if (shell.velocity.X == 0)
                return false;
            return true;
        }
    }
}
