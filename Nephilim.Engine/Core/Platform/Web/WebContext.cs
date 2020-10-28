using Nephilim.Engine.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.Core.Platform.Web
{
    class WebContext : IApplicationContext
    {
        public double UpdateFrequency => throw new NotImplementedException();

        public event Action Render;
        public event Action<double> Update;
        public event Action Loaded;
        public event Action UnLoaded;
        public event Action<int, int> Resize;

        public double GetDeltaTime()
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
