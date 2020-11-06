using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Core
{
    [JsonObject(MemberSerialization.Fields)]
    internal struct WindowConfig
    {

        internal enum WindowState
        {
            Fullscreen, Windowed, Borderless, None
        }

        internal enum FrameLimitType
        {
            VSyncOn, VSyncAdaptive, Limited, Unlimited, None
        }
        public uint Width;
        public uint Height;
        public WindowState WindowMode;
        public FrameLimitType FrameLimitMode;
        public string Title;
        public uint FrameLimit;

        public WindowConfig(string title, uint width, uint height, WindowState windowMode, FrameLimitType frameLimitMode, uint frameLimit = 0)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            WindowMode = windowMode;
            FrameLimitMode = frameLimitMode;
            Width = width;
            Height = height;
            FrameLimit = frameLimit;
        }

        public WindowConfig(string title, uint width, uint height)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            WindowMode = WindowState.Windowed;
            FrameLimitMode = FrameLimitType.Unlimited;
            Width = width;
            Height = height;
            FrameLimit = 0;
        }
    }

    struct GameConfig
    {
        internal enum PerspectiveMode
        {
            TwoDimensional, ThreeDimensional, None
        }

        internal enum PhysicsMode
        {
            None
        }
    }

    struct PhysicsConfig
    {
        public float TimeStep;

        public PhysicsConfig(float timeStep)
        {
            TimeStep = timeStep;
        }
    }

    internal enum TargetPlatform
    {
        Windows, Web, iOS, Android, None
    }

    public class Configuration
    {
        internal WindowConfig WindowConfig { get; set; } = new WindowConfig();
        internal GameConfig GameConfig { get; set; } = new GameConfig();

        public static Configuration Load()
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

            return config;
        }
    }
}
