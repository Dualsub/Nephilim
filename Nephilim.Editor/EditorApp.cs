using Nephilim.Engine.Core;
using System;

namespace Nephilim.Editor
{
    class EditorApp : Application
    {
        public override void OnLoad()
        {
            PushLayer<EditorLayer>();
        }
    }
    
    class EditorLayer : ILayer
    {
        public void OnAdded()
        {
        }

        public void OnDestroy()
        {
        }

        public void OnRenderLayer()
        {
        }

        public void OnStart()
        {
        }

        public void OnUpdateLayer(TimeStep ts)
        {
        }
    }

}
