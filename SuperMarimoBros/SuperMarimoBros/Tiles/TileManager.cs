using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarimoBros.Tiles;
using SuperMarimoBros;

namespace SuperMarimoBros
{
    class TileManager
    {
        List<Rectangle> tilePositions;
        static List<Tile> tiles;
        World world;
        Boolean[] isTileSolid;

        public TileManager ()
        {
            tiles = new List<Tile>();
            tilePositions = new List<Rectangle>();
            for (int y = 0; y < 86; y += 17)
                for (int x = 0; x < 360; x += 17)
                    tilePositions.Add(new Rectangle(x, y, 16, 16));
            isTileSolid = new Boolean[132] {
                false, //empty
                true, //brick base
                false, //bush
                false, //bush
                false, //bush
                false, //bush
                true, //brick
                true, //question block
                false, //dunno
                false, //dunno
                false, //dunno
                false, //dunno
                false, //dunno
                true, //white pipe
                true, //white pipe
                true, //green pipe
                true, //green pipe
                true, //red pipe
                true, //red pipe
                true, //mushroom
                true, //mushroom
                true, //mushroom
                false, //castle block
                false, //bush
                false, //bush
                false, //bush
                false, //bush
                false, //bush
                true, //gray brick
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                true, //white pipe
                true, //white pipe
                true, //green pipe
                true, //green pipe
                true, //red pipe
                true, //red pipe
                true, //bullet bill
                false, //mushroom stem
                false, //small tree
                false, //castle
                false, //castle
                false, //castle
                false, //dunno
                true, //blue brick
                true, //blue base
                false, // gray castle
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                false, //cloud
                true, //green pipe
                true, //green pipe
                true, //green pipe
                true, //green pipe
                true, //white pipe
                true, //white pipe
                true, //bullet bill
                false, //mushroom stem
                false, //small tree
                false, //castle
                false, //castle
                false, //castle
                true, //green platform
                true, //green platform
                true, //green platform
                false, //gray castle
                false, //gray castle
                false, //gray castle
                true, //coral
                false, //white tree
                true, //block
                true, //cloud block
                true, //green pipe
                true, //green pipe
                true, //green pipe
                true, //green pipe
                true, //white pipe
                true, //white pipe
                false, //ladder
                false, //small tree
                false, //tree bark
                true, //white mushroom
                true, //white mushroom
                true, //white mushroom
                true, //gray base
                false, //mushroom stem
                false, //mushroom stem
                false, //castle
                false, //castle
                false, //castle
                true, //underwater block
                false, //white tree
                false, //dunno
                false, //dunno
                false, //dunno
                false, //dunno
                false, //dunno
                false, //dunno
                false, //water
                false, //lava
                false, //water
                false, //white tree
                true, //bridge
                true, //block
                true, //block
                true, //block
                true, //block
                true, //block
                false, //coin
                true, //block
                true, //block
                false, //dunno
                false, //lava
                false, //water
                true, //gray brick
                false, //water
                true, //orange pipe
                true, //orange pipe
                true, //orange pipe
                true, //orange pipe
                true, //green block
                false, //dunno
                true, //thin pipe
                true, //whitespace
                true, //whitespace
            };
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
                        tiles.Add(new Brick(tileTexture, tilePositions[x], position, isTileSolid[x]));
                        break;
                    case 701: // create coin block
                        tiles.Add(new QuestionBlock(tileTexture, tilePositions[7], position, isTileSolid[7], Textures.GetTexture(Textures.Texture.coinBlockAnimation), Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), QuestionBlock.Contains.Coin));
                        break;
                    case 702: // create mushroom block
                        tiles.Add(new QuestionBlock(tileTexture, tilePositions[7], position, isTileSolid[7], Textures.GetTexture(Textures.Texture.coinBlockAnimation), Textures.GetTexture(Textures.Texture.coinFromBlockAnimation), QuestionBlock.Contains.Mushroom));
                        break;
                    default:
                        tiles.Add(new Tile(tileTexture, tilePositions[x], position, isTileSolid[x]));
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
            return ReturnTileAt(p).isSolid;
        }

        public static Tile ReturnTileAt(Point p)
        {
            int xOffset = -(int)tiles[0].position.X;

            int x = (xOffset + p.X) / 16;
            int y = p.Y / 16;
            int tile = x * 15 + y;
            if (tile > 0 && tile < tiles.Count)
                return tiles[tile];
            return tiles[0];
        }

        public void Update(GameTime gameTime)
        {
            foreach (Tile t in tiles)
                t.Update(gameTime);
        }

        public void UpdatePosition(float amount)
        {
            foreach (Tile t in tiles)
                t.position.X -= amount;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
        }
    }
}
