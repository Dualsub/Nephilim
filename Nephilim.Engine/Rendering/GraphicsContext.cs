using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Rendering
{
    public static class GraphicsContext
    {
        private static GameWindow _window = null;

        public static GameWindow Window { get => _window; }

        public static void Init(GameWindow window)
        {
            _window = window;
        }

        public static void FinlizeLoading()
        {
            if (_window.Context.IsCurrent)
            {
                Texture.GenrateTextures();
            }
        }

        public static void MakeCurrentAndFinlizeLoading()
        {
            if (!_window.Context.IsCurrent)
            {
                _window.Context.MakeCurrent();
            }
            Texture.GenrateTextures();
        }

        public static void MakeCurrent()
        {
            if (!_window.Context.IsCurrent)
                _window.MakeCurrent();
        }

        public static void MakeNoneCurrent()
        {
            if (_window.Context.IsCurrent)
                _window.Context.MakeNoneCurrent();
        }
    }
}
