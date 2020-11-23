using System;
using System.Collections.Generic;
using System.Text;

namespace Nephilim.Engine.Rendering
{
    class GLExepction : Exception
    {

        GLExepction()
        {

        }

        public override Exception GetBaseException()
        {



            return new Exception();
        }
    }
}
