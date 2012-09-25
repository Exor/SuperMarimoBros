using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using SuperMarimoBros.Enemies;
using SuperMarimoBros.Blocks;
using SuperMarimoBros.PowerUps;

namespace SuperMarimoBros
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
        bool isDead;
        bool wasHitByEnemy;
        bool isOnFlagpole;
        bool runFinishAnimation;
        bool levelIsFinished;

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

            if (runFinishAnimation)
            {
                timer += elapsedGameTime;
                if (timer >= 5)
                    levelIsFinished = true;
                else
                {
                    ChangeState(State.Walking);
                    velocity.X = 50f;
                    base.Update(gt);
                }
            }
            else if (isOnFlagpole)
            {
                ChangeState(State.Standing);
                if (!isOnSolidTile)
                    position.Y += Flagpole.FlagSpeed * elapsedGameTime;
                if (Flagpole.AnimationFinished == true)
                {
                    runFinishAnimation = true;
                    position.X += 18;
                }
            }
            else if (isDying)
            {
                ChangeState(State.Dying);
                base.Update(gt);
                timer += elapsedGameTime;
                if (timer > 2.5f)
                {
                    isDead = true;
                    Player.LoseLife();
                }

            }
            else if (wasHitByEnemy)
            {
                velocity = Vector2.Zero;
                timer += elapsedGameTime;
                if (timer > 1)
                {
                    wasHitByEnemy = false;
                    timer = 0;
                }
            }
            else
            {
                //Mario falls off the screen
                if (position.Y > 250)
                {
                    //position.Y = -32;
                    InitiateDeath();
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
                    return Rectangle.Empty;
                case State.Crouching:
                    return new Rectangle((int)position.X, (int)position.Y, Crouching.Frame.Width, Crouching.Frame.Height);
                case State.Firing:
                    return new Rectangle((int)position.X, (int)position.Y, Firing.Frame.Width, Firing.Frame.Height);
                default:
                    return Rectangle.Empty;
            }
        }
        
        public override void OnSideCollision(GameObject touchedObject)
        {
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Enemies")
            {
                if (touchedObject.GetType().Name == "Shell")
                { 
                    DoShellStuff((Shell)touchedObject);
                    //Need to check if the koopa shell is moving or not
                }
                else
                    OnHitEnemy();
            }
            base.OnSideCollision(touchedObject);
        }


        private void DoShellStuff(Shell shell)
        {
            if (shell.velocity.X != 0)
                OnHitEnemy();
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
            base.OnStomp(touchedObject);
            if (touchedObject.GetType().Namespace == "SuperMarimoBros.Enemies")
            {
                velocity.Y = -100f;
                Sounds.Play(Sounds.SoundFx.stomp);
            }
            
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Mushroom")
            {
                DealWithMushroom((Mushroom)touchedObject);
            }
            else if (touchedObject.GetType().Name == "Fireflower")
            {
                Sounds.Play(Sounds.SoundFx.mushroomeat);
                BecomeFireMario();
            }
            else if (touchedObject.GetType().Name == "Flagpole")
            {
                isOnFlagpole = true;
                velocity = Vector2.Zero;
            }

            base.OnTouch(touchedObject);
        }

        private void DealWithMushroom(Mushroom mushroom)
        {
            if (mushroom.IsOneUp)
            {
                Player.GainLife();
            }
            else
            {
                Sounds.Play(Sounds.SoundFx.mushroomeat);
                BecomeBigMario();
            }
        }

        private void OnHitEnemy()
        {
            if (isFireMario || isBig)
            {
                wasHitByEnemy = true;
                ChangeState(State.Standing);
                Sounds.Play(Sounds.SoundFx.shrink);
                BecomeSmallMario();
            }
            else
            {
                InitiateDeath();
            }
        }

        private void BecomeSmallMario()
        {
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

        private void InitiateDeath()
        {
            isDying = true;
            velocity.Y = -200;
            velocity.X = 0;
            Sounds.Play(Sounds.Music.death);
        }

        public static bool IsBig
        {
            get { return isBig; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public bool WasHitByEnemy
        {
            get { return wasHitByEnemy; }
        }

        public bool LevelIsFinished
        {
            get { return levelIsFinished; }
        }
    }
}
