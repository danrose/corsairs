using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public class DiamondSquareAlgorihm
    {
        public ArrayMap<int> Square
        {
            get;
            set;
        }

        public Func<int, int> HeightRandomiser { get; set; }

        public void DiamondStep()
        {
            // loops over the square, starting 1 square in and finishing 1 square before the edge, skipping 2
            // each time. This ensures it will target the center of new squares made by the subdivision process
            for (var w = 1; w <= Square.Size - 2; w += 2)
            {
                for (var h = 1; h <= Square.Size - 2; h += 2)
                {
                    var averagedHeight = (
                        Square[h - 1, w - 1] +
                        Square[h - 1, w + 1] +
                        Square[h + 1, w - 1] +
                        Square[h + 1, w + 1]
                    ) / 4;
                    var randomised = HeightRandomiser(averagedHeight);
                    Square[h, w] = randomised;
                }
            }
        }

        public void SquareStep()
        {
            // loops over the structure, filling in the gaps using the information provided by the 
            // diamond step. Swaps over starting at index 1 and 0

            for (var h = 0; h < Square.Size; h++)
            {
                // jump over alternate squares, e.g.
                // with square x-x
                //             -x-
                //             x-x
                // follow this sequence:
                // 1           xox
                //             -x-
                //             x-x
                //
                // 2           x-x
                //             ox-
                //             x-x
                //
                // 3           x-x
                //             -xo
                //             x-x
                // and so on...
                for (var w = h % 2 == 1 ? 0 : 1; w < Square.Size; w += 2)
                {
                    var coordsAround = Square.SquareCoords(h, w);
                    var countOfValid = coordsAround.Count;
                    var sum = 0;

                    foreach (var coordToSum in coordsAround)
                    {
                        sum += Square[coordToSum[0], coordToSum[1]];
                    }
                    var averagedHeight = sum / countOfValid;

                    var randomised = HeightRandomiser(averagedHeight);
                    Square[h, w] = randomised;
                }
            }
        }
    }
}
