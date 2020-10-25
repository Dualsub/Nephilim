using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets.Serializers
{
    interface ISerlizer
    {
        T Deserlize<T>(string source);
        object Deserlize(string source);
        void Serlize(string source);
    }
}
