using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace corsairs.xna
{
    public static class SpriteUtils
    {
        public static void DrawDots(this SpriteBatch spriteBatch, Texture2D texture, int texSize, 
            int texOffsetX, int texOffsetY, Vector2 from, Vector2 to, int numDots)
        {
            var direction = to - from;
            var normDirection = direction;
            normDirection.Normalize();

            // draw new path
            var step = direction.Multiply(1f / numDots);
            for (var i = 1; i <= numDots; i++)
            {
                var dot = from + step.Multiply(i);
                spriteBatch.Draw(texture, new Rectangle((int)dot.X, (int)dot.Y, texSize, texSize),
                    new Rectangle(texOffsetX, texOffsetY, texSize, texSize),
                    Color.White);
            } 
        }

        public static void DrawDots(this SpriteBatch spriteBatch, Texture2D texture, int texSize,
            Vector2 from, Vector2 to, int numDots)
        {
            DrawDots(spriteBatch, texture, texSize, 0, 0, from, to, numDots);
        }

        public static void DrawDottedSegments(this SpriteBatch spriteBatch, Texture2D texture, int texSize,
            int texOffsetX, int texOffsetY, Vector2 start, List<Vector2> segments, int numDots)
        {
            foreach (var segment in segments)
            {
                DrawDots(spriteBatch, texture, texSize, texOffsetX, texOffsetY, start, segment, numDots);
                start = segment;
            }
        }

        public static void DrawDottedSegments(this SpriteBatch spriteBatch, Texture2D texture, int texSize,
            Vector2 start, List<Vector2> segments, int numDots)
        {
            DrawDottedSegments(spriteBatch, texture, texSize, 0, 0, start, segments, numDots);
        }
    }
}
