using Nephilim.Engine.Util;
using Nephilim.Engine.World;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using Nephilim.Engine.Input;
using Nephilim.Engine.Rendering;
using System.IO;
using System.Drawing;
using OpenTK.Compute.OpenCL;
using OpenTK.Windowing.Common.Input;
using System.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nephilim.Engine.Core
{
    public class DesktopContext : GameWindow, IApplicationContext
    {

        Stopwatch _sw = null;

        double IApplicationContext.UpdateFrequency { get => UpdateFrequency; }
        public TimeSpan DeltaTime { get; private set; }

        public float WindowWidth { get => Size.X; }
        public float WindowHeight { get => Size.Y; }

        public event System.Action Render;
        public event System.Action<TimeStep> Update;
        public event System.Action Loaded;
        public event System.Action UnLoaded;
        public new event System.Action<int, int> Resize;

        public event Action<KeyboardKeyEventArgs> KeyDownEvent;
        public event Action<KeyboardKeyEventArgs> KeyUpEvent;

        private static NativeWindowSettings _nativeWindowSettings = null;
        private static GameWindowSettings _gameWindowSettings = null;
        private static GameWindowSettings GameWindowSettings
        {
            get
            {
                // Init if it hasn't already been 
                if (_gameWindowSettings is null)
                {
                    _gameWindowSettings = new GameWindowSettings();
                    //TODO: Enable multithreading
                    //_gameWindowSettings.IsMultiThreaded = true;
                }

                return _gameWindowSettings;
            }
        }

        private static NativeWindowSettings NativeWindowSettings
        {
            get
            {
                // Init if it hasn't already been 
                if (_nativeWindowSettings is null) 
                {
                    _nativeWindowSettings = new NativeWindowSettings();
                    _nativeWindowSettings.Icon = LoadIcon();
                }

                return _nativeWindowSettings;
            }
        }

#if DEBUG
        string title;
        float fpsFreq = 1 / 3f;
        double fpsTimeCache = 0;
#endif

        public DesktopContext(Configuration gameConfig) : base(GameWindowSettings, NativeWindowSettings)
        {
#if DEBUG
            title = gameConfig.WindowConfig.Title;
            Title = title;
#else
            Title = gameConfig.WindowConfig.Title;
#endif
            VSync = VSyncMode.Off;
            WindowState newWindowState = WindowState.Normal;

            //TODO: Fix
            int screenWidth = 1920;//Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = 1080;//Screen.PrimaryScreen.Bounds.Height;

            switch (gameConfig.WindowConfig.WindowMode)
            {
                case WindowConfig.WindowState.Fullscreen:
                    newWindowState = WindowState.Fullscreen;
                    break;
                case WindowConfig.WindowState.Windowed:
                    newWindowState = WindowState.Normal;
                    break;
                case WindowConfig.WindowState.Borderless:
                    newWindowState = WindowState.Normal;
                    Size = new Vector2i(screenWidth, screenHeight);
                    break;
                case WindowConfig.WindowState.None:
                    newWindowState = WindowState.Normal;
                    break;
                default:
                    newWindowState = WindowState.Fullscreen;
                    break;
            }
            WindowState = newWindowState;

            Size = newWindowState == WindowState.Fullscreen ? 
                new Vector2i(screenWidth, screenHeight) :
                new Vector2i(
                    (int)gameConfig.WindowConfig.Width, 
                    (int)gameConfig.WindowConfig.Height
                    );

            LoadInputEvents();
        }

        private void LoadInputEvents()
        {
            KeyDown += (e) => KeyDownEvent.Invoke(e); 
            KeyUp += (e) => KeyUpEvent.Invoke(e); 
        }

        private static WindowIcon LoadIcon()
        {
            string iconPath = UtilFunctions.FindFilePath("NephilimIconImage.png");

            if (string.IsNullOrEmpty(iconPath))
                return null;

            byte[] iconData;

            using(var ms = new MemoryStream())
            {
                var bm = Bitmap.FromFile(iconPath, true);
                ms.Position = 0;
                bm.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                iconData = ms.ToArray();
            }

            if (iconData.Length <= 0)
                throw new Exception("No icon data.");

            return new WindowIcon(new OpenTK.Windowing.Common.Input.Image[]
            {
                new OpenTK.Windowing.Common.Input.Image(128, 128, iconData)
            });
        }

        public void Init()
        {
            InputManager.Init(this);
            GraphicsContext.Init(this);
            GraphicsContext.MakeCurrent();
            Move += Context_Move;
            _sw = new Stopwatch();
            _sw.Start();
            LoadIcon();
#if DEBUG
            Log.Print(GL.GetString(StringName.Vendor));
            Log.Print(GL.GetString(StringName.Renderer));
            Log.Print(GL.GetString(StringName.ShadingLanguageVersion));
            Log.Print(GL.GetString(StringName.Version));
#endif

        }

        private void Context_Move(WindowPositionEventArgs obj)
        {
        }

        public void PostRender()
        {
            SwapBuffers();
            DeltaTime = _sw.Elapsed;
        }

        public void PostSceneLoad(Scene scene)
        {

        }

        public void RunDriver()
        {
            Run();
        }

        protected override void OnLoad()
        {
            Loaded.Invoke();
        }

        private void Context_Load()
        {
            
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Render.Invoke();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            if(Resize != null)
                Resize.Invoke(e.Width, e.Height);
            GL.Viewport(0,0, e.Width, e.Height);
        }
        public override void Close()
        {
            base.Close();
            UnLoaded.Invoke();
        }

        protected override void OnUnload()
        {

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _sw.Restart();
            if (this.IsFocused)
                Update.Invoke(new TimeStep(DeltaTime));
#if DEBUG
            fpsTimeCache += DeltaTime.TotalSeconds;
            if (fpsTimeCache > fpsFreq)
            {
                fpsTimeCache = 0;
                UpdateFPS();
            }
#endif
        }

        public double GetDeltaTime()
        {
            return DeltaTime.TotalSeconds;
        }

        public Tuple<int, int> GetViewportSize()
        {
            return new Tuple<int, int>(Size.X, Size.Y);
        }
#if DEBUG
        public void UpdateFPS()
        {
            Title = title + $"  FPS: { (uint) (1d / DeltaTime.TotalSeconds) } ms: {DeltaTime.TotalMilliseconds }";
        }
#endif

        new public bool IsKeyPressed(Keys key)
        {
            return IsAnyKeyDown ? IsKeyDown(key) : false;
        }

        new public bool IsMouseButtonPressed(MouseButton button)
        {
            return IsAnyMouseButtonDown ? IsMouseButtonDown(button) : false;
        }

        public Vector2 GetMousePosition()
        {
            return MousePosition;
        }

    }
}
