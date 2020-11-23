using Nephilim.Engine.Assets;
using Nephilim.Engine.Audio;
using Nephilim.Engine.Core;
using Nephilim.Engine.Rendering;
using Nephilim.Engine.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Nephilim.Sandbox.Components
{
    [Serializable]
    public class WeaponComponent : IComponent, ISerializable
    {
        public string CurrentWeapon { get; set; } = "";
        public bool WantsToFire { get; set; } = false;
        public float TimeSinceFire { get; set; } = 0.0f;
        public Texture BulletTexture { get; set; }

        public WeaponData WeaponData
        {
            get
            {
                if (_weapons.TryGetValue(CurrentWeapon, out var data))
                    return data;
                else
                {
                    throw new KeyNotFoundException(CurrentWeapon);
                }
            }
        }

        private Dictionary<string, WeaponData> _weapons = new Dictionary<string, WeaponData>();

        public WeaponComponent(SerializationInfo info, StreamingContext context)
        {
            CurrentWeapon = (string)info.GetValue("DefaultWeapon", typeof(string));
            
            var bulletTexture = (string)info.GetValue("BulletTextureFile", typeof(string));
            BulletTexture = Application.ResourceManager.Load<Texture>(bulletTexture);

            var weapons = (List<string>)info.GetValue("Weapons", typeof(List<string>));
            foreach (var weapon in weapons)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();

                settings.Formatting = Formatting.Indented;

                string fileText = Application.ResourceManager.Load<string>(weapon);

                var data = JsonConvert.DeserializeObject<WeaponData>(fileText, settings);

                _weapons.TryAdd(data.Name, data);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }

        public void SetInventorySlot(int index)
        {
            if (index >= _weapons.Count)
                return;

            string[] keys = new string[_weapons.Count];
            _weapons.Keys.CopyTo(keys, 0);
            CurrentWeapon = keys[index];
        }

    }
    [Serializable]
    public struct WeaponData : ISerializable
    {
        public string Name;
        public int DefaultMagSize;
        public float BaseDamage;
        public float FireRate;
        public float Spread;
        public float ShakeAmount;
        public string SpriteSheetName;
        public AudioClip GunSound;
        public int BulletsPerFire;

        public WeaponData(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("Name", typeof(string));
            DefaultMagSize = (int)info.GetValue("DefaultMagSize", typeof(int));
            BaseDamage = (float)info.GetValue("BaseDamage", typeof(float));
            FireRate = (float)info.GetValue("FireRate", typeof(float));
            Spread = (float)info.GetValue("Spread", typeof(float));
            ShakeAmount = (float)info.GetValue("ShakeAmount", typeof(float));
            var soundFile = (string)info.GetValue("GunSoundName", typeof(string));
            GunSound = Application.ResourceManager.Load<AudioClip>(soundFile);
            SpriteSheetName = (string)info.GetValue("SpriteSheetName", typeof(string));

            try
            {
                BulletsPerFire = (int)info.GetValue("BulletsPerFire", typeof(int));
            }
            catch (Exception)
            {
                BulletsPerFire = 1;
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
