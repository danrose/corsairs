﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public class Grassland : TemperateLand
    {
        public override int MinDrainage
        {
            get
            {
                return 15;
            }
        }

        public override int MaxDrainage
        {
            get
            {
                return 50;
            }
        }

        public override int MaxTemp
        {
            get
            {
                return 24;
            }
        }

        public override char DebugSymbol
        {
            get { return 'g'; }
        }
    }
}
