using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarimoBros.Enemies
{
    class Koopa : Enemy
    {
        float walkingSpeed = 20f;
        bool wasStomped = false;
        bool wasHitByFireball = false;
        float timeBetweenAnimation = 0.3f;
        float timeToDie = 1f;
        float elapsedGameTime;

        enum CurrentState
        {
            hopping,
            flying,
            walking,
            shell
        };

        
    }
}
