using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace corsairs.core.worldgen.topography
{
    public class Ocean
    {
        public int MaxH { get; private set; }
        public int MinH { get; private set; }
        public int MaxW { get; private set; }
        public int MinW { get; private set; }
        public int Count { get; private set; }
        public string Name { get; private set; }
        
        public int CenterH
        {
            get
            {
                return (MaxH - MinH) / 2;
            }
        }

        public int CenterW
        {
            get
            {
                return (MaxW - MinW) / 2;
            }
        }

        public Ocean(int maxH, int minH, int maxW, int minW, int count, string name)
        {
            this.MinH = minH;
            this.MaxH = maxH;
            this.MinW = minW;
            this.MaxW = maxW;
            this.Name = name;
            this.Count = count;
        }
    }
}
