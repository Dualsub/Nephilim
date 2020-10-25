using Nephilim.Engine.Util;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class ConsoleSystem : System 
    {
        protected override void OnLateUpdate(Registry registry)
        {
            if(Input.InputManager.IsKeyDown(Keys.Enter))
            {
                Task.Run(() => {
                    Console.Write(">>> ");
                    Console.WriteLine(Console.ReadLine());
                });
                //string message = Console.ReadLine();
                //if (message.Contains("Open Level "))
                //{
                //    string levelName = message.Replace("Open Level ", "");
                //    string levelPath = @"../../Resources/Scenes/" + levelName + ".scene";
                //    if (File.Exists(levelPath))
                //    {
                //        registry.LoadScene(levelPath);
                //        Log.Print($"Loaded {levelName}");
                //    } 
                //    else
                //    {
                //        Log.Print($"Scene {levelName} Does Not Exists!");
                //    }
                //}
            }

            if (Input.InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Z))
            {
                registry.ReloadScene();
            }

        }

    }
}
