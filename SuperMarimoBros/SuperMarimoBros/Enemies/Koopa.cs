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
        float jumpVelocity = -170f;
        float timeBetweenAnimation = 0.3f;
        
        Animation walking;
        Animation hopping;

        public enum CurrentState
        {
            hopping,
            flying,
            walking
        };

        CurrentState state;

        public Koopa(Vector2 initialPosition, CurrentState type)
            : base(new Vector2(initialPosition.X, initialPosition.Y - 8))
        {
            Texture2D koopaTexture = Textures.GetTexture(Textures.Texture.koopa);
            
            walking = new Animation(koopaTexture, new Rectangle(0, 0, 16, 24), 2, timeBetweenAnimation, 0);
            hopping = new Animation(koopaTexture, new Rectangle(48, 0, 16, 24), 2, timeBetweenAnimation, 0);
            velocity.X = -walkingSpeed;

            state = type;
        }

        public override Rectangle BoundingRectangle()
        {
            if (wasHitByFireball)
                return Rectangle.Empty;
            switch (state)
            {
                case CurrentState.walking:
                    return new Rectangle((int)position.X, (int)position.Y, walking.CurrentFrame.Width, walking.CurrentFrame.Height);
                case CurrentState.hopping:
                    return new Rectangle((int)position.X, (int)position.Y, hopping.CurrentFrame.Width, hopping.CurrentFrame.Height);
                case CurrentState.flying:
                    return new Rectangle((int)position.X, (int)position.Y, hopping.CurrentFrame.Width, hopping.CurrentFrame.Height);
                default:
                    return Rectangle.Empty;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!wasHitByFireball)
            {
                if (velocity.X < 0)
                    effects = SpriteEffects.None;
                else
                    effects = SpriteEffects.FlipHorizontally;

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
                    hopping.Update(gameTime);
                    if (isOnSolidTile)
                    {
                        velocity.Y = jumpVelocity;
                        isOnSolidTile = false;
                    }
                }
                else if (state == CurrentState.walking)
                {
                    walking.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            switch (state)
            {
                case CurrentState.walking:
                    walking.Draw(spriteBatch, position, effects);
                    break;
                case CurrentState.hopping:
                    hopping.Draw(spriteBatch, position, effects);
                    break;
                case CurrentState.flying:
                    hopping.Draw(spriteBatch, position, effects);
                    break;
            }
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                Player.AddPoints(points);

                switch (state)
                {
                    case CurrentState.flying:
                        state = CurrentState.walking;
                        break;
                    case CurrentState.hopping:
                        velocity.Y = 0;
                        state = CurrentState.walking;
                        break;
                    case CurrentState.walking:
                        this.shouldRemove = true;
                        World.AddObject(new Shell(new Vector2(position.X, position.Y + 8)));
                        break;
                }
            }

            base.OnHeadbutt(touchedObject);
        }


    }
}
