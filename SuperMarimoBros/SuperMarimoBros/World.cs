using System;
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
        TileManager tileManager;
        Camera camera;
        Marimo marimo;

        public World(TileManager tm, Marimo marimo)
        {
            this.marimo = marimo;
            tileManager = tm;
            gameObjects = new List<GameObject>();
            camera = new Camera(marimo);
        }

        public void AddGameObject(GameObject g)
        {
            gameObjects.Add(g);
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            tileManager.Update(gameTime);
            marimo.Update(gameTime);
            marimo.position.X -= camera.Position.X;
            gameObjects.RemoveAll(x => x.shouldRemove == true);
            foreach (GameObject g in gameObjects)
            {
                g.Update(gameTime);
                g.position.X -= camera.Position.X;
            }
            tileManager.UpdatePosition(camera.Position.X);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            tileManager.Draw(spriteBatch);
            foreach (GameObject g in gameObjects)
            {
                g.Draw(spriteBatch);
            }
            marimo.Draw(spriteBatch);
        }

        public static List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
    }
}
