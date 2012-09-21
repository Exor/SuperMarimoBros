using System;
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
        static List<MovingGameObject> movingGameObjects;
        static List<MovingGameObject> movingGameObjectsToAdd;
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
            movingGameObjects = new List<MovingGameObject>();
            movingGameObjectsToAdd = new List<MovingGameObject>();
            camera = new Camera(marimo);
            AddObject(marimo);
        }

        public static void AddObject(GameObject g)
        {
            gameObjectsToAdd.Add(g);
        }

        public static void AddObject(BackgroundTile t)
        {
            tilesToAdd.Add(t);
        }

        public static void AddObject(MovingGameObject m)
        {
            movingGameObjectsToAdd.Add(m);
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);

            UpdateTiles(gameTime);
            UpdateGameObjects(gameTime);
            UpdateMovingGameObjects(gameTime);

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
            foreach (MovingGameObject currentObject in movingGameObjects)
            {
                currentObject.isOnSolidTile = false; //Assume it isn't touching anything

                foreach (GameObject collisionObject in gameObjects) //Check against non-moving objects
                {
                    if (currentObject.BoundingRectangle().Intersects(collisionObject.BoundingRectangle()))
                    {
                        CheckForCollisions(currentObject, collisionObject, true);
                    }
                }

                foreach (GameObject collisionObject in movingGameObjects) // Check against moving objects
                {
                    if (collisionObject != currentObject) //All other objects
                    {
                        CheckForCollisions(currentObject, collisionObject, false);
                    }
                }

            }
        }

        private void CheckForCollisions(GameObject currentObject, GameObject collisionObject, bool both)
        {
            Rectangle collision = Rectangle.Intersect(currentObject.BoundingRectangle(), collisionObject.BoundingRectangle());
            if (collision.Width < collision.Height)
            {
                currentObject.OnSideCollision(collisionObject);
                if (both)
                    collisionObject.OnSideCollision(currentObject);
            }
            else if (collision.Width > collision.Height)
            {
                if (currentObject.BoundingRectangle().Y > collisionObject.BoundingRectangle().Y)
                {
                    currentObject.OnHeadbutt(collisionObject);
                    if (both)
                        collisionObject.OnStomp(currentObject);
                }
                else if (currentObject.BoundingRectangle().Y < collisionObject.BoundingRectangle().Y)
                {
                    currentObject.OnStomp(collisionObject);
                    if (both)
                        collisionObject.OnHeadbutt(currentObject);
                }
            }
        }

        private void UpdateGameObjects(GameTime gameTime)
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
                if (IsOutsideScreenBoundries(t.BoundingRectangle()))
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

        private void UpdateMovingGameObjects(GameTime gameTime)
        {
            movingGameObjects.RemoveAll(x => x.shouldRemove == true);

            foreach (MovingGameObject m in movingGameObjects)
            {
                m.Update(gameTime);
                m.position.X -= camera.Position.X;
                if (IsOutsideScreenBoundries(m.BoundingRectangle()))
                {
                    m.shouldRemove = true;
                }
            }
            foreach (MovingGameObject m in movingGameObjectsToAdd)
            {
                movingGameObjects.Add(m);
            }
            movingGameObjectsToAdd.RemoveAll(x => true);
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
            foreach (MovingGameObject m in movingGameObjects)
            {
                m.Draw(spriteBatch);
            }
        }
    }
}
