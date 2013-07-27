using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class LushGrassland : Biome
    {
        public override bool ConditionsMet(int height, int drainage, bool isWater, double temp)
        {
            return temp > 11 && !isWater && drainage > 20 && temp < 35;
        }

        public override char DebugSymbol
        {
            get { return 'G'; }
        }
    }
}
