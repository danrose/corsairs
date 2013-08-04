using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class MidWater : Water
    {
        public override int MaxHeight
        {
            get
            {
                return 74;
            }
        }

        public override int MinHeight
        {
            get
            {
                return 41;
            }
        }

        public override char DebugSymbol
        {
            get { return '~'; }
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
