using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros;
using SuperMarimoBros.Blocks;
using System.IO;

namespace SuperMarimoBros
{
    class LevelBuilder
    {
        List<Rectangle> tilePositions;
        static List<BackgroundTile> tiles;
        World world;
        int index = -5;

        static List<string[]> level;

        public LevelBuilder ()
        {
            level = new List<string[]>();

            tiles = new List<BackgroundTile>();
            tilePositions = new List<Rectangle>();
            for (int y = 0; y < 86; y += 17)
                for (int x = 0; x < 360; x += 17)
                    tilePositions.Add(new Rectangle(x, y, 16, 16));
        }

        public static void LoadLevelFile()
        {
            StreamReader streamReader = new StreamReader("Levels/levelOneOne.txt");

            while (!streamReader.EndOfStream)
            {
                string row = streamReader.ReadLine();
                string[] rowElements = row.Split(',');
                level.Add(rowElements);
            }

            streamReader.Close();

            LoadGameObjects();
        }

        private static void LoadGameObjects()
        {
            throw new NotImplementedException();
        }

        public void CreateTiles(string level, World world)
        {
            Texture2D tileTexture = Textures.GetTexture(Textures.Texture.smbTiles);

            this.world = world;
            Vector2 position = new Vector2(0, 0);


            string[] levelComponents = level.Replace(System.Environment.NewLine, "").Split(',');

            foreach (string piece in levelComponents)
            {
                int x = Convert.ToInt32(piece);

                switch (x)
                {
                    case 6:
                        World.AddGameObject(new Brick(position));
                        break;
                    case 701: // create coin block
                        World.AddGameObject(new QuestionBlock(tileTexture, tilePositions[7], position, isTileSolid[7], Textures.GetTexture(Textures.Texture.coinBlockAnimation), Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), QuestionBlock.Contains.Coin));
                        break;
                    case 702: // create mushroom block
                        World.AddGameObject(new QuestionBlock(tileTexture, tilePositions[7], position, isTileSolid[7], Textures.GetTexture(Textures.Texture.coinBlockAnimation), Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), QuestionBlock.Contains.Mushroom));
                        break;
                    default:
                        tiles.Add(new BackgroundTile(tilePositions[x], position));
                        break;
                }

                position.Y += 16;
                if (position.Y > 224)
                {
                    position.Y = 0;
                    position.X += 16;
                }
            }
        }

        public static Boolean SolidTileExistsAt(Point p)
        {
            return false;
        }

        public static BackgroundTile ReturnTileAt(Point p)
        {
            int xOffset = -(int)tiles[0].position.X;

            int x = (xOffset + p.X) / 16;
            int y = p.Y / 16;
            int tile = x * 15 + y;
            if (tile > 0 && tile < tiles.Count)
                return tiles[tile];
            return tiles[0];
        }

        public void UpdatePosition(float amount)
        {
            foreach (BackgroundTile t in tiles)
                t.position.X -= amount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundTile t in tiles)
                t.Draw(spriteBatch);
        }
    }
}
