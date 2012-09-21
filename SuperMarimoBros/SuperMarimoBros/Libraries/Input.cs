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
    public class Input
    {
        private KeyboardState newState;
        private KeyboardState oldState;

        public Input(Game game)
        {
            newState = new KeyboardState();
            oldState = newState;
        }

        public void Update()
        {
            oldState = newState;
            newState = Keyboard.GetState();
        }

        public bool IsButtonPressed(Keys key)
        {
            return newState.IsKeyDown(key);
        }

        public bool WasButtonReleased(Keys key)
        {
            return (newState.IsKeyUp(key) && oldState.IsKeyDown(key));
        }

        public bool WasButtonPressed(Keys key)
        {
            return (newState.IsKeyDown(key) && oldState.IsKeyUp(key));
        }

        public bool IsHoldingButtonDown(Keys key)
        {
            return (newState.IsKeyDown(key) && oldState.IsKeyDown(key));
        }

        public bool IsAnyButtonPressed()
        {
            return (newState != oldState);
        }
    }
}