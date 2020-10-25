using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;

namespace Nephilim.Engine.Input
{
    static class InputManager
    {
        private static GameWindow _driver = null;

        public static void Init(GameWindow gw)
        {
            _driver = gw;
        }

        public static bool IsKeyDown(Keys key)
        {
            return _driver.IsAnyKeyDown ? _driver.IsKeyDown(key) : false;
        }

        public static bool IsMouseButtonDown(MouseButton button)
        {
            return _driver.IsAnyMouseButtonDown ? _driver.IsMouseButtonDown(button) : false;
        }

        public static Vector2 GetMousePosition()
        {
            return _driver.MousePosition;
        }
    }
}
