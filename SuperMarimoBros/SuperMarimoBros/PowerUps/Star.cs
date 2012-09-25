using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.PowerUps
{
    class Star : MovingGameObject
    {
        Animation star;
        float animationSpeed = 0.1f;
        float spawnSpeed = 20f;
        float bounceSpeed = -170f;
        float movementSpeed = 70f;
        bool isSpawning = true;
        Vector2 initialPosition;

        public Star(Vector2 position)
            : base(position)
        {
            initialPosition = position;
            star = new Animation(Textures.GetTexture(Textures.Texture.star), new Rectangle(0, 0, 16, 16), 4, animationSpeed, 0);
            velocity = new Vector2(movementSpeed, bounceSpeed);
            Sounds.Play(Sounds.SoundFx.mushroomappear);
        }

        public override void Update(GameTime gameTime)
        {
            star.Update(gameTime);

            if (isSpawning)
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                }
            }
            else
            {
                if (isOnSolidTile)
                {
                    velocity.Y = bounceSpeed;
                    isOnSolidTile = false;
                }
                base.Update(gameTime);
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            star.Draw(spriteBatch, position, effects);
        }

        public override Rectangle BoundingRectangle()
        {
            if (isSpawning)
                return Rectangle.Empty;
            return new Rectangle((int)position.X, (int)position.Y, star.CurrentFrame.Width, star.CurrentFrame.Height);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                shouldRemove = true;
            }
            base.OnTouch(touchedObject);
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            velocity.X = -velocity.X;
            base.OnSideCollision(touchedObject);
        }
    }
}
