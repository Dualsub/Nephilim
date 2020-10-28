using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ShakePlayer : IComponent
    {
        [JsonIgnore]
        public float StartAngle { get; internal set; }
        public float CurrentRadius { get => _currentRadius; 
            internal set 
            {
                if (value < 0)
                {
                    _currentRadius = 0;
                }
                else
                {
                    _currentRadius = value;
                }
            }
        
        }

        [JsonIgnore]
        private float _currentRadius;

        [JsonIgnore]
        public bool ShouldPlay { get; set; }

        public float ShakeRadius { get; set; }
        public float FallOff { get; set; }
        public float Frequency { get; set; }
        [JsonIgnore]
        public float Timer { get; set; } = 0;
    }
}
