using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class WaterErosionAlgorithm
    {
        /// <summary>
        /// Watery areas dig themselves even further into the rock
        /// </summary>
        public static void Apply(ArrayMap<int> height, ArrayMap<bool> water, int lowerBy)
        {
            for (var h = 0; h < water.Size; h++)
            {
                for (var w = 0; w < water.Size; w++)
                {
                    if (water[h, w])
                    {
                        height[h, w] = height[h, w] - lowerBy >= 0 ? height[h, w] - lowerBy : 0;
                    }
                }
            }
        }
    }
}
