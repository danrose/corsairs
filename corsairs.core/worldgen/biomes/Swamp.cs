using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Swamp : TemperateLand
    {
        public override int MaxDrainage
        {
            get
            {
                return 9;
            }
        }

        public override char DebugSymbol
        {
            get { return 's'; }
        }
    }
}
