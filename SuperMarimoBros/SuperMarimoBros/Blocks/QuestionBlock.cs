using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using SuperMarimoBros.PowerUps;

namespace SuperMarimoBros.Blocks
{
    class QuestionBlock : GameObject
    {
        public enum Contains
        {
            Coin,
            Mushroom,
            Star,
            OneUp
        };

        Contains item;

        Animation blockAnimation;
        Animation coinAnimation;
        Sprite emptyBlock;
        Vector2 coinAnimationPosition;
        float animationSpeed = .15f;
        float coinAnimationSpeed = .01f;
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;
        bool isEmpty = false;

        public QuestionBlock(Vector2 position, Contains contains)
            : base(position)
        {
            item = contains;
            blockAnimation = new Animation(Textures.GetTexture(Textures.Texture.coinBlockAnimation), new Rectangle(0,0,16,16), 6, animationSpeed, 0);
            blockAnimation.Play();

            emptyBlock = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(34, 85, 16, 16));

            if (item == Contains.Coin)
            {
                coinAnimation = new Animation(Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), new Rectangle(0, 0, 8, 64), 30, coinAnimationSpeed, 0);
                coinAnimationPosition = new Vector2(position.X + 4, position.Y - 48);
                coinAnimation.IsLooping = false;
                coinAnimation.IsPlaying = false;
            }

            bumpAmount = position.Y - bumpAmount;
            originalPosition = position.Y;
        }

        public override void OnStomp(GameObject touchedObject)
        {
            if (touchedObject.GetType() == typeof(Marimo))
            {
                if (!isEmpty)
                {
                    if (item == Contains.Coin)
                    {
                        coinAnimation.Play();
                        Player.AddCoin();
                    }
                    if (item == Contains.Mushroom)
                    {
                        //spawn either mushroom or fire flower
                        if (Marimo.IsBig)
                            World.AddObject(new Fireflower(position));
                        else
                            World.AddObject(new Mushroom(position, false));
                    }
                    if (item == Contains.OneUp)
                    {
                        World.AddObject(new Mushroom(position, true));
                    }
                    wasBumped = true;
                    isEmpty = true;

                }
            }
            base.OnStomp(touchedObject);
        }

        public override void Update(GameTime gameTime)
        {
            blockAnimation.Update(gameTime);

            if (item == Contains.Coin && coinAnimation.IsPlaying)
            {
                coinAnimationPosition = new Vector2(position.X + 4, position.Y - 48);
                coinAnimation.Update(gameTime);
            }

            if (wasBumped && position.Y >= bumpAmount)
                position.Y -= bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (wasBumped)
                wasBumped = false;
            else if (position.Y < originalPosition)
                position.Y += bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, blockAnimation.CurrentFrame.Width, blockAnimation.CurrentFrame.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (item == Contains.Coin && coinAnimation.IsPlaying)
                coinAnimation.Draw(spriteBatch, coinAnimationPosition, effects);
            if (isEmpty)
                emptyBlock.Draw(spriteBatch, position, effects);
            else
                blockAnimation.Draw(spriteBatch, position, effects);
        }
    }
}
