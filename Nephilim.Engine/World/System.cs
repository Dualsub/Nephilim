using Nephilim.Engine.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World
{

    static class SystemDebugger 
    {
        static private Dictionary<Type, Dictionary<string, List<int>>> _results = new Dictionary<Type, Dictionary<string, List<int>>>();

        static public void AddResult(Type type, string methodName, int dt) 
        {
            if(_results.TryGetValue(type, out var method)) 
            {
                if (method.TryGetValue(methodName, out var list))
                    list.Add(dt);
                else
                {
                    method.Add(methodName, new List<int>() { dt });
                }
            }
            else
            {
                _results.Add(type, new Dictionary<string, List<int>>() {
                    { methodName, new List<int>(){ dt } }
                });
            } 
        }

        static public void PrintResults()
        {
            Log.Print("Results: ");
            foreach (var systems in _results)
            {
                float ssum = 0;
                Log.Print(systems.Key.Name.PadRight(30)); 
                foreach (var methods in systems.Value)
                {
                    float msum = 0;
                    foreach (var value in methods.Value)
                    {
                        msum += value;
                    }
                    var avgr = msum / methods.Value.Count;
                    avgr *= 1000;
                    ssum += avgr;
                    Log.Print("      "+methods.Key.PadRight(25) + $"{avgr}");
                }
                Log.Print("  Total:".PadRight(30) + $"{ssum}");
            }
        }
    }

    public abstract class System
    {

        [Flags]
        public enum UpdateFlags : short
        {
            Update = 0,
            FixedUpdate = 1,
            LateUpdate = 2,
            Render = 3,
            EntitySpawned = 4,
            EntityDestroyed = 5
        };

        public bool IsActive { get; set; } = false;

        internal void Activate(Registry registry)
        {
            IsActive = true;
            OnActivated(registry);
        }

        internal void Deactivate(Registry registry)
        {
            IsActive = false;
            OnDeactivated(registry);
        }

        internal void Update(Registry registry, double dt)
        {
            var sw = Stopwatch.StartNew();
            OnUpdate(registry, dt);
            sw.Stop();
        }

        internal void FixedUpdate(Registry registry)
        {
            var sw = Stopwatch.StartNew();
            OnFixedUpdate(registry);
            sw.Stop();
        }

        internal void LateUpdate(Registry registry)
        {
            var sw = Stopwatch.StartNew();
            OnLateUpdate(registry);
            sw.Stop();
        }

        internal void Render(Registry registry)
        {
            var sw = Stopwatch.StartNew();
            OnRender(registry);
            sw.Stop();
        }

        internal void EntitySpawned(Registry registry, EntityID entity)
        {
            OnEntitySpawned(registry, entity);
        }

        internal void EntityDestroyed(Registry registry, EntityID entity)
        {
            OnEntityDestroyed(registry, entity);
        }

        //Interface
        virtual protected void OnActivated(Registry registry) { }
        virtual protected void OnDeactivated(Registry registry) { }
        virtual protected void OnUpdate(Registry registry, double dt) { }
        virtual protected void OnFixedUpdate(Registry registry) { }
        virtual protected void OnLateUpdate(Registry registry) { }
        virtual protected void OnRender(Registry registry) { }
        virtual protected void OnEntitySpawned(Registry registry, EntityID entity) { }
        virtual protected void OnEntityDestroyed(Registry registry, EntityID entity) { }
    }

    public class SystemList
    {
        private List<Tuple<Type, World.System.UpdateFlags>> _systems = new List<Tuple<Type, System.UpdateFlags>>();

        internal void Add<T>(World.System.UpdateFlags flags)
        {
            _systems.Add(new Tuple<Type, System.UpdateFlags>(typeof(T), flags));
        }

        internal void CreateSystems(Registry registry)
        {
            _systems.ForEach((Tuple<Type, System.UpdateFlags> tuple) => registry.AddSystem(tuple.Item1, tuple.Item2));
        }
    }
}
