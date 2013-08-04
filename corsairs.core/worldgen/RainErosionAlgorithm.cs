using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class RainErosionAlgorithm
    {
        private static bool FindLowestSurrounding(ArrayMap<int> heightMap, int height, List<int[]> surroundings, out int h, out int w)
        {
            var lowest = height;
            var lowestIndex = -1;

            for (var index = 0; index < surroundings.Count; index++)
            {
                var coords = surroundings[index];
                if (heightMap[coords[0], coords[1]] < height)
                {
                    lowestIndex = index;
                    lowest = heightMap[coords[0], coords[1]];
                }
            }

            if (lowestIndex != -1)
            {
                h = surroundings[lowestIndex][0];
                w = surroundings[lowestIndex][1];
                return true;
            }

            h = w = 0;
            return false;
        }

        /// <summary>
        /// Erode terrain by dropping (eroding) rain downwards from random mountain starting points
        /// </summary>
        public static void PerformErosion(ArrayMap<int> heightMap, ArrayMap<int> erosionMap, ArrayMap<bool> water, Random seed, int reps, ArrayMap<bool> mask)
        {
            var size = heightMap.Size;
            var copy = new ArrayMap<int>(size);

            for (var i = 0; i < reps; i++)
            {
                // pick a random start point
                var h = seed.Next(size);
                var w = seed.Next(size);

                if (mask[h, w])
                {
                    ConsiderEroding(heightMap, erosionMap, water, copy, h, w, 0);
                }
            }
        }

        private static int LowerTerrain(ArrayMap<int> heightMap, int h, int w)
        {
            if (heightMap[h, w] > 0)
            {
                heightMap[h, w]--;
            }

            return heightMap[h, w];
        }

        private static void ConsiderEroding(ArrayMap<int> heightMap, ArrayMap<int> erosionMap, ArrayMap<bool> water, ArrayMap<int> erodeMap, int h, int w, int sediment)
        {
            int outH, outW;
            var neighbours = heightMap.Surroundings(h, w);

            if (FindLowestSurrounding(heightMap, heightMap[h, w], neighbours, out outH, out outW))
            {
                LowerTerrain(heightMap, outH, outW);
                erosionMap[h, w]++;
                sediment++;

                // recurse to the downhill location
                ConsiderEroding(heightMap, erosionMap, water, erodeMap, outH, outW, sediment);
            }
            else
            {
                // cannot go downhill - deposit sediment
                var heights = neighbours.Select(x => heightMap[x[0], x[1]]).ToList();
                var minHeight = heights.Min();
                var diff = minHeight - heightMap[h, w];

                Debug.Assert(diff > -1);

                if (sediment > diff)
                {
                    sediment = diff;
                }

                heightMap[h, w] += sediment;
                water[h, w] = true;
                erosionMap[h, w]++;
            }
        }
    }
}
