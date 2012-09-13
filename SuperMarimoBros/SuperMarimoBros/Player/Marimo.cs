using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarimoBros.Player
{
    class Marimo : MovingGameObject
    {
        Input input;

        float maximumRunSpeed = 170f;
        float maximumWalkSpeed = 120f;
        float acceleration = 500f;

        float launchVelocity = 280f;

        float timer = 0f;

        Sprite Standing;
        Animation Running;
        Animation Walking;
        Sprite Jumping;
        Sprite Sliding;
        Sprite Dying;
        Sprite Crouching;
        Sprite Firing;

        static bool isBig = true;
        static bool isFireMario;
        bool isDying;
        bool wasHitByEnemy;

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
        Crouching,
        Firing,
        };

        public Marimo(Vector2 position, Input inputHandler)
            : base(position)
        {
            input = inputHandler;
            Load();
        }

        public void Load()
        {
            Texture2D texture = Textures.GetTexture(Textures.Texture.marioSpriteSheet);

            isBig = false;

            Running = new Animation(texture, new Rectangle(81,0,16,16), 4, 0.08f, 4);
            Walking = new Animation(texture, new Rectangle(81,0,16,16), 4, 0.15f, 4);

            Standing = new Sprite(texture, new Rectangle(0, 0, 16, 16));
            Sliding = new Sprite(texture, new Rectangle(17, 0, 16, 16));
            Jumping = new Sprite(texture, new Rectangle(40, 0, 16, 16));
            Dying = new Sprite(texture, new Rectangle(60, 0, 16, 16));
            Crouching = new Sprite(texture, new Rectangle(61, 29, 16, 22));
            Firing = new Sprite(texture, new Rectangle(163, 55, 16, 32));

            velocity = Vector2.Zero;
            CurrentState = State.Standing;

#if DEBUG
            BecomeBigMario();
            BecomeFireMario();
#endif
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
                    Walking.Draw(sb, position, effects);
                    break;
                case State.Running:
                    Running.Play();
                    Running.Draw(sb, position, effects);
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
                case State.Firing:
                    Firing.Draw(sb, position, effects);
                    break;
            }
        }

        public override void Update(GameTime gt)
        {
            float elapsedGameTime = (float)gt.ElapsedGameTime.TotalSeconds;

            Walking.Update(gt);
            Running.Update(gt);

            if (isDying)
            {
                ChangeState(State.Dying);
            }
            else if (wasHitByEnemy)
            {
                velocity = Vector2.Zero;
                timer += elapsedGameTime;
                if (timer > 1)
                {
                    wasHitByEnemy = false;
                }
            }
            else
            {
                //Mario falls off the screen
                if (position.Y > 250)
                {
                    position.Y = -32;
                    //isDying = true;
                    //CurrentState = State.Dying;
                    //Sounds.Play(Sounds.Music.death);
                }

                DealWithControllerInput();
                CalculateHorizontalVelocity(elapsedGameTime);
                CalculateVerticalVelocity(elapsedGameTime);
                CalculatePosition(elapsedGameTime);
                CalculateSpriteEffects();
                FireAFireball();
                CalculateState();
                ResetFlags();
            }

            //SuperMariomoBros.AddDebugMessage("state: " + CurrentState.ToString() + " x: " + position.X + " y: " + position.Y);
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

        private void CalculateHorizontalVelocity(float elapsedGameTime)
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

        private void CalculateVerticalVelocity(float elapsedGameTime)
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

        private void CalculatePosition(float elapsedGameTime)
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
        }

        private void FireAFireball()
        {
            if (shouldFire)
            {
                if (effects == SpriteEffects.None) //shoot right
                    World.AddObject(new Fireball(position, 1));
                else //shoot left
                    World.AddObject(new Fireball(position, -1));
            }
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
                }

                if (oldState == State.Crouching) //stop crouching
                {
                    position.Y -= 16;
                }

                Walking.Stop();
                Running.Stop();
                CurrentState = newState;
            }
        }

        public override Rectangle BoundingRectangle()
        {
            switch (CurrentState)
            {
                case State.Standing:
                    return new Rectangle((int)position.X, (int)position.Y, Standing.Frame.Width, Standing.Frame.Height);
                case State.Walking:
                    return new Rectangle((int)position.X, (int)position.Y, Walking.CurrentFrame.Width, Walking.CurrentFrame.Height);
                case State.Running:
                    return new Rectangle((int)position.X, (int)position.Y, Running.CurrentFrame.Width, Running.CurrentFrame.Height);
                case State.Jumping:
                    return new Rectangle((int)position.X, (int)position.Y, Jumping.Frame.Width, Jumping.Frame.Height);
                case State.Falling:
                    return new Rectangle((int)position.X, (int)position.Y, Jumping.Frame.Width, Jumping.Frame.Height);
                case State.Sliding:
                    return new Rectangle((int)position.X, (int)position.Y, Sliding.Frame.Width, Sliding.Frame.Height);
                case State.Dying:
                    return new Rectangle((int)position.X, (int)position.Y, Dying.Frame.Width, Dying.Frame.Height);
                case State.Crouching:
                    return new Rectangle((int)position.X, (int)position.Y, Crouching.Frame.Width, Crouching.Frame.Height);
                case State.Firing:
                    return new Rectangle((int)position.X, (int)position.Y, Firing.Frame.Width, Firing.Frame.Height);
                default:
                    return Rectangle.Empty;
            }
        }

        //public override void OnStomp(Tile touchedObject, int y)
        //{
        //    base.OnStomp(touchedObject, y);
        //}

        //public override void OnHeadbutt(Tile touchedObject, int y)
        //{
        //    base.OnHeadbutt(touchedObject, y);
        //}

        //public override void OnSideCollision(Tile touchedObject, int x)
        //{
        //    velocity.X = 0;
        //    position.X = x;
        //}
        
        public override void OnSideCollision(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Enemies")
            {
                OnHitEnemy();
            }
            base.OnSideCollision(touchedObject);
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Enemies")
            {
                OnHitEnemy();
            }
            base.OnHeadbutt(touchedObject);
        }

        public override void OnStomp(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Enemies")
            {
                velocity.Y = -100f;
                Sounds.Play(Sounds.SoundFx.stomp);
            }
            base.OnStomp(touchedObject);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Mushroom")
            {
                Sounds.Play(Sounds.SoundFx.mushroomeat);
                BecomeBigMario();
            }
            else if (touchedObject.GetType().Name == "Fireflower")
            {
                Sounds.Play(Sounds.SoundFx.mushroomeat);
                BecomeFireMario();
            }

            base.OnTouch(touchedObject);
        }

        private void OnHitEnemy()
        {
            if (isFireMario || isBig)
            {
                ChangeState(State.Standing);
                Sounds.Play(Sounds.SoundFx.shrink);
                BecomeSmallMario();
            }
            else
                isDying = true;
        }

        public static bool IsBig
        {
            get { return isBig; }
        }

        private void BecomeSmallMario()
        {
            wasHitByEnemy = true;
            Standing.Frame = new Rectangle(0, 0, 16, 16);
            Jumping.Frame = new Rectangle(40, 0, 16, 16);
            Sliding.Frame = new Rectangle(17, 0, 16, 16);
            Running.FramePosition = new Point(81, 0);
            Running.HeightOfFrames = 16;
            Walking.FramePosition = new Point(81, 0);
            Walking.HeightOfFrames = 16;
            position.Y += 16;
            isFireMario = false;
            isBig = false;
        }

        private void BecomeBigMario()
        {
            Standing.Frame = new Rectangle(0, 19, 16, 32);
            Jumping.Frame = new Rectangle(41, 19, 16, 32);
            Sliding.Frame = new Rectangle(21, 19, 16, 32);
            Running.FramePosition = new Point(81, 19);
            Running.HeightOfFrames = 32;
            Walking.FramePosition = new Point(81, 19);
            Walking.HeightOfFrames = 32;
            position.Y -= 16;
            isBig = true;
        }

        private void BecomeFireMario()
        {
            Standing.Frame = new Rectangle(0, 53, 16, 32);
            Jumping.Frame = new Rectangle(41, 53, 16, 32);
            Sliding.Frame = new Rectangle(21, 53, 16, 32);
            Running.FramePosition = new Point(81, 53);
            Walking.FramePosition = new Point(81, 53);
            Crouching.Frame = new Rectangle(61, 63, 16, 22);
            isFireMario = true;
        }
    }
}
