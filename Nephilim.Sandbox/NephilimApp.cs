using Nephilim.Engine.Core;
using Nephilim.Sandbox.Systems;
using System.Security.Cryptography.X509Certificates;

namespace nephilim
{
    class NephilimApp : Application
    {
        public override void OnLoad()
        {
            var game = new Game2D();
            game.AddSystems += Game_AddSystems;
            PushLayer(game);
        }

        private void Game_AddSystems(Nephilim.Engine.World.Registry registry)
        {
            registry.AddSystem<PlayerControlSystem>(Nephilim.Engine.World.System.UpdateFlags.Update);
        }
    }


    // Entry point for the game.
    class Program
    {
        static void Main(string[] args)
        {
            new NephilimApp().Init(args);
        }
    }
}
