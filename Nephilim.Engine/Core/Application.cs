
using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using Nephilim.Engine.World.Components;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nephilim.Engine.Core
{
    public abstract class Application
    {

        private IApplicationContext _appContext = null;

        public static float Width { get; private set; } = 0;
        public static float Height { get; private set; } = 0;
        public static double DeltaTime { get; private set; }
        public static float TimeDilation { get; set; } = 1;

        List<ILayer> _layers = new List<ILayer>();

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

            SetContext(out var context, config);
            _appContext = context;

            Width = _appContext.GetViewportSize().Item1;
            Height = _appContext.GetViewportSize().Item2;

            _appContext.Loaded += WindowDriver_Loaded;
            _appContext.UnLoaded += WindowDriver_UnLoaded;
            _appContext.Update += WindowDriver_Update;
            _appContext.Render += WindowDriver_Render;
            _appContext.Resize += WindowDriver_Resize;

            OnLoad();

            _appContext.Init();
            
            _appContext.RunDriver();

        }

        private void WindowDriver_Loaded()
        {
            StartSimulation();
        }

        private void WindowDriver_UnLoaded()
        {
            DestroySimulation();
        }

        private void WindowDriver_Resize(int x, int y)
        {
            Width = x;
            Height = y;
        }

        private void WindowDriver_Update(double dt)
        {
            DeltaTime = TimeDilation * dt;
            Update(DeltaTime);
        }
        private void WindowDriver_Render()
        {
            Render();
            _appContext.PostRender();
        }


        public abstract void OnLoad();

        public virtual void SetContext(out IApplicationContext context, Configuration configuration)
        {
            // Sets the context to be a game context by default.
            context = new DesktopContext(configuration);
        }

        public void PushLayer<T>(T layer) where T : ILayer
        {
            _layers.Add(layer);
            layer.OnAdded();
        }

        public void PushLayer<T>() where T : ILayer
        {
            ILayer layer = (ILayer)Activator.CreateInstance(typeof(T));
            _layers.Add(layer);
            layer.OnAdded();
        }

        public void StartSimulation()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnStart();
            }
        }

        public void Update(double dt)
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnUpdateLayer(dt);
            }
        }

        public void Render()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnRenderLayer();
            }
        }

        public void DestroySimulation()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnDestroy();
            }
            _layers.Clear();
        }

    }
}
