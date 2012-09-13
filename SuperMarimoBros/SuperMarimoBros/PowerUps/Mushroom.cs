using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.PowerUps
{
    class Mushroom : MovingGameObject
    {
        Sprite mushroom;
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;

        public Mushroom(Vector2 position)
            :base(position)
        {
            mushroom = new Sprite(Textures.GetTexture(Textures.Texture.entities), new Rectangle(17, 0, 16, 16));
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
                {
                    isSpawning = false;
                }
            }
            else
            {
                base.Update(gameTime);
            }
            if (position.Y >= 256)
            {
                shouldRemove = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mushroom.Draw(spriteBatch, position, effects);
        }

        public override Rectangle BoundingRectangle()
        {
            if (isSpawning)
                return Rectangle.Empty;
            else
                return new Rectangle((int)position.X, (int)position.Y, mushroom.Frame.Width, mushroom.Frame.Height);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                shouldRemove = true;
            }
            //base.OnTouch(touchedObject);
        }
    }
}
