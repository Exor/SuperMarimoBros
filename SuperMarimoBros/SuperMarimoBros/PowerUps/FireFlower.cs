using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.PowerUps
{
    class Fireflower : GameObject
    {
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;
        Animation fireFlower;

        public Fireflower(Vector2 position)
            : base(position)
        {
            initialPosition = position;
            Sounds.Play(Sounds.SoundFx.mushroomappear);
            fireFlower = new Animation(Textures.GetTexture(Textures.Texture.fireflower), new Rectangle(0,0,16,16), 4, 0.1f, 0);
            fireFlower.IsLooping = true;
            fireFlower.Play();
        }

        public override void Update(GameTime gameTime)
        {
            if (isSpawning) //spawn routine
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                    runCollisionDetection = true;
                }
            }

            fireFlower.Position = position;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            fireFlower.Draw(spriteBatch);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                shouldRemove = true;
            }
        }

        public override Rectangle BoundingRectangle()
        {
            return fireFlower.CurrentFrame;
        }
    }
}
