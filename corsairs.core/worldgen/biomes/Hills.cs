using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Hills : Land
    {
        public override int MinHeight
        {
            get
            {
                return 50;
            }
        }

        public override int MinDrainage
        {
            get
            {
                return 50;
            }
        }

        public override char DebugSymbol
        {
            get { return 'h'; }
        }
    }
}
