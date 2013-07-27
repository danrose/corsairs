using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class MidWater : Biome
    {
        public override bool ConditionsMet(int height, int drainage, bool isWater, double temp)
        {
            return temp > 4 && isWater && height >= 40 && height < 80;
        }

        public override char DebugSymbol
        {
            get { return '~'; }
        }
    }
}
