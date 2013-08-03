using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Mountain : Land
    {
        public override int MinHeight
        {
            get
            {
                return 220;
            }
        }

        public override char DebugSymbol
        {
            get { return 'M'; }
        }
    }
}
