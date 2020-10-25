using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nephilim.Engine.World;

namespace Nephilim.Engine.Core
{
    public interface IDriver
    {
        double UpdateFrequency { get; }

        void Init();
        void RunDriver();
        void PostRender();
        void PostSceneLoad(Scene scene);
        double GetDeltaTime();
        Tuple<int, int> GetViewportSize();

        event Action Render;
        event Action<double> Update;

        event Action Loaded;
        event Action UnLoaded;

        event Action<int, int> Resize;
    }
}
