using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

namespace Nephilim.Engine.Assets.Serializers
{
    class JsonSerilizer : ISerlizer
    {
        public T Deserlize<T>(string source)
        {
            T obj = JsonConvert.DeserializeObject<T>(source);
            return obj;
        }

        public object Deserlize(string source)
        {
            return JsonConvert.DeserializeObject(source);
        }

        public void Serlize(object value)
        {
            var serlizer = Newtonsoft.Json.JsonSerializer.Create();
            serlizer.Serialize(,);

        }
    }
}
