using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.GameEntities
{
    class Mushroom : GameObjectWithGravity
    {
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;

        public Mushroom(Vector2 position)
            :base(Textures.GetTexture(Textures.Texture.entities), new Rectangle(17,0,16,16), position)
        {
            initialPosition = position;
            velocity.X = 50f;
            Sounds.Play(Sounds.SoundFx.mushroomappear);
        }

        public override void Update(GameTime gameTime)
        {
            if (isSpawning)
            {
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (position.Y <= initialPosition.Y - 16)
                    isSpawning = false;
            }
            else
            {
                base.Update(gameTime);
            }
        }
    }
}
