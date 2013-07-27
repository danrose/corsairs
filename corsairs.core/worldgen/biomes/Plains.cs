using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Plains : Biome
    {
        public override bool ConditionsMet(int height, int drainage, bool isWater, double temp)
        {
            return temp > 0 && !isWater && drainage > 35 && temp < 27;
        }

        public override char DebugSymbol
        {
            get { return 'p'; }
        }
    }
}
