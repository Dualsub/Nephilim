using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace Nephilim.Engine.World.Components
{
    [JsonObject(MemberSerialization.OptOut)]
    public class HealthComponent : IComponent
    {
        [JsonIgnore]
        private float _currentHealth = 0;

        [JsonIgnore]
        public float CurrentHealth { 
            
            get => _currentHealth; 
            
            set 
            {
                _currentHealth = MathHelper.Clamp(value, 0, BaseHealth);
            } 
        }

        public float BaseHealth { get; set; } = 100;

        public HealthComponent(float baseHealth)
        {
            BaseHealth = baseHealth;
            CurrentHealth = baseHealth;
        }
    }
}
