using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Nephilim.BuildTool.Builders
{
    class ByteArrayBuilder : IBuilder
    {
        public void ExcecuteBuild(string[] args)
        {
            byte[] rawData;
            using (MemoryStream ms = new MemoryStream())
            {
                new Bitmap(args[0]).Save(ms, ImageFormat.Png);
                rawData = ms.ToArray();
            }
            var fileText = string.Empty;
            foreach (byte b in rawData)
            {
                fileText += b.ToString() + ",\n";
            }

            File.WriteAllText(args[1], fileText);

        }

        public void GetArgsInfo(ref BuilderArgsInfo builderArgsInfo)
        {
        }

        public void Initilize()
        {
        }
    }
}
