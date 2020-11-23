using Nephilim.Engine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.World.Systems
{
    public class AudioSystem : System
    {
        protected override void OnActivated(Registry registry)
        {
            Audio.AudioManager.Init();
        }

        protected override void OnUpdate(Registry registry, TimeStep ts)
        {

        }
    }
}
