using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaLibrary
{
    public class TileManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        List<Rectangle> tilePositions;
        List<Tile> tiles;

        Boolean[] isTileSolid;

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
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
            base.Initialize();
        }

        public TileManager (Game game)
            : base (game)
        {

        }

        public void CreateTiles(string level, Texture2D texture)
        {
            Vector2 position = new Vector2(0, 0);

            string[] levelComponents = level.Replace(System.Environment.NewLine, "").Split(',');

            foreach (string piece in levelComponents)
            {
                //if (piece != "0")
                    tiles.Add(new Tile(texture, tilePositions[Convert.ToInt32(piece)], position, isTileSolid[Convert.ToInt32(piece)]));

                position.Y += 16;
                if (position.Y > 224)
                {
                    position.Y = 0;
                    position.X += 16;
                }
            }
        }

        public Boolean SolidTileExistsAt(Point p)
        {
            int x = p.X / 16;
            int y = p.Y / 16;
            int tile = x * 16 + y;
            return tiles[tile].IsSolid();
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Tile t in tiles)
                t.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
