﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros.Player;

namespace SuperMarimoBros
{
    class World
    {
        static List<BackgroundTile> tiles;
        static List<BackgroundTile> tilesToAdd;
        static List<GameObject> gameObjects;
        static List<GameObject> gameObjectsToAdd;
        LevelBuilder levelBuilder;
        Camera camera;
        Marimo marimo;
        Rectangle ScreenBoundry;

        public World(LevelBuilder tm, Marimo marimo)
        {
            ScreenBoundry = new Rectangle(-80, -80, 416, 416);
            this.marimo = marimo;
            levelBuilder = tm;
            gameObjects = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();
            tiles = new List<BackgroundTile>();
            tilesToAdd = new List<BackgroundTile>();
            camera = new Camera(marimo);
            AddGameObject(marimo);
        }

        public static void AddGameObject(GameObject g)
        {
            gameObjectsToAdd.Add(g);
        }

        public static void AddBackgroundTile(BackgroundTile t)
        {
            tilesToAdd.Add(t);
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            UpdateTiles(gameTime);
            UpdateGameObject(gameTime);

            levelBuilder.UpdateLevelFrame(camera.Position.X);

            RunCollisionDetection();
            //foreach (GameObject g in gameObjects)
            //{
            //    if (g.runCollisionDetection)
            //        CollisionDetection(g);
            //    else
            //        g.isOnSolidTile = false;
            //}
        }

        private void RunCollisionDetection()
        {
            foreach (GameObject currentObject in gameObjects)
            {
                if (currentObject.GetType().BaseType.ToString() == "SuperMarimoBros.MovingGameObject") //If the object is moving
                {
                    currentObject.isOnSolidTile = false; //Assume it isn't touching anything
                    foreach (GameObject collisionObject in gameObjects)
                    {
                        if (collisionObject != currentObject) //All other objects
                        {
                            if (currentObject.BoundingRectangle().Intersects(collisionObject.BoundingRectangle()))
                            {
                                //2 objects collide


                                Rectangle collision = Rectangle.Intersect(currentObject.BoundingRectangle(), collisionObject.BoundingRectangle());
                                if (collision.Width < collision.Height)
                                {
                                    currentObject.OnSideCollision(collisionObject);
                                }
                                else if (collision.Width > collision.Height)
                                {
                                    if (currentObject.BoundingRectangle().Y > collisionObject.BoundingRectangle().Y)
                                    {
                                        currentObject.OnHeadbutt(collisionObject);
                                        collisionObject.OnHeadbutt(currentObject);
                                    }
                                    else if (currentObject.BoundingRectangle().Y < collisionObject.BoundingRectangle().Y)
                                        currentObject.OnStomp(collisionObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void UpdateGameObject(GameTime gameTime)
        {
            gameObjects.RemoveAll(x => x.shouldRemove == true);
            foreach (GameObject g in gameObjects)
            {
                g.Update(gameTime);
                g.position.X -= camera.Position.X;
                if (IsOutsideScreenBoundries(g.BoundingRectangle()))
                {
                    g.shouldRemove = true;
                }
            }
            foreach (GameObject g in gameObjectsToAdd)
            {
                gameObjects.Add(g);
            }
            gameObjectsToAdd.RemoveAll(x => true);
        }

        private void UpdateTiles(GameTime gameTime)
        {
            tiles.RemoveAll(x => x.shouldRemove == true);

            foreach (BackgroundTile t in tiles)
            {
                t.position.X -= camera.Position.X;
                if (IsOutsideScreenBoundries(new Rectangle((int)t.position.X, (int)t.position.Y, 16, 16)))
                {
                    t.shouldRemove = true;
                }
            }
            foreach (BackgroundTile t in tilesToAdd)
            {
                tiles.Add(t);
            }
            tilesToAdd.RemoveAll(x => true);
        }

        private bool IsOutsideScreenBoundries(Rectangle frame)
        {
            if (ScreenBoundry.Intersects(frame))
                return false;
            else
                return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundTile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            foreach (GameObject g in gameObjects)
            {
                g.Draw(spriteBatch);
            }
        }

        public static List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }

        //internal void CollisionDetection(GameObject g)
        //{
        //    Point bottomLeft = new Point(g.BoundingRectangle().Left, g.BoundingRectangle().Bottom);
        //    Point bottomRight = new Point(g.BoundingRectangle().Right, g.BoundingRectangle().Bottom);
        //    Point topLeft = new Point(g.BoundingRectangle().Left, g.BoundingRectangle().Top);
        //    Point topRight = new Point(g.BoundingRectangle().Right, g.BoundingRectangle().Top);

        //    //check for tile collisions
        //    //if (TileManager.SolidTileExistsAt(bottomLeft))
        //    //    CollidesWithTile(g, TileManager.ReturnTileAt(bottomLeft));
        //    //if (TileManager.SolidTileExistsAt(bottomRight))
        //    //    CollidesWithTile(g, TileManager.ReturnTileAt(bottomRight));
        //    //if (TileManager.SolidTileExistsAt(topLeft))
        //    //    CollidesWithTile(g, TileManager.ReturnTileAt(topLeft));
        //    //if (TileManager.SolidTileExistsAt(topRight))
        //    //    CollidesWithTile(g, TileManager.ReturnTileAt(topRight));

        //    if (LevelBuilder.SolidTileExistsAt(new Point(bottomLeft.X + 2, bottomLeft.Y)) || LevelBuilder.SolidTileExistsAt(new Point(bottomRight.X - 2,bottomRight.Y)))
        //        g.isOnSolidTile = true;
        //    else
        //        g.isOnSolidTile= false;

        //    //check for object collisions
        //    foreach (GameObject o in GameObjects)
        //    {
        //        if (o != g)
        //        {
        //            if (g.BoundingRectangle().Intersects(o.BoundingRectangle()))
        //            {
        //                //the objects collided!
        //                Rectangle collision = Rectangle.Intersect(g.BoundingRectangle(), o.BoundingRectangle());
        //                if (collision.Width < collision.Height)
        //                    g.OnSideCollision(o);
        //                else if (collision.Width > collision.Height)
        //                {
        //                    if (g.BoundingRectangle().Y > o.BoundingRectangle().Y)
        //                        g.OnHeadbutt(o);
        //                    else if (g.BoundingRectangle().Y < o.BoundingRectangle().Y)
        //                        g.OnStomp(o);
        //                }
        //            }
        //        }
        //    }
        //}

        //private void CollidesWithTile(GameObject g, Tile t)
        //{
        //    Rectangle gameObject = g.BoundingRectangle();
        //    Rectangle tile = t.BoundingRectangle();
        //    Rectangle overlap = Rectangle.Intersect(gameObject, tile);

        //    if (overlap.Width < overlap.Height)
        //    {
                

        //        if (gameObject.X > tile.X)
        //        {
        //            g.OnSideCollision(t, overlap.X + overlap.Width);
        //            t.OnSideCollision(g);
        //        }

        //        if (gameObject.X < tile.X)
        //        {
        //            g.OnSideCollision(t, overlap.X - gameObject.Width + overlap.Width);
        //            t.OnSideCollision(g);
        //        }

        //    }
        //    else if (overlap.Width > overlap.Height)
        //    {
        //        if (gameObject.Y > tile.Y)
        //        {
        //            g.OnHeadbutt(t, overlap.Y + overlap.Height);
        //            t.OnHeadbutt(g);
        //        }
        //        if (gameObject.Y < tile.Y)
        //        {
        //            g.OnStomp(t, overlap.Y - gameObject.Height);
        //            t.OnStomp(g);
        //        }
        //    }
        //}
    }
}
