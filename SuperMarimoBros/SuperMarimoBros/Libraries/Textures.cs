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
            entities,
            fireflower,
            fireball,
            goomba,
            koopa,
            plant,
            spikey,
            star,
            axe,
            beetle,
            bowser,
            bulletBill,
            cheepCheep,
            coin,
            coinAnimation,
            fire,
            flag,
            hammer,
            koopaRed,
            lakitu,
            squid,
            upFire
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
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/flower"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/fireball"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/goomba"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/koopa"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/plant"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/spikey"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/star"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/axe"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/beetle"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/bowser"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/bulletbill"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/cheepcheep"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/coin"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/coinanimation"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/fire"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/flag"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/hammer"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/koopared"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/lakito"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/squid"));
            loadedTextures.Add(Content.Load<Texture2D>("Graphics/upfire"));
            
        }

        public static Texture2D GetTexture(Texture t)
        {
            return loadedTextures[(int)t];
        }
    }
}
