using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.GameEntities
{
    class Fireflower : GameObject
    {
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;
        Animation fireFlower;

        public Fireflower(Vector2 position)
            : base(Textures.GetTexture(Textures.Texture.fireflower), new Rectangle(0,0,16,16), position)
        {
            initialPosition = position;
            Sounds.Play(Sounds.SoundFx.mushroomappear);
            fireFlower = new Animation(Textures.GetTexture(Textures.Texture.fireflower), new Point(0, 0), new Point(16, 16), 4, 0.1f, 0);
            Animations.AddAnimation(fireFlower);
            fireFlower.IsLooping = true;
            fireFlower.Play();
        }

        public override void Update(GameTime gameTime)
        {
            if (isSpawning)
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                fireFlower.Position = position;

                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                    runCollisionDetection = true;
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
