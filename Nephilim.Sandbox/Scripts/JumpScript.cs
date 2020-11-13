using System;
using System.Collections.Generic;
using System.Text;
using Nephilim.Engine.Input;
using Nephilim.Engine.World.Components;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nephilim.Sandbox.Scripts
{
    public class JumpScript : Script
    {
        public override void Start()
        {
            InputManager.KeyDown += (e) => {
                if (e.Key == Keys.K)
                    GetComponent<RigidBody2D>().Velocity = new OpenTK.Mathematics.Vector2(0, 300);
            };
        }
    }
}
