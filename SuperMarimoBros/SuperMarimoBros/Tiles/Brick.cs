using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.Tiles
{
    class Brick : Tile
    {
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;

        public Brick(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid) 
            : base(texture, frame, position, solid)
        {
            bumpAmount = position.Y - bumpAmount;
            originalPosition = position.Y;
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            if (!Marimo.IsBig)
            {
                wasBumped = true;
            }
            else
            {
                //convert the tile to a blank tile
                isSolid = false;
                Frame = new Rectangle(0, 0, 16, 16);

                //create 4 particles that fly out
                World.AddGameObject(new BrickParticle(texture, new Vector2(150f, -200f), position));
                World.AddGameObject(new BrickParticle(texture, new Vector2(-150f, -200f), position));
                World.AddGameObject(new BrickParticle(texture, new Vector2(150f, -150f), position));
                World.AddGameObject(new BrickParticle(texture, new Vector2(-150f, -150f), position));

                Sounds.Play(SuperMarimoBros.Sounds.SoundFx.blockbreak);
            }
            base.OnHeadbutt(touchedObject);
        }

        public override void Update(GameTime gameTime)
        {
            //bump the block if it was bumped by mario
            if (wasBumped && position.Y >= bumpAmount)
                position.Y -= bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (wasBumped)
                wasBumped = false;
            else if (position.Y < originalPosition)
                position.Y += bumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
