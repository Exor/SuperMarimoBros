using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;

namespace SuperMarimoBros.Tiles
{
    class QuestionBlock : Tile
    {
        AnimationHandler animationHandler;
        Animation blockAnimation;
        Animation coinAnimation;
        float animationSpeed = .15f;
        float coinAnimationSpeed = .01f;
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;
        bool isRegularBlock = false;

        public QuestionBlock(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm, AnimationHandler ah, Texture2D questionBlockTexture, Texture2D coinBlockAnimation)
            : base(texture, frame, position, solid, sm)
        {
            animationHandler = ah;
            blockAnimation = new Animation(questionBlockTexture, new Point(0, 0), new Point(16, 16), 6, animationSpeed, 0);
            blockAnimation.Position = position;
            blockAnimation.Play();
            animationHandler.AddAnimation(blockAnimation);

            coinAnimation = new Animation(coinBlockAnimation, new Point(0, 0), new Point(8, 64), 30, coinAnimationSpeed, 0);
            coinAnimation.Position = new Vector2(position.X + 4, position.Y - 48);
            coinAnimation.IsLooping = false;
            animationHandler.AddAnimation(coinAnimation);

            bumpAmount = position.Y - bumpAmount;
            originalPosition = position.Y;
        }

        public override void OnHeadbutt()
        {
            if (!isRegularBlock)
            {
                coinAnimation.Play();
                animationHandler.DisposeOf(blockAnimation);
                Frame = new Rectangle(34, 85, 16, 16);
                wasBumped = true;
                isRegularBlock = true;
                soundManager.Play(SoundManager.Sound.coin);
            }
            base.OnHeadbutt();
        }

        public override void Update(GameTime gameTime)
        {
            blockAnimation.Position = position;
            coinAnimation.Position = new Vector2(position.X + 4, position.Y - 48);

            if (wasBumped && position.Y >= bumpAmount)
                position.Y -= bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (wasBumped)
                wasBumped = false;
            else if (position.Y < originalPosition)
                position.Y += bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                
                

            base.Update(gameTime);
        }
    }
}
