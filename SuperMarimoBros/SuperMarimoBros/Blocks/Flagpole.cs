using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Blocks
{
    class Flagpole : GameObject
    {
        Sprite pole;
        Sprite flag;
        Sprite ball;

        Vector2 flagPosition;
        static float flagSpeed = 70f;

        bool pullingDownFlag;
        static bool animationFinished;

        public Flagpole(Vector2 position)
            : base(position)
        {
            pole = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(237, 68, 16, 16));
            flag = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(85, 85, 16, 16));
            ball = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(187, 68, 16, 16));
            flagPosition = new Vector2(position.X - 16, position.Y);
        }

        public override Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X + 8, (int)position.Y, 2, 160);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            flag.Draw(spriteBatch, flagPosition, effects);
            ball.Draw(spriteBatch, position, effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 16), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 32), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 48), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 64), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 80), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 96), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 112), effects);
            pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 128), effects);
            //pole.Draw(spriteBatch, new Vector2(position.X, position.Y + 144), effects);
        }

        public override void Update(GameTime gameTime)
        {
            flagPosition.X = position.X - 16;

            if (pullingDownFlag)
            {
                flagPosition.Y += flagSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (flagPosition.Y >= position.Y + 128)
                {
                    pullingDownFlag = false;
                    animationFinished = true;
                }
            }
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                pullingDownFlag = true;
                Sounds.Play(Sounds.Music.levelend);
            }
            base.OnTouch(touchedObject);
        }

        public static float FlagSpeed
        {
            get { return flagSpeed; } 
        }

        public static bool AnimationFinished
        {
            get { return animationFinished; }
        }
    }
}
