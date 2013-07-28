using corsairs.core.worldgen;
using corsairs.game.worldgen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.console
{
    class Program
    {
        static void Main(string[] args)
        {
            ArrayMap<Location> locations;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var saveFile = Path.Combine(Directory.GetCurrentDirectory(), "save.foo");
            var saveFileInfo = new FileInfo(saveFile);
            if (saveFileInfo.Exists)
            {
                Console.WriteLine("Reading saved world from " + saveFile);
                using (var reader = saveFileInfo.OpenText())
                {
                    locations = FileEncoder.Decode(reader);
                    reader.Close();
                }
            }
            else
            {
                Console.WriteLine("Writing new world to " + saveFile);
                locations = Generator.GenerateMap();
                using (var writer = saveFileInfo.CreateText())
                {
                    var serialized = FileEncoder.Encode(locations);
                    writer.Write(serialized);
                    writer.Close();
                }
            }

            sw.Stop();
            Console.WriteLine("Took " + sw.ElapsedMilliseconds + " ms.");

            for (var x = 0; x < locations.Size; x++)
            {
                var bui = new StringBuilder();
                for (var y = 0; y < locations.Size; y++)
                {
                    var location = locations[x, y];
                    bui.Append(location.Biome == null ? '*' : location.Biome.DebugSymbol);
                }
                Console.WriteLine(bui.ToString());
            }

            Console.ReadKey();
        }
    }
}
