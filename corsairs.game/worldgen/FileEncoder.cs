using corsairs.core.worldgen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using corsairs.core.worldgen.topography;

namespace corsairs.game.worldgen
{
    public static class FileEncoder
    {
        private const string header = "corsairs-terrain-file";
        private static int headerLength = header.Length;
        private const int recordLength = 1 + 3 + 3 + 3 + 1 + 1;

        public static string Encode(WorldMap map)
        {
            var locations = map.Locations;
            // header, size of world map
            var ret = new StringBuilder(header + locations.Size.ToString().PadLeft(4));

            for (var h = 0; h < locations.Size; h++)
            {
                for (var w = 0; w < locations.Size; w++)
                {
                    var loc = locations[h, w];
                    ret.AppendFormat("{0}{1}{2}{3}{4}{5}",
                        loc.Biome.DebugSymbol,
                        loc.Height.ToString().PadLeft(3),
                        loc.Drainage.ToString().PadLeft(3),
                        loc.Erosion.ToString().PadLeft(3),
                        loc.IsWater ? "1" : "0",
                        loc.SuitableForPOI ? "1" : "0"
                    );
                }
            }

            return ret.ToString();
        }

        public static WorldMap Decode(StreamReader reader)
        {
            // Read the header and dimensions, throwing if invalid
            var headerBuf = new char[headerLength + 4];
            reader.ReadBlock(headerBuf, 0, headerLength + 4);
            var headerText = new string(headerBuf, 0, headerLength + 4);
            if(headerText.Substring(0, headerLength) != header)
            {
                throw new Exception("File format not understood");
            }
            var size = int.Parse(headerText.Substring(headerLength, 4));
            // Allocate the actual buffer for the content (n by n square, taking m chars per square)
            var fullSize = size * size * recordLength;

            var buf = new char[fullSize];
            int i = 0, count = 0;
            // read a single record at a time
            var ret = new ArrayMap<Location>(size);
            while (reader.EndOfStream == false && reader.ReadBlock(buf, i, recordLength) != 0)
            {
                var biome = BiomeClassifier.LookupBiome(buf[i]);
                var height = int.Parse(new string(buf, i + 1, 3).Trim());
                var drainage = int.Parse(new string(buf, i + 4, 3).Trim());
                var erosion = int.Parse(new string(buf, i + 7, 3).Trim());
                var isWater = buf[i + 10] == '1';
                var suitableForPOI = buf[i + 11] == '1';

                ret[count] = new Location(count % size, count / size, biome, height, erosion, isWater, drainage, suitableForPOI);

                // move offset to next record
                i += recordLength;
                count++;
            }

            return new WorldMap(ret, new List<Ocean>());
        }
    }
}
