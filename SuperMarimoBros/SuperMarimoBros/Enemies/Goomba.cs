using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros.Screens;

namespace SuperMarimoBros.Enemies
{
    class Goomba : Enemy
    {
        Sprite death;
        Sprite goomba;
        float walkingSpeed = 20f;
        bool wasStomped = false;
        bool wasHitByFireball = false;
        float timeBetweenAnimation = 0.3f;
        float timeToDie = 1f;
        float elapsedGameTime;

        public Goomba(Vector2 initialPosition)
            : base(initialPosition)
        {
            death = new Sprite(Textures.GetTexture(Textures.Texture.goomba), new Rectangle(16, 0, 16, 16));
            goomba = new Sprite(Textures.GetTexture(Textures.Texture.goomba), new Rectangle(0, 0, 16, 16));
            velocity.X = -walkingSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            

            elapsedGameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (wasStomped)
            {
                if (elapsedGameTime >= timeToDie)
                    shouldRemove = true;
            }
            else if (wasHitByFireball)
            {
                if (position.X >= 245)
                    shouldRemove = true;
                base.Update(gameTime);
            }
            else
            {
                if (elapsedGameTime >= timeBetweenAnimation)
                {
                    Flip();
                    elapsedGameTime = 0f;
                }
                base.Update(gameTime);
            }
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (wasStomped)
                death.Draw(spriteBatch, position, effects);
            else
                goomba.Draw(spriteBatch, position, effects);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                wasStomped = true;
            }
            base.OnHeadbutt(touchedObject);
        }

        public override void OnStomp(GameObject touchedObject)
        {
            base.OnStomp(touchedObject);
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            GameplayScreen.AddDebugMessage("HIT: " + touchedObject.GetType().Name.ToString());
            velocity.X = -velocity.X;
            base.OnSideCollision(touchedObject);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Fireball")
            {
                wasHitByFireball = true;
                isOnSolidTile = false;
                velocity.X = 0;
                velocity.Y = -150f;
                effects = SpriteEffects.FlipVertically;
                Sounds.Play(Sounds.SoundFx.stomp);
            }
        }

        private void Flip()
        {
            if (effects == SpriteEffects.None)
                effects = SpriteEffects.FlipHorizontally;
            else
                effects = SpriteEffects.None;
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasStomped || wasHitByFireball)
                return Rectangle.Empty;
            else
                return new Rectangle((int)position.X, (int)position.Y, goomba.Frame.Width, goomba.Frame.Height);
        }

    }
}
