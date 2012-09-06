using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using SuperMarimoBros.Player;
using SuperMarimoBros.PowerUps;

namespace SuperMarimoBros.Blocks
{
    class QuestionBlock : GameObject
    {
        public enum Contains
        {
            Coin,
            Mushroom,
            Star
        };

        Contains item;

        Animation blockAnimation;
        Animation coinAnimation;
        Sprite emptyBlock;
        float animationSpeed = .15f;
        float coinAnimationSpeed = .01f;
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;
        bool isRegularBlock = false;

        public QuestionBlock(Vector2 position, Contains contains)
            : base(position)
        {
            item = contains;
            blockAnimation = new Animation(Textures.GetTexture(Textures.Texture.coinBlockAnimation), new Rectangle(0,0,16,16), 6, animationSpeed, 0);
            blockAnimation.Position = position;
            blockAnimation.Play();

            emptyBlock = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(34, 85, 16, 16));

            if (item == Contains.Coin)
            {
                coinAnimation = new Animation(Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), new Rectangle(0, 0, 8, 64), 30, coinAnimationSpeed, 0);
                coinAnimation.Position = new Vector2(position.X + 4, position.Y - 48);
                coinAnimation.IsLooping = false;
            }

            bumpAmount = position.Y - bumpAmount;
            originalPosition = position.Y;
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (!isRegularBlock)
            {
                if (item == Contains.Coin)
                {
                    coinAnimation.Play();
                    Sounds.Play(Sounds.SoundFx.coin);
                }
                if (item == Contains.Mushroom)
                {
                    //spawn either mushroom or fire flower
                    if (Marimo.IsBig)
                        World.AddGameObject(new Fireflower(position));
                    else
                        World.AddGameObject(new Mushroom(position));
                }
                wasBumped = true;
                isRegularBlock = true;
                
            }
            base.OnHeadbutt(touchedObject);
        }

        public override void Update(GameTime gameTime)
        {
            blockAnimation.Position = position;
            blockAnimation.Update(gameTime);
            if (item == Contains.Coin)
                coinAnimation.Position = new Vector2(position.X + 4, position.Y - 48);

            if (wasBumped && position.Y >= bumpAmount)
                position.Y -= bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (wasBumped)
                wasBumped = false;
            else if (position.Y < originalPosition)
                position.Y += bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override Rectangle BoundingRectangle()
        {
            return blockAnimation.CurrentFrame;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (wasBumped)
                emptyBlock.Draw(spriteBatch, position, effects);
            else
                blockAnimation.Draw(spriteBatch);
        }
    }
}
