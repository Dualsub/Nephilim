using Nephilim.Engine.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Sandbox.Components
{
    enum MovementState
    {
        Idle, Moving, Acending, Decending, None
    }

    class PlayerStateComponent : IComponent
    {
        public bool WantToAttack { get; set; } = false;

        MovementState MovementState { get; set; } = MovementState.Idle;

    }
}
