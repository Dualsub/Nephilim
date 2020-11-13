using Nephilim.Engine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Components
{
    public abstract class Script : IComponent
    {

        Registry _registry;
        EntityID _owningEntity;

        public void Init(Registry registry, EntityID entityID)
        {
            _registry = registry;
            _owningEntity = entityID;
        }

        protected T GetComponent<T>()
        {
            if (_registry is null)
                throw new NullReferenceException();
            return (T)_registry.GetComponent(typeof(T), _owningEntity);
        }

        public virtual void Start()
        {
        }

        public virtual void Destroyed()
        {
        }

        public virtual void Update(TimeStep ts)
        {
        }

        public virtual void FixedUpdate()
        {
        }
    }
}
