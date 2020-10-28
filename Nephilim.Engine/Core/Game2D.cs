using Nephilim.Engine.Physics;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;
using Nephilim.Engine.World.Systems;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nephilim.Engine.World.Registry;

namespace Nephilim.Engine.Core
{
    public class Game2D : ILayer
    {
        public event Action<Registry> AddSystems;

        private Registry _registry = null;
        private double _fixedUpdateTime = 0f;

        private Task<SceneData> _loadingTask = null;
        private Entity _loadingEntity;
        private Entity _loadingScreenCamera;

        public void OnAdded()
        {
            _registry = new Registry();
        }

        public void OnStart()
        {
            _loadingTask = Scene.LoadSceneAsync(@"../../../Resources/Scenes/DefaultScene.scene");

            _loadingEntity = _registry.CreateAbstractEntity();
            _ =_registry.AddSingletonComponent(_loadingEntity, new LoadingScreenComponent());
            _registry.AddSystem<LoadingScreenSystem>(World.System.UpdateFlags.Update | World.System.UpdateFlags.Render);

            _loadingScreenCamera = _registry.CreateAbstractEntity();
            _ = _registry.AddSingletonComponent(_loadingScreenCamera, new OrthoCameraComponent(_registry.CachedTransform != Matrix4.Identity ? _registry.CachedTransform : Matrix4.Identity));
            
            _registry.ActivateSystems();
        }

        private void OnLoaded(Task<SceneData> task)
        {   
            if(task.IsFaulted)
                Log.Error(task.Exception.Message);
            if (task.IsCompleted && !task.IsFaulted)
            {
                Texture.GenrateTextures();

                _registry.DeactivateSystems();
                _registry.DestroyEntity(_loadingEntity);
                _registry.DestroyEntity(_loadingScreenCamera);

                _registry.LoadSceneData(task.Result);

                var entity1 = _registry.CreateAbstractEntity();
                _ = _registry.AddSingletonComponent(entity1, new OrthoCameraComponent(_registry.CachedTransform != Matrix4.Identity ? _registry.CachedTransform : Matrix4.Identity));

                var entity2 = _registry.CreateAbstractEntity();
                _ = _registry.AddSingletonComponent(entity2, new PhysicsWorld2D());

                AddSystems.Invoke(_registry);

                _registry.AddSystem<ParallaxSystem>(World.System.UpdateFlags.Update);
                _registry.AddSystem<Physics2DSystem>(World.System.UpdateFlags.FixedUpdate | World.System.UpdateFlags.EntitySpawned | World.System.UpdateFlags.EntityDestroyed);
                _registry.AddSystem<CameraShakeSystem>(World.System.UpdateFlags.Update);
                //_registry.AddSystem<LifeTimeSystem>(World.System.UpdateFlags.Update);
                //_registry.AddSystem<EnemySystem>(World.System.UpdateFlags.FixedUpdate | World.System.UpdateFlags.Update);
                //_registry.AddSystem<WeaponSystem>(World.System.UpdateFlags.FixedUpdate);
                _registry.AddSystem<TransformSystem>(World.System.UpdateFlags.EntitySpawned);
                _registry.AddSystem<SpriteRenderSystem>(World.System.UpdateFlags.Render);

#if DEBUG
                _registry.AddSystem<ConsoleSystem>(World.System.UpdateFlags.LateUpdate);
                _registry.AddSystem<DebugCameraSystem>(World.System.UpdateFlags.Update);
                //_registry.AddSystem<ColliderDebugSystem>(World.System.UpdateFlags.Render);
#endif
                _registry.ActivateSystems();
            }
        }

        public void OnUpdateLayer(double dt)
        {
            _registry.UpdateSystems(dt);

            _fixedUpdateTime += dt;

            if(_fixedUpdateTime >= PhysicsGlobals.TimeStep)
            {
                _registry.FixedUpdateSystems();
                _fixedUpdateTime -= PhysicsGlobals.TimeStep;
            }

            _registry.LateUpdateSystems();
        }

        public void OnRenderLayer()
        {
            if (!(_loadingTask is null))
            {
                if (_loadingTask.IsCompletedSuccessfully)
                {
                    if (Texture.TexturesToLoad.Count <= 0)
                    {
                        OnLoaded(_loadingTask);
                        _loadingTask = null;
                    }
                    else
                    {
                        Texture.GenrateTextures();
                    }
                }
                else if (_loadingTask.IsFaulted)
                {
                    throw _loadingTask.Exception;
                }
            }

            _registry.RenderSystems();
            _registry.PostUpdate();
        }

        public void OnDestroy()
        {
            _registry.DestroySystems();
        }

        virtual public void OnAddSystems(ref SystemList systems) { }
    }
}
