using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    class Beetle : Enemy
    {
        float walkingSpeed = 20f;
        float timeBetweenAnimation = 0.3f;

        Animation walking;

        public Beetle(Vector2 initialPosition)
            : base(initialPosition)
        {
            walking = new Animation(Textures.GetTexture(Textures.Texture.beetle), new Rectangle(0, 7 + (24 * World.WorldType), 16, 16), 2, timeBetweenAnimation, 0);
            velocity.X = -walkingSpeed;
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasHitByFireball)
                return Rectangle.Empty;
            return new Rectangle((int)position.X, (int)position.Y, walking.CurrentFrame.Width, walking.CurrentFrame.Height);
        }

        public override void Update(GameTime gameTime)
        {
            walking.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            walking.Draw(spriteBatch, position, effects);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType() == typeof(Marimo) && Marimo.IsStarMario((Marimo)touchedObject))
            {
                UpsideDownDeath();
            }
            else if (touchedObject.GetType() == typeof(Shell) && Shell.ShellIsMoving((Shell)touchedObject))
            {
                UpsideDownDeath();
            }
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType() == typeof(Marimo))
            {
                this.shouldRemove = true;
                World.AddObject(new Shell(position, false));
            }
            base.OnHeadbutt(touchedObject);
        }
    }
}
