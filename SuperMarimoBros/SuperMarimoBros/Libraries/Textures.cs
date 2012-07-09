using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarimoBros
{
    class Textures
    {
        static List<Texture2D> loadedTextures;

        public enum Texture
        {
            marioSpriteSheet,
            smbTiles,
            coinBlockAnimation,
            coinFromBlockAnimation,
            entities
        };

        public Textures()
        {
            loadedTextures = new List<Texture2D>();
        }

        internal void LoadTextures(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/mariospritesheet"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/smbtiles"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/coinblock"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/coinblockanimation"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/entities"));
            
        }

        public static Texture2D GetTexture(Texture t)
        {
            return loadedTextures[(int)t];
        }
    }
}
