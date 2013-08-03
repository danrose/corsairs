using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Tundra : Land
    {
        public override int MaxTemp
        {
            get
            {
                return 3;
            }
        }

        public override char DebugSymbol
        {
            get { return 't'; }
        }
    }
}
