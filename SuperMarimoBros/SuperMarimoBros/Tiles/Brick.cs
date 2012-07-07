using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;

namespace SuperMarimoBros.Tiles
{
    class Brick : Tile
    {
        List<GameObjectWithGravity> brickDebris;
        bool wasBumped = false;
        float bumpSpeed = 75f;
        float bumpAmount = 6; //pixels
        float originalPosition;

        public Brick(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm) 
            : base(texture, frame, position, solid, sm)
        {
            brickDebris = new List<GameObjectWithGravity>();
            originalPosition = position.Y;
        }

        public void OnHeadbutt(bool isBigMario)
        {
            if (isBigMario)
            {
                wasBumped = true;
            }
            else
            {
                //convert the tile to a blank tile
                isSolid = false;
                Frame = new Rectangle(0, 0, 16, 16);

                //create 4 particles that fly out

                brickDebris.Add(new BrickParticle(texture, new Vector2(150f, -200f), position));
                brickDebris.Add(new BrickParticle(texture, new Vector2(-150f, -200f), position));
                brickDebris.Add(new BrickParticle(texture, new Vector2(150f, -150f), position));
                brickDebris.Add(new BrickParticle(texture, new Vector2(-150f, -150f), position));

                soundManager.Play(XnaLibrary.SoundManager.Sound.blockbreak);
            }
            base.OnHeadbutt();
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

            //deal with debris
            brickDebris.RemoveAll(x => x.shouldRemove == true);
            foreach (GameObjectWithGravity debris in brickDebris)
                debris.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (GameObjectWithGravity debris in brickDebris)
                debris.Draw(sb);
            base.Draw(sb);
        }
    }
}
