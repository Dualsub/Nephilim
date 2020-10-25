﻿using Box2DX.Collision;
using Box2DX.Common;
using Box2DX.Dynamics;
using Nephilim.Engine.Physics;
using Nephilim.Engine.Core;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.Util;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Systems
{
	class Physics2DSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            if(registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
            {
				CreatePhysicsWorld(physicsWorld);
				var entities = registry.GetEntitiesWithComponent<RigidBody2D>();
				foreach (var entity in entities)
				{
					if (registry.TryGetComponent(entity, out TransformComponent transform))
					{
						var rigidBody = registry.GetComponent<RigidBody2D>(entity);
						DefineBody(entity, registry, physicsWorld, transform);
					}
				}
			}
        }

        protected override void OnEntitySpawned(Registry registry, EntityID entity)
        {
			if (registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
			{
				if (registry.TryGetComponent(entity, out TransformComponent transform) 
				 && registry.HasComponent<RigidBody2D>(entity))
				{
					DefineBody(entity, registry, physicsWorld, transform);
				}
			}
		}

		protected override void OnEntityDestroyed(Registry registry, EntityID entity)
		{
			if (registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
			{
				if (registry.TryGetComponent(entity, out RigidBody2D rigidBody))
				{
					physicsWorld.World.DestroyBody(rigidBody.Body);
					Log.Print(physicsWorld.World.GetBodyCount());
				}
			}
		}

		protected override void OnFixedUpdate(Registry registry)
        {
			if (registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
			{
				physicsWorld.World.Step(PhysicsGlobals.TimeStep * Application.TimeDilation, PhysicsWorld2D.VelocityIterations, PhysicsWorld2D.PositionIterations);
				var entities = registry.GetEntitiesWithComponent<RigidBody2D>();
				foreach (var entity in entities)
				{
					if (registry.TryGetComponent(entity, out TransformComponent transform))
					{
						if (transform.ParentTag != string.Empty)
							continue;

						var body = registry.GetComponent<RigidBody2D>(entity);
						transform.SetTransform(body.Position, body.Angle);
						if (registry.HasComponent<CameraFollowComponent>(entity) 
							&& registry.TryGetSingletonComponent(out OrthoCameraComponent cameraComponent))
						{
							cameraComponent.SetPosition(transform.Position);
						}
					}
				}

				//TODO: 

				foreach (var entity in registry.GetEntitiesWithComponent<TransformComponent>())
				{
					var transform = registry.GetComponent<TransformComponent>(entity);
					EntityID parent = registry.GetEntityByTag(transform.ParentTag);

					if (parent != default)
					{
						if (registry.TryGetComponent(parent, out TransformComponent parentTransform) 
						&& !(parentTransform is null))
						{
							transform.Transform = transform.DefaultTransform * parentTransform.Transform;
						}
					}
				}
			}

		}

		public static void DefineBody(EntityID entity, Registry registry, PhysicsWorld2D physicsWorld, TransformComponent transform)
		{
			var rigidBody = registry.GetComponent<RigidBody2D>(entity);
			BodyDef bodyDef = new BodyDef();
			bodyDef.Position = Util.UtilFunctions.ToVec2(transform.Position / PhysicsWorld2D.PixelToMeter);
			transform.Rotation.ToAxisAngle(out var axis, out var angle);
			bodyDef.Angle = axis.Z * angle;
			bodyDef.AngularDamping = rigidBody.AngularDamping;
			bodyDef.LinearDamping = rigidBody.LinearDamping;
			bodyDef.FixedRotation = rigidBody.FixedRotation;
			bodyDef.IsSleeping = false;
			bodyDef.IsBullet = rigidBody.IsBullet;
			Body body = physicsWorld.World.CreateBody(bodyDef);
			FixtureDef shapeDef = null;
			if (registry.TryGetComponent(entity, out BoxCollider box)) 
			{ 
				shapeDef = new PolygonDef();
				((PolygonDef)shapeDef).SetAsBox((box.Width / 2) / PhysicsWorld2D.PixelToMeter, (box.Height / 2) / PhysicsWorld2D.PixelToMeter);
			}
			else if(registry.TryGetComponent(entity, out CircleCollider circle)) 
			{ 
				shapeDef = new CircleDef();
				((CircleDef)shapeDef).Radius = circle.Radius;
			}

			shapeDef.Density = rigidBody.Density;
			shapeDef.Friction = rigidBody.Friction;
			shapeDef.Restitution = rigidBody.Restitution;

			body.CreateFixture(shapeDef);

			if(rigidBody.Mass > 0)
			{
				MassData massData = new MassData();
				massData.Mass = rigidBody.Mass;
				massData.Center = body.GetLocalCenter();
				body.SetMass(massData);
			} 
			else
			{
				body.SetMassFromShapes();
			}
			body.SetUserData(entity);
			if (rigidBody.StartVelocity != Vector2.Zero)
			{
				body.SetLinearVelocity(Util.UtilFunctions.ToVec2(rigidBody.StartVelocity));
			}
			rigidBody.SetBody(body);
		}

		private void CreatePhysicsWorld(PhysicsWorld2D physicsWorld)
		{
			AABB worldAABB = new AABB();
			worldAABB.LowerBound.Set(-1000, -1000);
			worldAABB.UpperBound.Set(1000, 1000);
			Vec2 gravity = physicsWorld.Gravity;
			Box2DX.Dynamics.World world = new Box2DX.Dynamics.World(worldAABB, gravity, false);
			physicsWorld.World = world;
			physicsWorld.SetBulletListner();
		}

		protected override void OnDeactivated(Registry registry)
		{
			base.OnDeactivated(registry);
			if (registry.TryGetSingletonComponent(out PhysicsWorld2D physicsWorld))
			{
				physicsWorld.World.Dispose();
			}
		}

	}
}