using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Nephilim.BuildTool.Builders
{
    /// <summary>
    /// This builder generates a spritesheet from a set of .png images and
    /// can optionally create an .anim-file based on the name of the images.
    /// </summary>

    class SpriteSheetBuilder : IBuilder
    {

        const int MaxNumColums = 8;

        public void Initilize()
        {

        }
     
        public void ExcecuteBuild(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Not enough arguments!");
                return;
            }
                
            var searchPath = args[0];
            var outputPath = args[1];

            bool shouldGenAnim = args.Length >= 3 && "gen-anim" == args[2];

            List<Bitmap> bitmaps = new List<Bitmap>();

            var files = Directory.GetFiles(searchPath, "*.png");

            foreach (var imagePath in files)
            {
                bitmaps.Add(new Bitmap(imagePath));
            }

            // Assuming all sprites are the same width.
            var cellWidth = bitmaps[0].Width;
            var cellHeight = bitmaps[0].Height;

            CreateAndSaveSheet(bitmaps, cellWidth, cellHeight, outputPath);

            if (shouldGenAnim)
                CreateAndSaveAnim(files, cellWidth, cellHeight, outputPath);

        }

        public void GetArgsInfo(ref BuilderArgsInfo builderArgsInfo)
        {
            builderArgsInfo.AddArgument("search-path", "Specifies the path of the images of the sprites.");
            builderArgsInfo.AddArgument("output-path", "Specifies the path where the sprite sheet will be put.");
            builderArgsInfo.AddArgument("gen-anim", "Whether or not to generate an .anim-file. The images has the be named in the following format: Anim_AnimationName_Index.");
        }

        private void CreateAndSaveSheet(List<Bitmap> bitmaps, int cellWidth, int cellHeight, string outputPath)
        {
            var sheetWidth = 0;
            var sheetHeight = 0;

            if (bitmaps.Count <= MaxNumColums)
            {
                sheetHeight = cellHeight;
                sheetWidth = cellWidth * bitmaps.Count;

            }
            else
            {
                int numRows = 1 + (int)((float)(bitmaps.Count + 1f) / (float)MaxNumColums);
                sheetHeight = cellHeight * numRows;
                sheetWidth = cellWidth * MaxNumColums;
            }


            Bitmap finalSheet = new Bitmap(sheetWidth, sheetHeight);
            Graphics g = Graphics.FromImage(finalSheet);
            g.Clear(Color.Transparent);

            var currentPos = new Point(0, 0);

            for (int i = 0; i < bitmaps.Count; i++)
            {
                int y = (i / (sheetWidth / cellWidth));
                int x = (i % (sheetWidth / cellWidth));
                g.DrawImage(bitmaps[i], new Point(x * cellWidth, y * cellHeight));
            }

            g.Dispose();

            if (File.Exists(outputPath))
                File.Delete(outputPath);

            finalSheet.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
        }

        private void CreateAndSaveAnim(string[] files, int cellWidth, int cellHeight, string outputPath)
        {

            string fileText = string.Empty;
            string spriteSheetName = Path.GetFileNameWithoutExtension(outputPath);

            fileText += "{\n";
            fileText += "\t\"SpriteSheet\": \"" + spriteSheetName+ "\",\n";
            fileText += "\t\"CellWidth\": "+ cellWidth .ToString()+ ",\n";
            fileText += "\t\"CellHeight\": " + cellHeight.ToString() + ",\n";
            fileText += "\t\"Animations\": {\n";

            for (int i = 0; i < files.Length; i++)
            {
                string[] fileName = Path.GetFileNameWithoutExtension(files[i]).Split("_");
                if(fileName[0].ToLower() == "anim")
                {
                    var animName = fileName[1];
                    var begin = i;
                    while(i < files.Length-1)
                    {
                        i++;
                        fileName = Path.GetFileNameWithoutExtension(files[i]).Split("_");
                        Console.WriteLine(animName);
                        if (fileName[1] != animName)
                        {
                            i--;
                            break;
                        }
                    }
                    var end = i;

                    fileText += "\t\t\"" + animName + "\": {\n";
                    fileText += "\t\t\t\"Begin\": " + begin.ToString() + ",\n";
                    fileText += "\t\t\t\"End\": " + end.ToString() + "\n";
                    fileText += "\t\t}"+(i >= files.Length - 1 ? "" : ",")+"\n";
                }
            }

            fileText += "\t}\n";
            fileText += "}";

            File.WriteAllText(outputPath.Replace(".png", "")+".anim", fileText);
        }
    }
}
