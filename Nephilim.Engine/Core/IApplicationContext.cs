using System;
using Nephilim.Engine.World;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Nephilim.Engine.Core
{
    public interface IApplicationContext
    {
        double UpdateFrequency { get; }

        void Init();
        void RunDriver();
        void PostRender();
        void PostSceneLoad(Scene scene);
        double GetDeltaTime();
        Tuple<int, int> GetViewportSize();

        event Action Render;
        event Action<TimeStep> Update;

        event Action Loaded;
        event Action UnLoaded;

        event Action<int, int> Resize;

        // Input

        event Action<KeyboardKeyEventArgs> KeyDownEvent;
        event Action<KeyboardKeyEventArgs> KeyUpEvent;
        public bool IsKeyPressed(Keys key);
        public bool IsMouseButtonPressed(MouseButton button);
        public Vector2 GetMousePosition();
    }
}
