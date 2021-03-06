﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Ice : Water
    {
        public override int MaxTemp
        {
            get
            {
                return 4;
            }
        }

        public override int MinTemp
        {
            get
            {
                return -273;
            }
        }

        public override char DebugSymbol
        {
            get { return 'i'; }
        }
    }
}
