using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Rock : Land
    {
        public override int MinHeight
        {
            get
            {
                return 170;
            }
        }

        public override char DebugSymbol
        {
            get { return 'r'; }
        }
    }
}
