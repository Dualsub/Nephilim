using Nephilim.Engine.Core;
using Nephilim.Engine.World;
using Nephilim.Sandbox.Resources;
using Nephilim.Sandbox.Systems;
using OpenTK.Graphics.GL;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using static Nephilim.Engine.World.System;

namespace Nephilim.Sandbox
{
    public class NephilimApp : Application
    {
        IApplicationContext _context;

        public NephilimApp(IApplicationContext context)
        {
            _context = context;
        }

        public override void SetContext(out IApplicationContext context, Configuration configuration)
        {
            context = _context;
        }

        public override void OnLoad()
        {
            var game = new Game2D("DefaultScene", LoadLoadingScreen());
            game.AddSystems += Game_AddSystems;
            PushLayer(game);
        }

        private Bitmap LoadLoadingScreen()
        {
            Bitmap bm;
            using (MemoryStream ms = new MemoryStream(LoadingScreenImage.data))
            {
                bm = new Bitmap(ms);
                bm.SetResolution(64, 64);
            }

            if (bm is null)
                throw new NullReferenceException();

            return bm;
        }

        private void Game_AddSystems(Registry registry)
        {
            registry.AddSystem<PlayerControlSystem>(UpdateFlags.Update);
        }
    }
}
