﻿using System;
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

        public Shell(Vector2 initialPosition)
            : base(initialPosition)
        {
            shell = new Sprite(Textures.GetTexture(Textures.Texture.koopa), new Rectangle(32, 10, 16, 16));
        }

        public override Rectangle BoundingRectangle()
        {
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
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                velocity.X = -velocity.X;
            }

            base.OnSideCollision(touchedObject);
        }

        private void Kick(GameObject touchedObject)
        {
            if (touchedObject.position.X < position.X)
                velocity.X = speed;
            else
                velocity.X = -speed;
        }
    }
}