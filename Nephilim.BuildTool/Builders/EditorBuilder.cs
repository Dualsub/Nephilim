using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.BuildTool.Builders
{
    class EditorBuilder : IBuilder
    {
        public void ExcecuteBuild(string[] args)
        {
        }

        public void Initilize()
        {
            Console.WriteLine("Initlizing Editor Build.");
        }
    }
}
