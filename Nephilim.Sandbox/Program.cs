using Nephilim.Engine.Core;
using System.Security.Cryptography.X509Certificates;

namespace nephilim
{
    class Program
    {
        static void Main(string[] args)
        {
            new Nephilim().Init(args);
        }

    }

    class Nephilim : Application
    {
        public override void OnLoad(ref SimulationManager simulationManager)
        {
            simulationManager.PushLayer<GameLayer>();
        }
    }

}
