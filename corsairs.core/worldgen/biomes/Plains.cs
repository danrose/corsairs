using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Plains : Land
    {
        public override int MaxDrainage
        {
            get
            {
                return 49;
            }
        }

        public override int MaxTemp
        {
            get
            {
                return 26;
            }
        }

        public override char DebugSymbol
        {
            get { return 'p'; }
        }
    }
}
