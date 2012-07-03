using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;
using Microsoft.Xna.Framework.Input;

namespace SuperMarimoBros
{
    class Marimo
    {
        InputHandler input;

        Texture2D texture;
        Vector2 position;
        Vector2 velocity;
        SpriteEffects effects;
        float maximumRunSpeed = 170f;
        float maximumWalkSpeed = 120f;
        float acceleration = 500f;
        float friction = 170f;
        float gravity = 500f;
        float maximumFallSpeed = 120f;

        float launchVelocity = 250f;

        Sprite Standing;
        Animation Running;
        Animation Walking;
        Sprite Jumping;
        Sprite Sliding;
        Sprite Dying;

        State CurrentState;

        enum State
        {
        Standing,
        Running,
        Walking,
        Jumping,
        Falling,
        Sliding,
        Dying
        };

        public void Load(Texture2D texture, Vector2 position, AnimationHandler animations, InputHandler input)
        {
            this.texture = texture;
            this.position = position;

            this.input = input;
            Running = new Animation(texture, new Point(0, 17), new Point(16, 16), 4, 0.08f, 4);
            Walking = new Animation(texture, new Point(0, 17), new Point(16, 16), 4, 0.15f, 4);

            animations.AddAnimation(Running);
            animations.AddAnimation(Walking);

            Standing = new Sprite(texture, new Point(0, 0), new Point(16, 16));
            Sliding = new Sprite(texture, new Point(17, 0), new Point(16, 16));
            Jumping = new Sprite(texture, new Point(40, 0), new Point(16, 16));
            Dying = new Sprite(texture, new Point(49, 0), new Point(16, 16));
            velocity = Vector2.Zero;
            CurrentState = State.Standing;
        }

        public void Draw(SpriteBatch sb)
        {
            switch (CurrentState)
            {
                case State.Standing:
                    Standing.Draw(sb, position, effects);
                    break;
                case State.Walking:
                    Walking.Play();
                    Walking.Position = position;
                    break;
                case State.Running:
                    Running.Play();
                    Running.Position = position;
                    break;
                case State.Jumping:
                    Jumping.Draw(sb, position, effects);
                    break;
                case State.Falling:
                    Jumping.Draw(sb, position, effects);
                    break;
                case State.Sliding:
                    Sliding.Draw(sb, position, effects);
                    break;
                case State.Dying:
                    Dying.Draw(sb, position, effects);
                    break;
            }
        }

        public void Update(GameTime gt)
        {
            float elapsedGameTime = (float)gt.ElapsedGameTime.TotalSeconds;

            CalculateHorizontalMovement(elapsedGameTime);
            CalculateVerticalMovement(elapsedGameTime);
            CalculatePosition(elapsedGameTime);

            CalculateState();
            
            //reset mario to the top of the screen if he falls off
            if (position.Y > 240)
                position.Y = 0;

            CalculateSpriteEffects();
        }

        private void CalculateState()
        {
            if (velocity.Y > 0)
                ChangeState(State.Falling);
            else if (velocity.Y < 0)
                ChangeState(State.Jumping);
            else if (velocity.X == 0)
                ChangeState(State.Standing);
            else if (velocity.X > 0 && input.IsButtonPressed(Keys.Left)
                    || velocity.X < 0 && input.IsButtonPressed(Keys.Right))
                ChangeState(State.Sliding);
            else if (input.IsButtonPressed(Keys.LeftShift))
                ChangeState(State.Running);
            else if (velocity.X >= -maximumWalkSpeed && velocity.X <= maximumWalkSpeed)
                ChangeState(State.Walking);
        }

        private void CalculateVerticalMovement(float elapsedGameTime)
        {                
            if (input.WasButtonPressed(Keys.Up))
                velocity.Y = -launchVelocity;
        }

        private void CalculateSpriteEffects()
        {
            if (velocity.X > 0)
                effects = SpriteEffects.None;
            else if (velocity.X < 0)
                effects = SpriteEffects.FlipHorizontally;

            Walking.Effects = effects;
            Running.Effects = effects;
        }

        private void CalculatePosition(float elapsedGameTime)
        {
            if (velocity.X > 0)
            {
                velocity.X = velocity.X - friction * elapsedGameTime;
                if (CurrentState == State.Walking)
                    velocity.X = MathHelper.Clamp(velocity.X, 0, maximumWalkSpeed);
                else if (CurrentState == State.Running)
                    velocity.X = MathHelper.Clamp(velocity.X, 0, maximumRunSpeed);
                if (CurrentState == State.Falling || CurrentState == State.Jumping)
                    velocity.X = MathHelper.Clamp(velocity.X, 0, maximumRunSpeed);

            }
            else if (velocity.X < 0)
            {
                velocity.X = velocity.X + friction * elapsedGameTime;
                if (CurrentState == State.Walking)
                    velocity.X = MathHelper.Clamp(velocity.X, -maximumWalkSpeed, 0);
                if (CurrentState == State.Running)
                    velocity.X = MathHelper.Clamp(velocity.X, -maximumRunSpeed, 0);
                if (CurrentState == State.Falling || CurrentState == State.Jumping)
                    velocity.X = MathHelper.Clamp(velocity.X, -maximumRunSpeed, 0);

            }
            if (CurrentState == State.Jumping)
            {
                velocity.Y = velocity.Y + gravity * elapsedGameTime;
            }
            else if (CurrentState == State.Falling)
            {
                velocity.Y = velocity.Y + gravity * elapsedGameTime;
                velocity.Y = MathHelper.Clamp(velocity.Y, 0, maximumFallSpeed);
            }
            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
        }

        private void CalculateHorizontalMovement(float elapsedGameTime)
        {
            if (input.IsButtonPressed(Keys.Right))
            {
                velocity.X += acceleration * elapsedGameTime;
            }
            if (input.IsButtonPressed(Keys.Left))
            {
                velocity.X -= acceleration * elapsedGameTime;
            }
        }

        private void ChangeState(State newState)
        {
            State oldState = CurrentState;
            if (oldState != newState)
            {
                Walking.Stop();
                Running.Stop();
                CurrentState = newState;
            }
            
            
        }

        public Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, 16, 16);
        }


        public void OnHeadbutt(int y)
        {
            position.Y = y;
            velocity.Y = 0;
        }

        public void OnStomp(int y)
        {
            ChangeState(State.Standing);
            position.Y = y;
            velocity.Y = 0;
        }

        public void OnSideCollision(int x)
        {
            position.X = x;
            velocity.X = 0;
        }

        public void ShouldFall()
        {
            if (CurrentState != State.Jumping)
                ChangeState(State.Falling);
        }
    }
}
