using Nephilim.Engine.Assets.Loaders;
using Nephilim.Engine.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nephilim.Engine.Assets
{
    class AssetManager
    {
        private static Dictionary<string, string> _assetPaths = new Dictionary<string, string>();
        private static Dictionary<Type, ILoader> _loaders = new Dictionary<Type, ILoader>();

//        public T Load<T>(string name)
//        {
//#if DEBUG
//            return LoadFromProject<T>(name);
//#elif RELEASE
//            return LoadFromPackage<T>(path);
//#endif
////        }
//////#if DEBUG
////        private T LoadFromProject<T>(string name)
////        {
////            if (_assetPaths.TryGetValue(name, out var path) && _loaders.TryGetValue(typeof(T), out var loader))
////                return loader.Load<T, string>(path);

////            throw new Exception($"Could not load asset {name}.");
//        }
////#else
        private T LoadFromPackage<T>(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Decompress, false))
                {
                    foreach (var item in ((Dictionary<string, string>)formatter.Deserialize(zipStream)))
                    {
                        Log.Print($"{Path.GetFileName(item.Key)} was retrived!");
                    }
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
            return (T)new object();

        }
//#endif
    }
}
