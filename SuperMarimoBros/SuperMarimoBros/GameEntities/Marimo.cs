using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarimoBros
{
    class Marimo : GameObjectWithGravity
    {
        Input input;

        float maximumRunSpeed = 170f;
        float maximumWalkSpeed = 120f;
        float acceleration = 500f;

        float launchVelocity = 250f;

        Sprite Standing;
        Animation Running;
        Animation Walking;
        Sprite Jumping;
        Sprite Sliding;
        Sprite Dying;

        bool isBig;

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

        public Marimo(Vector2 position, Input inputHandler)
            : base(Textures.GetTexture(Textures.Texture.marioSpriteSheet), new Rectangle(), position)
        {
            input = inputHandler;

            Load();
        }

        public void Load()
        {
            isBig = false;

            Running = new Animation(texture, new Point(0, 17), new Point(16, 16), 4, 0.08f, 4);
            Walking = new Animation(texture, new Point(0, 17), new Point(16, 16), 4, 0.15f, 4);

            Animations.AddAnimation(Running);
            Animations.AddAnimation(Walking);

            Standing = new Sprite(texture, new Rectangle(0, 0, 16, 16));
            Sliding = new Sprite(texture, new Rectangle(17, 0, 16, 16));
            Jumping = new Sprite(texture, new Rectangle(40, 0, 16, 16));
            Dying = new Sprite(texture, new Rectangle(49, 0, 16, 16));
            velocity = Vector2.Zero;
            CurrentState = State.Standing;
        }

        public override void Draw(SpriteBatch sb)
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

        public override void Update(GameTime gt)
        {
            float elapsedGameTime = (float)gt.ElapsedGameTime.TotalSeconds;

            CalculateHorizontalVelocity(elapsedGameTime);
            CalculateVerticalVelocity(elapsedGameTime);
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
            else
                ChangeState(State.Walking);
        }

        internal override void CalculateVerticalVelocity(float elapsedGameTime)
        {
            if (input.WasButtonPressed(Keys.Up) && CurrentState != State.Jumping && CurrentState != State.Falling)
            {
                velocity.Y = -launchVelocity;
                Sounds.Play(Sounds.SoundFx.jump);
            }
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

        internal override void CalculatePosition(float elapsedGameTime)
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
                else if (CurrentState == State.Running)
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
                velocity.Y = MathHelper.Clamp(velocity.Y, 0, terminalVelocity);
            }

            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
        }

        internal override void CalculateHorizontalVelocity(float elapsedGameTime)
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
            position.Y = y;
            velocity.Y = 0;
            CalculateState();
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

        public bool IsBig
        {
            get { return isBig; }
        }
    }
}
