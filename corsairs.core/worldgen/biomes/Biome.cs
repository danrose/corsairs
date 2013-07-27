using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public abstract class Biome
    {
        public abstract bool ConditionsMet(int height, int drainage, bool isWater, double temp);
        public abstract char DebugSymbol { get; }
    }
}
