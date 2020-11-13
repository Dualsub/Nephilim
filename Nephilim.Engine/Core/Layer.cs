using System;

namespace Nephilim.Engine.Core
{
    public interface ILayer
    {
        void OnUpdateLayer(TimeStep ts);

        void OnRenderLayer();
        
        void OnAdded();

        void OnStart();

        void OnDestroy();
    }
}