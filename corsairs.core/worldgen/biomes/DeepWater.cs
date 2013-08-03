using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class DeepWater : Water
    {
        public override int MaxHeight
        {
            get
            {
                return 40;
            }
        }

        public override char DebugSymbol
        {
            get { return '#'; }
        }
    }
}
