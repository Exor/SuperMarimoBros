using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros.Enemies
{
    class Spikey : Enemy
    {
        Animation walking;
        Animation thrown;
        float walkingSpeed = 20f;
        float thrownSpeed = -100f;
        bool wasHitByFireball = false;
        float timeBetweenAnimation = 0.3f;

        public static enum CurrentState
        {
            walking,
            thrown
        };

        CurrentState state;

        public Spikey(Vector2 initialPosition, CurrentState type)
            : base(initialPosition)
        {
            walking = new Animation(Textures.GetTexture(Textures.Texture.spikey), new Rectangle(0, 0, 16, 16), 2, timeBetweenAnimation, 0);
            thrown = new Animation(Textures.GetTexture(Textures.Texture.spikey), new Rectangle(32, 0, 16, 16), 2, timeBetweenAnimation, 0);

            state = type;

            if (state == CurrentState.walking)
                velocity.X = walkingSpeed;
            else
                velocity.Y = thrownSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            if (state == CurrentState.thrown && isOnSolidTile)
            {
                state = CurrentState.walking;
                velocity.X = walkingSpeed;
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (state == CurrentState.walking)
                walking.Draw(spriteBatch);
            else
                thrown.Draw(spriteBatch);
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasHitByFireball)
                return Rectangle.Empty;
            else if (state == CurrentState.walking)
                return walking.CurrentFrame;
            else
                return thrown.CurrentFrame;
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Fireball")
            {
                wasHitByFireball = true;
                runCollisionDetection = false;
                velocity.X = 0;
                velocity.Y = -150f;
                effects = SpriteEffects.FlipVertically;
                Sounds.Play(Sounds.SoundFx.stomp);
            }
            base.OnTouch(touchedObject);
        }
    }
}
