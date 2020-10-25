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

namespace Nephilim.Engine.Core
{
    class GameDriver : GameWindow, IDriver
    {

        Stopwatch _sw = null;

        double IDriver.UpdateFrequency { get => UpdateFrequency; }
        public TimeSpan DeltaTime { get; private set; }

        public float WindowWidth { get => Size.X; }
        public float WindowHeight { get => Size.Y; }

        public event System.Action Render;
        public event System.Action<double> Update;
        public event System.Action Loaded;
        public event System.Action UnLoaded;
        public new event System.Action<int, int> Resize;

#if DEBUG
        string title;
        float fpsFreq = 1 / 3f;
        double fpsTimeCache = 0;
#endif

        public GameDriver(Configuration gameConfig) : base(GetGameWindowSettings(), GetNativeWindowSettings())
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
        }


        public void Init()
        {
            InputManager.Init(this);
            GraphicsContext.Init(this);
            GraphicsContext.MakeCurrent();
            Move += GameDriver_Move;
            _sw = new Stopwatch();
            _sw.Start();
        }

        private void GameDriver_Move(WindowPositionEventArgs obj)
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

        private void GameDriver_Load()
        {
            throw new NotImplementedException();
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
                Update.Invoke(DeltaTime.TotalSeconds);
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

        private static GameWindowSettings GetGameWindowSettings()
        {
            var gws = new GameWindowSettings();
            // TODO: Multi-Thread Rendering
            //gws.IsMultiThreaded = true;
            return gws;
        }

        private static NativeWindowSettings GetNativeWindowSettings()
        {
            return new NativeWindowSettings();
        }

    }
}
