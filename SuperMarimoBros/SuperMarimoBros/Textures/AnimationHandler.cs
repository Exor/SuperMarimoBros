using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SuperMarimoBros
{
    class AnimationHandler
    {
        List<Animation> animations;
        SpriteBatch spriteBatch;

        float elapsedTime;

        public AnimationHandler()
        {
            elapsedTime = 0f;
            animations = new List<Animation>();
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Animation animation in animations)
            {
                if (animation.IsPlaying)
                {
                    if (elapsedTime > animation.TimeBetweenFrameTransitions)
                    {
                        elapsedTime -= animation.TimeBetweenFrameTransitions;
                        if (animation.IsLooping)
                        {
                            animation.CurrentFrameNumber = (animation.CurrentFrameNumber + 1) % animation.TotalNumberOfFrames;
                        }
                        else
                        {
                            if (animation.CurrentFrameNumber < animation.TotalNumberOfFrames - 1)
                                animation.CurrentFrameNumber = (animation.CurrentFrameNumber + 1) % animation.TotalNumberOfFrames;
                        }
                    }
                }
                animation.CurrentFrame = new Rectangle
                    (animation.FramePosition.X + animation.CurrentFrameNumber * (animation.WidthOfFrames + animation.FrameBuffer), animation.FramePosition.Y, animation.WidthOfFrames, animation.HeightOfFrames);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Animation animation in animations)
            {
                if (animation.IsPlaying)
                    spriteBatch.Draw(animation.Texture, animation.Position, animation.CurrentFrame, Color.White, animation.Rotation, Vector2.Zero, animation.Scale, animation.Effects, animation.Layer);
            }
        }
    }
}