using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    class GameObjectWithGravity : GameObject
    {
        internal Vector2 velocity;
        internal float friction = 170f;
        internal float gravity = 500f;
        internal float terminalVelocity = 200f;
        internal bool isFalling = false;

        public GameObjectWithGravity(Texture2D texture, Rectangle frame, Vector2 position)
            : base(texture, frame, position)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedGameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CalculateHorizontalVelocity(elapsedGameTime);
            CalculateVerticalVelocity(elapsedGameTime);
            CalculatePosition(elapsedGameTime);
            CollisionDetection();
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
            if (isFalling)
                velocity.Y = velocity.Y + gravity * elapsedGameTime;
        }

        internal void CollisionDetection()
        {
            Point bottomLeft = new Point(BoundingRectangle().Left, BoundingRectangle().Bottom);
            Point bottomRight = new Point(BoundingRectangle().Right, BoundingRectangle().Bottom);
            Point topLeft = new Point(BoundingRectangle().Left, BoundingRectangle().Top);
            Point topRight = new Point(BoundingRectangle().Right, BoundingRectangle().Top);

            if (TileManager.SolidTileExistsAt(bottomLeft))
                CollidesWithTile(TileManager.ReturnTileAt(bottomLeft));
            if (TileManager.SolidTileExistsAt(bottomRight))
                CollidesWithTile(TileManager.ReturnTileAt(bottomRight));
            if (TileManager.SolidTileExistsAt(topLeft))
                CollidesWithTile(TileManager.ReturnTileAt(topLeft));
            if (TileManager.SolidTileExistsAt(topRight))
                CollidesWithTile(TileManager.ReturnTileAt(topRight));

            if (!TileManager.SolidTileExistsAt(bottomLeft) && !TileManager.SolidTileExistsAt(bottomRight))
                isFalling = true;
            else
                isFalling = false;
        }

        private void CollidesWithTile(Tile t)
        {
            Rectangle gameObject = BoundingRectangle();
            Rectangle tile = t.BoundingRectangle();
            Rectangle collision = Rectangle.Intersect(gameObject, tile);
            if (collision.Width < collision.Height)
            {
                if (gameObject.X > tile.X)
                {
                    OnSideCollision(collision.X + collision.Width);
                    t.OnSideCollision(t, this);
                }

                if (gameObject.X < tile.X)
                {
                    OnSideCollision(collision.X - gameObject.Width + collision.Width);
                    t.OnSideCollision(this, t);
                }

            }
            else if (collision.Width > collision.Height)
            {
                if (gameObject.Y > tile.Y)
                {
                    OnHeadbutt(collision.Y + collision.Height);
                    t.OnHeadbutt(this, t);
                }
                if (gameObject.Y < tile.Y)
                {
                    OnStomp(collision.Y - gameObject.Height);
                    t.OnStomp(this, t);
                }
            }
        }

        internal virtual void OnSideCollision(int x)
        {
            position.X = x;
            velocity.X = -velocity.X;
        }

        internal virtual void OnHeadbutt(int y)
        {
            position.Y = y;
            velocity.Y = 0;
        }

        internal virtual void OnStomp(int y)
        {
            position.Y = y;
            velocity.Y = 0;
        }
    }
}
