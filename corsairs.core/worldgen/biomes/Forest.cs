using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Forest : Land
    {
        public override int MinDrainage
        {
            get
            {
                return 50;
            }
        }


        public override int MinTemp
        {
            get
            {
                return 10;
            }
        }

        public override char DebugSymbol
        {
            get { return 'f'; }
        }
    }
}
