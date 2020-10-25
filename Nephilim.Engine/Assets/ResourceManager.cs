using Nephilim.Engine.Util;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nephilim.Engine.Assets
{
    class ResourceManager
    {
        private static Dictionary<string, string> assetPaths = new Dictionary<string, string>();

        public T Load<T>(string path)
        {
#if DEBUG
            return LoadFromProject<T>(path);
#elif RELEASE
            return LoadFromPackage<T>(path);
#endif
        }
#if DEBUG
        private T LoadFromProject<T>(string path)
        {
            return (T)new object();
        }
#else
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
#endif
    }
}
