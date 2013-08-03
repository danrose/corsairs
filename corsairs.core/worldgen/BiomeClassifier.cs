using corsairs.core.worldgen.biomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class BiomeClassifier
    {
        private static List<Biome> biomeList = new List<Biome> 
			{ 

                new DeepWater(), 
				new MidWater(),
                new ShallowWater(),
                new Ice(),
                new Mountain(),
				new Rock(),
                new LushGrassland(),
                new Grassland(),
                new Plains(),
                new Tundra(),
                new Forest(),
                new Hills(),
                new Marsh(),
                new Swamp(),
                new Beach(),
			};

        private static Dictionary<char, Biome> lookup = biomeList.ToDictionary(x => x.DebugSymbol);

        public static Biome LookupBiome(char symbol)
        {
            if (!lookup.ContainsKey(symbol))
            {
                throw new Exception("Unknown biome " + symbol);
            }

            return lookup[symbol];
        }

        /// <summary>
        /// Assigns biomes to each map square
        /// </summary>
        public static ArrayMap<Biome> CreateBiomes(ArrayMap<int> height, ArrayMap<int> drainage, ArrayMap<bool> water)
        {
            var biomes = new ArrayMap<Biome>(height.Size);

            for (var h = 0; h < height.Size; h++)
            {
                for (var w = 0; w < height.Size; w++)
                {
                    var matched = biomeList.FirstOrDefault(b => b.ConditionsMet(height[h, w], drainage[h, w], water[h, w], 15));
                    
                    if (matched != null)
                    {
                        biomes[h, w] = matched;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Warning - no biome at (h:{0}, w:{1}). Height: {2}, drainage: {3}, water: {4}, temp: {5}",
                          h, w, height[h, w], drainage[h, w], water[h, w], 15));
                    } 
                }
            }

            return biomes;
        }

        public static string GetBiomeData(ArrayMap<Biome> datamap)
        {
            var data = datamap.CopyData();
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < data.Length; i++)
            {
                var biome = data[i];
                var biomeName = biome == null ? "[missing]" : biome.GetType().Name;
                if (!dict.ContainsKey(biomeName))
                {
                    dict[biomeName] = 0;
                }
                dict[biomeName]++;
            }

            var ret = new StringBuilder();
            foreach (var kvp in dict)
            {
                ret.AppendLine(kvp.Key + ": " + kvp.Value);
            }

            return ret.ToString();
        }
    }
}
