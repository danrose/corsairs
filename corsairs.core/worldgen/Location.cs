using corsairs.core.worldgen.biomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public struct Location
    {
        private readonly int x;
        private readonly int y;
        private readonly Biome biome;
        private readonly int height;
        private readonly bool isWater;
        private readonly int drainage;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public Biome Biome { get { return biome; } }
        public int Height { get { return height; } }
        public bool IsWater { get { return isWater; } }
        public int Drainage { get { return drainage; } }
        
        public Location(int x, int y, Biome biome, int height, bool isWater, int drainage)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.isWater = isWater;
            this.drainage = drainage;
            this.biome = biome;
        }
    }
}
