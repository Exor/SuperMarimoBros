using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros.GameEntities
{
    class Mushroom : GameObjectWithGravity
    {
        float spawnSpeed = 20f;
        bool isSpawning = true;
        Vector2 initialPosition;

        public Mushroom(Vector2 position)
            :base(Textures.GetTexture(Textures.Texture.entities), new Rectangle(17,0,16,16), position)
        {
            initialPosition = position;
            velocity.X = 50f;
            Sounds.Play(Sounds.SoundFx.mushroomappear);
            
        }

        public override void Update(GameTime gameTime)
        {
            if (isSpawning)
            {
                SuperMariomoBros.AddDebugMessage("Musroom Spawning");
                position.Y -= spawnSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //SuperMariomoBros.AddDebugMessage(gameTime.ElapsedGameTime.TotalSeconds.ToString());
                if (position.Y <= initialPosition.Y - 16)
                {
                    isSpawning = false;
                    runCollisionDetection = true;
                }
            }
            else
            {
                base.Update(gameTime);
                SuperMariomoBros.AddDebugMessage("Musroom Updating");
            }
            if (position.Y >= 256)
            {
                this.Remove();
                SuperMariomoBros.AddDebugMessage("Musroom Removed");
            }
        }

        public override void OnHeadbutt(GameObject touchedObject)
        {
            isMario(touchedObject);
            base.OnHeadbutt(touchedObject);
        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            isMario(touchedObject);
            base.OnSideCollision(touchedObject);
        }

        public override void OnStomp(GameObject touchedObject)
        {
            isMario(touchedObject);
            base.OnStomp(touchedObject);
        }

        private void isMario(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Marimo")
            {
                this.Remove();
                SuperMariomoBros.AddDebugMessage("Musroom Touched Mario");
            }
        }
    }
}
