using corsairs.core.worldgen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.console
{
    class Program
    {
        static void Main(string[] args)
        {
            var biomes = Generator.GenerateMap();

            for (var x = 0; x < biomes.Size; x++)
            {
                var bui = new StringBuilder();
                for (var y = 0; y < biomes.Size; y++)
                {
                    var biome = biomes[x, y];
                    bui.Append(biome == null ? '*' : biome.DebugSymbol);
                }
                Console.WriteLine(bui.ToString());
            }

        }
    }
}
