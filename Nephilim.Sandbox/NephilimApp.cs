using Nephilim.Engine.Core;
using Nephilim.Sandbox.Systems;
using OpenTK.Graphics.GL;
using System.Security.Cryptography.X509Certificates;

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
            var game = new Game2D("DebugScene");
            game.AddSystems += Game_AddSystems;
            PushLayer(game);
        }

        private void Game_AddSystems(Nephilim.Engine.World.Registry registry)
        {
            registry.AddSystem<PlayerControlSystem>(Nephilim.Engine.World.System.UpdateFlags.Update);
        }
    }
}
