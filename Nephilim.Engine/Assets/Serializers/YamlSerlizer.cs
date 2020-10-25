using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Assets.Serializers
{
    class YamlSerlizer : ISerlizer
    {
        public T Deserlize<T>(string source)
        {
            return (T)new object();
        }

        public object Deserlize(string source)
        {
            return new object();
        }

        public void Serlize(string source)
        {
        }
    }
}
