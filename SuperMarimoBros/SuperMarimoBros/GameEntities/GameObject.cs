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

        public virtual void OnHeadbutt(GameObject headbutter, GameObject headbuttee)
        {
        }

        public virtual void OnStomp(GameObject stomper, GameObject stompee)
        {
        }

        public virtual void OnSideCollision(GameObject left, GameObject right)
        {
        }

        public void Remove()
        {
            shouldRemove = true;
        }

        public Rectangle BoundingRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, frame.Width, frame.Height);
        }
    }
}
