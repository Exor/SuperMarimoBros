using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros.Player
{
    class Fireball : MovingGameObject
    {
        Animation fireball;
        Animation explode;
        bool isExploding = false;
        float elpasedTime = 0f;

        public Fireball(Vector2 initialPosition, int direction)
            : base(new Vector2(initialPosition.X + 5, initialPosition.Y))
        {
            fireball = new Animation(Textures.GetTexture(Textures.Texture.fireball), new Rectangle(0, 0, 8, 8), 4, 0.05f, 0);
            explode = new Animation(Textures.GetTexture(Textures.Texture.fireball), new Rectangle(32, 0, 16, 16), 3, 0.1f, 0);
            position.X += 11 * direction;
            velocity.X = 170f * direction;
            explode.IsLooping = false;
            //fireball.IsLooping = true;
            fireball.Play();
            Sounds.Play(Sounds.SoundFx.fireball);
        }

        public override void Update(GameTime gameTime)
        {
            fireball.Update(gameTime);
            if (isExploding)
            {
                explode.Update(gameTime);
                elpasedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (elpasedTime >= 0.3f)
                {
                    shouldRemove = true;
                }
            }
            else
            {
                base.Update(gameTime);
            }
            
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isExploding)
                explode.Draw(spriteBatch, position, effects);
            else
                fireball.Draw(spriteBatch, position, effects);
        }

        public override Rectangle BoundingRectangle()
        {
            if (isExploding)
                return Rectangle.Empty;
            else
                return new Rectangle((int)position.X, (int)position.Y, fireball.CurrentFrame.Width, fireball.CurrentFrame.Height);
        }

        //public override void OnStomp(Tile touchedObject, int y)
        //{
        //    velocity.Y = -150f;
        //}

        //public override void OnHeadbutt(Tile touchedObject, int y)
        //{
        //    Explode();
        //}

        //public override void OnSideCollision(Tile touchedObject, int x)
        //{
        //    Explode();
        //}

        public override void OnStomp(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                velocity.Y = -150f;
            }

            base.OnStomp(touchedObject);
            isOnSolidTile = false;
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                Explode();
            }
            base.OnSideCollision(touchedObject);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Blocks")
            {
                Explode();
            }
            base.OnHeadbutt(touchedObject);
        }

        private void Explode()
        {
            fireball.Stop();
            explode.Play();
            isExploding = true;
            Sounds.Play(Sounds.SoundFx.blockhit);
        }
    }
}
