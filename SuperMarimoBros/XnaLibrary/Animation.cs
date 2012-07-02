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
    public class Animation
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

        Rectangle? currentFrame;

        Point framePosition;

        Vector2 position;

        float rotation;
        SpriteEffects spriteEffects;
        Vector2 scale;
        float layer;

        //public Animation(string textureLocation, ContentManager content, float timeBetweenFrameTransitions, int numberOfFrames)
        //{
        //    texture = content.Load<Texture2D>(textureLocation);
        //    this.timeBetweenFrameTransitions = timeBetweenFrameTransitions;

        //    totalNumberOfFrames = numberOfFrames;
        //    widthOfFrames = texture.Width / totalNumberOfFrames;
        //    heightOfFrames = texture.Height;
        //    currentFrameNumber = 0;

        //    isLooping = true;
        //    isPlaying = false;

        //    position = Vector2.Zero;

        //    rotation = 0f;
        //    spriteEffects = SpriteEffects.None;
        //    scale = Vector2.One;
        //    layer = 0f;
        //}

        public Animation(Texture2D texture, Point framePosition, Point frameSize, int numberOfFrames, float timeBetweenFrameTransitions, int frameBuffer)
        {
            this.texture = texture;
            this.timeBetweenFrameTransitions = timeBetweenFrameTransitions;
            totalNumberOfFrames = numberOfFrames;
            widthOfFrames = frameSize.X;
            heightOfFrames = frameSize.Y;
            this.framePosition = framePosition;
            this.frameBuffer = frameBuffer;

            isLooping = true;
            isPlaying = false;

            currentFrameNumber = 0;

            position = Vector2.Zero;
            rotation = 0f;
            spriteEffects = SpriteEffects.None;
            scale = Vector2.One;
            layer = 0f;
        }

        //public Animation(string textureLocation,
        //    ContentManager content,
        //    float timeBetweenFrameTransitions,
        //    int totalNumberOfFrames,
        //    int widthOfFrames,
        //    int heightOfFrames,
        //    bool isLooping,
        //    bool isPlaying,
        //    Vector2 position)
        //{
        //    texture = content.Load<Texture2D>(textureLocation);
        //    this.timeBetweenFrameTransitions = timeBetweenFrameTransitions;

        //    this.totalNumberOfFrames = totalNumberOfFrames;
        //    this.widthOfFrames = widthOfFrames;
        //    this.heightOfFrames = texture.Height;
        //    currentFrameNumber = 0;

        //    this.isLooping = isLooping;
        //    this.isPlaying = isPlaying;

        //    this.position = position;
        //}

        public Point FramePosition
        {
            get { return framePosition; }
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
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Rectangle? CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public SpriteEffects Effects
        {
            get { return spriteEffects; }
            set { spriteEffects = value; }
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
    }
}