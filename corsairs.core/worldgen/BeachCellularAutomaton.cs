using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public static class BeachCellularAutomaton
    {
        /// <summary>
        /// Smooths transition between water and land
        /// </summary>
        public static void Apply(ArrayMap<bool> water, ArrayMap<bool> beaches)
        {
            for (var h = 1; h < water.Size - 1; h++)
            {
                for (var w = 1; w < water.Size - 1; w++)
                {
                    var surroundings = water.Surroundings(h, w);
                    int waterCount = 0, landCount = 0;

                    foreach (var coord in surroundings)
                    {
                        if (water[coord[0], coord[1]])
                        {
                            waterCount++;
                        }
                        else
                        {
                            landCount++;
                        }
                    }

                    beaches[h, w] = waterCount >= 3 && landCount >= 3;
                }
            }
        }
    }
}
