using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarimoBros
{
    public class Player
    {
        static int lives;
        static int score;
        static int world;
        static int level;
        static int coins;

        public Player(int initialLives, int initialWorld, int initialLevel)
        {
            lives = initialLives;
            world = initialWorld;
            level = initialLevel;
            score = 0;
            coins = 0;
        }

        public static void NextLevel()
        {
            if (level == 4)
            {
                level = 1;
                world++;
            }
            else
                level++;
        }

        public static void LoseLife()
        {
            lives--;
        }

        public static void GainLife()
        {
            lives++;
            Sounds.Play(Sounds.SoundFx.oneup);
        }

        public static void AddPoints(int amount)
        {
            score += amount;
        }

        public static void AddCoin()
        {
            Sounds.Play(Sounds.SoundFx.coin);
            AddPoints(100);

            if (coins == 99)
            {
                //1-up!
                GainLife();
                coins = 0;
            }
            else
                coins++;
        }

        public static int Lives
        {
            get { return lives; }
        }

        public static int Score
        {
            get { return score; }
        }

        public static int Level
        {
            get { return level; }
        }

        public static int World
        {
            get { return world; }
        }

        public static int Coins
        {
            get { return coins; }
        }
    }
}
