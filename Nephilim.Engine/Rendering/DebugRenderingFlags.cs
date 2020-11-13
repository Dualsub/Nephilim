using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Rendering
{
    [Flags]
    public enum DebugRenderingFlags : short
    {
        None = 0,
        Colliders = 1,
        DebugRenderers = 2
    };
}
