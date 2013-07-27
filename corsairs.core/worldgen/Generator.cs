using corsairs.core.worldgen.biomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class Generator
    {
        private static readonly Random seed = new Random();
        private const int Generations = 6;

        private static ArrayMap<int> CreateHeightMap()
        {
            var height = new ArrayMap<int>(2);
            height.SetData(new[] { 85, 85, 85, 85 });

            var heightAlgo = new DiamondSquareAlgorihm { Square = height };

            var heightRandomisation = 180;

            for (var heightGen = 0; heightGen < Generations; heightGen++)
            {
                height = height.DoubleInSize();
                heightAlgo.Square = height;
                heightAlgo.HeightRandomiser = MinMaxRandomiserFactory(seed, heightRandomisation, 0, 255);
                heightRandomisation = ReduceRandomisation(heightRandomisation, 0.88);
                heightAlgo.DiamondStep();
                heightAlgo.SquareStep();
            }

            return height;
        }

        private static ArrayMap<int> CreateDrainageMap()
        {
            var drainage = new ArrayMap<int>(2);
            drainage.SetData(new[] { seed.Next(50), seed.Next(50), seed.Next(50), seed.Next(50) });
            var drainageAlgo = new DiamondSquareAlgorihm { Square = drainage };
            var drainRandomisation = 70;

            for (var drainGen = 0; drainGen < Generations; drainGen++)
            {
                drainage = drainage.DoubleInSize();
                drainageAlgo.Square = drainage;
                drainageAlgo.HeightRandomiser = MinMaxRandomiserFactory(seed, drainRandomisation, 0, 100);
                drainRandomisation = ReduceRandomisation(drainRandomisation, 0.7);
                drainageAlgo.DiamondStep();
                drainageAlgo.SquareStep();
            }

            return drainage;
        }

        public static ArrayMap<Biome> GenerateMap()
        {
            var height = CreateHeightMap();
            var drainage = CreateDrainageMap();

            // create water table
            var water = height.Translate(x => x < 80);
            var mountainMask = height.Translate(x => x > 130);

            RainErosionAlgorithm.PerformErosion(height, water, seed, 15000, mountainMask);
            WaterCellularAutomaton.Apply(water, 4, 4);
            WaterErosionAlgorithm.Apply(height, water, 15);

            var biomes = BiomeClassifier.CreateBiomes(height, drainage, water);
            DetectBeaches(biomes, water);

            return biomes;
        }

        private static Beach beach = new Beach();

        private static void DetectBeaches(ArrayMap<Biome> biomes, ArrayMap<bool> water)
        {
            var beachMap = new ArrayMap<bool>(water.Size);
            BeachCellularAutomaton.Apply(water, beachMap);

            for (var h = 0; h < biomes.Size; h++)
            {
                for (var w = 0; w < biomes.Size; w++)
                {
                    if (beachMap[h, w])
                    {
                        biomes[h, w] = beach;
                    }
                }
            }
        }

        private static int ReduceRandomisation(int amount, double factor)
        {
            return (int)Math.Pow(amount, factor);
        }

        private static Func<int, int> MinMaxRandomiserFactory(Random seed, int size, int min, int max)
        {
            return h =>
            {
                var newHeight = h + seed.Next(-size, size);
                if (newHeight > max)
                    return max;
                if (newHeight < min)
                    return min;
                return newHeight;
            };
        }

        private static Func<int, int> HeightRandomiserFactory(Random seed, int size)
        {
            return h =>
            {
                var newHeight = h + seed.Next(-size, size);
                if (newHeight > 255)
                    return 255;
                if (newHeight < 0)
                    return 0;
                return newHeight;
            };
        }
    }
}
