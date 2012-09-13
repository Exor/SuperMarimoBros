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
    class Animation
    {
        Texture2D texture;

        float timeBetweenFrameTransitions;

        int widthOfFrames;
        int heightOfFrames;
        int totalNumberOfFrames;
        int currentFrameNumber;

        int frameBuffer;

        bool isLooping;
        bool isPlaying;

        Rectangle currentFrame;

        Point framePosition;

        float rotation;
        Vector2 scale;
        float layer;

        float elapsedTime;

        public Animation(Texture2D texture, Rectangle initialFrame, int numberOfFrames, float timeBetweenFrameTransitions, int frameBuffer)
        {
            this.texture = texture;
            this.timeBetweenFrameTransitions = timeBetweenFrameTransitions;
            totalNumberOfFrames = numberOfFrames;
            widthOfFrames = initialFrame.Width;
            heightOfFrames = initialFrame.Height;
            framePosition = new Point(initialFrame.X, initialFrame.Y);
            this.frameBuffer = frameBuffer;

            isLooping = true;
            isPlaying = true;

            currentFrameNumber = 0;
            rotation = 0f;
            scale = Vector2.One;
            layer = 0f;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effects)
        {
            if (IsPlaying)
                spriteBatch.Draw(Texture, position, CurrentFrame, Color.White, Rotation, Vector2.Zero, Scale, effects, Layer);
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (ElapsedTime > TimeBetweenFrameTransitions)
                {
                    ElapsedTime -= TimeBetweenFrameTransitions;
                    if (IsLooping)
                    {
                        CurrentFrameNumber = (CurrentFrameNumber + 1) % TotalNumberOfFrames;
                    }
                    else
                    {
                        if (CurrentFrameNumber < TotalNumberOfFrames - 1)
                            CurrentFrameNumber = (CurrentFrameNumber + 1) % TotalNumberOfFrames;
                        else
                            IsPlaying = false;
                    }
                }
            }
            CurrentFrame = new Rectangle
                (FramePosition.X + CurrentFrameNumber * (WidthOfFrames + FrameBuffer), FramePosition.Y, WidthOfFrames, HeightOfFrames);
        }

        public Point FramePosition
        {
            get { return framePosition; }
            set { framePosition = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public int FrameBuffer
        {
            get { return frameBuffer; }
        }

        public float TimeBetweenFrameTransitions
        {
            get { return timeBetweenFrameTransitions; }
        }

        public int WidthOfFrames
        {
            get { return widthOfFrames; }
        }

        public int HeightOfFrames
        {
            get { return heightOfFrames; }
            set { heightOfFrames = value; }
        }

        public int TotalNumberOfFrames
        {
            get { return totalNumberOfFrames; }
        }

        public int CurrentFrameNumber
        {
            get { return currentFrameNumber; }
            set { currentFrameNumber = value; }
        }

        public bool IsLooping
        {
            get { return isLooping; }
            set { isLooping = value; }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }

        public Rectangle CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public void Play()
        {
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
        }

        public float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
    }
}