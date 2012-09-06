using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.PowerUps
{
    class Star : GameObjectWithGravity
    {
        Animation star;
        float animationSpeed = 0.5f;
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
            if (isSpawning)
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                    runCollisionDetection = true;
                }
            }
            else
            {
                if (isOnSolidTile)
                    velocity.Y = bounceSpeed;
                base.Update(gameTime);
            }
            if (position.Y >= 256)
            {
                shouldRemove = true;
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            star.Draw(spriteBatch);
        }

        public override Rectangle BoundingRectangle()
        {
            return star.CurrentFrame;
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                shouldRemove = true;
            }
            base.OnTouch(touchedObject);
        }
    }
}
