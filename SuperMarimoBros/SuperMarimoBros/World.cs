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

        public World(TileManager tm, GameObject cameraFocus)
        {
            tileManager = tm;
            gameObjects = new List<GameObject>();
            camera = new Camera(cameraFocus);
        }

        public void AddGameObject(GameObject g)
        {
            gameObjects.Add(g);
        }

        public void Update(GameTime gameTime)
        {
            camera.Update(gameTime);
            tileManager.Update(gameTime);
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
        }

        public static List<GameObject> GameObjects
        {
            get { return gameObjects; }
        }
    }
}
