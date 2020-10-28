using System;
using System.Collections.Generic;

namespace Nephilim.Engine.Core
{
    public class SimulationManager
    {

        List<ILayer> _layers = new List<ILayer>();

        public void PushLayer<T>() where T : ILayer  
        {
            ILayer layer = (ILayer)Activator.CreateInstance(typeof(T));
            _layers.Add(layer);
            layer.OnAdded();
        }

        public void StartSimulation()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnStart();
            }
        }

        public void Update(double dt)
        {
            for(int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnUpdateLayer(dt);
            }
        }

        public void Render()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnRenderLayer();
            }
        }

        public void DestroySimulation()
        {
            for (int i = 0; i < _layers.Count; i++)
            {
                _layers[i].OnDestroy();
            }
            _layers.Clear();
        }

    }
}