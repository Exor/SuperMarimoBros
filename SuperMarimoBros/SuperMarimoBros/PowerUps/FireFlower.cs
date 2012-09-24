using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.PowerUps
{
    class Fireflower : GameObject
    {
        int points = 1000;
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
            fireFlower.Update(gameTime);
            if (isSpawning) //spawn routine
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                }
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            fireFlower.Draw(spriteBatch, position, effects);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                shouldRemove = true;
                Player.AddPoints(points);
            }
        }

        public override Rectangle BoundingRectangle()
        {
            if (isSpawning)
                return Rectangle.Empty;
            else
                return new Rectangle((int)position.X, (int)position.Y, fireFlower.CurrentFrame.Width, fireFlower.CurrentFrame.Height);
        }
    }
}
