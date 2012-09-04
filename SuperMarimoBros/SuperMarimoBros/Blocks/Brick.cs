using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;
using SuperMarimoBros.Player;

namespace SuperMarimoBros.Blocks
{
    class Brick : GameObject
    {
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;
        Sprite brick;

        public Brick(Vector2 position) 
            : base(position)
        {
            brick = new Sprite(Textures.GetTexture(Textures.Texture.smbTiles), new Rectangle(102, 0, 16, 16));
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
                shouldRemove = true;

                //create 4 particles that fly out
                World.AddGameObject(new BrickParticle(new Vector2(150f, -200f), position));
                World.AddGameObject(new BrickParticle(new Vector2(-150f, -200f), position));
                World.AddGameObject(new BrickParticle(new Vector2(150f, -150f), position));
                World.AddGameObject(new BrickParticle(new Vector2(-150f, -150f), position));

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
        }

        public override void Draw(SpriteBatch sb)
        {
            brick.Draw(sb, position, effects);
        }

        public override Rectangle BoundingRectangle()
        {
            return brick.Frame;
        }
    }
}
