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

        public static void LoadLevelFile(string levelNumber)
        {
            offset = 0;
            index = 20;
            

            StreamReader streamReader = new StreamReader("Levels/" + levelNumber + ".txt");

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
                    case "77":
                        World.AddObject(new SolidBlock(new Rectangle(187, 51, 16, 16), position));
                        break;
                    case "6":
                        World.AddObject(new Brick(position));
                        break;
                    case "701": // create coin block
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Coin, false));
                        break;
                    case "711":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Coin, true));
                        break;
                    case "702": // create mushroom block
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Mushroom, false));
                        break;
                    case "712":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Mushroom, true));
                        break;
                    case "703":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.OneUp, false));
                        break;
                    case "713":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.OneUp, true));
                        break;
                    case "704":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Star, false));
                        break;
                    case "714":
                        World.AddObject(new QuestionBlock(position, QuestionBlock.Contains.Star, true));
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
                    case "103":
                        World.AddObject(new Spikey(position, Spikey.CurrentState.walking));
                        break;
                    case "104":
                        World.AddObject(new Spikey(position, Spikey.CurrentState.thrown));
                        break;
                    case "99":
                        World.AddObject(new Flagpole(position));
                        break;
                    case "92":
                        World.AddObject(new Pipe(position, 2));
                        break;
                    case "93":
                        World.AddObject(new Pipe(position, 3));
                        break;
                    case "94":
                        World.AddObject(new Pipe(position, 4));
                        break;
                    case "95":
                        World.AddObject(new Pipe(position, 5));
                        break;
                    case "96":
                        World.AddObject(new Pipe(position, 6));
                        break;
                    default:
                        World.AddObject(new BackgroundTile(tilePositions[Convert.ToInt32(row[y])], position));
                        break;
                };
            }


        }

        public void UpdateLevelFrame(float amount)
        {
            offset += amount;
            if (offset >= 16)
            {
                index++;
                if (index < level.Count)
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

    }
}
