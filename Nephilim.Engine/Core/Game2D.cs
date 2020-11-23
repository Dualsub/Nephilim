using Nephilim.Engine.Assets;
using Nephilim.Engine.Input;
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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nephilim.Engine.World.Registry;
using static Nephilim.Engine.World.System;

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

        private string _defaultScene;
        private Bitmap _loadingScreen;

        public Game2D(string defaultScene, Bitmap loadingScreen)
        {
            _defaultScene = string.IsNullOrEmpty(defaultScene) ? "DefaultScene" : defaultScene;
            _loadingScreen = loadingScreen;
        }

        public void OnAdded()
        {
            _registry = new Registry();

            InputManager.KeyDown += (e) =>
            {
                if(e.Key == OpenTK.Windowing.GraphicsLibraryFramework.Keys.Backslash)
                    ReloadScene();
            };
        }

        public void OnStart()
        {
            LoadLevel(_defaultScene);
        }

        private void LoadLevel(string level)
        {
            _loadingEntity = _registry.CreateAbstractEntity();
            var component = new LoadingScreenComponent();
            component.Texture = Texture.LoadTextureUnsafe(_loadingScreen);
            component.Transform *= Matrix4.CreateScale(1f, 1f, 1);
            component.Transform *= Matrix4.CreateFromAxisAngle(new Vector3(1), 0);
            component.Transform *= Matrix4.CreateTranslation(Application.Width * 3f / 10f, -Application.Height * 3f / 10f, 0);
            _ = _registry.AddSingletonComponent(_loadingEntity, component);
            _registry.AddSystem<LoadingScreenSystem>(World.System.UpdateFlags.Update | World.System.UpdateFlags.Render);

            _loadingScreenCamera = _registry.CreateAbstractEntity();
            _ = _registry.AddSingletonComponent(_loadingScreenCamera, new OrthoCameraComponent(_registry.CachedTransform != Matrix4.Identity ? _registry.CachedTransform : Matrix4.Identity));

            _loadingTask = Scene.LoadSceneAsync(level);

            _registry.ActivateSystems();
        }

        private void ReloadScene()
        {
            _registry.FlushEntities();
            LoadLevel(_defaultScene);
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

                Application.ResourceManager.EndLoading();

                _registry.LoadSceneData(task.Result);

                var entity1 = _registry.CreateAbstractEntity();
                _ = _registry.AddSingletonComponent(entity1, new OrthoCameraComponent(_registry.CachedTransform != Matrix4.Identity ? _registry.CachedTransform : Matrix4.Identity));

                var entity2 = _registry.CreateAbstractEntity();
                _ = _registry.AddSingletonComponent(entity2, new PhysicsWorld2D());


                _registry.AddSystem<Physics2DSystem>(UpdateFlags.FixedUpdate | UpdateFlags.EntitySpawned | UpdateFlags.EntityDestroyed);
                _registry.AddSystem<ParallaxSystem>(UpdateFlags.FixedUpdate);
                _registry.AddSystem<CameraShakeSystem>(UpdateFlags.Update);
                _registry.AddSystem<LifeTimeSystem>(UpdateFlags.Update);
                _registry.AddSystem<ParticleSystem>(UpdateFlags.Update);
                _registry.AddSystem<SpriteRenderSystem>(UpdateFlags.Render);
#if DEBUG
                _registry.AddSystem<ConsoleSystem>(UpdateFlags.LateUpdate);
                _registry.AddSystem<DebugCameraSystem>(UpdateFlags.Update);
                _registry.AddSystem<ColliderDebugSystem>(UpdateFlags.Render);
#endif
                AddSystems.Invoke(_registry);
                _registry.ActivateSystems();
            }
        }

        public void OnUpdateLayer(TimeStep ts)
        {
            _registry.UpdateSystems(ts);

            _fixedUpdateTime += ts.DeltaTime;

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
