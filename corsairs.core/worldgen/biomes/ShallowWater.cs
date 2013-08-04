using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class ShallowWater : Water
    {
        public override int MinHeight
        {
            get
            {
                return 75;
            }
        }

        public override char DebugSymbol
        {
            get { return '.'; }
        }

        public override bool SuitableForPOI
        {
            get
            {
                return true;
            }
        }
    }
}
