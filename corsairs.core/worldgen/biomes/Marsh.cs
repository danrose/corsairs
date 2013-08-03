using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Marsh : TemperateLand
    {
        public override int MaxDrainage
        {
            get
            {
                return 20;
            }
        }

        public override int MinDrainage
        {
            get
            {
                return 10;
            }
        }

        public override char DebugSymbol
        {
            get { return 'm'; }
        }
    }
}
