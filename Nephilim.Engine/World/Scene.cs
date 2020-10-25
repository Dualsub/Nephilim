using Nephilim.Engine.Rendering;
using Nephilim.Engine.Util;
using Nephilim.Engine.World.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.GL;
using OpenTK.Windowing.Common;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Immutable;
using System.Threading.Tasks;
using System.Collections;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World
{
    public class Scene
    {

        public static SceneData LoadData(IEnumerable<List<EntityData>> enititySets)
        {
            var components = new Dictionary<Type, Dictionary<EntityID, IComponent>>();
            var singletonComponents = new Dictionary<Type, Tuple<EntityID, ISingletonComponent>>();
            var tags = new Dictionary<string, EntityID>();



            foreach(var entityList in enititySets)
            {
                foreach (var entity in entityList)
                {
                    EntityID id = EntityID.GenerateNew();
                    foreach (var component in entity)
                    {
                        if (component.Item1 == typeof(TagComponent))
                        {
                            string tag = ((TagComponent)component.Item2).Tag;
                            if (!tags.ContainsKey(tag))
                            {
                                tags.Add(tag, id);
                            }
                        }

                        if (!components.ContainsKey(component.Item1))
                        {
                            var newArchetype = new Dictionary<EntityID, IComponent>();
                            newArchetype.Add(id, component.Item2);

                            components.Add(component.Item1, newArchetype);
                        }
                        else if (components.TryGetValue(component.Item1, out Dictionary<EntityID, IComponent> archetype))
                        {
                            archetype.Add(id, component.Item2);
                        }
                    }

                }
            }
           
            return new SceneData(components, singletonComponents, tags);
        }

        public static async Task<SceneData> LoadSceneAsync(string path)
        {
            var serlizerSettings = new JsonSerializerSettings();

            serlizerSettings.Formatting = Formatting.Indented;

            string fileText;

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(fileText))
                return default;

            var tasks = new List<Task<List<EntityData>>>();

            foreach (var prefab in JsonConvert.DeserializeObject<List<SerializedPrefab>>(fileText, serlizerSettings))
            {
                tasks.Add(Task.Run(() => LoadEnity(prefab, serlizerSettings)));
            }

            return LoadData(await Task.WhenAll(tasks));
        }

        private static List<EntityData> LoadEnity(SerializedPrefab serializedPrefab, JsonSerializerSettings serlizerSettings) 
        {
            string fileText;

            using (var sr = new StreamReader(serializedPrefab.Path, Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(fileText))
                return null;

            Dictionary<string, object> components = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileText, serlizerSettings);

            List<EntityData> result = new List<EntityData>();

            Matrix4 entityTransform = Matrix4.Identity;

            EntityData entityData = new EntityData(new List<Tuple<Type, IComponent>>());

            foreach (var component in components)
            {
                Type componentType = Util.UtilFunctions.GetTypeByName(component.Key);
                if (componentType is null)
                    return null;
                IComponent comp = (IComponent)((JObject)component.Value).ToObject(componentType);
                if (comp is TransformComponent && serializedPrefab.Transform != default)
                {
                    var transform = comp as TransformComponent;
                    transform.Transform *= serializedPrefab.Transform;
                    entityTransform = transform.Transform;
                }
                entityData.Components.Add(new Tuple<Type, IComponent>(componentType, comp));
            }

            result.Add(entityData);

            //TODO: Make so that transform is set first.
            foreach (var component in components)
            {
                Type componentType = Util.UtilFunctions.GetTypeByName(component.Key);
                if (componentType is null)
                    return null;
                IComponent comp = (IComponent)((JObject)component.Value).ToObject(componentType);
                if (comp is PrefabComponent)
                {
                    var child = comp as PrefabComponent;
                    foreach(var childEntity in LoadEnity(new SerializedPrefab(child.PrefabFile, entityTransform), serlizerSettings))
                        result.Add(childEntity);
                }
            }

            return result;
        }
    }


    public struct SceneData
    {
        public readonly Dictionary<Type, Dictionary<EntityID, IComponent>> Components;
        public readonly Dictionary<Type, Tuple<EntityID, ISingletonComponent>> SingletonComponents;
        public readonly Dictionary<string, EntityID> Tags;

        public SceneData(Dictionary<Type, Dictionary<EntityID, IComponent>> components, Dictionary<Type, Tuple<EntityID, ISingletonComponent>> singletonComponents, Dictionary<string, EntityID> tags)
        {
            Components = components ?? throw new ArgumentNullException(nameof(components));
            SingletonComponents = singletonComponents ?? throw new ArgumentNullException(nameof(singletonComponents));
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
        }


    }

    public struct EntityData : IEnumerable<Tuple<Type, IComponent>>
    {
        List<Tuple<Type, IComponent>> _components;

        public List<Tuple<Type, IComponent>> Components { get => _components; }

        public EntityData(List<Tuple<Type, IComponent>> components)
        {
            this._components = components ?? throw new ArgumentNullException(nameof(components));
        }

        public IEnumerator<Tuple<Type, IComponent>> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }
    }
}