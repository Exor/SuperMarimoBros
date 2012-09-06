using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros.Enemies
{
    class Koopa : Enemy
    {
        float walkingSpeed = 20f;
        float shellSpeed = 70f;
        float jumpVelocity = -170f;
        float timeBetweenAnimation = 0.3f;
        float timeToDie = 1f;
        float elapsedGameTime;

        bool wasHitByFireball;

        Sprite shell;
        Animation walking;
        Animation hopping;

        enum CurrentState
        {
            hopping,
            flying,
            walking,
            shell
        };

        CurrentState state;

        public Koopa(Vector2 initialPosition, CurrentState type)
            : base(initialPosition)
        {
            Texture2D koopaTexture = Textures.GetTexture(Textures.Texture.koopa);
            shell = new Sprite(koopaTexture, new Rectangle(32, 10, 16, 16));
            walking = new Animation(koopaTexture, new Rectangle(0, 0, 16, 24), 2, timeBetweenAnimation, 0);
            hopping = new Animation(koopaTexture, new Rectangle(48, 0, 16, 24), 2, timeBetweenAnimation, 0);
            velocity.X = -walkingSpeed;
            runCollisionDetection = true;

            state = type;
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasHitByFireball)
                return Rectangle.Empty;
            switch (state)
            {
                case CurrentState.walking:
                    return walking.CurrentFrame;
                case CurrentState.shell:
                    return shell.Frame;
                case CurrentState.hopping:
                    return hopping.CurrentFrame;
                case CurrentState.flying:
                    return hopping.CurrentFrame;
                default:
                    return Rectangle.Empty;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (state == CurrentState.flying)
            {
                // needs implemented
                // isn't affected by gravity
                // can be floating up/down or left/right
                // maybe I should implement a flight path for the koopa to follow?
                // or FlyingKoopa as a child class?
            }
            else if (state == CurrentState.hopping)
            {
                if (isOnSolidTile)
                    velocity.Y = jumpVelocity;
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case CurrentState.walking:
                    walking.Draw(spriteBatch);
                    break;
                case CurrentState.shell:
                    shell.Draw(spriteBatch, position, effects);
                    break;
                case CurrentState.hopping:
                    hopping.Draw(spriteBatch);
                    break;
                case CurrentState.flying:
                    hopping.Draw(spriteBatch);
                    break;
            }
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                switch (state)
                {
                    case CurrentState.flying:
                        state = CurrentState.walking;
                        break;
                    case CurrentState.hopping:
                        state = CurrentState.walking;
                        break;
                    case CurrentState.shell:
                        velocity.X = shellSpeed;
                        break;
                    case CurrentState.walking:
                        state = CurrentState.shell;
                        break;
                }
            }

            base.OnHeadbutt(touchedObject);
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
