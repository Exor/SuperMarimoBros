﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros
{
    class World
    {
        static List<GameObject> gameObjects;
        static List<GameObject> gameObjectsToAdd;
        TileManager tileManager;
        Camera camera;
        Marimo marimo;
        static bool isUpdating;

        public World(TileManager tm, Marimo marimo)
        {
            this.marimo = marimo;
            tileManager = tm;
            gameObjects = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();
            camera = new Camera(marimo);
            isUpdating = false;
            AddGameObject(marimo);
        }

        public static void AddGameObject(GameObject g)
        {
            if (!isUpdating)
                gameObjects.Add(g);
            else
                gameObjectsToAdd.Add(g);
        }

        public void Update(GameTime gameTime)
        {
            isUpdating = true;
            camera.Update(gameTime);
            tileManager.Update(gameTime);
            //marimo.Update(gameTime);
            //marimo.position.X -= camera.Position.X;
            gameObjects.RemoveAll(x => x.shouldRemove == true);
            foreach (GameObject g in gameObjects)
            {
                g.Update(gameTime);
                g.position.X -= camera.Position.X;
            }
            tileManager.UpdatePosition(camera.Position.X);

            foreach (GameObject g in gameObjects)
            {
                CollisionDetection(g);
            }
            
            isUpdating = false;
            foreach (GameObject g in gameObjectsToAdd)
            {
                gameObjects.Add(g);
            }
            gameObjectsToAdd.RemoveAll(x => x.IsTrue() == true);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tileManager.Draw(spriteBatch);
            foreach (GameObject g in gameObjects)
            {
                g.Draw(spriteBatch);
            }
            //marimo.Draw(spriteBatch);
        }

        public static List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }

        internal void CollisionDetection(GameObject g)
        {
            Point bottomLeft = new Point(g.BoundingRectangle().Left, g.BoundingRectangle().Bottom);
            Point bottomRight = new Point(g.BoundingRectangle().Right, g.BoundingRectangle().Bottom);
            Point topLeft = new Point(g.BoundingRectangle().Left, g.BoundingRectangle().Top);
            Point topRight = new Point(g.BoundingRectangle().Right, g.BoundingRectangle().Top);

            //check for tile collisions
            if (TileManager.SolidTileExistsAt(bottomLeft))
                CollidesWithTile(g, TileManager.ReturnTileAt(bottomLeft));
            if (TileManager.SolidTileExistsAt(bottomRight))
                CollidesWithTile(g, TileManager.ReturnTileAt(bottomRight));
            if (TileManager.SolidTileExistsAt(topLeft))
                CollidesWithTile(g, TileManager.ReturnTileAt(topLeft));
            if (TileManager.SolidTileExistsAt(topRight))
                CollidesWithTile(g, TileManager.ReturnTileAt(topRight));

            if (!TileManager.SolidTileExistsAt(bottomLeft) && !TileManager.SolidTileExistsAt(bottomRight))
                g.isFalling = true;
            else
                g.isFalling = false;

            //check for object collisions
            foreach (GameObject o in GameObjects)
            {
                if (o != g)
                {
                    if (g.BoundingRectangle().Intersects(o.BoundingRectangle()))
                    {
                        //the objects collided!
                        Rectangle collision = Rectangle.Intersect(g.BoundingRectangle(), o.BoundingRectangle());
                        if (collision.Width < collision.Height)
                            g.OnSideCollision(o);
                        else if (collision.Width > collision.Height)
                        {
                            if (g.BoundingRectangle().Y > o.BoundingRectangle().Y)
                                g.OnHeadbutt(o);
                            else if (g.BoundingRectangle().Y < o.BoundingRectangle().Y)
                                g.OnStomp(o);
                        }
                    }
                }
            }
        }

        private void CollidesWithTile(GameObject g, Tile t)
        {
            Rectangle gameObject = g.BoundingRectangle();
            Rectangle tile = t.BoundingRectangle();
            Rectangle collision = Rectangle.Intersect(gameObject, tile);
            if (collision.Width < collision.Height)
            {
                if (gameObject.X > tile.X)
                {
                    g.OnSideCollision(t, collision.X + collision.Width);
                    t.OnSideCollision(g);
                }

                if (gameObject.X < tile.X)
                {
                    g.OnSideCollision(t, collision.X - gameObject.Width + collision.Width);
                    t.OnSideCollision(g);
                }

            }
            else if (collision.Width > collision.Height)
            {
                if (gameObject.Y > tile.Y)
                {
                    g.OnHeadbutt(t, collision.Y + collision.Height);
                    t.OnHeadbutt(g);
                }
                if (gameObject.Y < tile.Y)
                {
                    g.OnStomp(t, collision.Y - gameObject.Height);
                    t.OnStomp(g);
                }
            }
        }
    }
}
