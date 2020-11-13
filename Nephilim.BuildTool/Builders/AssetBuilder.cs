using Nephilim.BuildTool.Utils;
using Nephilim.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nephilim.BuildTool.Builders
{
    class AssetBuilder : IBuilder
    {
        public void ExcecuteBuild(string[] args)
        {
            string path = args[0];

            List<Task<string[]>> tasks = new List<Task<string[]>>()
            {
                Task.Run(() => Directory.GetFiles(path, "*.png", SearchOption.AllDirectories)),
                Task.Run(() => Directory.GetFiles(path, "*.scene", SearchOption.AllDirectories)),
                Task.Run(() => Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories)),
                Task.Run(() => Directory.GetFiles(path, "*.anim", SearchOption.AllDirectories)),
                Task.Run(() => Directory.GetFiles(path, "*.glsl", SearchOption.AllDirectories))
            };

            var loadingTask = Task.WhenAll(tasks);

            Console.WriteLine($"Getting Files in: {path}");
            Console.WriteLine($"{tasks.Count} loading tasks initilized...");

            LoadingAnimation.StartAnimation($"Finding Files");

            while (!loadingTask.IsCompleted)
            {
                if (loadingTask.IsFaulted)
                    throw loadingTask.Exception;
                Thread.Sleep(10);
                LoadingAnimation.WriteAnimation();
            }

            LoadingAnimation.EndAnimation();

            var result = loadingTask.Result;

            if (result is null)
                throw new NullReferenceException("Result was null.");

            ResourceData data = new ResourceData();

            foreach (string[] type in result)
            {
                Console.WriteLine("Task Entries: "+ type.Length);
                foreach (string asset in type)
                {
                    var name = Path.GetFileNameWithoutExtension(asset);

                    switch (Path.GetExtension(asset).ToLower())
                    {
                        case ".png":
                            byte[] rawData;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                new Bitmap(asset).Save(ms, ImageFormat.Png);
                                rawData = ms.ToArray();
                            }
                            data.Textures.TryAdd(name, rawData);
                            break;
                        case ".waw":
                            break;
                        default:
                            var fileText = string.Empty;

                            using (var sr = new StreamReader(asset, Encoding.UTF8))
                            {
                                fileText = sr.ReadToEnd();
                            }

                            data.Assets.TryAdd(name, fileText);
                            break;
                    }
                    Console.WriteLine($"\tLoaded {name}.");
                }
            }

            Console.WriteLine($"\nLoaded {data.Count} files.");

            BinaryFormatter binaryFormatter = new BinaryFormatter();

            var newFile = Path.Combine(args.Length > 1 ? args[1] : path, "resources.dat");

            Console.WriteLine($"\nCreating {newFile}");

            if (File.Exists(newFile))
                File.Delete(newFile);

            FileStream fs = new FileStream(newFile, FileMode.OpenOrCreate);

            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (GZipStream zipStream = new GZipStream(fs, CompressionMode.Compress, false))
                {
                    formatter.Serialize(zipStream, data);
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
                Console.WriteLine($"Packaging Successful!");
            }
            Console.WriteLine("Serlized data.");

            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\tBuild Complete!");
            Console.ForegroundColor = color;
        }

        public void GetArgsInfo(ref BuilderArgsInfo builderArgsInfo)
        {
            throw new NotImplementedException();
        }

        public void Initilize()
        {
            Console.WriteLine("Beginning assets build...");
        }
    }
}
