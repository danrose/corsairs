using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace corsairs.xna
{
    public static class Vectors
    {
        public static float ToLength(this Vector2 point)
        {
            return Vectors.ToLength(point.X, point.Y);
        }

        public static float ToLength(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static void Normalise(this Vector2 point, out float X, out float Y)
        {
            Normalise(point.X, point.Y, out X, out Y);
        }

        public static void Normalise(float x, float y, out float X, out float Y)
        {
            var length = ToLength(x, y);
            X = x / length;
            Y = y / length;
        }

        public static double DotProduct(this Vector2 p1, Vector2 p2)
        {
            return DotProduct(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static double DotProduct(float x1, float y1, float x2, float y2)
        {
            return x1 * x2 + y1 * y2;
        }

        public static Vector2 Subtract(this Vector2 p1, Vector2 p2)
        {
            return new Vector2(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Vector2 Multiply(this Vector2 p, float scalar)
        {
            return new Vector2(p.X * scalar, p.Y * scalar);
        }
    }
}
