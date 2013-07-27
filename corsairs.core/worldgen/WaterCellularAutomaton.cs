using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class WaterCellularAutomaton
    {
        /// <summary>
        /// Smooths transition between water and land
        /// </summary>
        public static void Apply(ArrayMap<bool> map, int waterThreshold, int landThreshold)
        {
            for (var h = 0; h < map.Size; h++)
            {
                for (var w = 0; w < map.Size; w++)
                {
                    var surroundings = map.Surroundings(h, w);
                    int waterCount = 0, landCount = 0;

                    foreach (var coord in surroundings)
                    {
                        if (map[coord[0], coord[1]])
                        {
                            waterCount++;
                        }
                        else
                        {
                            landCount++;
                        }
                    }

                    if (waterCount >= waterThreshold)
                    {
                        map[h, w] = true;
                    }
                    else if (landCount >= landThreshold)
                    {
                        map[h, w] = false;
                    }
                }
            }
        }
    }
}
