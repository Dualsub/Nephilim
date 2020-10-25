
using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.IO;
using System.Text;

namespace Nephilim.Engine.Core
{
    public abstract class Application
    {

        private IDriver _windowDriver = null;
        private SimulationManager _simulationManager = null;

        public static float Width { get; private set; } = 0;
        public static float Height { get; private set; } = 0;
        public static double DeltaTime { get; private set; }

        public static float TimeDilation { get; set; } = 1;

        public void Init(params string[] args)
        {
            var config = new Configuration();
            string windowPath = @"../../../Configs/Window.config";

            if (File.Exists(windowPath))
            {
                var settings = new JsonSerializerSettings();
                settings.Formatting = Formatting.Indented;

                string fileText;

                using (var sr = new StreamReader(windowPath, Encoding.UTF8))
                {
                    fileText = sr.ReadToEnd();
                }
                
                config.WindowConfig = JsonConvert.DeserializeObject<WindowConfig>(fileText, settings);
            } 
            else
            {
                config.WindowConfig = new WindowConfig("Nephilim",
                1600, 900,
                WindowConfig.WindowState.Windowed,
                WindowConfig.FrameLimitType.Unlimited);
            }


            _windowDriver = new GameDriver(config);

            Width = _windowDriver.GetViewportSize().Item1;
            Height = _windowDriver.GetViewportSize().Item2;

            _windowDriver.Loaded += WindowDriver_Loaded;
            _windowDriver.UnLoaded += WindowDriver_UnLoaded;
            _windowDriver.Update += WindowDriver_Update;
            _windowDriver.Render += WindowDriver_Render;
            _windowDriver.Resize += WindowDriver_Resize;

            _simulationManager = new SimulationManager();
            OnLoad(ref _simulationManager);

            _windowDriver.Init();
            
            _windowDriver.RunDriver();

        }

        private void WindowDriver_Loaded()
        {
            _simulationManager.StartSimulation();
        }

        private void WindowDriver_UnLoaded()
        {
            _simulationManager.DestroySimulation();
        }

        private void WindowDriver_Resize(int x, int y)
        {
            Width = x;
            Height = y;
        }

        private void WindowDriver_Update(double dt)
        {
            DeltaTime = TimeDilation * dt;
            _simulationManager.Update(DeltaTime);
        }
        private void WindowDriver_Render()
        {
            _simulationManager.Render();
            _windowDriver.PostRender();
        }


        public abstract void OnLoad(ref SimulationManager simulationManager);
    }
}
