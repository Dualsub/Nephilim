using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.BuildTool.Builders
{
    class EditorBuilder : IBuilder
    {
        public void ExcecuteBuild(string[] args)
        {
        }

        public void GetArgsInfo(ref BuilderArgsInfo builderArgsInfo)
        {
        }

        public void Initilize()
        {
            Console.WriteLine("Initlizing Editor Build.");
        }
    }
}
