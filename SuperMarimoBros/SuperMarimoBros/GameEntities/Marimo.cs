using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SuperMarimoBros.GameEntities;

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
        Sprite Crouching;

        static bool isBig = true;
        static bool isFireMario;

        bool shouldMoveRight;
        bool shouldMoveLeft;
        bool shouldRun;
        bool shouldCrouch;
        bool shouldJump;
        bool shouldFire;

        State CurrentState;

        enum State
        {
        Standing,
        Running,
        Walking,
        Jumping,
        Falling,
        Sliding,
        Dying,
        Crouching
        };

        public Marimo(Vector2 position, Input inputHandler)
            : base(Textures.GetTexture(Textures.Texture.marioSpriteSheet), new Rectangle(), position)
        {
            input = inputHandler;
            frame = new Rectangle(0, 0, 16, 16);
            Load();
        }

        public void Load()
        {
            runCollisionDetection = true;
            isBig = false;

            Running = new Animation(texture, new Point(81, 0), new Point(16, 16), 4, 0.08f, 4);
            Walking = new Animation(texture, new Point(81, 0), new Point(16, 16), 4, 0.15f, 4);

            Animations.AddAnimation(Running);
            Animations.AddAnimation(Walking);

            Standing = new Sprite(texture, new Rectangle(0, 0, 16, 16));
            Sliding = new Sprite(texture, new Rectangle(17, 0, 16, 16));
            Jumping = new Sprite(texture, new Rectangle(40, 0, 16, 16));
            Dying = new Sprite(texture, new Rectangle(49, 0, 16, 16));
            Crouching = new Sprite(texture, new Rectangle(61, 29, 16, 22));

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
                case State.Crouching:
                    Crouching.Draw(sb, new Vector2(position.X, position.Y - 6), effects);
                    break;
            }
        }

        public override void Update(GameTime gt)
        {
            float elapsedGameTime = (float)gt.ElapsedGameTime.TotalSeconds;

            DealWithControllerInput();
            CalculateHorizontalVelocity(elapsedGameTime);
            CalculateVerticalVelocity(elapsedGameTime);
            CalculatePosition(elapsedGameTime);
            CalculateSpriteEffects();
            CalculateState();
            ResetFlags();

            //SuperMariomoBros.AddDebugMessage("current state: " + CurrentState.ToString());

            //reset mario to the top of the screen if he falls off
            if (position.Y > 240)
                position.Y = 0;
        }

        private void DealWithControllerInput()
        {
            if(input.IsButtonPressed(Keys.Right))
            {
                //either move right
                shouldMoveRight = true;
            }

            else if (input.IsButtonPressed(Keys.Left))
            {
                //or move left
                shouldMoveLeft = true;
            }

            if (input.IsButtonPressed(Keys.O))
            {
                shouldRun = true;
            }

            if (input.WasButtonPressed(Keys.Up))
            {
                //initiate jump
                shouldJump = true;
            }

            if (input.IsButtonPressed(Keys.Down) && isBig)
            {
                //initiate crouch only if big
                shouldCrouch = true;
            }

            if (input.WasButtonPressed(Keys.A) && isFireMario)
            {
                //initiate fireball projectile only if mario has fire power
                shouldFire = true;
            }
        }

        internal override void CalculateHorizontalVelocity(float elapsedGameTime)
        {
            //add acceleration if the player has a button down
            if (shouldMoveRight)
            {
                velocity.X += acceleration * elapsedGameTime;
            }
            if (shouldMoveLeft)
            {
                velocity.X -= acceleration * elapsedGameTime;
            }

            //calculate friction and clamp the velocity
            if (velocity.X > 0)
            {
                velocity.X = velocity.X - friction * elapsedGameTime;
                if (shouldRun)
                    velocity.X = MathHelper.Clamp(velocity.X, 0, maximumRunSpeed);
                else
                    velocity.X = MathHelper.Clamp(velocity.X, 0, maximumWalkSpeed);
            }

            if (velocity.X < 0)
            {
                velocity.X = velocity.X + friction * elapsedGameTime;
                if (shouldRun)
                    velocity.X = MathHelper.Clamp(velocity.X, -maximumRunSpeed, 0);
                else
                    velocity.X = MathHelper.Clamp(velocity.X, -maximumWalkSpeed, 0);
            }
        }

        internal override void CalculateVerticalVelocity(float elapsedGameTime)
        {
            if (isOnSolidTile)
            {
                if (shouldJump && isOnSolidTile)
                {
                    velocity.Y = -launchVelocity;
                    Sounds.Play(Sounds.SoundFx.jump);
                }
                else
                {
                    velocity.Y = 0f;
                }
            }
            else //in the air
            {
                velocity.Y += gravity * elapsedGameTime;

                if (velocity.Y > 0) //moving down
                {
                    velocity.Y = MathHelper.Clamp(velocity.Y, 0, terminalVelocity);
                }
            }
        }

        internal override void CalculatePosition(float elapsedGameTime)
        {
            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
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

        private void CalculateState()
        {
            if (isBig && shouldCrouch)
                ChangeState(State.Crouching);
            else if (velocity.Y > 0)
                ChangeState(State.Falling);
            else if (velocity.Y < 0)
                ChangeState(State.Jumping);
            else if (velocity.X == 0)
                ChangeState(State.Standing);
            else if ((velocity.X > 0 && shouldMoveLeft) || (velocity.X < 0 && shouldMoveRight))
                ChangeState(State.Sliding);
            else if (velocity.X > maximumWalkSpeed || velocity.X < -maximumWalkSpeed)
                ChangeState(State.Running);
            else
                ChangeState(State.Walking);
        }

        private void ResetFlags()
        {
            shouldRun = false;
            shouldMoveRight = false;
            shouldMoveLeft = false;
            shouldJump = false;
            shouldFire = false;
            shouldCrouch = false;
        }

        private void ChangeState(State newState)
        {
            State oldState = CurrentState;
            if (oldState != newState)
            {
                if (newState == State.Crouching) //start crouching
                {
                    position.Y += 16;
                    frame.Height = 16;
                }

                if (oldState == State.Crouching) //stop crouching
                {
                    position.Y -= 16;
                    frame.Height = 32;
                }

                Walking.Stop();
                Running.Stop();
                CurrentState = newState;
            }
        }

        public override void OnStomp(Tile touchedObject, int y)
        {
            base.OnStomp(touchedObject, y);
        }

        public override void OnHeadbutt(Tile touchedObject, int y)
        {
            base.OnHeadbutt(touchedObject, y);
        }

        public override void OnSideCollision(Tile touchedObject, int x)
        {
            velocity.X = 0;
            position.X = x;
        }
        
        public override void OnSideCollision(GameObject touchedObject)
        {
            IsMushroom(touchedObject);

            base.OnSideCollision(touchedObject);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            IsMushroom(touchedObject);

            base.OnHeadbutt(touchedObject);
        }

        public override void OnStomp(GameObject touchedObject)
        {
            IsMushroom(touchedObject);

            base.OnStomp(touchedObject);
        }

        public static bool IsBig
        {
            get { return isBig; }
        }

        private void IsMushroom(GameObject o)
        {
            if (o.GetType().Name == "Mushroom")
            {
                Standing.Frame = new Rectangle(0, 19, 16, 32);
                Jumping.Frame = new Rectangle(41, 19, 16, 32);
                Sliding.Frame = new Rectangle(21, 19, 16, 32);
                Running.FramePosition = new Point(81, 19);
                Running.HeightOfFrames = 32;
                Walking.FramePosition = new Point(81, 19);
                Walking.HeightOfFrames = 32;
                frame.Height = 32;
                position.Y -= 16;
                isBig = true;
                Sounds.Play(Sounds.SoundFx.mushroomeat);
            }
        }
    }
}
