using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using corsairs.core.worldgen.topography;

namespace corsairs.core.worldgen
{
    public class WorldMap
    {
        public ArrayMap<Location> Locations { get; private set; }
        public IEnumerable<Ocean> Oceans { get; private set; }

        public WorldMap(ArrayMap<Location> locations, IEnumerable<Ocean> oceans)
        {
            this.Locations = locations;
            this.Oceans = oceans;
        }
    }
}
