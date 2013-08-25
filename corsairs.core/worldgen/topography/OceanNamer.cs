using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace corsairs.core.worldgen.topography
{
    public static class OceanNamer
    {
        private const int MinSize = 30;

        public static string[] Adjectives { get; private set; }
        public static string[] Names { get; private set; }
        public static string[] Nouns { get; private set; }
        public static string[] Patterns { get; private set; }
        public static string[] Plurals { get; private set; }
        private static bool initialised;
        private static Random seed = new Random();

        public static void Initialise(string[] adjectives, string[] names, string[] nouns, string[] patterns,
                                      string[] plurals)
        {
            Adjectives = adjectives;
            Names = names;
            Nouns = nouns;
            Patterns = patterns;
            Plurals = plurals;
            initialised = true;
        }


        public static IEnumerable<Ocean> NameOceans(ArrayMap<bool> water)
        {
            if (!initialised)
            {
                throw new Exception("Ocean namer must be initialised");
            }

            var sw = new Stopwatch();
            sw.Start();

            var seen = new bool[water.Size,water.Size];
            var ret = new List<Ocean>();
            for (var h = 1; h < water.Size - 1; h++)
            {
                for (var w = 1; w < water.Size - 1; w++)
                {
                    var square = water[h, w];
                    if (square == false || seen[h, w])
                    {
                        continue;
                    }

                    var currentSize = 0;
                    var minH = h;
                    var maxH = h;
                    var minW = w;
                    var maxW = w;

                    /* this is a queue-based 4-way flood fill. I had to use this slightly more complex version to
                     * avoid blowing up with too much recursion.*/
                    var queue = new Queue<int[]>();
                    queue.Enqueue(new [] { h, w });

                    while(queue.Any())
                    {
                        Console.WriteLine(queue.Count);
                        var item = queue.Dequeue();

                        if (seen[item[0], item[1]])
                        {
                            continue;
                        }

                        foreach (var recursion in Flood(item[1], item[0], water, seen, ref minH, ref maxH, ref minW, ref maxW, ref currentSize))
                        {
                            queue.Enqueue(new [] { recursion[0], recursion[1] });
                        }
                    }

                    if (currentSize >= MinSize)
                    {
                        var oceanName = string.Format(
                            Patterns[seed.Next(Patterns.Length)],
                            Names[seed.Next(Names.Length)],
                            Adjectives[seed.Next(Adjectives.Length)],
                            Nouns[seed.Next(Nouns.Length)],
                            Plurals[seed.Next(Plurals.Length)]
                        );

                        ret.Add(new Ocean(maxH, minH, maxW, minW, currentSize, oceanName));
                    }
                }
            }
            sw.Stop();

            return ret;
        }

        private static IEnumerable<int[]> Flood(int w, int h, ArrayMap<bool> water, bool[,] seen, ref int minH, ref int maxH,
                                  ref int minW, ref int maxW, ref int count)
        {
            seen[h, w] = true;

            if (water[h, w])
            {
                count++;
                if (h < minH)
                {
                    minH = h;
                }
                if (h > maxH)
                {
                    maxH = h;
                }
                if (w < minW)
                {
                    minW = w;
                }
                if (w > maxW)
                {
                    maxW = w;
                }

                return water.Surroundings4(h, w).Where(x => !seen[x[0], x[1]]);
            }

            return new int[0][];
        }
    }
}
