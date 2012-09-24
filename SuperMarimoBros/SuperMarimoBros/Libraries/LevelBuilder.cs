using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros;
using SuperMarimoBros.Blocks;
using System.IO;
using SuperMarimoBros.Enemies;

namespace SuperMarimoBros
{
    class LevelBuilder
    {
        static List<Rectangle> tilePositions;
        
        static int index;

        static float offset;

        static List<string[]> level;

        public LevelBuilder ()
        {
            level = new List<string[]>();

            tilePositions = new List<Rectangle>();
            for (int y = 0; y < 86; y += 17)
                for (int x = 0; x < 360; x += 17)
                    tilePositions.Add(new Rectangle(x, y, 16, 16));
        }

        public static void LoadLevelFile()
        {
            offset = 0;
            index = 20;
            

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
            for (int x = 0; x <= index; x++)
            {
                string[] row = level[x];

                    LoadRow(x,row);
            }
        }

        private static void LoadRow(int xIndex, string[] row)
        {
            for (int y = 0; y < 15; y++)
            {
                Vector2 position = new Vector2(xIndex * 16, y * 16);

                switch (row[y])
                {
                    case "0":
                        break;
                    case "1":
                        World.AddObject(new SolidBlock(new Rectangle(17, 0, 16, 16), position));
                        break;
                    case "6":
                        World.AddObject(new Brick(position));
                        break;
                    case "701": // create coin block
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Coin));
                        break;
                    case "702": // create mushroom block
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Mushroom));
                        break;
                    case "100":
                        World.AddObject(new Goomba(position));
                        break;
                    case "101":
                        World.AddObject(new Koopa(position, Koopa.CurrentState.walking));
                        break;
                    case "102":
                        World.AddObject(new Koopa(position, Koopa.CurrentState.hopping));
                        break;
                    default:
                        World.AddObject(new BackgroundTile(tilePositions[Convert.ToInt32(row[y])], position));
                        break;
                };
            }


        }

        //public static Boolean SolidTileExistsAt(Point p)
        //{
        //    return false;
        //}

        //public static BackgroundTile ReturnTileAt(Point p)
        //{
        //    int xOffset = -(int)tiles[0].position.X;

        //    int x = (xOffset + p.X) / 16;
        //    int y = p.Y / 16;
        //    int tile = x * 15 + y;
        //    if (tile > 0 && tile < tiles.Count)
        //        return tiles[tile];
        //    return tiles[0];
        //}

        public void UpdateLevelFrame(float amount)
        {
            offset += amount;
            if (offset >= 16)
            {
                index++;
                LoadRow(20, level[index]);
                offset -= 16;
            }
            else if (offset <= -16)
            {
                index--;
                if (index - 25 >= 0)
                {
                    LoadRow(-5, level[index - 25]);
                }
                offset += 16;
            }

        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (BackgroundTile t in tiles)
        //        t.Draw(spriteBatch);
        //}
    }
}
