using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Tundra : Biome
    {
        public override bool ConditionsMet(int height, int drainage, bool isWater, double temp)
        {
            return temp < 2 && !isWater;
        }

        public override char DebugSymbol
        {
            get { return 't'; }
        }
    }
}
