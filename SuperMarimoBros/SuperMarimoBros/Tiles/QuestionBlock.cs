using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaLibrary;

namespace SuperMarimoBros.Tiles
{
    class QuestionBlock : Tile
    {
        AnimationHandler animationHandler;
        Animation animation;
        float animationSpeed = .15f;

        public QuestionBlock(Texture2D texture, Rectangle frame, Vector2 position, Boolean solid, SoundManager sm, AnimationHandler ah, Texture2D questionBlockTexture)
            : base(texture, frame, position, solid, sm)
        {
            animationHandler = ah;
            animation = new Animation(questionBlockTexture, new Point(0, 0), new Point(16, 16), 6, animationSpeed, 0);
            animation.Position = position;
            animation.Play();
            animationHandler.AddAnimation(animation);
        }

        public override void OnHeadbutt()
        {
            animationHandler.DisposeOf(animation);
            Frame = new Rectangle(34, 85, 16, 16);
            base.OnHeadbutt();
        }
    }
}
