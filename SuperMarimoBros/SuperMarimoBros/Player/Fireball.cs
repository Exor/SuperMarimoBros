using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.GameEntities
{
    class Fireball : GameObjectWithGravity
    {
        Animation fireball;
        Animation explode;
        bool isExploding = false;
        float elpasedTime = 0f;

        public Fireball(Vector2 initialPosition, int direction)
            : base(Textures.GetTexture(Textures.Texture.fireball), new Rectangle(0,0,8,8), new Vector2(initialPosition.X + 5, initialPosition.Y))
        {
            fireball = new Animation(Textures.GetTexture(Textures.Texture.fireball), new Rectangle(0, 0, 8, 8), 4, 0.05f, 0);
            explode = new Animation(Textures.GetTexture(Textures.Texture.fireball), new Rectangle(32, 0, 16, 16), 3, 0.1f, 0);
            Animations.AddAnimation(fireball);
            Animations.AddAnimation(explode);
            isAnimation = true;
            position.X += 11 * direction;
            velocity.X = 170f * direction;
            runCollisionDetection = true;
            explode.IsLooping = false;
            fireball.IsLooping = true;
            fireball.Play();
            Sounds.Play(Sounds.SoundFx.fireball);
        }

        public override void Update(GameTime gameTime)
        {
            if (isExploding)
            {
                explode.Position = position;
                elpasedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (elpasedTime >= 0.3f)
                {
                    Animations.DisposeOf(fireball);
                    Animations.DisposeOf(explode);
                    this.Remove();
                }
            }
            else
            {
                fireball.Position = position;
                base.Update(gameTime);
            }
            
            
        }

        public override void OnStomp(Tile touchedObject, int y)
        {
            velocity.Y = -150f;
        }

        public override void OnHeadbutt(Tile touchedObject, int y)
        {
            Explode();
        }

        public override void OnSideCollision(Tile touchedObject, int x)
        {
            Explode();
        }

        private void Explode()
        {
            runCollisionDetection = false;
            fireball.Stop();
            explode.Play();
            isExploding = true;
            explode.Position = position;
            Sounds.Play(Sounds.SoundFx.blockhit);
        }
    }
}
