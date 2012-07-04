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
        List<GameEntity> brickDebris;
        
        public Brick(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm) 
            : base(texture, frame, position, solid, sm)
        {
            brickDebris = new List<GameEntity>();
        }

        public override void OnHeadbutt()
        {


            //convert the tile to a blank tile
            isSolid = false;
            Frame = new Rectangle(0, 0, 16, 16);
            
            //create 4 particles that fly out
            
            brickDebris.Add(new BrickParticle(Texture, new Vector2(150f, -200f), Position));
            brickDebris.Add(new BrickParticle(Texture, new Vector2(-150f, -200f), Position));
            brickDebris.Add(new BrickParticle(Texture, new Vector2(150f, -150f), Position));
            brickDebris.Add(new BrickParticle(Texture, new Vector2(-150f, -150f), Position));

            soundManager.Play(XnaLibrary.SoundManager.Sound.blockbreak);

            base.OnHeadbutt();
        }

        public override void Update(GameTime gameTime)
        {
            brickDebris.RemoveAll(x => x.shouldRemove == true);
            foreach (GameEntity debris in brickDebris)
                debris.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (GameEntity debris in brickDebris)
                debris.Draw(sb);
            base.Draw(sb);
        }
    }
}
