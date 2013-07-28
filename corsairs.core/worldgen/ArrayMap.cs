using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace corsairs.core.worldgen
{
    public class ArrayMap<T>
    {
        private T[] data;
        private int size;

        public ArrayMap(int size)
        {
            this.size = size;
            data = (T[])Array.CreateInstance(typeof(T), size * size);
        }

        public ArrayMap(ArrayMap<T> copy)
        {
            this.data = copy.data;
            this.size = copy.size;
        }

        public void SetData(T[] data)
        {
            this.data = data;
            this.size = data == null ? 0 : (int)Math.Sqrt(data.Length);
        }

        public T[] CopyData()
        {
            var copy = new T[data.Length];
            Array.Copy(data, copy, data.Length);
            return copy;
        }

        public int Size
        {
            get { return size; }
        }

        public T this[int x]
        {
            get
            {
                return data[x];
            }
            set
            {
                data[x] = value;
            }
        }

        private int CoordToIndex(int heightCoord, int widthCoord)
        {
            if (heightCoord > size - 1 || widthCoord > size - 1)
            {
                throw new Exception("Coord cannot be larger than the size - 1");
            }

            return (heightCoord * size) + widthCoord;
        }

        public T this[int heightCoord, int widthCoord]
        {
            get
            {
                if (heightCoord > size - 1 || widthCoord > size - 1)
                {
                    throw new Exception("Coord cannot be larger than the size - 1");
                }
                return data[CoordToIndex(heightCoord, widthCoord)];
            }
            set
            {
                if (heightCoord > size - 1 || widthCoord > size - 1)
                {
                    throw new Exception("Coord cannot be larger than the size - 1");
                }
                data[CoordToIndex(heightCoord, widthCoord)] = value;
            }
        }

        public ArrayMap<T> DoubleInSize()
        {
            // divided up, a square's new size will be 2 * oldsize - 1
            var newSize = -1 + 2 * size;
            var ret = new ArrayMap<T>(newSize);

            // multiply all values by 2 to get the new coordinate
            // w iterates over width
            for (var w = 0; w < size; w++)
            {
                // h iterates over height
                for (var h = 0; h < size; h++)
                {
                    ret[2 * h, 2 * w] = this[h, w];
                }
            }

            size = newSize;
            return ret;
        }

        public List<int[]> SquareCoords(int heightCoord, int widthCoord)
        {
            var ret = new List<int[]>();

            if (heightCoord > 0)
            {
                ret.Add(new[] { heightCoord - 1, widthCoord });
            }

            if (heightCoord < size - 1)
            {
                ret.Add(new[] { heightCoord + 1, widthCoord });
            }

            if (widthCoord > 0)
            {
                ret.Add(new[] { heightCoord, widthCoord - 1 });
            }

            if (widthCoord < size - 1)
            {
                ret.Add(new[] { heightCoord, widthCoord + 1 });
            }

            return ret;
        }

        public List<int[]> Surroundings(int heightCoord, int widthCoord)
        {
            var ret = new List<int[]>();

            var atTop = heightCoord == 0;
            var atRight = widthCoord == size - 1;
            var atLeft = widthCoord == 0;
            var atBottom = heightCoord == size - 1;

            if (!atTop)
            {
                ret.Add(new[] { heightCoord - 1, widthCoord });

                if (!atLeft)
                {
                    ret.Add(new[] { heightCoord - 1, widthCoord - 1 });
                }
                if (!atRight)
                {
                    ret.Add(new[] { heightCoord - 1, widthCoord + 1 });
                }
            }

            if (!atBottom)
            {
                ret.Add(new[] { heightCoord + 1, widthCoord });

                if (!atLeft)
                {
                    ret.Add(new[] { heightCoord + 1, widthCoord - 1 });
                }

                if (!atRight)
                {
                    ret.Add(new[] { heightCoord + 1, widthCoord + 1 });
                }
            }

            if (!atLeft)
            {
                ret.Add(new[] { heightCoord, widthCoord - 1 });
            }

            if (!atRight)
            {
                ret.Add(new[] { heightCoord, widthCoord + 1 });
            }

            return ret;
        }

        public ArrayMap<TNew> Translate<TNew>(Func<T, TNew> mapFunction)
        {
            var ret = new ArrayMap<TNew>(size);

            for (var i = 0; i < data.Length; i++)
            {
                ret[i] = mapFunction(this[i]);
            }

            return ret;
        }
    }
}
