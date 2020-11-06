using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets
{
    public class Prefab
    {
        string _name = string.Empty;
        List<IComponent> _components;

        //public static Prefab(string name, Dictionary<string, object> components)
        //{
        //    var newComponents = new List<IComponent>();

        //    foreach (var keyValuePair in components)
        //    {
        //        Type type = Util.UtilFunctions.GetTypeByName(keyValuePair.Key);
        //        newComponents.Add(keyValuePair.Value);
        //    }

        //    return new Prefab(name, newComponents);
        //}

        private Prefab(string name, List<IComponent> components)
        {
            _name = name;
            _components = components;
        }
    }
}
