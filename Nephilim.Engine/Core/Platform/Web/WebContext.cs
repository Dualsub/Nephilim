using Nephilim.Engine.World;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Core.Platform.Web
{
    public class WebContext : IApplicationContext
    {
        public double UpdateFrequency => throw new NotImplementedException();

        public event Action Render;
        public event Action<TimeStep> Update;
        public event Action Loaded;
        public event Action UnLoaded;
        public event Action<int, int> Resize;
        public event Action<KeyboardKeyEventArgs> KeyDownEvent;
        public event Action<KeyboardKeyEventArgs> KeyUpEvent;

        public double GetDeltaTime()
        {
            throw new NotImplementedException();
        }

        public Vector2 GetMousePosition()
        {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetViewportSize()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool IsKeyPressed(Keys key)
        {
            throw new NotImplementedException();
        }

        public bool IsMouseButtonPressed(MouseButton button)
        {
            throw new NotImplementedException();
        }

        public void PostRender()
        {
            throw new NotImplementedException();
        }

        public void PostSceneLoad(Scene scene)
        {
            throw new NotImplementedException();
        }

        public void RunDriver()
        {
            throw new NotImplementedException();
        }
    }
}
