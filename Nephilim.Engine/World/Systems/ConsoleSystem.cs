using Nephilim.Engine.Util;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Systems
{
    class ConsoleSystem : System 
    {
        //REMOVE
        Task _task = null;
        ConcurrentStack<string> _args = new ConcurrentStack<string>();
        protected override void OnLateUpdate(Registry registry)
        {
            if(Input.InputManager.IsKeyDown(Keys.Enter))
            {
                if(_task == null)
                {
                    _task = Task.Run(() => {
                        Console.Write(">>> ");
                        var command = Console.ReadLine();
                        _args.Push(command);
                    });
                }
                else if(_task.IsCompleted)
                {
                    if(_args.TryPop(out var command))
                    {
                        var args = command.Split(":");
                        if (args[0].ToLower() == "colliderdebug")
                        {
                            if (args[1].ToLower() == "on")
                            {
                                registry.ActiveSystem<ColliderDebugSystem>();
                            }
                            else if (args[1].ToLower() == "off")
                            {
                                registry.DeactivateSystem<ColliderDebugSystem>();
                            }
                        }
                    }
                    _task = null;
                }

            }

            if (Input.InputManager.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Z))
            {
                registry.ReloadScene();
            }

        }

    }
}
