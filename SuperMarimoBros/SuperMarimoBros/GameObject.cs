using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    public abstract class GameObject
    {
        internal Vector2 position;
        internal SpriteEffects effects;
        internal bool shouldRemove = false;
        internal bool runCollisionDetection = false;
        internal bool isOnSolidTile = false;

        public GameObject(Vector2 position)
        {
            this.position = position;
            effects = SpriteEffects.None;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public abstract Rectangle BoundingRectangle();

        public virtual void OnHeadbutt(GameObject touchedObject)
        {
            OnTouch(touchedObject);
        }

        public virtual void OnStomp(GameObject touchedObject)
        {
            OnTouch(touchedObject);
        }

        public virtual void OnSideCollision(GameObject touchedObject)
        {
            OnTouch(touchedObject);
        }

        internal virtual void OnTouch(GameObject touchedObject)
        {

        }
    }
}
