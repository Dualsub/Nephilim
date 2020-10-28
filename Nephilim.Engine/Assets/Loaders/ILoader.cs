using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets.Loaders
{
    interface ILoader
    {
        T Load<T, P>(P data);

    }
}
