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
        private readonly int erosion;
        private readonly bool isWater;
        private readonly bool suitableForPOI;
        private readonly int drainage;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public Biome Biome { get { return biome; } }
        public int Height { get { return height; } }
        public int Erosion { get { return erosion; } }
        public bool IsWater { get { return isWater; } }
        public int Drainage { get { return drainage; } }
        public bool SuitableForPOI { get { return suitableForPOI; } }

        public Location(int x, int y, Biome biome, int height, int erosion, bool isWater, int drainage, bool suitableForPOI)
        {
            this.x = x;
            this.y = y;
            this.height = height;
            this.isWater = isWater;
            this.drainage = drainage;
            this.biome = biome;
            this.erosion = erosion;
            this.suitableForPOI = suitableForPOI;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || !GetType().IsAssignableFrom(obj.GetType()))
            {
                return false;
            }
            var other = (Location)obj;

            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}
