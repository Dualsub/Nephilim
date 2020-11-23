using Nephilim.Engine.Assets;
using Nephilim.Engine.Core;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nephilim.Engine.World
{
    public class Registry
    {
        public static int entityIDCounter = 0;

        private List<Tuple<System.UpdateFlags, System>> _systems = new List<Tuple<System.UpdateFlags, System>>();

        private Dictionary<Type, Dictionary<EntityID, IComponent>> _components = new Dictionary<Type, Dictionary<EntityID, IComponent>>();
        private Dictionary<Type, Tuple<EntityID, ISingletonComponent>> _singletonComponents = new Dictionary<Type, Tuple<EntityID, ISingletonComponent>>();

        private Dictionary<string, EntityID> _tags = new Dictionary<string, EntityID>();

        private List<Tuple<EntityID, IComponent[]>> _spawnBuffer = new List<Tuple<EntityID, IComponent[]>>();
        private List<EntityID> _destroyBuffer = new List<EntityID>();

        private JsonSerializerSettings _serlizerSettings = new JsonSerializerSettings();

        private bool _pendingSceneChange = false;
        private string _pendingScenePath = string.Empty;

        private string _currentLevel = "";
        private Matrix4 _cachedTransform = Matrix4.Identity;

        public Matrix4 CachedTransform { get => _cachedTransform; }

        public int ComponentTypeCount { get => _components.Keys.Count; }

        public Registry()
        {
            _serlizerSettings.Formatting = Formatting.Indented;
        }

        public Entity SpawnEntity(params IComponent[] components)
        {
            Entity entity = new Entity(GenerateEntityID());
            _spawnBuffer.Add(new Tuple<EntityID, IComponent[]>(entity.ID, components));
            return entity;
        }

        public Entity CreateEntity(string tag = null, params IComponent[] components)
        {
            Entity entity = new Entity(GenerateEntityID());

            AddComponent(entity, new TransformComponent(Matrix4.Identity));
            AddComponent(entity, new TagComponent(tag == null ? entity.ToString() : tag));

            foreach (var component in components)
            {
                AddComponent(entity, component);
            }

            return entity;
        }

        public Entity CreateEntity(string tag, Matrix4 transform)
        {
            Entity entity = new Entity(GenerateEntityID());

            AddComponent(entity, new TransformComponent(transform));
            AddComponent(entity, new TagComponent(tag == null ? entity.ToString() : tag));

            return entity;
        }

        public Entity CreateAbstractEntity()
        {
            return new Entity(GenerateEntityID());
        }

        private void InstatiateEntity(EntityID entity, IComponent[] components)
        {
            foreach (var component in components)
            {
                AddComponent(entity, component.GetType(), component);
            }
        }

        public void DestroyEntity(Entity entity, bool destroyChildren = false)
        {
            DestroyEntity(entity.ID, destroyChildren);
        }

        public void DestroyEntity(EntityID entity, bool destroyChildren = false)
        {
            if(!_destroyBuffer.Contains(entity))
                _destroyBuffer.Add(entity);

            if(TryGetComponent(entity, out TransformComponent transformComponent))
            {
                foreach(var child in transformComponent.Children)
                {
                    if (!_destroyBuffer.Contains(child))
                        _destroyBuffer.Add(child);
                }
            }
        }

        internal void FixedUpdateSystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                if ((_systems[i].Item1 & System.UpdateFlags.FixedUpdate) == System.UpdateFlags.FixedUpdate && _systems[i].Item2.IsActive)
                    _systems[i].Item2.FixedUpdate(this);
            }
        }

        internal void DestroySystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Item2.Deactivate(this);
            }
            _systems.Clear();
        }

        internal void PostUpdate()
        {
            if(_pendingSceneChange)
            {
                LoadSceneInternal(_pendingScenePath);
                _pendingSceneChange = false;
                return;
            }

            foreach (EntityID entity in _destroyBuffer)
            {
                for (int i = 0; i < _systems.Count; i++)
                {
                    if ((_systems[i].Item1 & System.UpdateFlags.EntityDestroyed) == System.UpdateFlags.EntityDestroyed && _systems[i].Item2.IsActive)
                    {
                        _systems[i].Item2.EntityDestroyed(this, entity);
                    }
                }
            }

            foreach (var components in _components.Values)
                foreach(EntityID entity in _destroyBuffer)
                    components.Remove(entity);

            _destroyBuffer.Clear();

            foreach (var components in _spawnBuffer)
            {
                InstatiateEntity(components.Item1, components.Item2);
                for (int i = 0; i < _systems.Count; i++)
                {
                    if ((_systems[i].Item1 & System.UpdateFlags.EntitySpawned) == System.UpdateFlags.EntitySpawned && _systems[i].Item2.IsActive)
                        _systems[i].Item2.EntitySpawned(this, components.Item1);
                }
            }

            _spawnBuffer.Clear();
        }

        internal void DeactivateSystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Item2.Deactivate(this);
            }
        }
        internal void ActivateSystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Item2.Activate(this);
            }
        }

        internal void UpdateSystems(TimeStep ts)
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                if ((_systems[i].Item1 & System.UpdateFlags.Update) == System.UpdateFlags.Update && _systems[i].Item2.IsActive)
                    _systems[i].Item2.Update(this, ts);
            }
        }

        internal void LateUpdateSystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                if ((_systems[i].Item1 & System.UpdateFlags.LateUpdate) == System.UpdateFlags.LateUpdate && _systems[i].Item2.IsActive)
                    _systems[i].Item2.LateUpdate(this);
            }
        }

        public Entity CreateEntityFromPrefab(string path, Matrix4 offset = default)
        {
            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            string fileText;

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }

            Dictionary<string, object> entities = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileText, settings);

            var newEntity = CreateAbstractEntity();
            foreach (var component in entities)
            {
                Type componentType = Type.GetType(component.Key);
                if(componentType is null)
                {
                    componentType = Assembly.GetEntryAssembly().GetType(component.Key);
                }
                IComponent comp = (IComponent)((JObject)component.Value).ToObject(componentType);
                if (comp is TransformComponent && offset != default)
                {
                    var transform = comp as TransformComponent;
                    transform.SetTransform(transform.GetTransform() * offset);
                }
                AddComponent(newEntity, componentType, comp);
            }

            return newEntity;
        }

        internal void RenderSystems()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                if ((_systems[i].Item1 & System.UpdateFlags.Render) == System.UpdateFlags.Render && _systems[i].Item2.IsActive)
                    _systems[i].Item2.Render(this);
            }
        }

        public void AddSystem(Type type, System.UpdateFlags flags, bool insertInFront = true)
        {
            System system = (System)Activator.CreateInstance(type);
            if (insertInFront)
                _systems.Add(new Tuple<System.UpdateFlags, System>(flags, system));
            else
                _systems.Insert(0, new Tuple<System.UpdateFlags, System>(flags, system));
        }

        public void AddSystem<T>(System.UpdateFlags flags, bool insertInFront = true) where T : System
        {
            AddSystem(typeof(T), flags, insertInFront);
        }

        public void AddSystem<T>(System.UpdateFlags flags, int index) where T : System
        {
            System system = (System)Activator.CreateInstance(typeof(T));
            _systems.Insert(0, new Tuple<System.UpdateFlags, System>(flags, system));
        }

        public void ActiveSystem<T>()
        {
            foreach (var system in _systems)
            {
                if (typeof(T) == system.Item2.GetType())
                {
                    system.Item2.IsActive = true;
                }
            }
        }

        public void DeactivateSystem<T>()
        {
            foreach (var system in _systems)
            {
                if (typeof(T) == system.Item2.GetType())
                {
                    Log.Print(typeof(T));
                    system.Item2.IsActive = false;
                }
            }
        }

        public bool AddSingletonComponent<T>(Entity entity, T component) where T : ISingletonComponent
        {
            return AddSingletonComponent(entity.ID, component);
        }

        public bool AddSingletonComponent<T>(EntityID entityID, T component) where T : ISingletonComponent
        {
            if (!_singletonComponents.ContainsKey(typeof(T)))
            {
                var newType = new Tuple<EntityID, ISingletonComponent>(entityID, component);
                _singletonComponents.Add(typeof(T), newType);
                
                return true;
            }
            
            return false;
        }

        public bool AddComponent(Entity entity, Type type, IComponent component)
        {
            return AddComponent(entity.ID, type, component);
        }

        public bool AddComponent(EntityID entityID, Type type, IComponent component) 
        {
            if (type == typeof(TagComponent))
            {
                string tag = ((TagComponent)component).Tag;
                if(!_tags.ContainsKey(tag))
                {
                    _tags.Add(tag, entityID);
                }
            }

            if (!_components.ContainsKey(type))
            {
                var newArchetype = new Dictionary<EntityID, IComponent>();
                newArchetype.Add(entityID, component);

                _components.Add(type, newArchetype);

                return true;
            }
            else if (_components.TryGetValue(type, out Dictionary<EntityID, IComponent> archetype))
            {
                archetype.Add(entityID, component);
                return true;
            }


            return false;
        }

        public bool AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            return AddComponent(entity.ID, component);
        }

        public bool AddComponent<T>(EntityID entityID, T component) where T : IComponent
        {
            if (typeof(T) == typeof(TagComponent))
            {
                _tags.Add((component as TagComponent).Tag, entityID);
            }

            if (!_components.ContainsKey(typeof(T)))
            {
                var newArchetype = new Dictionary<EntityID, IComponent>();
                newArchetype.Add(entityID, component);

                _components.Add(typeof(T), newArchetype);

                return true;
            }
            else if (_components.TryGetValue(typeof(T), out Dictionary<EntityID, IComponent> archetype))
            {
                archetype.Add(entityID, component);
                return true;
            }


            return false;
        }

        public IEnumerable<EntityID> GetEntitiesWithComponent<T>() where T : IComponent
        {
            if(_components.TryGetValue(typeof(T), out Dictionary<EntityID, IComponent> archetype))
            {
                return archetype.Keys;
            }
            else
            {
                return Enumerable.Empty<EntityID>();
            }
        }

        public EntityID GetEntityByTag(string tag)
        {
            if(_tags.TryGetValue(tag, out EntityID entity))
                return entity;

            return default;
        }

        public T GetSingletonComponent<T>(Entity entity) where T : ISingletonComponent
        {
            return GetSingletonComponent<T>(entity.ID);
        }

        public T GetSingletonComponent<T>(EntityID entityID) where T : ISingletonComponent
        {
            if (_singletonComponents.TryGetValue(typeof(T), out Tuple<EntityID, ISingletonComponent> component))
            {
                return (T)component.Item2;
            }
            return default;
        }

        public T GetComponent<T>(Entity entity) where T : IComponent
        {
            return GetComponent<T>(entity.ID);
        }

        public T GetComponent<T>(EntityID entityID) where T : IComponent
        {
            return (T)GetComponent(typeof(T), entityID);
        }

        public object GetComponent(Type type, EntityID entityID)
        {
            if (_components.TryGetValue(type, out Dictionary<EntityID, IComponent> archetype))
            {
                archetype.TryGetValue(entityID, out IComponent component);
                return component;
            }
            return default;
        }

        public IEnumerable<IComponent> GetAllComponentsOfType<T>() where T : IComponent
        {
            if (_components.TryGetValue(typeof(T), out var components))
                return components.Values;
            else
                return null;
        }
        public bool TryGetComponent<T>(Entity entity, out T outComponent) where T : IComponent
        {
            if (_components.TryGetValue(typeof(T), out Dictionary<EntityID, IComponent> archetype))
            {
                archetype.TryGetValue(entity.ID, out IComponent component);

                outComponent = (T)component;

                return true;
            }

            outComponent = default;
            return false;
        }

        public bool TryGetComponent<T>(EntityID entityID, out T outComponent) where T : IComponent
        {
            if (_components.TryGetValue(typeof(T), out Dictionary<EntityID, IComponent> archetype) 
            && archetype.TryGetValue(entityID, out IComponent component))
            {
                outComponent = (T)component;
                return true;
            }
            outComponent = default;
            return false;
        }

        public bool TryGetSingletonComponent<T>(out T outComponent)
        {
            if (_singletonComponents.TryGetValue(typeof(T), out Tuple<EntityID, ISingletonComponent> component))
            {
                outComponent = (T)component.Item2;
                return true;
            }
            outComponent = default;
            return false;
        }

        public bool HasComponent<T>(Entity entity) where T : IComponent
        {
            return HasComponent<T>(entity.ID);
        }

        public bool HasComponent<T>(EntityID entityID) where T : IComponent
        {
            if (!_components.TryGetValue(typeof(T), out Dictionary<EntityID, IComponent> archetype))
                return false;

            return archetype.ContainsKey(entityID);
        }

        private EntityID GenerateEntityID()  
        {
            return EntityID.GenerateNew();
        }

        public void FlushEntities()
        {
            _components.Clear();
            _singletonComponents.Clear();
            _tags.Clear();
        }

        public void LoadSceneUnsafe(string path)
        {
            LoadSceneInternal(path);
        }

        public void LoadScene(string path)
        {
            _pendingSceneChange = true;
            _pendingScenePath = path;
        }

        internal void LoadSceneData(SceneData data)
        {
            Log.Print($"Recived Scene Data.\n There where{data.Components.Values} types of components \nand {data.Tags.Count} tags.");
            _components = data.Components;
            _singletonComponents = data.SingletonComponents;
            _tags = data.Tags;
        }

        private void LoadSceneInternal(string path)
        {
            if(_systems.Count > 0)
                DeactivateSystems();
            FlushEntities();

            var entity = CreateAbstractEntity();
            AddSingletonComponent(entity, new OrthoCameraComponent(_cachedTransform != Matrix4.Identity ? _cachedTransform : Matrix4.Identity));
            var entity2 = CreateAbstractEntity();
            AddSingletonComponent(entity2, new PhysicsWorld2D());


            string fileText;

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(fileText))
                return;

            foreach(var prefab in JsonConvert.DeserializeObject<List<SerializedPrefab>>(fileText, _serlizerSettings))
            {
                try
                {
                    CreateEntityFromPrefab(prefab.Name, prefab.Transform);
                }
                catch (Exception)
                {
                    throw new Exception($"There was an error when reading the prefabfile {Path.GetFileNameWithoutExtension(prefab.Name)} in the path:{prefab.Name}.");
                }
            }

            _currentLevel = path;
            ActivateSystems();
        }

        public void ReloadScene()
        {
            if(TryGetSingletonComponent(out OrthoCameraComponent orthoCamera) && GetAllComponentsOfType<CameraFollowComponent>().Count() <= 0)
            {
                _cachedTransform = Matrix4.CreateTranslation(orthoCamera.Position);
            }
            LoadScene(_currentLevel);
        }

    }
}
