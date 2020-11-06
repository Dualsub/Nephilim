using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets.Loaders
{
    interface ILoader
    {
        object Load(string name, ResourceData resourceData);

    }
}
