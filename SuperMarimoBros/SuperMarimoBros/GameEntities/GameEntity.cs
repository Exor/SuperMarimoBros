using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros
{
    internal class GameEntity
    {
        internal Texture2D texture;
        internal Sprite sprite;
        internal Vector2 position;
        internal Vector2 velocity;
        internal SpriteEffects effects;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 120f;

        internal bool shouldRemove = false;

        public virtual void Load(Texture2D texture, Rectangle frame, Vector2 position)
        {
            sprite = new Sprite(texture, new Point(frame.X, frame.Y), new Point(frame.Width, frame.Height));
            this.position = position;
            effects = SpriteEffects.None;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch, position, effects);
        }

        public virtual void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CalculateHorizontalVelocity(elapsedGameTime);
            CalculateVerticalVelocity(elapsedGameTime);
            CalculatePosition(elapsedGameTime);
        }


        internal virtual void CalculatePosition(float elapsedGameTime)
        {
            position.X = position.X + (velocity.X * elapsedGameTime);
            position.Y = position.Y + (velocity.Y * elapsedGameTime);
        }

        internal virtual void CalculateHorizontalVelocity(float elapsedGameTime)
        {

        }

        internal virtual void CalculateVerticalVelocity(float elapsedGameTime)
        {
            velocity.Y = velocity.Y + gravity * elapsedGameTime;
        }

        public virtual void OnHeadbutt()
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
