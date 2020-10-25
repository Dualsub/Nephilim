using System;

namespace Nephilim.Engine.Core
{
    public interface ILayer
    {
        void OnUpdateLayer(double dt);

        void OnRenderLayer();
        
        void OnAdded();

        void OnStart();

        void OnDestroy();
    }
}