using System;
using System.Collections.Generic;
using System.Text;
using Nephilim.Engine.Input;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nephilim.Sandbox.Scripts
{
    public class GunScript : Script
    {
        public override void Start()
        {
            InputManager.KeyDown += (e) => {
                if (e.Key == Keys.K)
                    Fire();
            };
        }

        private void Fire()
        {
            var pos = new Vector3(10, 0, 0);
                //new Vector3(
            //(float)(60f * Math.Cos(armAngle + MathHelper.ThreePiOver2)),
            //(float)(60f * Math.Sin(armAngle + MathHelper.ThreePiOver2)),
            //0);
            var transformComponent = new TransformComponent(
            GetComponent<TransformComponent>().Position + pos,
            new Quaternion(0, 0, 0/*armAngle - MathHelper.PiOver2*/),
            new Vector3(1)
            );
            var rigidBody = new RigidBody2D(1, 0, 0, 0, 0, false, true);
            rigidBody.Velocity = pos.Xy.Normalized() * 50;
            var collider = new BoxCollider(10, 4);
            var renderer = new DebugRenderer(new Vector4(1, 1, 0, 1));
            Registry.SpawnEntity(transformComponent, rigidBody, collider, renderer, new LifeTimeComponent(1), new BulletComponent(10));
        }
    }
}
