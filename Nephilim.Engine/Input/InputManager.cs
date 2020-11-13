using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Windowing.Common;
using Nephilim.Engine.Core;

namespace Nephilim.Engine.Input
{
    public static class InputManager
    {
        private static IApplicationContext _driver = null;
        public static Action<KeyboardKeyEventArgs> KeyDown;
        public static Action<KeyboardKeyEventArgs> KeyUp;


        public static void Init(IApplicationContext driver)
        {
            _driver = driver;
            _driver.KeyDownEvent += (e) =>
            {
                if (KeyDown is null || e.Equals(default))
                    return;    
                KeyDown.Invoke(e);
            };
            _driver.KeyUpEvent += (e) =>
            {
                if (KeyUp is null || e.Equals(default))
                    return;
                KeyUp.Invoke(e);
            };

        }

        public static bool IsKeyDown(Keys key) => _driver.IsKeyPressed(key);

        public static bool IsMouseButtonDown(MouseButton button) =>  _driver.IsMouseButtonPressed(button);

        public static Vector2 GetMousePosition() => _driver.GetMousePosition();


    }
}
