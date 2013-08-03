using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen.biomes
{
    public abstract class Biome
    {
        public virtual int MinHeight { get { return 0; } }
        public virtual int MaxHeight { get { return Generator.MaxHeight; } }
        public virtual int MinDrainage { get { return 0; } }
        public virtual int MaxDrainage { get { return Generator.MaxDrainage; } }
        public virtual int MinTemp { get { return -273; } }
        public virtual int MaxTemp { get { return 999; } }
        public virtual bool InWater { get { return true; } }
        public virtual bool InLand { get { return true; } }

        public virtual bool ConditionsMet(int height, int drainage, bool isWater, double temp)
        {
            return height >= MinHeight && height <= MaxHeight &&
                drainage >= MinDrainage && drainage <= MaxDrainage &&
                (isWater && InWater || InLand) &&
                temp >= MinTemp && temp <= MaxTemp;
        }

        public abstract char DebugSymbol { get; }
    }

    public abstract class Water : Biome
    {
        public override bool InLand
        {
            get
            {
                return false;
            }
        }

        public override int MinTemp
        {
            get
            {
                return 4;
            }
        }
    }

    public abstract class Land : Biome
    {
        public override bool InWater
        {
            get
            {
                return false;
            }
        }
    }

    public abstract class TemperateLand : Land
    {
        public override int MinTemp
        {
            get
            {
                return 4;
            }
        }
    }
}
