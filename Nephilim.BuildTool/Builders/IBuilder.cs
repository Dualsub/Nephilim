using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nephilim.BuildTool.Builders
{
    interface IBuilder
    {
        void Initilize();
        void ExcecuteBuild(string[] args);
    }
}
