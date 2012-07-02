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


namespace XnaLibrary
{
    public class AnimationHandler : Microsoft.Xna.Framework.DrawableGameComponent
    {
        List<Animation> animations;
        SpriteBatch spriteBatch;

        float elapsedTime;

        public AnimationHandler(Game game)
            : base(game)
        {
            
        }

        public void AddAnimation(Animation animation)
        {
            animations.Add(animation);
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            elapsedTime = 0f;
            animations = new List<Animation>();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (Animation animation in animations)
            {
                if (animation.IsPlaying)
                    spriteBatch.Draw(animation.Texture, animation.Position, animation.CurrentFrame, Color.White, animation.Rotation, Vector2.Zero, animation.Scale, animation.Effects, animation.Layer);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}