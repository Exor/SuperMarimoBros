using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarimoBros.Enemies
{
    abstract class Enemy : MovingGameObject
    {
        internal int points = 100;
        internal bool wasHitByFireball;

        public Enemy(Vector2 position)
            : base(position)
        {

        }

        public override void OnSideCollision(GameObject touchedObject)
        {
            velocity.X = -velocity.X;
            base.OnSideCollision(touchedObject);
        }

        internal override void OnTouch(GameObject touchedObject)
        {
            if (touchedObject.GetType().Name == "Fireball")
            {
                UpsideDownDeath();
                FireballExplode((Fireball)touchedObject);
            }
            else if (touchedObject.GetType() == typeof(Marimo) && Marimo.IsStarMario((Marimo)touchedObject))
            {
                UpsideDownDeath();
            }
            else if (touchedObject.GetType().Name == "Shell" && Shell.ShellIsMoving((Shell)touchedObject))
            {
                UpsideDownDeath();
            }
        }

        private void FireballExplode(Fireball fireball)
        {
            fireball.OnTouch(this);
        }

        internal void UpsideDownDeath()
        {
            wasHitByFireball = true;
            isOnSolidTile = false;
            velocity.X = 0;
            velocity.Y = -150f;
            effects = SpriteEffects.FlipVertically;
            Sounds.Play(Sounds.SoundFx.stomp);
            Player.AddPoints(points);
        }
    }
}
