﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.Blocks
{
    class Brick : GameObject
    {
        int points = 100;
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

        public override void OnStomp(GameObject touchedObject)
        {
            if (touchedObject.GetType() == typeof(Marimo))
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
                    World.AddObject(new BrickParticle(new Vector2(150f, -200f), position));
                    World.AddObject(new BrickParticle(new Vector2(-150f, -200f), position));
                    World.AddObject(new BrickParticle(new Vector2(150f, -150f), position));
                    World.AddObject(new BrickParticle(new Vector2(-150f, -150f), position));

                    //Add the score
                    Player.AddPoints(points);

                    Sounds.Play(SuperMarimoBros.Sounds.SoundFx.blockbreak);
                }
            }
            base.OnStomp(touchedObject);
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
            return new Rectangle((int)position.X, (int)position.Y, brick.Frame.Width, brick.Frame.Height);
        }
    }
}
