using Nephilim.Engine.Assets.Loaders;
using Nephilim.Engine.Audio;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Assets
{
    public class ResourceManager
    {

        private ResourceData _resources;
        private Task<SceneData> _loadingTask;

        private Dictionary<Type, ILoader> _loaders = new Dictionary<Type, ILoader>()
        {
            { typeof(Texture), new TextureLoader() },
            { typeof(AudioClip), new AudioLoader() },
            { typeof(string), new AssetLoader() }
        };

        public bool IsLoading { get; private set; } = false;

        public void StartLoading()
        {
            IsLoading = true;

            string path = UtilFunctions.FindFilePath("Resources/resources.dat");

            FileStream fs = new FileStream(path, FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress, false))
                {
                    _resources = (ResourceData)formatter.Deserialize(zipStream);
                }
            }
            catch (SerializationException e)
            {
                Log.Print("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                Log.Print($"Packaging Successful!");
            }
        }

        public T Load<T>(string name)
        {
            //if(!IsLoading)
            //    throw new Exception($"Trying to load while loading time is over.");

            if (_loaders.TryGetValue(typeof(T), out var loader))
            {
                var asset = (T)loader.Load(name, _resources);
                if (asset is null)
                    throw new Exception($"There was an error when loading the asset named: {name}");
                else
                    return asset;
            }
                
            else
                throw new Exception($"There was no asset named: {name}");
        }

        public void EndLoading()
        {
            IsLoading = false;
        }

        public void Update()
        {

        }

        private void PushQue()
        {

        }

        //public static SceneData LoadData(IEnumerable<List<EntityData>> enititySets)
        //{
        //    var components = new Dictionary<Type, Dictionary<EntityID, IComponent>>();
        //    var singletonComponents = new Dictionary<Type, Tuple<EntityID, ISingletonComponent>>();
        //    var tags = new Dictionary<string, EntityID>();



        //    foreach (var entityList in enititySets)
        //    {
        //        foreach (var entity in entityList)
        //        {
        //            EntityID id = EntityID.GenerateNew();
        //            foreach (var component in entity)
        //            {
        //                if (component.Item1 == typeof(TagComponent))
        //                {
        //                    string tag = ((TagComponent)component.Item2).Tag;
        //                    if (!tags.ContainsKey(tag))
        //                    {
        //                        tags.Add(tag, id);
        //                    }
        //                }

        //                if (!components.ContainsKey(component.Item1))
        //                {
        //                    var newArchetype = new Dictionary<EntityID, IComponent>();
        //                    newArchetype.Add(id, component.Item2);

        //                    components.Add(component.Item1, newArchetype);
        //                }
        //                else if (components.TryGetValue(component.Item1, out Dictionary<EntityID, IComponent> archetype))
        //                {
        //                    archetype.Add(id, component.Item2);
        //                }
        //            }

        //        }
        //    }

        //    return new SceneData(components, singletonComponents, tags);
        //}

        //public static async Task<SceneData> LoadSceneAsync(string path)
        //{
        //    var serlizerSettings = new JsonSerializerSettings();

        //    serlizerSettings.Formatting = Formatting.Indented;

        //    string fileText;

        //    using (var sr = new StreamReader(path, Encoding.UTF8))
        //    {
        //        fileText = sr.ReadToEnd();
        //    }

        //    if (string.IsNullOrEmpty(fileText))
        //        return default;

        //    var tasks = new List<Task<List<EntityData>>>();

        //    foreach (var prefab in JsonConvert.DeserializeObject<List<SerializedPrefab>>(fileText, serlizerSettings))
        //    {
        //        tasks.Add(Task.Run(() => LoadEnity(prefab, serlizerSettings)));
        //    }

        //    return LoadData(await Task.WhenAll(tasks));
        //}

        //private static List<EntityData> LoadEnity(SerializedPrefab serializedPrefab, JsonSerializerSettings serlizerSettings)
        //{
        //    string fileText;

        //    using (var sr = new StreamReader(serializedPrefab.Name, Encoding.UTF8))
        //    {
        //        fileText = sr.ReadToEnd();
        //    }

        //    if (string.IsNullOrEmpty(fileText))
        //        return null;

        //    Dictionary<string, object> components = JsonConvert.DeserializeObject<Dictionary<string, object>>(fileText, serlizerSettings);

        //    List<EntityData> result = new List<EntityData>();

        //    Matrix4 entityTransform = Matrix4.Identity;

        //    EntityData entityData = new EntityData(new List<Tuple<Type, IComponent>>());

        //    foreach (var component in components)
        //    {
        //        Type componentType = Util.UtilFunctions.GetTypeByName(component.Key);
        //        if (componentType is null)
        //            return null;
        //        IComponent comp = (IComponent)((JObject)component.Value).ToObject(componentType);
        //        if (comp is TransformComponent && serializedPrefab.Transform != default)
        //        {
        //            var transform = comp as TransformComponent;
        //            transform.Transform *= serializedPrefab.Transform;
        //            entityTransform = transform.Transform;
        //        }
        //        entityData.Components.Add(new Tuple<Type, IComponent>(componentType, comp));
        //    }

        //    result.Add(entityData);

        //    return result;
        //}
    }


    //public struct SceneData
    //{
    //    public readonly Dictionary<Type, Dictionary<EntityID, IComponent>> Components;
    //    public readonly Dictionary<Type, Tuple<EntityID, ISingletonComponent>> SingletonComponents;
    //    public readonly Dictionary<string, EntityID> Tags;

    //    public SceneData(Dictionary<Type, Dictionary<EntityID, IComponent>> components, Dictionary<Type, Tuple<EntityID, ISingletonComponent>> singletonComponents, Dictionary<string, EntityID> tags)
    //    {
    //        Components = components ?? throw new ArgumentNullException(nameof(components));
    //        SingletonComponents = singletonComponents ?? throw new ArgumentNullException(nameof(singletonComponents));
    //        Tags = tags ?? throw new ArgumentNullException(nameof(tags));
    //    }


    //}

    //public struct EntityData : IEnumerable<Tuple<Type, IComponent>>
    //{
    //    List<Tuple<Type, IComponent>> _components;

    //    public List<Tuple<Type, IComponent>> Components { get => _components; }

    //    public EntityData(List<Tuple<Type, IComponent>> components)
    //    {
    //        this._components = components ?? throw new ArgumentNullException(nameof(components));
    //    }

    //    public IEnumerator<Tuple<Type, IComponent>> GetEnumerator()
    //    {
    //        return _components.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return _components.GetEnumerator();
    //    }
    //}

    [Serializable]
    public class ResourceData : ISerializable
    {
        public readonly Dictionary<string, string> Assets = null;
        public readonly Dictionary<string, byte[]> Textures = null;
        public readonly Dictionary<string, byte[]> AudioFiles = null;

        public int Count { get => Assets.Count + Textures.Count + AudioFiles.Count; }

        public ResourceData()
        {
            Assets = new Dictionary<string, string>();
            Textures = new Dictionary<string, byte[]>();
            AudioFiles = new Dictionary<string, byte[]>();
        }

        public ResourceData(SerializationInfo info, StreamingContext context)
        {
            Assets = (Dictionary<string, string>)info.GetValue("Assets", typeof(Dictionary<string, string>));
            Textures = (Dictionary<string, byte[]>)info.GetValue("Textures", typeof(Dictionary<string, byte[]>));
            AudioFiles = (Dictionary<string, byte[]>)info.GetValue("SoundFiles", typeof(Dictionary<string, byte[]>));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Assets", Assets);
            info.AddValue("Textures", Textures);
            info.AddValue("SoundFiles", AudioFiles);
        }
    }

}
