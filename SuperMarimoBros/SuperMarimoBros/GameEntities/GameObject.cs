using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    public class GameObject
    {
        internal Texture2D texture;
        internal Sprite sprite;
        internal Vector2 position;
        internal SpriteEffects effects;
        internal bool shouldRemove = false;
        internal Rectangle frame;

        public GameObject(Texture2D texture, Rectangle frame, Vector2 position)
        {
            this.texture = texture;
            this.frame = frame;
            sprite = new Sprite(texture, frame);
            this.position = position;
            effects = SpriteEffects.None;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, effects);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void OnHeadbutt(bool isBigMario)
        {
        }

        public virtual void OnStomp()
        {
        }

        public virtual void OnSideCollision()
        {
        }

        public void Remove()
        {
            shouldRemove = true;
        }   
    }
}
