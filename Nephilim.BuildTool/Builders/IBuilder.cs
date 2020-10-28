using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.BuildTool.Builders
{
    interface IBuilder
    {
        void Initilize();
        void ExcecuteBuild(string[] args);
    }
}
