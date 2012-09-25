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
        int points = 1000;
        Sprite mushroom;
        Sprite oneUp;
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;
        bool isOneUp;

        public Mushroom(Vector2 position, bool isOneUp)
            :base(position)
        {
            mushroom = new Sprite(Textures.GetTexture(Textures.Texture.entities), new Rectangle(17, 0, 16, 16));
            oneUp = new Sprite(Textures.GetTexture(Textures.Texture.entities), new Rectangle(34, 0, 16, 16));
            this.isOneUp = isOneUp;            
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isOneUp)
                oneUp.Draw(spriteBatch, position, effects);
            else
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
                Player.AddPoints(points);
            }
            //base.OnTouch(touchedObject);
        }

        public bool IsOneUp
        {
            get { return isOneUp; }
        }
    }
}
